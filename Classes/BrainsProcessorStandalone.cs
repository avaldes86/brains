using Brains.Classes;
using ContecAI.ROOT.CIMV2;
using NeuronDotNet.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brains
{
    public class BrainsProcessorStandAlone: BrainsProcessorNetBase
    {

        public BrainsProcessorStandAlone(BackgroundWorker backgroundWorker)
        {
            bw = backgroundWorker;
        }

        public BrainsProcessorStandAlone() : base()
        {
        }
        
        /// <summary>
        /// Recieve brains from a cluster coworker over a TcpConnection
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="sr"></param>
        /// <param name="sw"></param>
        public void RecieveBrains(NetworkStream ns, StreamReader sr, StreamWriter sw)
        {
            var q = int.Parse(sr.ReadLine());
            var rbrains = new List<Brain>();
            for (int i = 0; i < q; i++)
            {
                var id = sr.ReadLine();
                var size = int.Parse(sr.ReadLine());
                var hash = sr.ReadLine();
                var ms = ReadToMemoryStream(ns, size);
                var chash = Convert.ToBase64String(MD5.Create().ComputeHash(ms.ToArray()));
                sw.WriteLine(chash);
                sw.Flush(); ns.Flush();
                if (chash != hash)
                    throw new Exception(ErrorTransmission);
                rbrains.Add(Brain.Deserialize(ms));
                ms.Dispose();
            }
            
                brains = rbrains;
        }

        /// <summary>
        /// Generate all the ANNs structures to be tested.
        /// </summary>
        /// <param name="listBox1">The list of input data</param>
        /// <param name="listBox2">The list of output data</param>
        /// <param name="listBox4">The list of hidden layers combinations</param>
        /// <returns></returns>
        internal override List<Brain> GenerateBrains(ListBox listBox1, ListBox listBox2, ListBox listBox4)
        {
            var combox = Combox(listBox4);
            bw.ReportProgress(0, combox.Count + " combinations. Generating ANNs...");
            var brains = new List<Brain>();
            var inputs = listBox1.Items.Count;
            var outputs = listBox2.Items.Count;
            foreach (var item in combox)
            {
                var k = item.ToList();
                k.Insert(0, inputs);
                k.Add(outputs);
                //for (int i = 0; i < 10; i++) 
                brains.Add(new Brain(k.ToArray()));
            }
            bw.ReportProgress(0, combox.Count + " ANNs generated. Starting training...");
            this.brains = brains;
            return brains;
        }


        /// <summary>
        /// This method creates the training set, and the verification set
        /// </summary>
        /// <param name="inputListBox">ListBox with the input parameters</param>
        /// <param name="outputListBox">ListBox with the output parameters</param>
        /// <param name="headers">List of headers to determine the fields indexes</param>
        internal override void  PrepareTrainingVerificationSets(ListBox inputListBox, ListBox outputListBox, string[] headers)
        {
            var inputIndexes = new List<int>();
            maxs = new double[data[0].Length];
            mins = new double[data[0].Length];
            for (int i = 0; i < maxs.Length; i++)
            {
                maxs[i] = data.Max(j => j[i]);
                mins[i] = data.Min(j => j[i]);
            }
            foreach (var item in inputListBox.Items)
            {
                inputIndexes.Add(headers.ToList().IndexOf(item.ToString()));
            }
            var outputIndexes = new List<int>();
            foreach (var item in outputListBox.Items)
            {
                outputIndexes.Add(headers.ToList().IndexOf(item.ToString()));
            }

            #region Generate training set
            bw.ReportProgress(0, "Generating training set...");
            var tset = data.Take(data.Count / 2).ToList();
            training = new TrainingSet(inputIndexes.Count, outputIndexes.Count);
            foreach (var item in tset)
            {
                var ivect = new List<double>();
                var ovect = new List<double>();
                for (int i = 0; i < item.Length; i++)
                {
                    if (inputIndexes.Contains(i))
                    {
                        ivect.Add(item[i] / (maxs[i] - mins[i]));
                    }
                    else
                    {
                        ovect.Add(item[i] / (maxs[i] - mins[i]));
                    }
                }
                var ts = new TrainingSample(ivect.ToArray(), ovect.ToArray());
                training.Add(ts);
            }
            #endregion
            #region Generate verification set
            bw.ReportProgress(0, "Generating verification set...");
            var tver = data.Except(tset).ToList();
            verification = new TrainingSet(inputIndexes.Count, outputIndexes.Count);
            foreach (var item in tver)
            {
                var ivect = new List<double>();
                var ovect = new List<double>();
                for (int i = 0; i < item.Length; i++)
                {
                    if (inputIndexes.Contains(i))
                    {
                        ivect.Add(item[i] / (maxs[i] - mins[i]));
                    }
                    else
                    {
                        ovect.Add(item[i] / (maxs[i] - mins[i]));
                    }
                }
                var ts = new TrainingSample(ivect.ToArray(), ovect.ToArray());
                verification.Add(ts);
                #endregion
            }
        }

        public event EventHandler UpdateChart;

        /// <summary>
        /// Process ANNs in Standalone mode
        /// </summary>
        /// <param name="tc">Initial training epochs</param>
        /// <param name="bases">The base used to increase exponentially the number of training epochs in each iteration</param>
        /// <param name="ii">Current iteration</param>
        /// <param name="cont">Flag to determine when to stop/continue with a new iteration</param>
        public void ProcessBrains(int tc, int bases, ref int ii, out bool cont, DoWorkEventArgs e)
        {
            var dt = DateTime.Now;
            var trainCount = (int)(tc * Math.Pow(bases, ii));
            var ss = $"Loop: {(ii + 1)} starting at {dt.ToString()}. {brains.Count} brains. {brains.Count * trainCount} learnings";
            bw.ReportProgress(0, ss);
            var CancelAndExit = new CancellationToken(e.Cancel);
            var pi = Parallel.ForEach(brains, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount > brains.Count ? brains.Count : Environment.ProcessorCount, CancellationToken = CancelAndExit }, i =>
            {
                if (e.Cancel) return;
                i.Learn(training, verification, trainCount);
                if (minprec > i.Prec)
                {
                    minprec = i.Prec;
                    brain = i;
                    UpdateChart?.Invoke(this, new EventArgs());
                }
            });
            var total = brains.Where(i => i.Prec != double.NaN && i.Prec!=0).OrderBy(i => i.Prec).ToList();

            // get and remove the half
            var remove = total.Skip(total.Count / bases).ToList(); //
            foreach (var brain in remove)
            {
                brains.Remove(brain);
            }
            brains.ForEach(i => i.Reset());
            ss = $"Loop {(ii + 1)} ended at {DateTime.Now} (Slapsed time: {DateTime.Now.Subtract(dt)})";
            bw.ReportProgress(0, ss);
            //Save the winner of the iteration
            var fs = new FileStream($"{brains[0].ToString().Replace(":", "_")}_race_{(ii + 1)}.brain", FileMode.Create);
            brains[0].Serialize(fs);
            fs.Close();
            brain = brains[0];
            cont = brains.Count != 1;
            ii++;
        }
    }


}
