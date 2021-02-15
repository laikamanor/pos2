﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Branch;
using AB.API_Class.Warehouse;
using AB.API_Class.Transfer;
using AB.API_Class.User;
using Newtonsoft.Json.Linq;
namespace AB
{
    public partial class Transfer2 : Form
    {

        DataTable dtBranch = new DataTable(), dtWarehouse = new DataTable();
        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        transfer_class transferc = new transfer_class();
        user_clas userc = new user_clas();
        string gForType = "";

        int cBranch = 1, cWarehouse = 1, cStatus = 1, cDate = 1, cToWarehouse = 1;
        public Transfer2(string forType)
        {
            gForType = forType;
            InitializeComponent();
        }

        private void Transfer2_Load(object sender, EventArgs e)
        {
            cmbStatusTransactions.SelectedIndex = 0;
            loadBranch(cmbBranch);
            loadWarehouse(cmbWhse, this.Text.Equals("Received Transactions") ? true : false);
            loadWarehouse(cmbToWhse, this.Text.Equals("Received Transactions") ? false : true);
            loadData();
            cBranch = 0;
            cWarehouse = 0;
            cStatus = 0;
            cDate = 0;
            cToWarehouse = 0;
            label5.Visible = (this.Text.Equals("Pullout Transactions") ? false : true);
            cmbToWhse.Visible = (this.Text.Equals("Pullout Transactions") ? false : true);
            cmbStatusTransactions.Visible = this.Text != "Pull Out Transactions" ? false : true;
            Label2.Visible = this.Text != "Pull Out Transactions" ? false : true;
            txtsearchTransactions.Visible = this.Text != "Pull Out Transactions" ? false : true;
            btnsearch.Visible = this.Text != "Pull Out Transactions" ? false : true;
        }

        public void checkVariance()
        {
            for (int i = 0; i < dgvTransactions.Rows.Count; i++)
            {
                if (Convert.ToDouble(dgvTransactions.Rows[i].Cells["variance_count"].Value.ToString()) != 0)
                {
                    dgvTransactions.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        public async void loadBranch(ComboBox cmb)
        {
            int isAdmin = 0;
            dtBranch = await Task.Run(() => branchc.returnBranches());
            cmb.Items.Clear();
            string branch = "";
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
                                    foreach (DataRow row in dtBranch.Rows)
                                    {
                                        if (row["code"].ToString() == branch)
                                        {
                                            cmb.Items.Add(row["name"].ToString());
                                            if (cmb.Items.Count > 0)
                                            {
                                                cmb.SelectedIndex = 0;
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
                            else if (y.Key.Equals("isAccounting"))
                            {
                                if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                {
                                    foreach (DataRow row in dtBranch.Rows)
                                    {
                                        if (row["code"].ToString() == branch && isAdmin <= 0)
                                        {
                                            cmb.Items.Add(row["name"].ToString());
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (cmb.Items.Count <= 0)
                {
                    foreach (DataRow row in dtBranch.Rows)
                    {
                        cmb.Items.Add(row["name"]);
                    }
                }
            }
            if (cmb.Items.Count > 0)
            {
                string branchName = "";
                foreach (DataRow row in dtBranch.Rows)
                {
                    if (row["code"].ToString() == branch)
                    {
                        branchName = row["name"].ToString();
                        break;
                    }
                    else
                    {
                        cmbBranch.SelectedIndex = 0;
                    }
                }
                cmbBranch.SelectedIndex = cmbBranch.Items.IndexOf(branchName);
            }
        }

        public async void loadWarehouse(ComboBox cmb, bool isTo)
        {
            string warehouse = "";
            cmb.Items.Clear();
            cmb.Items.Add("All");
            if (isTo)
            {
                dtWarehouse = await Task.Run(() => warehousec.returnWarehouse("", ""));
                foreach (DataRow row in dtWarehouse.Rows)
                {
                    cmb.Items.Add(row["whsename"]);
                }
            }
            else
            {
                string branchCode = "";
                foreach (DataRow row in dtBranch.Rows)
                {
                    if (cmbBranch.Text.Equals(row["name"].ToString()))
                    {
                        branchCode = row["code"].ToString();
                        break;
                    }
                }
                dtWarehouse = await Task.Run(() => warehousec.returnWarehouse(branchCode, ""));
                int isAdmin = 0;
                foreach (DataRow row in dtWarehouse.Rows)
                {
                    cmb.Items.Add(row["whsename"]);
                }
                if (Login.jsonResult != null)
                {
                    foreach (var x in Login.jsonResult)
                    {
                        if (x.Key.Equals("data"))
                        {
                            JObject jObjectData = JObject.Parse(x.Value.ToString());
                            foreach (var y in jObjectData)
                            {
                                if (y.Key.Equals("whse"))
                                {
                                    warehouse = y.Value.ToString();
                                }
                                else if (y.Key.Equals("isAdmin"))
                                {

                                    if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                    {
                                        foreach (DataRow row in dtWarehouse.Rows)
                                        {
                                            if (row["whsecode"].ToString() == warehouse)
                                            {
                                                cmbWhse.Items.Add(row["whsename"].ToString());
                                                if (cmbWhse.Items.Count > 0)
                                                {
                                                    cmbWhse.SelectedIndex = 0;
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
                                else if (y.Key.Equals("isAccounting"))
                                {
                                    if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                    {
                                        foreach (DataRow row in dtWarehouse.Rows)
                                        {
                                            if (row["whsecode"].ToString() == warehouse && isAdmin <= 0)
                                            {
                                                cmbWhse.Items.Add(row["whsename"].ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (y.Key.Equals("isManager"))
                                {
                                    if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                    {
                                        foreach (DataRow row in dtWarehouse.Rows)
                                        {
                                            if (row["whsecode"].ToString() == warehouse && isAdmin <= 0)
                                            {
                                                cmbWhse.Items.Add(row["whsename"].ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (cmb.Items.Count <= 0)
                    {
                        foreach (DataRow row in dtWarehouse.Rows)
                        {
                            cmb.Items.Add(row["whsename"]);
                        }
                    }
                }
            }
            if (cmb.Items.Count > 0 && !isTo)
            {
                string whseName = "";
                foreach (DataRow row in dtWarehouse.Rows)
                {
                    if (row["whsecode"].ToString() == warehouse)
                    {
                        whseName = row["whsename"].ToString();
                        break;
                    }
                }
                cmb.SelectedIndex = cmb.Items.IndexOf(whseName);
                if (cmbWhse.Text == "")
                {
                    cmb.SelectedIndex = 0;
                }
            }
            else if (cmb.Items.Count > 0 && isTo)
            {
                cmb.SelectedIndex = 0;
            }
        }

        public void loadData()
        {
            dgvTransactions.Rows.Clear();
            string statusCode = cmbStatusTransactions.SelectedIndex <= 0 ? "" : cmbStatusTransactions.SelectedIndex == 1 ? "O" :
    cmbStatusTransactions.SelectedIndex == 2 ? "C" :
    cmbStatusTransactions.SelectedIndex == 3 ? "N" : "";
            DataTable dtTransfers = new DataTable();


            string url = "trfr";
            if (this.Text == "Transfer Transactions")
            {
                url = "trfr";
            }
            else if (this.Text == "Pullout Transactions")
            {
                url = "pullout";
            }
            else
            {
                url = "recv";
            }
            //MessageBox.Show("TRANSFER 2: "  +url);

            string warehouseCode = "", branchCode = "", toWarehouseCode = "";
            //WAREHOUSE
            foreach (DataRow row in dtWarehouse.Rows)
            {
                if (cmbWhse.Text.Equals(row["whsename"].ToString()))
                {
                    warehouseCode = row["whsecode"].ToString();
                    break; 
                }
            }
            //TO WAREHOUSE
            foreach (DataRow row in dtWarehouse.Rows)
            {
                if (cmbToWhse.Text.Equals(row["whsename"].ToString()))
                {
                    toWarehouseCode = row["whsecode"].ToString();
                    break;
                }
            }
            //BRANCH
            foreach (DataRow row in dtBranch.Rows)
            {
                if (cmbBranch.Text.Equals(row["name"].ToString()))
                {
                    branchCode = row["code"].ToString();
                    break;
                }
            }
            string whseName = (url.Equals("pullout") ? "whsecode" : "from_whse");
          
            string sWarehouse = string.IsNullOrEmpty(warehouseCode) ? "" : "&" + whseName + "=" + warehouseCode;
            string sToWarehouse = string.IsNullOrEmpty(toWarehouseCode) ? "" : "&to_whse=" + toWarehouseCode;
            string sBranch = string.IsNullOrEmpty(branchCode) ? "" : "&branch=" + branchCode;
            string sUnderScore = "", sURL = "";
            if (url.Equals("recv") || url.Equals("pullout"))
            {
                sUnderScore = "_";
            }
            if (url.Equals("pullout"))
            {
                sURL = "/api/";
            }
            else
            {
                sURL = "/api/inv/";
            }
            //MessageBox.Show("class: " + URL);
            dtTransfers = transferc.loadData(sURL + url + "/get" + sUnderScore + "all", statusCode, txtsearchTransactions.Text.Trim(), dtDate.Value.ToString("yyyy-MM-dd"), gForType , sBranch , sWarehouse, sToWarehouse);
            if (dtTransfers.Rows.Count > 0)
            {
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                foreach (DataRow row in dtTransfers.Rows)
                {
                    string decodeDocStatus = row["docstatus"].ToString() == "O" ? "Open" : row["docstatus"].ToString() == "C" ? "Closed" : "Cancelled";
                    auto.Add(row["transnumber"].ToString());

                    string replaceT = row["transdate"].ToString().Replace("T", "");
                   DateTime  dtTransDate = Convert.ToDateTime(replaceT);    

                    dgvTransactions.Rows.Add(row["id"], row["transnumber"], row["reference"], row["remarks"], decodeDocStatus, row["sap_number"], dtTransDate.ToString("yyyy-MM-dd"),row["variance_count"].ToString());
                }
                txtsearchTransactions.AutoCompleteCustomSource = auto;
            }
            lblNoDataFound.Visible = (dgvTransactions.Rows.Count <= 0 ? true : false);


            if (this.Text == "Transfer Transactions")
            {
                checkVariance();
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void txtsearchTransactions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                loadData();
            }
        }

        private void dgvTransactions_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTransactions.Rows.Count > 0)
            {
                string sText = "";
                if (this.Text.Equals("Transfer Transactions"))
                {
                    sText= "Transfer Items";
                }else if(this.Text.Equals("Received Transactions"))
                {
                    sText = "Received Items";
                }
                else
                {
                    sText = "Pullout Items";

                }
                TransferItems transferItems = new TransferItems(gForType);
                transferItems.selectedID = Convert.ToInt32(dgvTransactions.CurrentRow.Cells["id"].Value.ToString());

                transferItems.Text = sText;
                transferItems.ShowDialog();
                if (TransferItems.isSubmit)
                {
                    loadData();
                }
            }
        }

        private void cmbToWhse_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cToWarehouse <= 0)
            {
                loadData();
            }
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
           if(cDate <= 0)
            {
                loadData();
            }
        }



        private  void cmbBranch_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cBranch <= 0)
            {
                loadWarehouse(cmbWhse, true);
            }
        }

        private void cmbWhse_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cWarehouse <= 0)
            {
                loadData();
            }
        }


        private void cmbUser_SelectedValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cmbStatusTransactions_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cStatus <= 0)
            {
                loadData();
            }
        }
    }
}
