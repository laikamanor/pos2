﻿using Newtonsoft.Json.Linq;
using RestSharp;
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
using AB.API_Class.User;
using AB.API_Class.Payment_Type;
using AB.API_Class.Branch;
using AB.API_Class.Warehouse;
namespace AB
{
    public partial class CashTransactionReport : Form
    {
        DataTable dtBranch = new DataTable(), dtWarehouse = new DataTable();
        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        utility_class utilityc = new utility_class();
        paymenttype_class paymenttypec = new paymenttype_class();
        user_clas userc = new user_clas();
        DataTable dtUsers = new DataTable();
        DataTable dtCashier = new DataTable();
        int cBranch = 1, cWarehouse = 1, cUser = 1, cPaymentType = 1, cSalesType = 1;
        public CashTransactionReport()
        {
            InitializeComponent();
        }

        public async void loadBranch()
        {
            int isAdmin = 0;
            string branch = "";
            dtBranch = await Task.Run(() => branchc.returnBranches());
            cmbBranch.Items.Clear();
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
                                            cmbBranch.Items.Add(row["name"].ToString());
                                            if (cmbBranch.Items.Count > 0)
                                            {
                                                cmbBranch.SelectedIndex = 0;
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
                                            cmbBranch.Items.Add(row["name"].ToString());
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (cmbBranch.Items.Count <= 0)
                {
                    foreach (DataRow row in dtBranch.Rows)
                    {
                        cmbBranch.Items.Add(row["name"]);
                    }
                }
            }
            if (cmbBranch.Items.Count > 0)
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

        public async void loadWarehouse()
        {
            string branchCode = "";
            string warehouse = "";
            foreach (DataRow row in dtBranch.Rows)
            {
                if (cmbBranch.Text.Equals(row["name"].ToString()))
                {
                    branchCode = row["code"].ToString();
                    break;
                }
            }
            dtWarehouse = await Task.Run(() => warehousec.returnWarehouse(branchCode,""));
            cmbWhse.Items.Clear();
            int isAdmin = 0;
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
                            else if (y.Key.Equals("isAdmin") || y.Key.Equals("isManager") || y.Key.Equals("isAccounting"))
                            {
                                if (y.Value.ToString().ToLower() == "true")
                                {
                                    cmbWhse.Items.Add("All");
                                    foreach (DataRow row in dtWarehouse.Rows)
                                    {
                                        cmbWhse.Items.Add(row["whsename"].ToString());
                                        cmbWhse.SelectedIndex = 0;
                                    }
                                    return;
                                }
                                else
                                {
                                    isAdmin += 1;
                                }
                            }
                        }
                    }
                }
            }
            if (isAdmin > 0)
            {
                string whseName = "";
                foreach (DataRow row in dtWarehouse.Rows)
                {
                    if (row["whsecode"].ToString() == warehouse)
                    {
                        whseName = row["whsename"].ToString();
                        cmbWhse.Items.Add(whseName);
                    }
                }
                cmbWhse.SelectedIndex = cmbWhse.Items.IndexOf(whseName);
            }
        }

        public string findWarehouseCode()
        {
            string result = "";
            foreach(DataRow row in dtWarehouse.Rows)
            {
                if(row["whsename"].ToString() == cmbWhse.Text)
                {
                    result = row["whsecode"].ToString();
                    break;
                }
            }
            return result;
        }

        public string findBranchCode()
        {
            string result = "";
            foreach (DataRow row in dtBranch.Rows)
            {
                if (row["name"].ToString() == cmbBranch.Text)
                {
                    result = row["code"].ToString();
                    break;
                }
            }
            return result;
        }

        public void loadUsers(ComboBox cmb, bool isCashier)
        {
            DataTable adtUsers = new DataTable();
            string sBranch = "?branch=" + findBranchCode();
            string sWhse = "&whse=" +findWarehouseCode();
            string sCashier = "&isCashier=" + (isCashier ? "1" : "");
            adtUsers = userc.returnUsers(sBranch + sWhse + sCashier);
            if (isCashier)
            {
                dtUsers = userc.returnUsers(sBranch + sWhse + sCashier);
            }
            else if (isCashier)
            {
                dtCashier = userc.returnUsers(sBranch + sWhse + sCashier);
            }
    
            cmb.Items.Clear();
            cmb.Items.Add("All");
            foreach(DataRow r0w in adtUsers.Rows)
            {
                cmb.Items.Add(r0w["username"].ToString());
            }
            if(cmb.Items.Count > 0)
            {
                cmb.SelectedIndex = 0;
            }
        }

        private void SalesReport_Load(object sender, EventArgs e)
        {
            dgv.Columns["amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            loadBranch();
            loadWarehouse();
            loadUsers(cmbCashier, true);
            loadPaymentType("sales", cmbSalesType);
            loadPaymentType("payment", cmbPaymentType);
            loadData();
            cBranch = 0;
            cWarehouse = 0;
            cUser = 0;
            cPaymentType = 0;
            cSalesType = 0;
        }

        public void loadPaymentType(string urlType, ComboBox cmb)
        {
            cmb.Items.Clear();
            cmb.Items.Add("All");
            DataTable dtPaymentTypes = new DataTable();
            dtPaymentTypes = paymenttypec.loadPaymentType(urlType);       
            if (dtPaymentTypes.Rows.Count > 0)
            {
                foreach (DataRow row in dtPaymentTypes.Rows)
                {
                    cmb.Items.Add(row["code"].ToString());
                }
            }
            cmb.SelectedIndex = 0;
        }

        public void loadData()
        {
            if (Login.jsonResult != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                string token = "";
                foreach (var x in Login.jsonResult)
                {
                    if (x.Key.Equals("token"))
                    {
                        token = x.Value.ToString();
                    }
                }
                if (!token.Equals(""))
                {
                    dgv.Rows.Clear();
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;

                    int cashierID = 0;
                    foreach (DataRow r0wSales in dtUsers.Rows)
                    {
                        if (r0wSales["username"].ToString() == cmbCashier.Text)
                        {
                            cashierID = Convert.ToInt32(r0wSales["userid"].ToString());
                        }
                    }

                    string  sCashier = (cashierID <= 0 ? "&cashier_id=" : "&cashier_id=" + cashierID);
                    string sSalesType = (cmbSalesType.SelectedIndex <= 0 ? "&sales_type=" : "&sales_type=" + cmbSalesType.Text);
                    string sPaymentType = (cmbPaymentType.SelectedIndex <= 0 ? "&payment_type=" : "&payment_type=" + cmbPaymentType.Text);

                    string sBranch = "&branch=" + findBranchCode();
                    string sWarehouse = "&whse=" + findWarehouseCode();

                    var request = new RestRequest("/api/report/cs?from_date=" +dtFrom.Value.ToString("yyyy-MM-dd") + "&to_date=" + dtTo.Value.ToString("yyyy-MM-dd") + sCashier + sSalesType + sPaymentType + sBranch + sWarehouse);
                    Console.WriteLine("/api/report/cs?from_date=" + dtFrom.Value.ToString("yyyy-MM-dd") + "&to_date=" + dtTo.Value.ToString("yyyy-MM-dd") + sCashier + sSalesType + sPaymentType + sBranch + sWarehouse);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = new JObject();
                    jObject = JObject.Parse(response.Content.ToString());
                    bool isSuccess = false;
                    foreach (var x in jObject)
                    {
                        if (x.Key.Equals("success"))
                        {
                            isSuccess = Convert.ToBoolean(x.Value.ToString());
                            break;
                        }
                    }
                    if (isSuccess)
                    {
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("data"))
                            {
                                if (x.Value.ToString() != "{}")
                                {
                                    JObject jObjectData = JObject.Parse(x.Value.ToString());
                                    foreach (var y in jObjectData)
                                    {
                                        if (y.Key.Equals("cash_trans"))
                                        {
                                            JArray jArrayCashTans = JArray.Parse(y.Value.ToString());
                                            for (int aa = 0; aa < jArrayCashTans.Count(); aa++)
                                            {
                                                JObject jObjectCashTans = JObject.Parse(jArrayCashTans[aa].ToString());
                                                foreach (var z in jObjectCashTans)
                                                {
                                                    if (z.Key.Equals("TotalCashOnHand"))
                                                    {
                                                        lblCashOnHand.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                    else if (z.Key.Equals("TotalCashPayment"))
                                                    {

                                                        lblCashSales.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                    else if (z.Key.Equals("DepositCash"))
                                                    {

                                                        lblADVCash.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                    else if (z.Key.Equals("FromDep"))
                                                    {

                                                        lblUsedADV.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                    else if (z.Key.Equals("BankDep"))
                                                    {

                                                        lblBankDeposit.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                    else if (z.Key.Equals("EPAY"))
                                                    {

                                                        lblEpay.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                    else if (z.Key.Equals("GCert"))
                                                    {

                                                        lblGCert.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                    else if (z.Key.Equals("Commission"))
                                                    {

                                                        lblComission.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                    else if (z.Key.Equals("Cashout"))
                                                    {

                                                        lblCashOut.Text = (z.Value.ToString() != "" ? Convert.ToDouble(z.Value.ToString()).ToString("n2") : "0.00");
                                                    }
                                                }
                                            }
                                        }
                                        else if (y.Key.Equals("sales_rows"))
                                        {
                                            dgv.Rows.Clear();
                                            JArray jArraySalesRows = JArray.Parse(y.Value.ToString());
                                            for (int aa = 0; aa < jArraySalesRows.Count(); aa++)
                                            {
                                                
                                                JObject jObjectSalesRows = JObject.Parse(jArraySalesRows[aa].ToString());

                                                string referenceNumber = "", url = "", salesType = "", paymentType = "";
                                                double amount = 0.00;
                                                DateTime dtTransdate = new DateTime();
                                                foreach (var z in jObjectSalesRows)
                                                {
                                                    if (z.Key.Equals("reference"))
                                                    {
                                                        referenceNumber = z.Value.ToString();
                                                    }else if (z.Key.Equals("url"))
                                                    {
                                                        url= z.Value.ToString();
                                                    }
                                                    else if (z.Key.Equals("amount"))
                                                    {
                                                        amount = Convert.ToDouble(z.Value.ToString());
                                                    }
                                                    else if (z.Key.Equals("SalesType"))
                                                    {
                                                        salesType = z.Value.ToString();
                                                    }
                                                    else if (z.Key.Equals("PaymentType"))
                                                    {
                                                        paymentType = z.Value.ToString();
                                                    }
                                                    else if (z.Key.Equals("transdate"))
                                                    {
                                                        string replaceT = z.Value.ToString().Replace("T", "");
                                                        dtTransdate = Convert.ToDateTime(replaceT);
                                                    }

                                                }
                                                dgv.Rows.Add(referenceNumber, Convert.ToDecimal(string.Format("{0:0.00}", amount)), salesType,paymentType, dtTransdate.ToString("yyyy-MM-dd HH:mm"), url);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string msg = "No message response found";
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }
                        if (msg.Equals("Token is invalid"))
                        {
                            MessageBox.Show("Your login session is expired. Please login again", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            lblNoDataFound.Visible = (dgv.Rows.Count > 0 ? false : true);
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cmbCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cUser <= 0)
            {
                loadData();
            }
        }


        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                CashTransactionReportItems cashTransactionReportItems = new CashTransactionReportItems();
                cashTransactionReportItems.URLDetails = dgv.CurrentRow.Cells["url"].Value.ToString();
                cashTransactionReportItems.ShowDialog();
                if (CashTransactionReportItems.isSubmit)
                {
                    loadData();
                }
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cPaymentType <= 0)
            {
                loadData();
            }
        }

        private void cmbBranch_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cBranch <= 0)
            {
                loadWarehouse();
            }
        }

        private void cmbWhse_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cWarehouse <= 0)
            {
                loadUsers(cmbCashier, true);
                loadData();
            }
        }

        private void cmbSalesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cSalesType <= 0)
            {
                loadData();
            }
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
