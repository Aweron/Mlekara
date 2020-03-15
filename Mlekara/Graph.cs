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

namespace Mlekara
{
    public partial class Graph : Form
    {
        public Graph()
        {
            InitializeComponent();
        }

        private void Graph_Load(object sender, EventArgs e)
        {
            chart1.Series[0].Points.AddXY("0", "32");
            chart1.Series[0].Points.AddXY("1", "35");
            chart1.Series[0].Points.AddXY("2", "70");
            chart1.Series[0].Points.AddXY("3", "10");
            chart1.Series[0].Points.AddXY("4", "23.5");
            //chart title  
            chart1.Titles.Add("Naziv");
        }
    }
}
