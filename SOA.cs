using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.SOA;
namespace AB
{
    public partial class SOA : Form
    {
        public SOA()
        {
            InitializeComponent();
        }
        soa_class soac = new soa_class();
        DataTable dtSOA = new DataTable();
        public async Task loadSOA()
        {
            dtSOA = await Task.Run(() => soac.getSOA());
            AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
            dgv.Rows.Clear();
            if (dtSOA.Rows.Count > 0)
            {
                foreach (DataRow row in dtSOA.Rows)
                {
                    auto.Add(row["cust_code"].ToString());
                    if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
                    {
                        if (txtSearch.Text.ToString().Trim().ToLower().Contains(row["cust_code"].ToString().ToLower()))
                        {
                            dgv.Rows.Add(row["id"].ToString(), row["reference"].ToString(), row["docstatus"].ToString(), row["transdate"].ToString(), row["cust_code"].ToString(), Convert.ToDecimal(string.Format("{0:0.00}", row["total_amount"].ToString())));
                        }
                    }
                    else
                    {
                        dgv.Rows.Add(row["id"].ToString(), row["reference"].ToString(), row["docstatus"].ToString(), row["transdate"].ToString(), row["cust_code"].ToString(), Convert.ToDecimal(string.Format("{0:0.00}", row["total_amount"].ToString())));
                    }
                }
                txtSearch.AutoCompleteCustomSource = auto;
            }
            dgv.Columns["total_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private async void SOA_Load(object sender, EventArgs e)
        {
            await loadSOA();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await loadSOA();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await loadSOA();
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                await loadSOA();
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if(e.RowIndex >= 0)
                {
                    if(dgv.Rows.Count > 0)
                    {
                        SOA_Details frm = new SOA_Details();
                        int intTemp = 0;
                        frm.selectedID = int.TryParse(dgv.CurrentRow.Cells["id"].Value.ToString(), out intTemp) ? Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString()) : 0;
                        frm.ShowDialog();
                    }
                }
            }
        }
    }
}
