using Brains;
using Brains.Classes;
using Gif.Components;
using NeuronDotNet.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brains
{
    public partial class MainForm : Form
    {
        List<double[]> data;
        private string[] headers;
        TrainingSet training, verification;
        private double[] mins;
        private double[] maxs;
        private Brain brain;
        private BrainsProcessorMaster master_proccessor;
        private BrainsProcessorSlave slave_proccessor;
        private BrainsProcessorStandAlone standalone_proccessor;

        public MainForm()
        {
            InitializeComponent();
            var interfaces = Dns.GetHostAddresses(Dns.GetHostName()).Cast<object>().ToList();
            interfaces.Add(IPAddress.Any);
            comboBox1.Items.AddRange(interfaces.ToArray());
            //comboBox2.Items.AddRange(interfaces.ToArray());
        }

        /// <summary>
        /// Add a layer to possibles design
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            listBox4.Items.Add(numericUpDown1.Value + "-" + numericUpDown2.Value);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem != null)
                listBox4.Items.Remove(listBox4.SelectedItem);
        }

        /// <summary>
        /// Loads data for Master mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() { Filter = "CSV Files (*.csv)|*.csv", Title = "Open data file" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var data = File.ReadAllLines(ofd.FileName);
            headers = data[0].Split(';');
            this.data = data.Skip(1).Select(i => i.Split(';').Select(double.Parse).ToArray()).ToList();
            listBox1.Items.AddRange(headers.Cast<object>().ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            listBox2.Items.Add(listBox1.SelectedItem);
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;
            listBox1.Items.Add(listBox2.SelectedItem);
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = listBox1.Enabled = listBox2.Enabled = listBox3.Enabled = listBox4.Enabled = true;
            timer1.Enabled = false;
        }
        int ii;

        List<MemoryStream> images = new List<MemoryStream>();

        private void timer1_Tick(object sender, EventArgs e)
        {
            label22.Text = standalone_proccessor.minprec.ToString("N10");
            //label24.Text = 
            if (update_standalone_chart)
            {
                update_standalone_chart = false;
                ii++;
                var ann = standalone_proccessor.brain.Eval(standalone_proccessor.training, standalone_proccessor.verification);
                chart1.Series[2].Points.Clear();
                for (int i = 0; i < 2; i++)
                {
                    chart1.Series[i].Points.Clear();
                    for (int j = 0; j < ann.GetLength(0); j++)
                    {
                        chart1.Series[i].Points.Add(new[] { ann[j][i] });
                        if (i == 0) continue;
                        chart1.Series[2].Points.Add(new[] { ann[j][0] - ann[j][1] });
                    }
                }
                chart1.Legends[0].Title = standalone_proccessor.brain?.ToString();
                chart1.Invalidate();
                var mms = new MemoryStream();
                chart1.SaveImage(mms, ImageFormat.Gif);
                images.Add(mms);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            master_proccessor = new BrainsProcessorMaster(bwMaster);
            master_proccessor.IterationComplete += Master_proccessor_IterationComplete;
            master_proccessor.StartListen(IPAddress.Parse(comboBox1.SelectedItem.ToString()), (int)numericUpDown3.Value);
            timer3.Enabled = timer2.Enabled = true;
            button1.Enabled = button11.Enabled = true;
            comboBox1.Enabled = false;
            numericUpDown3.Enabled = false;
            master_proccessor.OnNewSlave += Processor_OnNewSlave;
            master_proccessor.OnSlaveUnregister += Processor_OnSlaveUnregister;
            mspeed = master_proccessor.maxSpeed;
        }


        private void Master_proccessor_IterationComplete(object sender, EventArgs e)
        {
            update_master_chart = true;
        }

        private void Processor_OnSlaveUnregister(object sender, EventArgs e)
        {
            del_slave.Add((RegisteredSlave)sender);
            mspeed = master_proccessor.maxSpeed;
        }

        private void Processor_OnNewSlave(object sender, EventArgs e)
        {
            //Add to queue and activate timerx hilos
            new_slave.Add((RegisteredSlave)sender);
            mspeed = master_proccessor.maxSpeed;
            //timer2.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            slave_proccessor = new BrainsProcessorSlave(textBox2.Text, (int)numericUpDown5.Value, bwSlave);
            //slave_proccessor.StartListen(IPAddress.Parse(comboBox2.SelectedItem.ToString()), (int)numericUpDown4.Value);
            slave_proccessor.RegisterAsSlave(textBox2.Text, (int)numericUpDown5.Value);
            button13.Enabled = false;
            button12.Enabled = true;
            timer4.Enabled = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            slave_proccessor.StopListen();
            button10.Enabled = true;
            button11.Enabled = false;
            comboBox1.Enabled = true;
            numericUpDown3.Enabled = true;
        }

        List<RegisteredSlave> new_slave = new List<RegisteredSlave>();
        List<RegisteredSlave> del_slave = new List<RegisteredSlave>();
        private bool update_standalone_chart;
        private bool update_master_chart;

        private void timer2_Tick(object sender, EventArgs e)
        {

            listBox5.Items.Remove(del_slave.Cast<object>().ToArray());
            listBox5.Items.AddRange(new_slave.Cast<object>().ToArray());
            del_slave = new List<RegisteredSlave>();
            new_slave = new List<RegisteredSlave>();
            //button4.Enabled = button2.Enabled = listBox5.Items.Cast<object>().Any()  && master_proccessor.listening;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            slave_proccessor.StopListen();
            button13.Enabled = true;
            button12.Enabled = true;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            bwStandalone.RunWorkerAsync();
            timer1.Enabled = true;
        }

        private void Standalone_proccessor_UpdateChart(object sender, EventArgs e)
        {
            update_standalone_chart = true;
        }

        /// <summary>
        /// Load data for Standalone mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button19_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() { Filter = "CSV Files (*.csv)|*.csv", Title = "Open data file" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var data = File.ReadAllLines(ofd.FileName);
            headers = data[0].Split(';');
            this.data = data.Skip(1).Select(i => i.Split(';').Select(double.Parse).ToArray()).ToList();
            listBox10.Items.AddRange(headers.Cast<object>().ToArray());
        }

        /// <summary>
        /// Moves a parameter to the output of networks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            if (listBox10.SelectedItem == null) return;
            listBox9.Items.Add(listBox10.SelectedItem);
            listBox10.Items.Remove(listBox10.SelectedItem);
        }

        /// <summary>
        /// Moves a parameter to the input of the networks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            if (listBox9.SelectedItem == null) return;
            listBox10.Items.Add(listBox9.SelectedItem);
            listBox9.Items.Remove(listBox9.SelectedItem);
        }

        private void DoWork_bwStandalone(object sender, DoWorkEventArgs e)
        {
            standalone_proccessor = new BrainsProcessorStandAlone(bwStandalone);
            standalone_proccessor.UpdateChart += Standalone_proccessor_UpdateChart;
            standalone_proccessor.SetData(data);

            standalone_proccessor.PrepareTrainingVerificationSets(listBox10, listBox9, headers);
            bwStandalone.ReportProgress(0, "Generating combinations...");
            standalone_proccessor.GenerateBrains(listBox10, listBox9, listBox7);
            const int bases = 2;
            var tc = int.Parse(textBox3.Text);
            if (e.Cancel) return;
            try
            {
                bool cont = true;
                int ii = 0;
                while (cont)
                {
                    if (e.Cancel) break;
                    standalone_proccessor.ProcessBrains(tc, bases, ref ii, out cont, e);
                    update_standalone_chart = true;
                }
                if (checkBox1.Checked)
                {
                    var ms = new MemoryStream();
                    standalone_proccessor.brain.Reset();
                    standalone_proccessor.brain.Serialize(ms);
                    var l = new Dictionary<int, Brain>();
                    for (int i = tc; i < 40000; i += 25)
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        l.Add(i, Brain.Deserialize(ms));
                    }
                    training = standalone_proccessor.training;
                    verification = standalone_proccessor.verification;
                    do
                    {
                        Parallel.ForEach(l, i =>
                        {
                            i.Value.Learn(training, verification, i.Key);
                        });
                        var l1 = l.OrderBy(i => i.Value.Prec).Where(i => i.Value.Prec != 0 && !double.IsNaN(i.Value.Prec)).ToDictionary(i => i.Key, i => i.Value);
                        l1 = l1.Take(l1.Count / 2).ToDictionary(i => i.Key, i => i.Value);
                        l = l1;
                        ii++;
                        bwStandalone.ReportProgress(0, $"Iteration {ii}: {l1.Count} ANNs");
                        standalone_proccessor.brain = l.First().Value;
                        if (standalone_proccessor.minprec > standalone_proccessor.brain.Prec)
                        {
                            update_standalone_chart = true;
                        }
                        cont = l.Count > 1;
                    }
                    while (cont);
                }
            }
            catch (Exception ee)
            {
                //Log the exception in some file
                File.AppendAllLines("error.log", new[] { DateTime.Now + ": " + ee.Message });
            }
        }

        private void DoWork_bwSlave(object sender, DoWorkEventArgs e)
        {

        }

        private void DoWork_bwMaster(object sender, DoWorkEventArgs e)
        {
            master_proccessor.SetData(data);
            master_proccessor.PrepareTrainingVerificationSets(listBox1, listBox2, headers);
            bwMaster.ReportProgress(0, "Generating combinations...");
            master_proccessor.GenerateBrains(listBox1, listBox2, listBox4);
            const int bases = 2;
            var tc = int.Parse(textBox1.Text);
            try
            {
                master_proccessor.ProcessBrains(tc, bases);
            }
            catch (Exception ee)
            {
                bwMaster.ReportProgress(0, "Error: " + ee.Message);
                File.AppendAllLines("error.log", new[] { DateTime.Now + ": " + ee.Message, ee.StackTrace });
                MessageBox.Show(ee.StackTrace);
            }
        }

        private void ProgressChanged_bwStandalone(object sender, ProgressChangedEventArgs e)
        {
            listBox8.Items.Insert(0, DateTime.Now.ToString() + ": " + e.UserState);
            label22.Text = standalone_proccessor.minprec.ToString("N10");
        }

        private void AddLayerClick(object sender, EventArgs e)
        {
            listBox7.Items.Add(numericUpDown7.Value + "-" + numericUpDown6.Value);
        }

        private void RemoveLayerClick(object sender, EventArgs e)
        {
            if (listBox7.SelectedItem == null) return;
            listBox7.Items.Remove(listBox7.SelectedItem);
        }

        private void ProgressChanged_bwSlave(object sender, ProgressChangedEventArgs e)
        {

        }

        private void ProgressChanged_bwMaster(object sender, ProgressChangedEventArgs e)
        {
            listBox3.Items.Insert(0, DateTime.Now+": "+e.UserState);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            listBox10.Items.Clear();
            listBox9.Items.Clear();
            listBox7.Items.Clear();
            standalone_proccessor = null;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            bwStandalone.CancelAsync();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog() { Title = "Save as Animated GIF", Filter = "GIF animation|*.gif", FileName = chart1.Legends[0].Title.Replace(":", "_") + " " + DateTime.Now.ToString("yy-MM-dd HH-mm") };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            SaveAnimation(sfd.FileName, images);
            foreach (var item in images)
            {
                item.Dispose();
            }
            images = new List<MemoryStream>();
        }

        private void SaveAnimation(string fileName, List<MemoryStream> images)
        {
            AnimatedGifEncoder e = new AnimatedGifEncoder();
            e.Start(fileName);
            e.SetDelay(1000);
            e.SetRepeat(1);
            foreach (var item in images)
            {
                e.AddFrame(Image.FromStream(item));
            }
            e.Finish();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog() { Title = "Save winner ANN", Filter = "ANN Binary|*.ann", FileName = chart1.Legends[0].Title.Replace(":", "_") + " " + DateTime.Now.ToString("yy-MM-dd HH-mm") };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            var fs = new FileStream(sfd.FileName, FileMode.Create);
            standalone_proccessor.brain.Serialize(fs);
            fs.Close();
        }

        double mspeed;

        private void timer3_Tick(object sender, EventArgs e)
        {
            label8.Text = master_proccessor?.brain?.Prec.ToString("N8");
            label14.Text = mspeed.ToString("N2") + " GHz";
            if (update_master_chart)
            {
                ii++;
                var ann = master_proccessor.brain.Eval(master_proccessor.training, master_proccessor.verification);
                chart2.Series[2].Points.Clear();
                for (int i = 0; i < 2; i++)
                {
                    chart2.Series[i].Points.Clear();
                    for (int j = 0; j < ann.GetLength(0); j++)
                    {
                        chart2.Series[i].Points.Add(new[] { ann[j][i] });
                        if (i == 0) continue;
                        chart2.Series[2].Points.Add(new[] { ann[j][0] - ann[j][1] });
                    }
                }
                chart2.Legends[0].Title = master_proccessor.brain?.ToString();
                chart2.Invalidate();
                var mms = new MemoryStream();
                chart2.SaveImage(mms, ImageFormat.Gif);
                images.Add(mms);
                update_master_chart = false;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            while (slave_proccessor.logs.Any())
                listBox6.Items.Insert(0, slave_proccessor.logs.Dequeue());
        }

        private void button22_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog() { Title = "Save winner ANN", Filter = "ANN Binary|*.ann", FileName = chart1.Legends[0].Title.Replace(":", "_") + " " + DateTime.Now.ToString("yy-MM-dd HH-mm") };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            var fs = new FileStream(sfd.FileName, FileMode.Create);
            master_proccessor.brain.Serialize(fs);
            fs.Close();
        }

        private void bwMaster_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listBox4.Enabled = button8.Enabled = button9.Enabled = button1.Enabled = true;
        }

        /// <summary>
        /// Start the work as Master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //master_proccessor = new BrainsProcessorMaster(bwMaster);
            button1.Enabled = listBox1.Enabled = listBox2.Enabled = listBox3.Enabled = listBox4.Enabled = false;
            bwMaster.RunWorkerAsync();
            timer3.Enabled = true;
        }
    }
}
