using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;
using Mlekara.Models;

namespace Mlekara
{
    public partial class Graph : Form
    {
        public Graph(GraphDataModel graphData)
        {
            InitializeComponent();

            try
            {

                chart1.Titles["Naziv"].Text = graphData.Company;
                chart1.Titles["Device"].Text = SqliteDataAccess.LoadDevice(graphData.Probe.DeviceId).Name;
                chart1.Titles["Senzor"].Text = graphData.Probe.Name;
                chart1.Titles["Datum"].Text += graphData.Date;
                chart1.Titles["Vreme"].Text += graphData.StartHour + ":00 - " + (graphData.StartHour + graphData.HourCount) + ":00";

                List<MeasurementModel> measurements = SqliteDataAccess.LoadMeasurements(graphData.Probe.Id, graphData.Date, graphData.StartHour, graphData.HourCount);

                // X axis range
                int endHour = graphData.StartHour + graphData.HourCount;

                DateTime min = Convert.ToDateTime(graphData.Date + " " + graphData.StartHour + ":00");
                DateTime max = Convert.ToDateTime(graphData.Date + " " + (endHour - 1) + ":59");

                chart1.ChartAreas[0].AxisX.Minimum = min.ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = max.ToOADate();

                // Y axis range
                chart1.ChartAreas[0].AxisY.Minimum = graphData.Probe.Min;
                chart1.ChartAreas[0].AxisY.Maximum = graphData.Probe.Max;

                chart1.Series["Timestamps"].XValueType = ChartValueType.Time;
                chart1.Series["Timestamps"].Points.AddXY(min, graphData.Probe.Marker);
                chart1.Series["Timestamps"].Points.AddXY(max, graphData.Probe.Marker);

                // Marker line
                chart1.Series["Marker"].XValueType = ChartValueType.DateTime;

                chart1.Series["Marker"].Points.AddXY(min, graphData.Probe.Marker);
                chart1.Series["Marker"].Points.AddXY(max, graphData.Probe.Marker);

                // Measurements X axis value
                chart1.Series["Temps"].XValueType = ChartValueType.DateTime;

                foreach (MeasurementModel measurement in measurements)
                {
                    DateTime time = Convert.ToDateTime(measurement.Date + " " + measurement.Hour + ":" + measurement.Minute + ":" + measurement.Second);
                    chart1.Series["Temps"].Points.AddXY(time, measurement.Value);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                printPreviewDialog1.ClientSize = new Size(1130, 800); // A4: 2480 pixels x 3508 pixels
                printPreviewDialog1.ShowDialog();
            }
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            chart1.Titles["Datum"].Text = "Datum: " + dateTimePicker1.Value.ToShortDateString();
        }

        private void chart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == (Keys.Control | Keys.Alt))
                dateTimePicker1.Visible = !dateTimePicker1.Visible;
        }

        private void txtNapomena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == (Keys.Control | Keys.Alt))
                dateTimePicker1.Visible = !dateTimePicker1.Visible;
        }

        private void btnPrint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == (Keys.Control | Keys.Alt))
                dateTimePicker1.Visible = !dateTimePicker1.Visible;
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == (Keys.Control | Keys.Alt))
                dateTimePicker1.Visible = !dateTimePicker1.Visible;
        }
    }
}
