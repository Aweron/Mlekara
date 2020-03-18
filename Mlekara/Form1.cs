using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Mlekara
{
    public partial class Form1 : Form
    {
        public byte[] SendData { get; set; }
        public byte[] ReceivedData { get; set; }

        public Form1()
        {
            InitializeComponent();

            ReceivedData = new byte[21];

            // Open port at startup (TODO: Take values from file or db instead of hardcoded)
            try
            {
                serialPort1.PortName = "COM14";
                serialPort1.BaudRate = 9600;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;

                serialPort1.Open();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!serialPort1.IsOpen)
                {
                    lblConnected.Text = "Disconnected";
                    lblConnected.ForeColor = Color.Red;
                    onOffToolStripMenuItem.Checked = false;
                }
            }
            finally
            {
                if (serialPort1.IsOpen)
                {
                    lblConnected.Text = "Connected";
                    lblConnected.ForeColor = Color.Green;
                    onOffToolStripMenuItem.Checked = true;
                }
            }

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PortSettings settings = new PortSettings(serialPort1);
            settings.ShowDialog();
        }

        private void onOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                lblConnected.Text = "Disconnected";
                lblConnected.ForeColor = Color.Red;
                onOffToolStripMenuItem.Checked = false;
            }
            else
            {
                serialPort1.Open();
                lblConnected.Text = "Connected";
                lblConnected.ForeColor = Color.Green;
                onOffToolStripMenuItem.Checked = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // System Time
            int hours = DateTime.Now.Hour;
            int minutes = DateTime.Now.Minute;
            int seconds = DateTime.Now.Second;
            lblTime.Text = hours + ":" + minutes + ":" + seconds;

            // Sending a Request for Data
            if (serialPort1.IsOpen)
            {
                SendData = StringToByteArray("010300000008440C");
                serialPort1.Write(SendData, 0, 8);
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            serialPort1.Read(ReceivedData, 0, 21);
            this.Invoke(new EventHandler(ShowData));
            
        }

        private void ShowData(object sender, EventArgs e)
        {
            byte[] data = ReceivedData;

            float higherValue;
            float lowerValue;
            float value;
            string text;

            for (int i = 3; i < 19; i += 2) 
            {
                higherValue = Convert.ToInt32(data[i]) * 25.5f; // Higher byte of temp value
                lowerValue = Convert.ToSingle(data[i+1]) / 10f; // Lower byte of temp value
                value = higherValue + lowerValue;
                text = value.ToString();
                if (value % 1 == 0)
                    text += ".0";

                switch (i) // Displaying temperature for each sensor
                {
                    case 3:
                        textBox1.Text = text;
                        break;
                    case 5:
                        textBox2.Text = text;
                        break;
                    case 7:
                        textBox3.Text = text;
                        break;
                    case 9:
                        textBox4.Text = text;
                        break;
                    case 11:
                        textBox5.Text = text;
                        break;
                    case 13:
                        textBox6.Text = text;
                        break;
                    case 15:
                        textBox7.Text = text;
                        break;
                    case 17:
                        textBox8.Text = text;
                        break;
                    default:
                        break;
                }
            }
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        // Unused so far.
        public static string ByteArrayToString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AdminSettings settings = new AdminSettings();
            settings.ShowDialog();
        }
    }
}
