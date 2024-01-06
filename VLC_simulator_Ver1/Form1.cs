using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using InputSignal;

using System.Net;
using System.Net.Sockets;

namespace VLC_simulator_Ver1
{
    public partial class Form1 : Form
    {
        //List<int> list = new List<int>();
        Timer t = new Timer();
        Timer mattimer = new Timer();
        List<string> RealConvertData = new List<string>();
        int[] list = new int[20];
        byte[] temp = new byte[20000];
        List<string> convertStringList = new List<string>();
        static string inputsignal = string.Empty;
        static string sendsignal = string.Empty;
        System.Threading.ThreadStart ts = new System.Threading.ThreadStart(matlabMethod);       
        static InputSignal.InputSignal aa = new InputSignal.InputSignal();
        System.Threading.Thread th1;
        
        public Form1()
        {
            InitializeComponent();

            SetMatTimer();
            t.Start();
            t.Tick += new EventHandler(t_Tick);
            t.Interval = 3000;   
        }

        private void SetMatTimer()
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Step = 11;
            progressBar1.Value = 0;

            mattimer.Tick += new EventHandler(mattimer_Tick);
            mattimer.Interval = 100;
            if (progressBar1.Value == 100)
            {
                mattimer.Stop();
            }
        }

        void mattimer_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
        }

        void t_Tick(object sender, EventArgs e)
        {            
            aa.Dispose();            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                SetMatTimer();
                string txtPath;

                OpenFileDialog dlg = new OpenFileDialog();

                dlg.DefaultExt = "txt.*";
                dlg.Filter = "Text Files(.txt)|*.txt";
                dlg.ShowDialog();
                txtPath = dlg.FileName;
     
                temp = System.IO.File.ReadAllBytes(dlg.FileName);
                
                for (int i = 0; i < temp.Count(); i++)
                {
                    string convertstring = Convert.ToString(temp[i], 2);
                    if (int.Parse(convertstring) < 10000000)
                    {
                        string t = "0" + convertstring;
                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }
                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        convertstring = t;
                    }
                    convertStringList.Add(convertstring);
                    for (int j = 0; j < convertstring.Length; j++)
                    {
                        RealConvertData.Add(convertstring[j].ToString());
                    }
                }

                for (int i = 0; i < RealConvertData.Count; i = i+4)
                {
                    Convert4B6B(i);                   
                }
            }
            catch { }

            th1 = new System.Threading.Thread(ts);
            th1.Start();
            mattimer.Start();
            SendFileToClient();
        }

        private void SendFileToClient()
        {
            try
            {
                IPAddress ip = IPAddress.Parse("210.119.32.84");
                IPEndPoint endPoint = new IPEndPoint(ip, 8000);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(endPoint);
                string sendstring = sendsignal;
                byte[] sendbuffer = Encoding.UTF8.GetBytes(sendstring);

                socket.Send(sendbuffer);
                socket.Close();
            }
            catch { }
        }

        private void Convert4B6B(int i)
        {
            string tempstr = string.Empty;
            tempstr = RealConvertData[i];
            tempstr += RealConvertData[i + 1];
            tempstr += RealConvertData[i + 2];
            tempstr += RealConvertData[i + 3];

            Set4Bto6B(tempstr);
        }

        private void Set4Bto6B(string tempstr)
        {
            if (inputsignal == string.Empty)
            {
                inputsignal += "[";
            }
            switch (tempstr)
            {
                case "0000": inputsignal += "0,0,1,1,1,0,";
                                    sendsignal+= "001110";
                                    ListBoxAddOrigin("0000");
                                    ListBoxAdd4B6B("001110");        
                                    break;
                case "0001": inputsignal += "0,0,1,1,0,1,";
                                    sendsignal+= "001101";         
                                    ListBoxAddOrigin("0001");
                                    ListBoxAdd4B6B("001101");  
                                    break;
                case "0010": inputsignal += "0,1,0,0,1,1,"; 
                                    sendsignal+= "010011";
                                    ListBoxAddOrigin("0010");
                                    ListBoxAdd4B6B("010011");  
                                    break;
                case "0011": inputsignal += "0,1,0,1,1,0,";
                                    sendsignal+= "010110";
                                    ListBoxAddOrigin("0011");
                                    ListBoxAdd4B6B("010110");  
                                    break;
                case "0100": inputsignal += "0,1,0,1,0,1,"; 
                                    sendsignal+= "010101";
                                    ListBoxAddOrigin("0100");
                                    ListBoxAdd4B6B("010101");  
                                    break;
                case "0101": inputsignal += "1,0,0,0,1,1,"; 
                                    sendsignal+= "100011";
                                    ListBoxAddOrigin("0101");
                                    ListBoxAdd4B6B("100011");  
                                    break;
                case "0110": inputsignal += "1,0,0,1,1,0,"; 
                                    sendsignal+= "100110";
                                    ListBoxAddOrigin("0110");
                                    ListBoxAdd4B6B("100110");  
                                    break;
                case "0111": inputsignal += "1,0,0,1,0,1,"; 
                                    sendsignal+= "100101";
                                    ListBoxAddOrigin("0111");
                                    ListBoxAdd4B6B("100101");  
                                    break;

                case "1000": inputsignal += "0,1,1,0,0,1,"; 
                                    sendsignal+= "011001";
                                    ListBoxAddOrigin("1000");
                                    ListBoxAdd4B6B("011001");  
                                    break;
                case "1001": inputsignal += "0,1,1,0,1,0,"; 
                                    sendsignal+= "011010";
                                    ListBoxAddOrigin("1001");
                                    ListBoxAdd4B6B("011010");  
                                    break;
                case "1010": inputsignal += "0,1,1,1,0,0,"; 
                                    sendsignal+= "011100";
                                    ListBoxAddOrigin("1010");
                                    ListBoxAdd4B6B("011100");  
                                    break;
                case "1011": inputsignal += "1,1,0,0,0,1,"; 
                                    sendsignal+= "110001";
                                    ListBoxAddOrigin("1011");
                                    ListBoxAdd4B6B("110001");  
                                    break;
                case "1100": inputsignal += "1,1,0,0,1,0,"; 
                                    sendsignal+= "110010";
                                    ListBoxAddOrigin("1100");
                                    ListBoxAdd4B6B("110010");  
                                    break;
                case "1101": inputsignal += "1,0,1,0,0,1,"; 
                                    sendsignal+= "101001";
                                    ListBoxAddOrigin("1101");
                                    ListBoxAdd4B6B("101001");  
                                    break;
                case "1110": inputsignal += "1,0,1,0,1,0,"; 
                                    sendsignal+= "101010";
                                    ListBoxAddOrigin("1110");
                                    ListBoxAdd4B6B("101010");  
                                    break;
                case "1111": inputsignal += "1,0,1,1,0,0,";
                                   sendsignal+= "101100";
                                   ListBoxAddOrigin("1111");
                                   ListBoxAdd4B6B("101100");  
                                    break;
                                    
                default: break;
            }
        }

        private void ListBoxAdd4B6B(string p)
        {            
            listBox4B6B.Items.Add(p);
        }

        private void ListBoxAddOrigin(string p)
        {
            listBoxOrigin.Items.Add(p);
        }

        static void matlabMethod()
        {
            try
            {                
                inputsignal = inputsignal.Remove(inputsignal.Length - 1);
                inputsignal += "]";
                MWArray array = 1;
                MWArray number = inputsignal.Length;
                MWArray mwlist = inputsignal;
                aa.VLC(mwlist, array, number);                
            }
            catch { }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //th1.Abort();                      
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SetMatTimer();
                
                string txtPath;

                OpenFileDialog dlg = new OpenFileDialog();

                dlg.DefaultExt = "txt.*";
                dlg.Filter = "Text Files(.txt)|*.txt";
                dlg.ShowDialog();
                txtPath = dlg.FileName;
                listBoxOrigin.Items.Clear();
                listBox4B6B.Items.Clear();
                temp = System.IO.File.ReadAllBytes(dlg.FileName);

                for (int i = 0; i < temp.Count(); i++)
                {
                    string convertstring = Convert.ToString(temp[i], 2);
                    if (int.Parse(convertstring) < 10000000)
                    {
                        string t = "0" + convertstring;
                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }
                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        convertstring = t;
                    }
                    convertStringList.Add(convertstring);
                    for (int j = 0; j < convertstring.Length; j++)
                    {
                        RealConvertData.Add(convertstring[j].ToString());
                    }
                }

                for (int i = 0; i < RealConvertData.Count; i = i + 4)
                {
                    Convert4B6B(i);
                }
            }
            catch { }

            th1 = new System.Threading.Thread(ts);
            th1.Start();
            mattimer.Start();
            SendFileToClient();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("asd");
        }
    }
}
