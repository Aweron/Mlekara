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
using Mlekara.Models;

namespace Mlekara
{
    public partial class Graph : Form
    {
        public Graph(GraphDataModel graphData)
        {
            InitializeComponent();

            List<MeasurementModel> measurements = SqliteDataAccess.LoadMeasurements(graphData.Probe.Id, graphData.Date, graphData.StartHour, graphData.HourCount);

            // X axis range
            int endHour = graphData.StartHour + graphData.HourCount;

            DateTime min = Convert.ToDateTime(graphData.StartHour + ":00");
            DateTime max = Convert.ToDateTime(endHour + ":00");

            chart1.ChartAreas[0].AxisX.Minimum = min.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = max.ToOADate();
            
            // Y axis range
            chart1.ChartAreas[0].AxisY.Minimum = graphData.Probe.Min;
            chart1.ChartAreas[0].AxisY.Maximum = graphData.Probe.Max;

            // Marker line
            //chart1.Series["Marker"].XValueType = ChartValueType.Time;

            chart1.Series["Marker"].Points.AddXY(min, graphData.Probe.Marker);
            chart1.Series["Marker"].Points.AddXY(max, graphData.Probe.Marker);

            // Measurements X axis value
            chart1.Series["Temps"].XValueType = ChartValueType.DateTime;

            foreach (MeasurementModel measurement in measurements)
            {
                DateTime time = Convert.ToDateTime(measurement.Date + " " + measurement.Hour + ":" + measurement.Minute + ":" + measurement.Second);
                chart1.Series["Temps"].Points.AddXY(time, measurement.Value);

                textBox2.Text = time.ToString();
            }
            
        }
    }
}
