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
        public Form1()
        {
            InitializeComponent();

            // Open port at startup
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
            
            // TODO: Refresh temps
        }
    }
}
