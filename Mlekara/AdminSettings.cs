using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mlekara.Models;

namespace Mlekara
{
    public partial class AdminSettings : Form
    {
        public AdminSettings()
        {
            InitializeComponent();
        }

        private void btnCompany_Click(object sender, EventArgs e)
        {
            SqliteDataAccess.SaveCompanyName(txtCompany.Text);
        }

        private void btnDevice_Click(object sender, EventArgs e)
        {
            DeviceModel device = new DeviceModel(Convert.ToInt32(cmbDeviceId.Text), txtDeviceName.Text, chkDeviceActive.Checked);
            SqliteDataAccess.SaveDevice(device);
        }

        private void btnProbe_Click(object sender, EventArgs e)
        {
            try
            {
                ProbeModel probe = new ProbeModel(Convert.ToInt32(cmbProbeId.Text), Convert.ToInt32(cmbProbeDeviceId.Text), txtProbeName.Text, chkProbeActive.Checked,
                Convert.ToInt32(numMin.Value), Convert.ToInt32(numMax.Value), Convert.ToInt32(numMarker.Value));
                SqliteDataAccess.SaveProbe(probe);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
