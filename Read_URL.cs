using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.UI_Class;
using System.IO;

namespace AB
{
    public partial class Read_URL : Form
    {
        public Read_URL()
        {
            InitializeComponent();
        }
        utility_class utilityc = new utility_class();
        private void  Read_URL_Load(object sender, EventArgs e)
        {
            txt.Text = utilityc.URL;
            txt.Focus();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt.Text.Trim())){
                MessageBox.Show("URL field is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                linkPassword.isSubmit = false;
                linkPassword frm = new linkPassword();
                frm.ShowDialog();
                if (linkPassword.isSubmit)
                {
                    this.Hide();
                    File.WriteAllText(System.Environment.CurrentDirectory + @"\URL.txt", txt.Text.Trim());
                }
            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                btnSubmit.PerformClick();
            }
        }
    }
}
