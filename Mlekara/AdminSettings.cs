using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

    }
}
