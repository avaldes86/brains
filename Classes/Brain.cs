using NeuronDotNet.Core;
using NeuronDotNet.Core.Backpropagation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Brains
{
    [Serializable]
    public class Brain 
    {
        BackpropagationNetwork nw;
        ILayer iLayer, oLayer;
        private double lastprecision;
        public string[] headers;
        public string[] inputs;
        public string[] outputs;
        public double[] maxs, mins;
        public readonly Guid id;
        public bool pending;
        private List<BackpropagationConnector> connectors;

        public List<ILayer> layers { get; set; }
        public int[] slayers { get; set; }

        /// <summary>
        /// The constructor of a new ANN
        /// </summary>
        /// <param name="slayers">Layer's sizes (asume full-connected layers)</param>
        public Brain(int[] slayers) 
        {
            id = Guid.NewGuid();
            Initialize(slayers);
        }

        public void Initialize(int[] slayers)
        {
            (iLayer = new SigmoidLayer(slayers[0])).Initialize();
            (oLayer = new SigmoidLayer(slayers[slayers.Count() - 1])).Initialize();
            var layer = (ActivationLayer)iLayer;
            layers = new List<ILayer>();
            layers.Add(iLayer);
            connectors = new List<BackpropagationConnector>();
            foreach (var c in slayers.Skip(1).Take(slayers.Count() - 2).Select(size => new SigmoidLayer(size)))
            {
                c.Initialize();                
                var bnc = new BackpropagationConnector(layer, c, ConnectionMode.Complete);
                layer = c;
                connectors.Add(bnc);
                
            }
            var bnco = new BackpropagationConnector(layer, (ActivationLayer)oLayer, ConnectionMode.Complete);
            this.slayers = slayers;
            layers.Add(oLayer);
            nw = new BackpropagationNetwork((ActivationLayer)iLayer, (ActivationLayer)oLayer);
            SaveState();
        }

        private MemoryStream state_ms;
        private byte[] state_bytes;
        //internal double sprec { get; set; } = double.MaxValue;

        private void SaveState()
        {
            var bf = new BinaryFormatter();
            state_ms = new MemoryStream();
            bf.Serialize(state_ms, nw);
            state_bytes = state_ms.ToArray();
        }

        public void Reset()
        {
            var bf = new BinaryFormatter();
            nw = (BackpropagationNetwork)bf.Deserialize(new MemoryStream(state_bytes));
        }

        internal string Serialize64()
        {
            var ms = new MemoryStream();
            Serialize(ms);
            var data = ms.ToArray();
            ms.Dispose();
            var resp = Convert.ToBase64String(data);
            return resp;
        }

        /// <summary>
        /// This method is to save extra data on the network class
        /// </summary>
        /// <param name="heads">The field list in the primary data</param>
        /// <param name="inps">The fields in the input</param>
        /// <param name="outs">The fields in the output</param>
        /// <param name="min">Minimal values of each field</param>
        /// <param name="max">Maximal values of each field</param>
        public void SetExtra(string[] heads, string[] inps, string[] outs, double[] min, double[] max)
        {
            headers = heads;
            inputs = inps;
            outputs = outs;
            mins = min;
            maxs = max;
        }

        /// <summary>
        /// Executes a test with the verification test
        /// </summary>
        /// <param name="testSet">Verification set</param>
        /// <returns>Average of differences betwen the calculated and the desired result of all the verification samples</returns>
        public double PrecisionTest(TrainingSet testSet)
        {
            var d1 = 0d;
            int ic = 0;
            foreach (var item in testSet.TrainingSamples)
            {
                var delta = 0d;
                var result = nw.Run(item.InputVector);
                for (int i = 0; i < result.Length; i++)
                {
                    delta += Math.Abs(Math.Abs(result[i]) - Math.Abs(item.OutputVector[i]));
                }
                d1 += delta;
                ic++;
            }
            return lastprecision = d1 /* / (double)ic*nw.MeanSquaredError*/;
        }

        /// <summary>
        /// Start the training epochs and when the learning stage is complete, verifies the weights of synapsis with the verification set.
        /// </summary>
        /// <param name="tset">Training set</param>
        /// <param name="settest">Verification set</param>
        /// <param name="epochs">Number of training epochs</param>
        public void Learn(TrainingSet tset, TrainingSet settest, int epochs)
        {
            nw.Learn(tset, epochs);
            PrecisionTest(settest);
        }

        /// <summary>
        /// Executes the ANN with input parameters
        /// </summary>
        /// <param name="input">Input vector</param>
        /// <returns>Estimated output vector</returns>
        public double[] Run(double[] input)
        {
            return nw.Run(input);
        }

        internal double[][] Eval(TrainingSet tset, TrainingSet settest)
        {
            var d1 = 0d;
            int ic = 0;
            var list = new List<double[]>();
            foreach (var item in tset.TrainingSamples.Concat(settest.TrainingSamples))
            {
                var delta = new[] { 0d, 0d };
                var result = nw.Run(item.InputVector);
                for (int i = 0; i < result.Length; i++)
                {
                    delta[0] += Math.Abs(item.OutputVector[i]);
                    delta[1] += Math.Abs(result[i]);
                }
                list.Add(delta);
                ic++;
            }
            return list.ToArray();
        }

        /// <summary>
        /// Saves an instance of Brain class into an stream
        /// </summary>
        /// <param name="stream">Stream to serialize over</param>
        public void Serialize(Stream stream)
        {
            var bf = new BinaryFormatter();
            bf.Serialize(stream, this);
        }

        /// <summary>
        /// Load a Brain class instance from a stream
        /// </summary>
        /// <param name="stream">Stream to read</param>
        /// <returns>Brain instance previously serialized</returns>
        public static Brain Deserialize(Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();
            var brain = (Brain)bf.Deserialize(stream);
            return brain;
        }

        public override string ToString()
        {
            var s = "";// nw.InputLayer.NeuronCount.ToString() ;
            foreach (var item in slayers)
            {
                s += ":" + item;
            }
            //s += ":" + nw.OutputLayer.NeuronCount;
            s = "P_"+(Prec).ToString("N8")+" [" + s.Substring(1) + "]";
            return s;
        }

        public double Prec
        {
            get
            {
                return lastprecision;
            }
            set
            {
                lastprecision = value; 
            }
        }

        internal static Brain Deserialize64(string value)
        {
            var cc = Convert.FromBase64String(value);
            var ms = new MemoryStream(cc);
            var b = Deserialize(ms);
            ms.Dispose();
            return b;
        }
    }
}
