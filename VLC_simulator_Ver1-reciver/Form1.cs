using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;
namespace VLC_simulator_Ver1_reciver
{
    public partial class Form1 : Form
    {
        static string resultsignal = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "수신 대기";
                IPAddress ip = IPAddress.Parse("210.119.32.84");

                IPEndPoint endPoint = new IPEndPoint(ip, 8000);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(endPoint);
                socket.Listen(100);
                Socket clientSocket = socket.Accept();

                byte[] reciveBuffer = new byte[8000];
                int lenght = clientSocket.Receive(reciveBuffer, reciveBuffer.Length, SocketFlags.None);
                string result = Encoding.UTF8.GetString(reciveBuffer, 0, lenght);

                //MessageBox.Show("수신완료");
                label1.Text = "수신 완료";
                clientSocket.Close();
                socket.Close();


                TranslateResult(result);
                finalTranslate(resultsignal);                
            }
            catch { }
        }

        private void finalTranslate(string result)
        {
            byte[] resultbyte = new byte[10000];
            string temp = string.Empty;
            for (int i = 0; i < result.Length - 1; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i * 8 + 8 < result.Length - 1)
                    {
                        temp += result[i * 8 + j];
                    }
                }
                if (temp != "" && i < resultbyte.Count() - 1)
                {
                    resultbyte[i] = byte.Parse(Convert.ToInt16(temp, 2).ToString());
                    temp = string.Empty;
                }
                if (i > resultbyte.Count())
                {
                    break;
                }
            }
            string txtPath;
            SaveFileDialog slg = new SaveFileDialog();
            slg.DefaultExt = "txt.*";
            slg.Filter = "Text Files(.txt)|*.txt";
            slg.ShowDialog();
            txtPath = slg.FileName;

            System.IO.File.WriteAllBytes(txtPath, resultbyte);
        }

        private void TranslateResult(string result)
        {
            string temp = string.Empty;
            for (int i = 0; i < result.Length - 1; i = i+6)
            {
                temp = string.Empty;
                for (int k = 0; k < 6; k++)
                {
                    temp += result[(i ) + k];
                }
                switch (temp)
                {
                    case "001110": resultsignal += "0000";
                        break;
                    case "001101": resultsignal += "0001";
                        break;
                    case "010011": resultsignal += "0010";
                        break;
                    case "010110": resultsignal += "0011";
                        break;
                    case "010101": resultsignal += "0100";
                        break;
                    case "100011": resultsignal += "0101";
                        break;
                    case "100110": resultsignal += "0110";
                        break;
                    case "100101": resultsignal += "0111";
                        break;

                    case "011001": resultsignal += "1000";
                        break;
                    case "011010": resultsignal += "1001";
                        break;
                    case "011100": resultsignal += "1010";
                        break;
                    case "110001": resultsignal += "1011,";
                        break;
                    case "110010": resultsignal += "1100";
                        break;
                    case "101001": resultsignal += "1101";
                        break;
                    case "101010": resultsignal += "1110";
                        break;
                    case "101100": resultsignal += "1111";
                        break;

                    default: break;
                }
                
            }
        }
    }
}
