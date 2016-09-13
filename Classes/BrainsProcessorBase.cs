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
    public class BrainsProcessorBase
    {
        public List<Brain> brains = new List<Brain>();
        public TrainingSet training, verification;
        public double[] mins;
        public double[] maxs;
        public List<double[]> data;
        public double minprec = double.MaxValue;
        public Brain brain;
        public BackgroundWorker bw;
        protected List<Brain> pending;

        protected BrainsProcessorBase(BackgroundWorker backgroundWorker)
        {
            bw = backgroundWorker;
        }

        protected BrainsProcessorBase() : base()
        {
        }

        /// <summary>
        /// Method to train all ANNs
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="bases"></param>
        public virtual void ProcessBrains(int tc, int bases)
        {
        }
        

        /// <summary>
        /// Get the CPU information
        /// </summary>
        /// <returns></returns>
        internal Processor GetProcessorInfo()
        {
            return Processor.GetInstances().Cast<Processor>().ToArray()[0];
        }

        /// <summary>
        /// Get the load balance information to send it to the cluster master
        /// </summary>
        /// <returns></returns>
        internal int Pound()
        {
            return (int)GetProcessorInfo().MaxClockSpeed * (Environment.ProcessorCount/* - 1*/);
        }

        /// <summary>
        /// Reads a memory dump from a NetworkStrea,
        /// </summary>
        /// <param name="ns">NetworkStream</param>
        /// <param name="size">Size of data</param>
        /// <returns>MemoryStream with the data</returns>
        protected MemoryStream ReadToMemoryStream(NetworkStream ns, int size)
        {
            ns.ReadTimeout = 1000;
            var buff = new byte[size];
            int readed = 0, pos = 0;
            do
            {
                if (pos - size >= 0) break; // Error in transmission, only for debuggin
                readed = ns.Read(buff, pos, size - pos);
                pos += readed;
            }
            while (readed > 0 || pos < size);
            return new MemoryStream(buff);
        }

        internal virtual void SetData(List<double[]> data)
        {
            this.data = data;
        }

        /// <summary>
        /// Get all the possibles combinations of layers for ANNs
        /// </summary>
        /// <param name="listBox4">ListBox that contains the x-y combinations for each layer</param>
        /// <returns>List of combinations</returns>
        protected List<int[]> Combox(ListBox listBox4)
        {
            var l = new List<string>();
            var data = listBox4.Items.Cast<object>().Select(i => i.ToString().Split('-').Select(int.Parse).ToArray()).ToList();
            int ii = 0;
            foreach (var item in data)
            {
                string a = "";
                var lx = new List<string>();
                for (int i = item[0]; i <= item[1]; i++)
                {
                    a = "_" + i;
                    if ((l.Count <= item[1] - item[0]) && ii == 0)
                    {
                        l.Add(a);
                    }
                    else
                        foreach (var itemx in l.ToList())
                        {
                            lx.Add(itemx + a);
                        }

                }
                l.AddRange(lx.ToArray());
                ii++;
            }
            return l.Distinct().Select(i => i.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToList();
        }

        /// <summary>
        /// Generate all the ANNs structures to be tested.
        /// </summary>
        /// <param name="listBox1">The list of input data</param>
        /// <param name="listBox2">The list of output data</param>
        /// <param name="listBox4">The list of hidden layers combinations</param>
        /// <returns></returns>
        internal virtual List<Brain> GenerateBrains(ListBox listBox1, ListBox listBox2, ListBox listBox4)
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
                brains.Add(new Brain(k.ToArray()));
            }
            bw.ReportProgress(0, combox.Count + " ANNs generated. Starting training...");
            this.brains = brains;
            return brains;
        }


        /// <summary>
        /// This method creates the training set, and the verification set
        /// </summary>
        /// <param name="listBox1">ListBox with the input parameters</param>
        /// <param name="listBox2">ListBox with the output parameters</param>
        /// <param name="headers">List of headers to determine the fields indexes</param>
        internal virtual void PrepareTrainingVerificationSets(ListBox listBox1, ListBox listBox2, string[] headers)
        {
            var inputIndexes = new List<int>();
            maxs = new double[data[0].Length];
            mins = new double[data[0].Length];
            for (int i = 0; i < maxs.Length; i++)
            {
                maxs[i] = data.Max(j => j[i]);
                mins[i] = data.Min(j => j[i]);
            }
            foreach (var item in listBox1.Items)
            {
                inputIndexes.Add(headers.ToList().IndexOf(item.ToString()));
            }
            var outputIndexes = new List<int>();
            foreach (var item in listBox2.Items)
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
                        ivect.Add(item[i] /*/ (maxs[i] - mins[i])*/);
                    }
                    else
                    {
                        ovect.Add(item[i] /*/ (maxs[i] - mins[i])*/);
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
                        ivect.Add(item[i] /*/ (maxs[i] - mins[i])*/);
                    }
                    else
                    {
                        ovect.Add(item[i] /*/ (maxs[i] - mins[i])*/);
                    }
                }
                var ts = new TrainingSample(ivect.ToArray(), ovect.ToArray());
                verification.Add(ts);
                #endregion
            }
        }

    }
}
