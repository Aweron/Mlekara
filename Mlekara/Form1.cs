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
using Code4Bugs.Utils.IO;
using Code4Bugs.Utils.IO.Modbus;
using System.Windows.Forms.DataVisualization.Charting;

namespace Mlekara
{
    public partial class Form1 : Form
    {

        public ICommStream CommStream { get; set; }
        public byte[] ReceivedData { get; set; }

        private Stack<MeasurementModel>[] measurementStacks;
        private int stackSize = 30;

        private List<DeviceModel> devices;
        private List<ProbeModel> probes;

        // Arrays of Form Controls
        GroupBox[] groupBoxes;
        TextBox[] tempDisplays;
        NumericUpDown[] numMins;
        NumericUpDown[] numMaxs;
        NumericUpDown[] numMarkers;
        DateTimePicker[] dateTimePickers;
        NumericUpDown[] numStartHours;
        NumericUpDown[] numHourCounts;
        Chart[] charts;

        public Form1()
        {
            InitializeComponent();

            ReceivedData = new byte[21];

            groupBoxes = new GroupBox[] { groupBox1, groupBox2, groupBox3, groupBox4, groupBox5, groupBox6, groupBox7, groupBox8, groupBox9, groupBox10, groupBox11, groupBox12, groupBox13, groupBox14, groupBox15, groupBox16, groupBox17, groupBox18, groupBox19, groupBox20, groupBox21, groupBox22, groupBox23, groupBox24 };
            tempDisplays = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14, textBox15, textBox16, textBox17, textBox18, textBox19, textBox20, textBox21, textBox22, textBox23, textBox24 };
            numMins = new NumericUpDown[] { numMin1, numMin2, numMin3, numMin4, numMin5, numMin6, numMin7, numMin8, numMin9, numMin10, numMin11, numMin12, numMin13, numMin14, numMin15, numMin16, numMin17, numMin18, numMin19, numMin20, numMin21, numMin22, numMin23, numMin24 };
            numMaxs = new NumericUpDown[] { numMax1, numMax2, numMax3, numMax4, numMax5, numMax6, numMax7, numMax8, numMax9, numMax10, numMax11, numMax12, numMax13, numMax14, numMax15, numMax16, numMax17, numMax18, numMax19, numMax20, numMax21, numMax22, numMax23, numMax24 };
            numMarkers = new NumericUpDown[] { numMarker1, numMarker2, numMarker3, numMarker4, numMarker5, numMarker6, numMarker7, numMarker8, numMarker9, numMarker10, numMarker11, numMarker12, numMarker13, numMarker14, numMarker15, numMarker16, numMarker17, numMarker18, numMarker19, numMarker20, numMarker21, numMarker22, numMarker23, numMarker24 };
            dateTimePickers = new DateTimePicker[] { dateTimePicker1, dateTimePicker2, dateTimePicker3, dateTimePicker4, dateTimePicker5, dateTimePicker6, dateTimePicker7, dateTimePicker8, dateTimePicker9, dateTimePicker10, dateTimePicker11, dateTimePicker12, dateTimePicker13, dateTimePicker14, dateTimePicker15, dateTimePicker16, dateTimePicker17, dateTimePicker18, dateTimePicker19, dateTimePicker20, dateTimePicker21, dateTimePicker22, dateTimePicker23, dateTimePicker24 };
            numStartHours = new NumericUpDown[] { numStartHour1, numStartHour2, numStartHour3, numStartHour4, numStartHour5, numStartHour6, numStartHour7, numStartHour8, numStartHour9, numStartHour10, numStartHour11, numStartHour12, numStartHour13, numStartHour14, numStartHour15, numStartHour16, numStartHour17, numStartHour18, numStartHour19, numStartHour20, numStartHour21, numStartHour22, numStartHour23, numStartHour24 };
            numHourCounts = new NumericUpDown[] { numHourCount1, numHourCount2, numHourCount3, numHourCount4, numHourCount5, numHourCount6, numHourCount7, numHourCount8, numHourCount9, numHourCount10, numHourCount11, numHourCount12, numHourCount13, numHourCount14, numHourCount15, numHourCount16, numHourCount17, numHourCount18, numHourCount19, numHourCount20, numHourCount21, numHourCount22, numHourCount23, numHourCount24 };
            charts = new Chart[] { chart1, chart2, chart3 };


            measurementStacks = new Stack<MeasurementModel>[24];

            devices = SqliteDataAccess.LoadDevices();
            probes = SqliteDataAccess.LoadProbes();
            DisplayData();
            //DisplayGraphs();

            // Open port at startup
            try
            {
                PortSettingsModel portSettingsModel = SqliteDataAccess.LoadPortSettings();
                if (portSettingsModel != null)
                {
                    serialPort1.PortName = portSettingsModel.Port;
                    serialPort1.BaudRate = portSettingsModel.BaudRate;
                    serialPort1.DataBits = portSettingsModel.DataBits;
                    serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), portSettingsModel.StopBits); // StopBits.One
                    serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), portSettingsModel.ParityBits); // Parity.None

                    serialPort1.Open();
                    CommStream = new SerialStream(serialPort1);
                    CommStream.ReadTimeout = 500;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (serialPort1.IsOpen)
                ShowPortConnected(true);
            else
                ShowPortConnected(false);
        }

        public void DisplayData()
        {
            lblCompany.Text = SqliteDataAccess.LoadCompanyName();

            for (int i = 0; i < devices.Count; i++)
            {
                tabTemperature.TabPages[i].Text = devices[i].Name;
                tabGraphics.TabPages[i].Text = devices[i].Name;
            }

            for (int i = 0; i < probes.Count; i++)
            {
                groupBoxes[i].Text = probes[i].Name;
                numMins[i].Value = probes[i].Min;
                numMaxs[i].Value = probes[i].Max;
                numMarkers[i].Value = probes[i].Marker;

                if (probes[i].Active)
                {
                    measurementStacks[i] = new Stack<MeasurementModel>(30);
                    groupBoxes[i].Enabled = true;
                }
                else
                    groupBoxes[i].Enabled = false;
            }
        }

        /*
        public void DisplayGraphs()
        {
            foreach (DeviceModel device in devices)
            {
                if (device.Active)
                {
                    List<ProbeModel> deviceProbes = SqliteDataAccess.LoadProbes(device.Id);
                    foreach (ProbeModel probe in deviceProbes)
                    {
                        // TODO
                        //chart1.Series.Add()
                        List<MeasurementModel> measurements = SqliteDataAccess.LoadMeasurements(probe.Id, DateTime.Now.Date.ToShortDateString(), DateTime.Now.Hour - 2, 2);
                        for (int i = 0; i < measurements.Count; i++)
                            charts[i].Series.
                    }
                }
            }
        }
        */

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PortSettings settings = new PortSettings(serialPort1);
            settings.ShowDialog();

            if (serialPort1.IsOpen)
                ShowPortConnected(true);
        }

        private void onOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                ShowPortConnected(false);
            }
            else
            {
                try
                {
                    serialPort1.Open();
                    CommStream = new SerialStream(serialPort1);
                    CommStream.ReadTimeout = 500;
                    ShowPortConnected(true);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // System Time
            lblTime.Text = DateTime.Now.ToLongTimeString();

            // Sending a Request for Data
            if (serialPort1.IsOpen)
            {
                try
                {
                    for (int i = 0; i < devices.Count; i++)
                        if (devices[i].Active)
                        {
                            ReceivedData = CommStream.RequestFunc3(i + 1, 0, 8);
                            ShowData(i);
                        }
                }
                catch (Exception err)
                {
                    timer1.Stop();
                    btnTimerRestart.Visible = true;
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowData(int slaveNo)
        {
            byte[] data = ReceivedData;

            double higherValue;
            double lowerValue;
            double value;
            string text;

            for (int i = 3; i < 19; i += 2) 
            {
                higherValue = Convert.ToInt32(data[i]) * 25.5f; // Higher byte of temp value
                lowerValue = Convert.ToSingle(data[i+1]) / 10f; // Lower byte of temp value
                value = higherValue + lowerValue;

                int j = (slaveNo * 8) + ((i - 3) / 2); // Probe ID

                MeasurementModel measurement = new MeasurementModel
                {
                    ProbeId = j + 1,
                    Value = value,
                    Date = DateTime.Now.Date.ToString(),
                    Hour = DateTime.Now.Hour,
                    Minute = DateTime.Now.Minute,
                    Second = DateTime.Now.Second
                };

                text = value.ToString();
                if (value % 1 == 0)
                    text += ".0";

                
                if (probes[j].Active)
                {
                    tempDisplays[j].Text = text;
                    measurementStacks[j].Push(measurement);

                    // Check if stacks are full to approximate and save to DB
                    if (measurementStacks[j].Count == stackSize)
                        SaveApproximateTemp(j);
                }
            }
        }

        private void SaveApproximateTemp(int j)
        {
            MeasurementModel measurement = new MeasurementModel
            {
                ProbeId = j + 1,
                Value = 0,
                Date = DateTime.Now.Date.ToShortDateString(),
                Hour = DateTime.Now.Hour,
                Minute = DateTime.Now.Minute,
                Second = DateTime.Now.Second
            };

            foreach (MeasurementModel m in measurementStacks[j])
            {
                measurement.Value += m.Value;
            }
            measurement.Value = Math.Round(measurement.Value / stackSize, 1);
            SqliteDataAccess.SaveMeasurement(measurement);

            measurementStacks[j].Clear();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AdminSettings settings = new AdminSettings();
            settings.ShowDialog();
        }

        public void ShowPortConnected(bool isConnected)
        {
            if (isConnected)
            {
                lblConnected.Text = "Connected";
                lblConnected.ForeColor = Color.Green;
                onOffToolStripMenuItem.Checked = true;
            }
            else
            {
                lblConnected.Text = "Disconnected";
                lblConnected.ForeColor = Color.Red;
                onOffToolStripMenuItem.Checked = false;
            }
        }

        private void btnTimerRestart_Click(object sender, EventArgs e)
        {
            timer1.Start();
            btnTimerRestart.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommStream?.Dispose();
        }

        private void btnShow1_Click(object sender, EventArgs e)
        {
            Graph graph = new Graph(AssembleGraphDataModel(1));
            graph.ShowDialog();
        }

        private GraphDataModel AssembleGraphDataModel(int i)
        {
            ProbeModel probe = new ProbeModel
            {
                Id = i,
                DeviceId = ((i - 1) / 8) + 1,
                Name = groupBoxes[i - 1].Text,
                Active = true,
                Min = Convert.ToInt32(numMins[i - 1].Value),
                Max = Convert.ToInt32(numMaxs[i - 1].Value),
                Marker = Convert.ToInt32(numMarkers[i - 1].Value)
            };

            return new GraphDataModel
            {
                Probe = probe,
                Date = dateTimePickers[i - 1].Value.ToShortDateString(),
                StartHour = Convert.ToInt32(numStartHours[i - 1].Value),
                HourCount = Convert.ToInt32(numHourCounts[i - 1].Value)
            };
        }
    }
}
