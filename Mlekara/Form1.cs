using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using Mlekara.Models;
using Code4Bugs.Utils.IO;
using Code4Bugs.Utils.IO.Modbus;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Text;
using System.Drawing.Printing;

namespace Mlekara
{
    public partial class Form1 : Form
    {
        public ICommStream CommStream { get; set; }
        public byte[] ReceivedData { get; set; }

        private bool autoRefresh;

        private Stack<MeasurementModel>[] measurementStacks;
        private int stackSize;

        private List<DeviceModel> devices;
        private List<ProbeModel> probes;
        private DefaultsModel defaults;

        private List<GroupBox> grpAll;
        private List<TextBox> txtAll;

        // Arrays of Form Controls
        public GroupBox[] groupBoxes;
        public TextBox[] tempDisplays;
        public NumericUpDown[] numMins;
        public NumericUpDown[] numMaxs;
        public NumericUpDown[] numMarkers;
        public DateTimePicker[] dateTimePickers;
        public NumericUpDown[] numStartHours;
        public NumericUpDown[] numHourCounts;
        public Label[] lblGraphs;
        public Button[] btnShowGraphics;

        public Font font;
        public FontFamily fontFamily;

        public Form1()
        {
            InitializeComponent();

            ReceivedData = new byte[21];

            autoRefresh = false;

            measurementStacks = new Stack<MeasurementModel>[24];

            grpAll = new List<GroupBox>();
            txtAll = new List<TextBox>();

            groupBoxes = new GroupBox[] { groupBox1, groupBox2, groupBox3, groupBox4, groupBox5, groupBox6, groupBox7, groupBox8, groupBox9, groupBox10, groupBox11, groupBox12, groupBox13, groupBox14, groupBox15, groupBox16, groupBox17, groupBox18, groupBox19, groupBox20, groupBox21, groupBox22, groupBox23, groupBox24 };
            tempDisplays = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14, textBox15, textBox16, textBox17, textBox18, textBox19, textBox20, textBox21, textBox22, textBox23, textBox24 };
            numMins = new NumericUpDown[] { numMin1, numMin2, numMin3, numMin4, numMin5, numMin6, numMin7, numMin8, numMin9, numMin10, numMin11, numMin12, numMin13, numMin14, numMin15, numMin16, numMin17, numMin18, numMin19, numMin20, numMin21, numMin22, numMin23, numMin24 };
            numMaxs = new NumericUpDown[] { numMax1, numMax2, numMax3, numMax4, numMax5, numMax6, numMax7, numMax8, numMax9, numMax10, numMax11, numMax12, numMax13, numMax14, numMax15, numMax16, numMax17, numMax18, numMax19, numMax20, numMax21, numMax22, numMax23, numMax24 };
            numMarkers = new NumericUpDown[] { numMarker1, numMarker2, numMarker3, numMarker4, numMarker5, numMarker6, numMarker7, numMarker8, numMarker9, numMarker10, numMarker11, numMarker12, numMarker13, numMarker14, numMarker15, numMarker16, numMarker17, numMarker18, numMarker19, numMarker20, numMarker21, numMarker22, numMarker23, numMarker24 };
            dateTimePickers = new DateTimePicker[] { dateTimePicker1, dateTimePicker2, dateTimePicker3, dateTimePicker4, dateTimePicker5, dateTimePicker6, dateTimePicker7, dateTimePicker8, dateTimePicker9, dateTimePicker10, dateTimePicker11, dateTimePicker12, dateTimePicker13, dateTimePicker14, dateTimePicker15, dateTimePicker16, dateTimePicker17, dateTimePicker18, dateTimePicker19, dateTimePicker20, dateTimePicker21, dateTimePicker22, dateTimePicker23, dateTimePicker24 };
            numStartHours = new NumericUpDown[] { numStartHour1, numStartHour2, numStartHour3, numStartHour4, numStartHour5, numStartHour6, numStartHour7, numStartHour8, numStartHour9, numStartHour10, numStartHour11, numStartHour12, numStartHour13, numStartHour14, numStartHour15, numStartHour16, numStartHour17, numStartHour18, numStartHour19, numStartHour20, numStartHour21, numStartHour22, numStartHour23, numStartHour24 };
            numHourCounts = new NumericUpDown[] { numHourCount1, numHourCount2, numHourCount3, numHourCount4, numHourCount5, numHourCount6, numHourCount7, numHourCount8, numHourCount9, numHourCount10, numHourCount11, numHourCount12, numHourCount13, numHourCount14, numHourCount15, numHourCount16, numHourCount17, numHourCount18, numHourCount19, numHourCount20, numHourCount21, numHourCount22, numHourCount23, numHourCount24 };
            lblGraphs = new Label[] { lblGraph1, lblGraph2, lblGraph3 };
            btnShowGraphics = new Button[] { btnShowGraphic1, btnShowGraphic2, btnShowGraphic3 };

            // Font
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(@".\digital-7.ttf");
            fontFamily = new FontFamily("digital-7", pfc);
            font = new Font(fontFamily, 36);
            for (int i = 0; i < tempDisplays.Length; i++)
                tempDisplays[i].Font = font;

            DisplayData();

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
                    CommStream.ReadTimeout = 900;
                }

            }
            catch { }
            /*
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
            ShowPortConnected(serialPort1.IsOpen); // If port is open, show port connected; else show port disconnected.
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!HasPermission())
            {
                PasswordCheck passwordCheck = new PasswordCheck();
                if (passwordCheck.ShowDialog() == DialogResult.OK)
                {
                    WriteFilePermission();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        public bool HasPermission()
        {
            //FileStream fileStream;
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TempCheckerPermission.txt");

            if (File.Exists(fileName))
            {
                FileStream fileStream = File.OpenRead(fileName);
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    string data = streamReader.ReadLine();
                    fileStream.Close();

                    if (data == "True")
                        return true;
                }
            }
            return false;
        }

        public void WriteFilePermission()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TempCheckerPermission.txt");

            FileStream fileStream = File.Create(fileName);
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(true);
            }

        }

        #region User Interface Drawing

        public void DisplayData()
        {
            devices = SqliteDataAccess.LoadDevices();
            probes = SqliteDataAccess.LoadProbes();
            defaults = SqliteDataAccess.LoadDefaults();

            lblCompany.Text = SqliteDataAccess.LoadCompanyName();

            ArrangeTempDisplays();

            // Device Settings
            for (int i = 0; i < devices.Count; i++)
            {
                tabTemperature.TabPages[i].Text = devices[i].Name;
                lblGraphs[i].Text = devices[i].Name;
                btnShowGraphics[i].Enabled = devices[i].Active; // If device is active, enable graphic button; else disable it.
            }

            // Probe Settings
            for (int i = 0; i < probes.Count; i++)
            {
                groupBoxes[i].Text = probes[i].Name;
                numMins[i].Value = probes[i].Min;
                numMaxs[i].Value = probes[i].Max;
                numMarkers[i].Value = probes[i].Marker;

                //tempDisplays[i].Font = font;

                if (probes[i].Active)
                {
                    if (measurementStacks[i] == null)
                        measurementStacks[i] = new Stack<MeasurementModel>(30);
                    groupBoxes[i].Enabled = true;
                }
                else
                    groupBoxes[i].Enabled = false;
            }

            // Graph Defaults
            numMinGraph.Value = defaults.GraphMin;
            numMaxGraph.Value = defaults.GraphMax;
            stackSize = defaults.StackSize;
            timer2.Interval = stackSize * 1000;
        }

        public void ShowPortConnected(bool isConnected)
        {
            if (isConnected)
            {
                lblConnected.Text = "Connected";
                lblConnected.ForeColor = Color.Green;
                onOffToolStripMenuItem.Checked = true;
                btnRestartConnection.Visible = false;
            }
            else
            {
                lblConnected.Text = "Disconnected";
                lblConnected.ForeColor = Color.Red;
                onOffToolStripMenuItem.Checked = false;
                btnRestartConnection.Visible = true;
            }
        }

        private void btnRestartConnection_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Open();
                CommStream = new SerialStream(serialPort1);
                CommStream.ReadTimeout = 900;
                ShowPortConnected(true);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public GroupBox CreateTempDisplay(int probeID, string probeName)
        {
            GroupBox groupBox = new GroupBox();

            groupBox.Name = "grpAll" + probeID;

            groupBox.Text = probeName;
            groupBox.Size = new Size(204, 80);

            TextBox textDisplay = new TextBox();
            textDisplay.Parent = groupBox;

            textDisplay.Name = "txtAll" + probeID;

            textDisplay.BackColor = Color.Black;
            textDisplay.Font = font;
            textDisplay.ForeColor = Color.Lime;
            textDisplay.Text = "00.0";
            textDisplay.TextAlign = HorizontalAlignment.Center;
            textDisplay.ReadOnly = true;
            textDisplay.Location = new Point(6, 19);
            textDisplay.Size = new Size(192, 55);

            txtAll.Add(textDisplay);

            Label c = new Label();
            c.Parent = groupBox;

            c.BackColor = Color.Black;
            c.Font = new Font("Verdana", 22);
            c.ForeColor = Color.Lime;
            c.Text = "°C";
            c.AutoSize = true;
            c.Location = new Point(141, 33);
            c.Size = new Size(51, 35);
            c.BringToFront();

            return groupBox;
        }

        public void ArrangeTempDisplays()
        {
            foreach (GroupBox grp in grpAll)
            {
                tabPage4.Controls.Remove(grp);
                grp.Dispose();
            }

            grpAll.Clear();
            txtAll.Clear();

            int col = 0;
            int row = 0;

            int x;
            int y;

            foreach (ProbeModel probe in probes)
            {
                if (probe.Active)
                {
                    x = 6 + 210 * col;
                    y = 6 + 86 * row;

                    GroupBox groupBox = CreateTempDisplay(probe.Id, probe.Name);

                    groupBox.Parent = tabPage4;
                    groupBox.Location = new Point(x, y);

                    grpAll.Add(groupBox);

                    if (x + 420 > tabPage4.Width) // Blaze it
                    {
                        row++;
                        col = 0;
                    }
                    else col++;
                }
            }
        }

        #endregion

        #region ToolStripMenuItems

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
                ShowPortConnected(false);
            }
            else
            {
                try
                {
                    serialPort1.Open();
                    CommStream = new SerialStream(serialPort1);
                    CommStream.ReadTimeout = 900;
                    ShowPortConnected(true);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PasswordCheck passwordCheck = new PasswordCheck();
            if (passwordCheck.ShowDialog() == DialogResult.OK)
            {
                AdminSettings settings = new AdminSettings();
                settings.ShowDialog();
            }

            DisplayData();
        }

        #endregion

        #region Timer and Data Request

        private void ShowData(int slaveNo)
        {
            byte[] data = ReceivedData;

            double higherValue;
            double lowerValue;
            double value; // Temp as double
            string text; // Temp to be displayed in string format

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

                    TextBox textBox = txtAll.Find(x => x.Name == "txtAll" + j);
                    textBox.Text = text;

                    // Check if stacks are full to approximate and save to DB
                    if (measurementStacks[j].Count >= stackSize)
                    {
                        SaveApproximateTemp(j);
                    }
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
                    ShowPortConnected(serialPort1.IsOpen);
                }
            }
            
            ShowPortConnected(serialPort1.IsOpen);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Live graph ticks
            if (autoRefresh)
                ShowGraph(Convert.ToInt32(cmbLiveGraphDevices.Text) - 1, DateTime.Now.Date, DateTime.Now.TimeOfDay, 4);
            
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            // Date check (00:00)
            if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                UpdateDates();
        }

        private void UpdateDates()
        {
            for (int i = 0; i < dateTimePickers.Length; i++)
                dateTimePickers[i].Value = DateTime.Now;
            dateTimeGraph.Value = DateTime.Now;
        }

        private void btnTimerRestart_Click(object sender, EventArgs e)
        {
            timer1.Start();
            btnTimerRestart.Visible = false;
        }

        #endregion

        #region Graph Tab

        public void ShowGraph(int deviceNum, DateTime date, int startHour, int hourCount)
        {
            chart1.Series.Clear();

            chart1.Titles["Naziv"].Text = lblCompany.Text;
            chart1.Titles["Device"].Text = tabTemperature.TabPages[deviceNum].Text;
            chart1.Titles["Datum"].Text = "Datum: " + date.ToShortDateString(); //dateTimeGraph.Value.ToShortDateString();
            chart1.Titles["Vreme"].Text = "Vreme: " + startHour + ":00 - " + (startHour + hourCount) + ":00";

            // X axis range
            int endHour = Convert.ToInt32(startHour + hourCount);

            DateTime min = Convert.ToDateTime(date.ToShortDateString() + " " + startHour + ":00");
            DateTime max = Convert.ToDateTime(date.ToShortDateString() + " " + (endHour - 1) + ":59");

            chart1.ChartAreas[0].AxisX.Minimum = min.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = max.ToOADate();

            // Y axis range
            chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(numMinGraph.Value);
            chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(numMaxGraph.Value);

            chart1.Series.Add("Timestamps");
            chart1.Series["Timestamps"].IsVisibleInLegend = false;
            chart1.Series["Timestamps"].XValueType = ChartValueType.Time;
            chart1.Series["Timestamps"].Points.AddXY(min, 0);
            chart1.Series["Timestamps"].Points.AddXY(max, 0);

            // Marker line
            //chart1.Series["Marker"].XValueType = ChartValueType.DateTime;

            //chart1.Series["Marker"].Points.AddXY(min, graphData.Probe.Marker);
            //chart1.Series["Marker"].Points.AddXY(max, graphData.Probe.Marker);

            // Measurements X axis value
            List<ProbeModel> deviceProbes = SqliteDataAccess.LoadProbes(deviceNum + 1);

            foreach (ProbeModel probeModel in deviceProbes)
            {
                if (probeModel.Active)
                {
                    chart1.Series.Add(probeModel.Id.ToString());
                    chart1.Series[probeModel.Id.ToString()].ChartType = SeriesChartType.Line;
                    chart1.Series[probeModel.Id.ToString()].Enabled = true;
                    chart1.Series[probeModel.Id.ToString()].IsVisibleInLegend = true;
                    chart1.Series[probeModel.Id.ToString()].BorderWidth = 2;

                    chart1.Series[probeModel.Id.ToString()].XValueType = ChartValueType.DateTime;

                    chart1.Series[probeModel.Id.ToString()].LegendText = probeModel.Name;

                    List<MeasurementModel> measurements = SqliteDataAccess.LoadMeasurements(probeModel.Id, date.ToShortDateString(), startHour, hourCount);

                    foreach (MeasurementModel measurement in measurements)
                    {
                        DateTime time = Convert.ToDateTime(measurement.Date + " " + measurement.Hour + ":" + measurement.Minute + ":" + measurement.Second);
                        chart1.Series[probeModel.Id.ToString()].Points.AddXY(time, measurement.Value);
                    }
                }
            }
        }

        public void ShowGraph(int deviceNum, DateTime date, TimeSpan _time, int hourCount)
        {
            chart1.Series.Clear();

            TimeSpan time;
            if (_time.Hours >= 2)
            {
                time = _time.Subtract(new TimeSpan(2, 0, 0));
            }
            else if (_time.Hours >= 1)
            {
                time = _time.Subtract(new TimeSpan(1, 0, 0));

            }
            else
            {
                time = _time.Subtract(_time);
            }

            TimeSpan endTime;
            if (time.Hours + hourCount < 24)
                endTime = time.Add(new TimeSpan(hourCount, 0, 0));
            else
                endTime = new TimeSpan(23, 59, 59);


            chart1.Titles["Naziv"].Text = lblCompany.Text;
            chart1.Titles["Device"].Text = tabTemperature.TabPages[deviceNum].Text;
            chart1.Titles["Datum"].Text = "Datum: " + date.ToShortDateString(); //dateTimeGraph.Value.ToShortDateString();
            chart1.Titles["Vreme"].Text = "Vreme: " + time.ToString("hh':'mm") + " - " + endTime.ToString("hh':'mm");
            //chart1.Titles["Vreme"].Text = "Vreme: " + time.Hours + ":" + time.Minutes + " - " + endTime.Hours + ":" + time.Minutes;

            // X axis range
            int endHour = Convert.ToInt32(time.Hours + hourCount);

            DateTime min = Convert.ToDateTime(date.ToShortDateString() + " " + time);
            DateTime max = Convert.ToDateTime(date.ToShortDateString() + " " + endTime);


            chart1.ChartAreas[0].AxisX.Minimum = min.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = max.ToOADate();

            // Y axis range
            chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(numMinGraph.Value);
            chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(numMaxGraph.Value);

            chart1.Series.Add("Timestamps");
            chart1.Series["Timestamps"].IsVisibleInLegend = false;
            chart1.Series["Timestamps"].XValueType = ChartValueType.Time;
            chart1.Series["Timestamps"].Points.AddXY(min, 0);
            chart1.Series["Timestamps"].Points.AddXY(max, 0);

            // Marker line
            //chart1.Series["Marker"].XValueType = ChartValueType.DateTime;

            //chart1.Series["Marker"].Points.AddXY(min, graphData.Probe.Marker);
            //chart1.Series["Marker"].Points.AddXY(max, graphData.Probe.Marker);

            // Measurements X axis value
            List<ProbeModel> deviceProbes = SqliteDataAccess.LoadProbes(deviceNum + 1);

            foreach (ProbeModel probeModel in deviceProbes)
            {
                if (probeModel.Active)
                {
                    chart1.Series.Add(probeModel.Id.ToString());
                    chart1.Series[probeModel.Id.ToString()].ChartType = SeriesChartType.Line;
                    chart1.Series[probeModel.Id.ToString()].Enabled = true;
                    chart1.Series[probeModel.Id.ToString()].IsVisibleInLegend = true;

                    chart1.Series[probeModel.Id.ToString()].XValueType = ChartValueType.DateTime;

                    chart1.Series[probeModel.Id.ToString()].LegendText = probeModel.Name;

                    List<MeasurementModel> measurements = SqliteDataAccess.LoadMeasurements(probeModel.Id, date.ToShortDateString(), time.Hours, hourCount);

                    foreach (MeasurementModel measurement in measurements)
                    {
                        DateTime MeasurementTime = Convert.ToDateTime(measurement.Date + " " + measurement.Hour + ":" + measurement.Minute + ":" + measurement.Second);
                        chart1.Series[probeModel.Id.ToString()].Points.AddXY(MeasurementTime, measurement.Value);
                    }
                }
            }
        }

        public bool CheckMinMaxTempsGraph()
        {
            int min = Convert.ToInt32(numMinGraph.Value);
            int max = Convert.ToInt32(numMaxGraph.Value);
            if (min >= max)
                return false;
            else return true;
        }

        private void btnShowGraphic1_Click(object sender, EventArgs e)
        {
            if (CheckMinMaxTempsGraph())
                ShowGraph(0, dateTimeGraph.Value, Convert.ToInt32(numStartHourGraph.Value), Convert.ToInt32(numHourCountGraph.Value));
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShowGraphic2_Click(object sender, EventArgs e)
        {
            if (CheckMinMaxTempsGraph())
                ShowGraph(1, dateTimeGraph.Value, Convert.ToInt32(numStartHourGraph.Value), Convert.ToInt32(numHourCountGraph.Value));
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShowGraphic3_Click(object sender, EventArgs e)
        {
            if (CheckMinMaxTempsGraph())
                ShowGraph(2, dateTimeGraph.Value, Convert.ToInt32(numStartHourGraph.Value), Convert.ToInt32(numHourCountGraph.Value));
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtNapomena_TextChanged(object sender, EventArgs e)
        {
            chart1.Titles["Napomena"].Text = "Napomena: " + txtNapomena.Text;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printDialog1.Document = chart1.Printing.PrintDocument;
            printDialog1.Document.DefaultPageSettings.Landscape = true;
            printDialog1.Document.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printPreviewDialog1.Document = printDialog1.Document;
                printPreviewDialog1.ClientSize = new Size(1200, 800); // A4: 2480 pixels x 3508 pixels
                printPreviewDialog1.ShowDialog();
            }
        }

        private void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if(chkAutoRefresh.Checked)
            {
                if (CheckMinMaxTempsGraph())
                {
                    autoRefresh = true;
                    cmbLiveGraphDevices.Enabled = false;
                    grpTimeSettings.Enabled = false;
                    grpTempSettings.Enabled = false;
                    foreach (Button button in btnShowGraphics)
                        button.Enabled = false;

                    ShowGraph(Convert.ToInt32(cmbLiveGraphDevices.Text) - 1, DateTime.Now.Date, DateTime.Now.TimeOfDay, 4);
                }
                else
                {
                    chkAutoRefresh.Checked = false;
                    MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                autoRefresh = false;
                cmbLiveGraphDevices.Enabled = true;
                grpTimeSettings.Enabled = true;
                grpTempSettings.Enabled = true;
                for (int i = 0; i < devices.Count; i++)
                    if (devices[i].Active)
                        btnShowGraphics[i].Enabled = true;
            }

        }

        #endregion

        #region Graph Show Button Click Events

        public bool CheckMinMaxTemps(int num)
        {
            int min = Convert.ToInt32(numMins[num - 1].Value);
            int max = Convert.ToInt32(numMaxs[num - 1].Value);
            if (min >= max)
                return false;
            else return true;
        }

        public GraphDataModel AssembleGraphDataModel(int i)
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
                Company = lblCompany.Text,
                Probe = probe,
                Date = dateTimePickers[i - 1].Value.ToShortDateString(),
                StartHour = Convert.ToInt32(numStartHours[i - 1].Value),
                HourCount = Convert.ToInt32(numHourCounts[i - 1].Value)
            };
        }

        private void btnShow1_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(1))
            {
                graph = new Graph(AssembleGraphDataModel(1));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow2_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(2))
            {
                graph = new Graph(AssembleGraphDataModel(2));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow3_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(3))
            {
                graph = new Graph(AssembleGraphDataModel(3));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow4_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(4))
            {
                graph = new Graph(AssembleGraphDataModel(4));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow5_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(5))
            {
                graph = new Graph(AssembleGraphDataModel(5));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow6_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(6))
            {
                graph = new Graph(AssembleGraphDataModel(6));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow7_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(7))
            {
                graph = new Graph(AssembleGraphDataModel(7));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow8_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(8))
            {
                graph = new Graph(AssembleGraphDataModel(8));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow9_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(9))
            {
                graph = new Graph(AssembleGraphDataModel(9));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow10_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(10))
            {
                graph = new Graph(AssembleGraphDataModel(10));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow11_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(11))
            {
                graph = new Graph(AssembleGraphDataModel(11));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow12_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(12))
            {
                graph = new Graph(AssembleGraphDataModel(12));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow13_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(13))
            {
                graph = new Graph(AssembleGraphDataModel(13));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow14_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(14))
            {
                graph = new Graph(AssembleGraphDataModel(14));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow15_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(15))
            {
                graph = new Graph(AssembleGraphDataModel(15));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow16_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(16))
            {
                graph = new Graph(AssembleGraphDataModel(16));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow17_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(17))
            {
                graph = new Graph(AssembleGraphDataModel(17));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow18_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(18))
            {
                graph = new Graph(AssembleGraphDataModel(18));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow19_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(19))
            {
                graph = new Graph(AssembleGraphDataModel(19));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow20_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(20))
            {
                graph = new Graph(AssembleGraphDataModel(20));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow21_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(21))
            {
                graph = new Graph(AssembleGraphDataModel(21));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow22_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(22))
            {
                graph = new Graph(AssembleGraphDataModel(22));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow23_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(23))
            {
                graph = new Graph(AssembleGraphDataModel(23));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShow24_Click(object sender, EventArgs e)
        {
            Graph graph;
            if (CheckMinMaxTemps(24))
            {
                graph = new Graph(AssembleGraphDataModel(24));
                graph.ShowDialog();
            }
            else MessageBox.Show("Vrednost min mora biti manja od max.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Max 24 Hour Limit Handlers

        public void LimitHourCount(int probeID)
        {
            numHourCounts[probeID - 1].Maximum = 24 - numStartHours[probeID - 1].Value;
        }

        private void numStartHour1_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(1);
        }

        private void numStartHour2_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(2);
        }

        private void numStartHour3_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(3);
        }

        private void numStartHour4_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(4);
        }

        private void numStartHour5_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(5);
        }

        private void numStartHour6_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(6);
        }

        private void numStartHour7_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(7);
        }

        private void numStartHour8_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(8);
        }

        private void numStartHour9_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(9);
        }

        private void numStartHour10_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(10);
        }

        private void numStartHour11_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(11);
        }

        private void numStartHour12_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(12);
        }

        private void numStartHour13_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(13);
        }

        private void numStartHour14_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(14);
        }

        private void numStartHour15_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(15);
        }

        private void numStartHour16_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(16);
        }

        private void numStartHour17_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(17);
        }

        private void numStartHour18_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(18);
        }

        private void numStartHour19_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(19);
        }

        private void numStartHour20_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(20);
        }

        private void numStartHour21_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(21);
        }

        private void numStartHour22_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(22);
        }

        private void numStartHour23_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(23);
        }

        private void numStartHour24_ValueChanged(object sender, EventArgs e)
        {
            LimitHourCount(24);
        }

        private void numStartHourGraph_ValueChanged(object sender, EventArgs e)
        {
            numHourCountGraph.Maximum = 24 - numStartHourGraph.Value;
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommStream?.Dispose();
        }

        private void kontaktToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ESTHETIC LABS DOO" + Environment.NewLine +
                                       "NIŠ" + Environment.NewLine +
                                        "www.elabs.rs" + Environment.NewLine +
                                        "e-mail: office@elabs.rs" + Environment.NewLine +
                                        "mob: +381 64 453 26 26" + Environment.NewLine +
                                        "tel: +381 18 4200 020", "Kontakt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized || WindowState == FormWindowState.Normal)
            {
                ArrangeTempDisplays();
            }

        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            ArrangeTempDisplays();
        }
    }
}
