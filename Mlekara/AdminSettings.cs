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
        private DefaultsModel defaults;

        public AdminSettings()
        {
            InitializeComponent();

            txtCompany.Text = SqliteDataAccess.LoadCompanyName();

            defaults = SqliteDataAccess.LoadDefaults();

            numGraphMin.Value = defaults.GraphMin;
            numGraphMax.Value = defaults.GraphMax;
            numStackSize.Value = defaults.StackSize;
        }

        #region Save Buttons

        private void btnCompany_Click(object sender, EventArgs e)
        {
            try
            {
                SqliteDataAccess.SaveCompanyName(txtCompany.Text);
                MessageBox.Show("Company name saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDevice_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceModel device = new DeviceModel(Convert.ToInt32(cmbDeviceId.Text), txtDeviceName.Text, chkDeviceActive.Checked);
                SqliteDataAccess.SaveDevice(device);

                List<DeviceModel> data = SqliteDataAccess.LoadDevices();
                dataGridView1.DataSource = data;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProbe_Click(object sender, EventArgs e)
        {
            try
            {
                ProbeModel probe = new ProbeModel(Convert.ToInt32(cmbProbeId.Text), Convert.ToInt32(txtProbeDeviceId.Text), txtProbeName.Text, chkProbeActive.Checked,
                Convert.ToInt32(numMin.Value), Convert.ToInt32(numMax.Value), Convert.ToInt32(numMarker.Value));
                SqliteDataAccess.SaveProbe(probe);

                List<ProbeModel> data = SqliteDataAccess.LoadProbes();
                dataGridView1.DataSource = data;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            try
            {
                SqliteDataAccess.SaveDefaults(Convert.ToInt32(numStackSize.Value), Convert.ToInt32(numGraphMin.Value), Convert.ToInt32(numGraphMax.Value));
                MessageBox.Show("Defaults saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Show Buttons

        private void btnShowDevices_Click(object sender, EventArgs e)
        {
            try
            {
                List<DeviceModel> data = SqliteDataAccess.LoadDevices();
                dataGridView1.DataSource = data;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowProbes_Click(object sender, EventArgs e)
        {
            try
            {
                List<ProbeModel> data = SqliteDataAccess.LoadProbes();
                dataGridView1.DataSource = data;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowProbesByDevice_Click(object sender, EventArgs e)
        {
            try
            {
                List<ProbeModel> data = SqliteDataAccess.LoadProbes(Convert.ToInt32(cmbShowDeviceId.Text));
                dataGridView1.DataSource = data;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void cmbDeviceId_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceModel device = SqliteDataAccess.LoadDevice(Convert.ToInt32(cmbDeviceId.Text));
            chkDeviceActive.Checked = device.Active;
            txtDeviceName.Text = device.Name;
        }

        private void cmbProbeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ProbeModel probe = SqliteDataAccess.LoadProbe(Convert.ToInt32(cmbProbeId.Text));
                txtProbeDeviceId.Text = probe.DeviceId.ToString();
                chkProbeActive.Checked = probe.Active;
                txtProbeName.Text = probe.Name;
                numMin.Value = probe.Min;
                numMax.Value = probe.Max;
                numMarker.Value = probe.Marker;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
