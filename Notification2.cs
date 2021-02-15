using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Notification;
using AB.API_Class.Branch;
using Newtonsoft.Json.Linq;
using AB.API_Class.Warehouse;
namespace AB
{
    public partial class Notification2 : Form
    {
        public Notification2()
        {
            InitializeComponent();
        }
        DataTable dtBranches = new DataTable(), dtWarehouse = new DataTable();
        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        int cBranch = 1, cWhse = 1, cDate = 1, cToDate = 1, cCheckDate = 1, cCheckToDate = 1;
        private async void Notification2_Load(object sender, EventArgs e)
        {
            dtFromDate.Value = DateTime.Now;
            dtToDate.Value = DateTime.Now;
            loadBranches();
            loadWarehouse();
            await loadData();
            cBranch = 0;
            cWhse = 0;
            cDate = 0;
            cToDate = 0;
            cCheckDate = 0;
            cCheckToDate = 0;
        }

        public async void loadBranches()
        {
            int isAdmin = 0;
            string branch = "";
            dtBranches = await Task.Run(() => branchc.returnBranches());
            cmbBranches.Items.Clear();
            if (Login.jsonResult != null)
            {
                foreach (var x in Login.jsonResult)
                {
                    if (x.Key.Equals("data"))
                    {
                        JObject jObjectData = JObject.Parse(x.Value.ToString());
                        foreach (var y in jObjectData)
                        {
                            if (y.Key.Equals("branch"))
                            {
                                branch = y.Value.ToString();
                            }
                            else if (y.Key.Equals("isAdmin"))
                            {

                                if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                {
                                    foreach (DataRow row in dtBranches.Rows)
                                    {
                                        if (row["code"].ToString() == branch)
                                        {
                                            cmbBranches.Items.Add(row["name"].ToString());
                                            if (cmbBranches.Items.Count > 0)
                                            {
                                                cmbBranches.SelectedIndex = 0;
                                            }
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    isAdmin += 1;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (cmbBranches.Items.Count <= 0)
                {
                    foreach (DataRow row in dtBranches.Rows)
                    {
                        cmbBranches.Items.Add(row["name"]);
                    }
                }
            }
            if (cmbBranches.Items.Count > 0)
            {
                string branchName = "";
                foreach (DataRow row in dtBranches.Rows)
                {
                    if (row["code"].ToString() == branch)
                    {
                        branchName = row["name"].ToString();
                        break;
                    }
                }
                cmbBranches.SelectedIndex = cmbBranches.Items.IndexOf(branchName);
            }
        }

        public async void loadWarehouse()
        {
            cmbWarehouse.Items.Clear();
            cmbWarehouse.Items.Add("All");
            string branch = "";
            foreach (DataRow row in dtBranches.Rows)
            {
                if (cmbBranches.Text == row["name"].ToString())
                {
                    branch = row["code"].ToString();
                    break;
                }
            }
            dtWarehouse = await Task.Run(() => warehousec.returnWarehouse(branch, ""));
            foreach(DataRow row in dtWarehouse.Rows)
            {
                cmbWarehouse.Items.Add(row["whsename"].ToString());
            }
            cmbWarehouse.SelectedIndex = 0;
        }


        public async Task loadData()
        {
            notification_class notifc = new notification_class();
            string branch = "", fromDate = "", toDate = "", whseCode = "";
            string branchValue = "", fromDateValue = "", toDateValue = "", warehouseValue = "";
            bool isFromDateCheck = false, isToDateCheck = false;
            cmbBranches.Invoke(new Action(delegate ()
            {
                branchValue= cmbBranches.Text;
            }));
            dtFromDate.Invoke(new Action(delegate ()
            {
                fromDateValue = dtFromDate.Value.ToString("yyyy-MM-dd");
            }));
            dtToDate.Invoke(new Action(delegate ()
            {
                toDateValue = dtToDate.Value.ToString("yyyy-MM-dd");
            }));
            cmbWarehouse.Invoke(new Action(delegate ()
            {
                warehouseValue = cmbWarehouse.Text;
            }));
            checkDate.Invoke(new Action(delegate ()
            {
                isFromDateCheck = checkDate.Checked;
            }));
            checkToDate.Invoke(new Action(delegate ()
            {
                isToDateCheck = checkToDate.Checked;
            }));

            foreach (DataRow row in dtBranches.Rows)
            {
                if (branchValue == row["name"].ToString())
                {
                    branch = row["code"].ToString();
                    break;
                }
            }
            foreach (DataRow row in dtWarehouse.Rows)
            {
                if (warehouseValue == row["whsename"].ToString())
                {
                    whseCode = row["whsecode"].ToString();
                    break;
                }
            }
            fromDate = !isFromDateCheck? "&from_date=" : "&from_date=" + fromDateValue;
            toDate = !isToDateCheck ? "&to_date=" : "&to_date=" + toDateValue;
            DataTable dt = await Task.Run(() => notifc.getUnreadNotif(branch,fromDate,toDate, whseCode));
            DataRow roww = dt.Rows[0];
            int count = string.IsNullOrEmpty(roww["count"].ToString()) ? 0 : Convert.ToInt32(roww["count"].ToString());
            lblCount.Text = "Count: " + count.ToString("N0");
            loadUI(dt);
            await Task.Delay(2000);
        }

        public void loadUI(DataTable dt)
        {
            dgv.Invoke(new Action(delegate ()
            {
                dgv.Rows.Clear();
            }));
            AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
            foreach (DataRow row in dt.Rows)
            {
                dgv.Invoke(new Action(delegate ()
                {
                    int id = string.IsNullOrEmpty(row["id"].ToString()) ? 0 : Convert.ToInt32(row["id"].ToString()),
                        age = string.IsNullOrEmpty(row["age"].ToString()) ? 0 : Convert.ToInt32(row["age"].ToString());
                    string itemCode = row["item_code"].ToString(), whseCode = row["whsecode"].ToString(), branch = row["branch"].ToString(), dateCreated = row["date_created"].ToString();
                    double quantity = string.IsNullOrEmpty(row["quantity"].ToString()) ? 0 : Convert.ToInt32(row["quantity"].ToString());
                    auto.Add(itemCode);
                    if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
                    {
                        if (txtSearch.Text.ToString().Trim().ToLower().Contains(itemCode.ToLower()))
                        {
                            dgv.Rows.Add(id, branch, itemCode, quantity, whseCode, age, dateCreated);
                        }
                    }
                    else
                    {
                        dgv.Rows.Add(id, whseCode, itemCode, quantity, branch, age, dateCreated);
                    }
                }));
            }
            txtSearch.AutoCompleteCustomSource = auto;
        }

        private async void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgv.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        Notification frm = new Notification();
                        int temp = 0;
                        frm.selectedID = int.TryParse(dgv.CurrentRow.Cells["id"].Value.ToString(), out temp) ? Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString()) : 1;
                        frm.ShowDialog();
                    }
                    else if (e.ColumnIndex == 7)
                    {
                        RemarksDetails frm = new RemarksDetails();
                        int temp = 0;
                        frm.selectedID = int.TryParse(dgv.CurrentRow.Cells["id"].Value.ToString(), out temp) ? Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString()) : 1;
                        frm.ShowDialog();
                    }else if (e.ColumnIndex == 8)
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to Mark as Done?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            int temp = 0;
                            int selectedID = int.TryParse(dgv.CurrentRow.Cells["id"].Value.ToString(), out temp) ? Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString()) : 1;
                            notification_class notifc = new notification_class();
                            string msgDone = "";
                            msgDone = await notifc.markAsRead(selectedID);
                            MessageBox.Show(msgDone, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await loadData();
                        }
                    }
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await loadData();
        }

        private void checkDate_CheckedChanged(object sender, EventArgs e)
        {
            if(cCheckDate <= 0)
            {
                btnRefresh.PerformClick();
            }
        }

        private void checkToDate_CheckedChanged(object sender, EventArgs e)
        {
            if(cCheckToDate <= 0)
            {
                btnRefresh.PerformClick();
            }
        }

        private async void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cWhse <= 0)
            {
                await loadData();
            }
        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            if(cToDate <= 0)
            {
                btnRefresh.PerformClick();
            }
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                await loadData(); 
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await loadData();
        }

        private void cmbBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBranch <= 0)
            {
                loadWarehouse();
            }
        }

        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            if(cDate <= 0)
            {
                btnRefresh.PerformClick();
            }
        }
    }
}
