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
using Mlekara.Models;

namespace Mlekara
{
    public partial class PortSettings : Form
    {
        private SerialPort port;

        public PortSettings(SerialPort serialPort1)
        {
            InitializeComponent();

            port = serialPort1;

            string[] ports = SerialPort.GetPortNames();
            cmbPort.Items.AddRange(ports);

            if (port.IsOpen)
            {
                cmbPort.Text = port.PortName;
                cmbBaudRate.Text = port.BaudRate.ToString();
                cmbDataBits.Text = port.DataBits.ToString();
                cmbStopBits.Text = port.StopBits.ToString();
                cmbParityBits.Text = port.Parity.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                }
                port.PortName = cmbPort.Text;
                port.BaudRate = Convert.ToInt32(cmbBaudRate.Text);
                port.DataBits = Convert.ToInt32(cmbDataBits.Text);
                port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmbStopBits.Text);
                port.Parity = (Parity)Enum.Parse(typeof(Parity), cmbParityBits.Text);

                port.Open();

                PortSettingsModel portSettingsModel = new PortSettingsModel(cmbPort.Text, Convert.ToInt32(cmbBaudRate.Text), Convert.ToInt32(cmbDataBits.Text), cmbStopBits.Text, cmbParityBits.Text);
                SqliteDataAccess.SavePortSettings(portSettingsModel);

                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
