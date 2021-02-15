using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AB.UI_Class;
using AB.API_Class.Branch;
using AB.API_Class.Warehouse;
using AB.API_Class.Customer_Type;
using System.Threading.Tasks;

namespace AB
{
    public partial class PendingOrder2 : Form
    {
        UI_Class.utility_class utilityc = new utility_class();
        customertype_class customertypec = new customertype_class();        
        DataTable dtSalesAgent = new DataTable();
        public static DataTable dtSelectedDeposit;
        DataTable dtPayment = new DataTable();
        string gSalesType = "", gForType = "";
        int cCheck = 0, cFromTime = 1, cToTime = 1, cCustType = 1;

        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        DataTable dtBranches = new DataTable();
        DataTable dtWarehouse = new DataTable();
        DataTable dtSearch = new DataTable();
        DataTable dtCustType = new DataTable();
        int cBranch = 1, cDate = 1, cSales = 1, cToDate = 1;
        public PendingOrder2(string salesType, string forType)
        {
            gSalesType = salesType;
            gForType = forType;
            InitializeComponent();
        }

        public  async 
        Task
loadCustomerType()
        {
            cmbCustomerType.Items.Clear();
            dtCustType = await Task.Run(() => customertypec.loadCustomerTypes());
            if (dtCustType.Rows.Count > 0)
            {
                cmbCustomerType.Items.Add("All");
                foreach(DataRow row in dtCustType.Rows)
                {
                    cmbCustomerType.Items.Add(row["code"].ToString());
                }
                cmbCustomerType.SelectedIndex = 0;
            }
        }

        private async void PendingOrder_Load(object sender, EventArgs e)
        {

            dtSearch = new DataTable();
            dtSearch.Rows.Clear();
            dtSearch.Columns.Add("search", typeof(string));
            dtSearch.Columns.Add("type", typeof(string));

            dgvOrders.Columns["days_due"].Visible = gForType.Equals("for Payment") ? true : false;
            dgvOrders.Columns["btnEditTotalPayment"].Visible = gForType.Equals("for Payment") ? true : false;
            dtFromDate.Visible = true;
            label1.Visible = true;
            dtFromDate.Value = DateTime.Now;
            dtToDate.Value = DateTime.Now;
           if(gForType.Equals("for Confirmation") || gForType.Equals("for Payment"))
            {
                checkDate.Checked = false;
            }
            else
            {
                checkDate.Checked = true;
            }
            checkToDate.Checked = true;
            cmbFromTime.SelectedIndex = 0;
            cmbSearchType.SelectedIndex = 0;
            cmbToTime.SelectedIndex = cmbToTime.Items.Count - 1;
            if (gForType == "for Payment")
            {
                btnConfirm.Text = "PAY";
                button1.Visible = true;
                //btnVoid.Visible = true;
                btnPaymentMethod.Visible = true;
            }
            else
            {
                btnConfirm.Text = "CONFIRM";
                button1.Visible = false;
                //btnVoid.Visible = false;
                btnPaymentMethod.Visible = false;
            }
            if (gForType.Equals("for Payment") || gForType.Equals("for Confirmation"))
            {
                btnVoid.Visible = true;
            }
            else
            {
                btnVoid.Visible = false;
            }
            dtSelectedDeposit = new DataTable();
            dtSelectedDeposit.Columns.Clear();
            dtSelectedDeposit.Columns.Add("id");
            dtSelectedDeposit.Columns.Add("amount");
            dtSelectedDeposit.Columns.Add("payment_type");
            dtSelectedDeposit.Columns.Add("sapnum");
            dtSelectedDeposit.Columns.Add("reference2");
            dtSelectedDeposit.Columns.Add("type");
            dtPayment.Columns.Add("id");
            dtPayment.Columns.Add("amount");
            dtPayment.Columns.Add("payment_type");
            dtPayment.Columns.Add("sapnum");
            dtPayment.Columns.Add("reference2");

            dtSelectedDeposit.Rows.Clear();
            label7.Visible = true;
            cmbBranches.Visible = true;
            await loadBranches();

            await Task.Run(() => loadSalesAgent());
            await loadCustomerType();
            string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
            await Task.Run(() => loadData(subURL));
            searchFilter();
            cBranch = 0;
            cDate = 0;
            cSales = 0;
            cFromTime = 0;
            cToTime = 0;
            cToDate = 0;
            cCustType = 0;

            counts();
            dgvitems.Columns["item"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvOrders.Columns["amountdue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public async void loadSalesAgent()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Login.jsonResult != null)
            {
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
                    dtSalesAgent.Columns.Clear();
                    dtSalesAgent.Columns.Add("id");
                    dtSalesAgent.Columns.Add("username");
                    dtSalesAgent.Rows.Clear();
                    string brnch = "";
                    cmbsales.Invoke(new Action(delegate ()
                    {
                        cmbsales.Items.Clear();
                   
                    }));
                    cmbBranches.Invoke(new Action(delegate ()
                    {
                        brnch = cmbBranches.Text;
                    }));
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/auth/user/get_all?isSales=1&branch="+ findCode(brnch, "Branch"));
                    request.AddHeader("Authorization", "Bearer " + token);
                    Task<IRestResponse> t = client.ExecuteAsync(request);
                    t.Wait();
                    var response = await t;
                    if (response.ErrorMessage == null)
                    {
                        JObject jObjectResponse = JObject.Parse(response.Content);
                        if (response.Content.Substring(0, 1).Equals("{"))
                        {
                            cmbsales.Invoke(new Action(delegate ()
                            {
                                cmbsales.Items.Add("All");
                            }));

                            bool isSuccess = false;
                            foreach (var x in jObjectResponse)
                            {
                                if (x.Key.Equals("success"))
                                {
                                    isSuccess = Convert.ToBoolean(x.Value.ToString());
                                }
                            }
                            if (isSuccess)
                            {
                                foreach (var x in jObjectResponse)
                                {
                                    if (x.Key.Equals("data"))
                                    {
                                        if (x.Value.ToString() != "[]")
                                        {
                                            lblNoDataFound.Invoke(new Action(delegate ()
                                            {
                                                lblNoDataFound.Visible = false;
                                            }));
                                            JArray jsonArray = JArray.Parse(x.Value.ToString());
                                            for (int i = 0; i < jsonArray.Count(); i++)
                                            {
                                                JObject jObjectData = JObject.Parse(jsonArray[i].ToString());
                                                int id = 0;
                                                string username = "";
                                                foreach (var y in jObjectData)
                                                {
                                                    if (y.Key.Equals("id"))
                                                    {
                                                        id = Convert.ToInt32(y.Value.ToString());
                                                    }
                                                    else if (y.Key.Equals("username"))
                                                    {
                                                        username = y.Value.ToString();
                                                    }
                                                }
                                                dtSalesAgent.Rows.Add(id, username);
                                                cmbsales.Invoke(new Action(delegate ()
                                                {
                                                    cmbsales.Items.Add(username);
                                                }));
                                            }
                                        }
                                    }
                                }
                                cmbsales.Invoke(new Action(delegate ()
                                {
                                    cmbsales.SelectedIndex = 0;
                                }));
                            }
                            else
                            {
                                string msg = "No message response found";
                                foreach (var x in jObjectResponse)
                                {
                                    if (x.Key.Equals("message"))
                                    {
                                        msg = x.Value.ToString();
                                    }
                                }
                                if (msg.Equals("Token is invalid"))
                                {
                                    Cursor.Current = Cursors.Default;
                                    MessageBox.Show("Your login session is expired. Please login again", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show(response.Content.ToString(), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        public async Task loadBranches()
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

        public string findCode(string value, string typee)
        {
            string result = "";
            if (typee.Equals("Warehouse"))
            {
                foreach (DataRow row in dtWarehouse.Rows)
                {
                    if (row["whsename"].ToString() == value)
                    {
                        result = row["whsecode"].ToString();
                        break;
                    }
                }
            }
            else
            {
                foreach (DataRow row in dtBranches.Rows)
                {
                    if (row["name"].ToString() == value)
                    {
                        result = row["code"].ToString();
                        break;
                    }
                }
            }
            return result;
        }

        public async Task loadData(string subURL)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Login.jsonResult != null)
            {
                string token = "", branch = "";
                foreach (var x in Login.jsonResult)
                {
                    if (x.Key.Equals("token"))
                    {
                        token = x.Value.ToString();
                    }
                    else if (x.Key.Equals("data"))
                    {
                        JObject jObjectData = JObject.Parse(x.Value.ToString());
                        foreach (var y in jObjectData)
                        {
                            if (y.Key.Equals("branch"))
                            {
                                branch = y.Value.ToString();
                            }
                        }
                    }
                }
                if (!token.Equals(""))
                {
                    dgvitems.Invoke(new Action(delegate ()
                    {
                        dgvitems.Rows.Clear();
                    }));
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;

                    int selectedSalesAgentID = 0;
                    foreach (DataRow r0w in dtSalesAgent.Rows)
                    {
                        cmbsales.Invoke(new Action(delegate ()
                        {
                            if (r0w["username"].Equals(cmbsales.Text))
                            {
                                selectedSalesAgentID = Convert.ToInt32(r0w["id"]);
                            }
                        }));
                    }
                    string sUserID = "";
                    if (selectedSalesAgentID != 0)
                    {
                        sUserID = selectedSalesAgentID.ToString();
                    }
                    string arSalesDate = "";
                    string sSearch = "";

                    bool chckDate = false, chckToDate = false, isDtFromDateVisible = false;
                    string sDtFromDate = "", sDtToDate = "", brnch = "";
                    checkDate.Invoke(new Action(delegate ()
                    {
                        chckDate = checkDate.Checked;
                    }));
                    checkToDate.Invoke(new Action(delegate ()
                    {
                        chckToDate = checkToDate.Checked;
                    }));
                    checkToDate.Invoke(new Action(delegate ()
                    {
                        chckToDate = checkToDate.Checked;
                    }));
                    dtFromDate.Invoke(new Action(delegate ()
                    {
                        sDtFromDate = dtFromDate.Value.ToString("yyyy-MM-dd");
                        isDtFromDateVisible = dtFromDate.Visible;
                    }));
                    dtToDate.Invoke(new Action(delegate ()
                    {
                        sDtToDate = dtToDate.Value.ToString("yyyy-MM-dd");
                    }));
                    cmbBranches.Invoke(new Action(delegate ()
                    {
                        brnch = cmbBranches.Text;
                    }));

                    if (gSalesType == "AR Sales" && label1.Visible == true && isDtFromDateVisible)
                    {
                        arSalesDate = !chckDate ? "&from_date=" : "&from_date=" + sDtFromDate;
                        arSalesDate += !chckToDate ? "&to_date=" : "&to_date=" + sDtToDate;
                    }
                    else if (gForType.ToLower().Equals("for payment"))
                    {
                        arSalesDate = !chckDate ? "&from_date=" : "&from_date=" + sDtFromDate;
                        arSalesDate += !chckToDate ? "&to_date=" : "&to_date=" + sDtToDate;
                    }
                    sSearch = "&search=" + txtsearch.Text.Trim();
                    string sBranch = "&branch=" + findCode(brnch, "Branch");
                    string sCmbFromTime = "", sCmbToTime = "";
                    cmbFromTime.Invoke(new Action(delegate ()
                    {
                        sCmbFromTime = cmbFromTime.Text;
                    }));
                    string sFromTime = cFromTime > 0 ? "&from_time=" : "&from_time=" + sCmbFromTime;
                    cmbToTime.Invoke(new Action(delegate ()
                    {
                        sCmbToTime = cmbToTime.Text;
                    }));
                    string sToTime = cToTime > 0 ? "&to_time=" : "&to_time=" + sCmbToTime;

                    string forQuery = subURL + "?transtype=" + gSalesType + "&user_id=" + sUserID + sSearch + arSalesDate + sBranch + sFromTime + sToTime;

                    string sCustType = "", sCustomerType = "";
                    int sCustomerTypeIndex = 0;
                    cmbCustomerType.Invoke(new Action(delegate ()
                    {
                        sCustomerType = cmbCustomerType.Text;
                        sCustomerTypeIndex = cmbCustomerType.SelectedIndex;
                    }));
                    if (sCustomerType == "All" || sCustomerTypeIndex == 0)
                    {
                        sCustType = "&cust_type=";
                    }
                    else
                    {
                        foreach (DataRow row in dtCustType.Rows)
                        {
                            if (row["code"].ToString() == cmbCustomerType.Text)
                            {
                                sCustType = "&cust_type=" + row["id"].ToString();
                                break;
                            }
                        }
                    }

                    string resultQuery;
                    if (gForType.Equals("for SAP"))
                    {
                        resultQuery = "/api/sales/for_sap/get_all?from_date=" + (!checkDate.Checked ? "" : dtFromDate.Value.ToString("yyyy-MM-dd")) + "&to_date=" + (!checkToDate.Checked ? "" : dtToDate.Value.ToString("yyyy-MM-dd")) + "&transtype=" + gSalesType + "&search=" + txtsearch.Text + "&created_by=" + sUserID + sFromTime + sToTime + sBranch;
                    }
                    else
                    {
                        resultQuery = forQuery;
                    }
                    resultQuery += sCustType;
                    // 
                    //Console.WriteLine(resultQuery);
                    var request = new RestRequest(resultQuery);
                    request.AddHeader("Authorization", "Bearer " + token);
                    Task<IRestResponse> t = client.ExecuteAsync(request);
                    t.Wait();
                    var response = await t;
                    JObject jObject = new JObject();

                    if (response.ErrorMessage == null)
                    {
                        jObject = JObject.Parse(response.Content.ToString());
                        dgvOrders.Invoke(new Action(delegate ()
                        {
                            dgvOrders.Rows.Clear();
                        }));
                        dgvitems.Invoke(new Action(delegate ()
                        {
                            dgvitems.Rows.Clear();
                        }));
                         this.Invoke(new Action(delegate ()
                         {
                             clearBillsField();
                         }));

                        bool isSuccess = false;
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("success"))
                            {
                                isSuccess = Convert.ToBoolean(x.Value.ToString());
                            }
                        }
                        if (isSuccess)
                        {
                            foreach (var x in jObject)
                            {
                                if (x.Key.Equals("data"))
                                {
                                    if (x.Value.ToString() != "[]")
                                    {
                                        lblNoDataFound.Invoke(new Action(delegate ()
                                        {
                                            lblNoDataFound.Visible = false;
                                        }));
                                        JArray jsonArray = JArray.Parse(x.Value.ToString());
                                        for (int i = 0; i < jsonArray.Count(); i++)
                                        {
                                            string forSAPAmountColumnName = (gForType.Equals("for SAP") ? "doctotal" : "amount_due");
                                            JObject data = JObject.Parse(jsonArray[i].ToString());
                                            int id = 0, transNumber = 0, daysDue = 0;
                                            string referenceNumber = "", salesAgent = "N/A", cust_code = "";
                                            decimal amountDue = 0, tenderAmount = 0, discAmt = 0;
                                            DateTime dtTransDate = new DateTime();
                                            foreach (var q in data)
                                            {
                                                if (q.Key.Equals("reference"))
                                                {
                                                    referenceNumber = q.Value.ToString();
                                                }
                                                else if (q.Key.Equals("disc_amount"))
                                                {
                                                    discAmt = decimal.Round(Convert.ToDecimal(q.Value.ToString()), 2, MidpointRounding.AwayFromZero);
                                                }
                                                else if (q.Key.Equals("cust_code"))
                                                {
                                                    cust_code = q.Value.ToString();
                                                }
                                                if (q.Key.Equals(forSAPAmountColumnName))
                                                {
                                                    //Console.WriteLine(q.Value);
                                                    amountDue = decimal.Round(Convert.ToDecimal(q.Value.ToString()), 2, MidpointRounding.AwayFromZero);
                                                }
                                                else if (q.Key.Equals("id"))
                                                {
                                                    id = Convert.ToInt32(q.Value.ToString());
                                                }

                                                else if (q.Key.Equals("transnumber"))
                                                {
                                                    transNumber = Convert.ToInt32(q.Value.ToString());
                                                }
                                                else if (q.Key.Equals("tenderamt"))
                                                {
                                                    tenderAmount = decimal.Round(Convert.ToDecimal(q.Value.ToString()), 2, MidpointRounding.AwayFromZero);
                                                }
                                                else if (q.Key.Equals("user"))
                                                {
                                                    salesAgent = q.Value.ToString();
                                                }
                                                else if (q.Key.Equals("days_due"))
                                                {
                                                    double toDouble = Convert.ToDouble(q.Value.ToString());
                                                    daysDue = Convert.ToInt32(toDouble);
                                                }
                                                else if (q.Key.Equals("transdate"))
                                                {
                                                    string replaceT = q.Value.ToString().Replace("T", "");
                                                    dtTransDate = Convert.ToDateTime(replaceT);
                                                }
                                            }
                                            dgvOrders.Invoke(new Action(delegate ()
                                            {
                                                dgvOrders.Rows.Add(false, id, transNumber, referenceNumber, Convert.ToDecimal(string.Format("{0:0.00}", amountDue)), Convert.ToDecimal(string.Format("{0:0.00}", tenderAmount)), Convert.ToDecimal(string.Format("{0:0.00}", discAmt)), Convert.ToDecimal(string.Format("{0:0.00}", amountDue)), cust_code, salesAgent, "", dtTransDate.ToString("yyyy-MM-dd HH:mm"), daysDue);
                                            }));
                                            dtSearch.Rows.Add(cust_code, "Customer");
                                            dtSearch.Rows.Add(referenceNumber, "Transnum");
                                        }
                                    }
                                    else
                                    {
                                        lblNoDataFound.Invoke(new Action(delegate ()
                                        {
                                            lblNoDataFound.Visible = true;
                                        }));
                                        lblOrderCount.Invoke(new Action(delegate ()
                                        {
                                            lblOrderCount.Text = "Orders (0)";
                                        }));
                                        lblpendingamount.Invoke(new Action(delegate ()
                                        {
                                            lblpendingamount.Text = "Selected Amount: 0.00";
                                        }));
                                        dgvitems.Invoke(new Action(delegate ()
                                        {
                                            dgvitems.Rows.Clear();
                                        }));
                                        this.Invoke(new Action(delegate ()
                                        {
                                            clearBillsField();
                                        }));
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblOrderCount.Text = "Orders (0)";
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
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Your login session is expired. Please login again", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                lblOrderCount.Invoke(new Action(delegate ()
                                {
                                    lblOrderCount.Text = "Orders (0)";
                                }));
                                lblpendingamount.Invoke(new Action(delegate ()
                                {
                                    lblpendingamount.Text = "Selected Amount: 0.00";
                                }));
                                dgvitems.Invoke(new Action(delegate ()
                                {
                                    dgvitems.Rows.Clear();
                                }));
                                this.Invoke(new Action(delegate ()
                                {
                                    clearBillsField();
                                }));
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                lblOrderCount.Invoke(new Action(delegate ()
                                {
                                    lblOrderCount.Text = "Orders (0)";
                                }));
                                lblpendingamount.Invoke(new Action(delegate ()
                                {
                                    lblpendingamount.Text = "Selected Amount: 0.00";
                                }));
                                dgvitems.Invoke(new Action(delegate ()
                                {
                                    dgvitems.Rows.Clear();
                                }));
                                this.Invoke(new Action(delegate ()
                                {
                                    clearBillsField();
                                }));
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            dgvOrders.Columns["amountdue"].HeaderText = (gForType.Equals("for SAP") ? "Doc. Total" : "Balance Due");
            highlightHaveDiscount();
            searchFilter();
            lblOrderCount.Invoke(new Action(delegate ()
            {
                lblOrderCount.Text = "Orders (" + dgvOrders.Rows.Count.ToString("N0") + ")";
            }));
            Cursor.Current = Cursors.Default;
        }

        public void highlightHaveDiscount()
        {
            if (dgvOrders.Rows.Count > 0)
            {
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToDouble(dgvOrders.Rows[i].Cells["discount_amount"].Value.ToString()) > 0)
                    {
                        dgvOrders.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                        dgvOrders.Rows[i].Cells["btnEditTotalPayment"].Style.BackColor = Color.FromArgb(192, 0, 192);
                        dgvOrders.Rows[i].Cells["selectt"].Style.BackColor = Color.White;
                    }
                }
            }
        }

        public void searchFilter()
        {
            AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
            cmbsales.Invoke(new Action(delegate ()
            {
                if (cmbSearchType.SelectedIndex == 0)
                {
                    foreach (DataRow row in dtSearch.Rows)
                    {
                        if (row["type"].ToString().Equals("Transnum"))
                        {
                            auto.Add(row["search"].ToString());
                        }
                    }
                }
                else
                {
                    foreach (DataRow row in dtSearch.Rows)
                    {
                        if (row["type"].ToString().Equals("Customer"))
                        {
                            auto.Add(row["search"].ToString());
                        }
                    }
                }
            }));
            txtsearch.Invoke(new Action(delegate ()
            {
                txtsearch.AutoCompleteCustomSource = auto;
            }));
        }

        public void clearBillsField()
        {
            txtGrossPrice.Text = "0.00";
            txtDiscountAmount.Text = "0.00";
            txtlAmountPayable.Text = "0.00";
            txtTotalPayment.Text = "0.00";
            txtChange.Text = "0.00";
            checkSelectAll.Checked = false;
        }

        public void selectOrders(bool value)
        {
            Cursor.Current = Cursors.WaitCursor;
            //DataTable dt = new DataTable();
            if (Login.jsonResult != null)
            {
                dgvitems.Rows.Clear();
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
                   
                    dgvOrders.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    double totalPendingAmount = 0.00;
                    //string ids /*= "";*/
                    //int isCheckAll_int = 0;
                    if (value)
                    {
                        for (int i = 0; i < dgvOrders.Rows.Count; i++)
                        {
                            dgvOrders.Rows[i].Cells["selectt"].Value = checkSelectAll.Checked;
                        }
                    }
                    else
                    {
                        int isCheckAll_int = 0;
                        for (int i = 0; i < dgvOrders.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                            {
                                isCheckAll_int += 1;
                            }
                        }
                        if (checkSelectAll.Checked && !isCheckAll_int.Equals(dgvOrders.Rows.Count))
                        {
                            cCheck = 1;
                            checkSelectAll.Checked = false;
                        }
                        else if (!checkSelectAll.Checked && isCheckAll_int.Equals(dgvOrders.Rows.Count))
                        {
                            checkSelectAll.Checked = true;
                        }
                    }

                    JArray jarrayBody = new JArray();
                    for (int i = 0; i < dgvOrders.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                        {
                            totalPendingAmount += Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString());
                            //ids += dgvOrders.Rows[i].Cells["base_id"].Value.ToString() + ",";
                            jarrayBody.Add(Convert.ToInt32(dgvOrders.Rows[i].Cells["base_id"].Value));
                            //isCheckAll_int += 1;
                        }
                    }
                    //Console.WriteLine(jarrayBody);
                    //checkSelectAll.Checked = isCheckAll_int.Equals(dgvOrders.Rows.Count) ? true : false;

                    //MessageBox.Show(isCheckAll_int.ToString() + "/" + dgvOrders.Rows.Count.ToString());

                    //ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(0, ids.Length - 1));
                    lblpendingamount.Text = "Pending Amount: " + totalPendingAmount.ToString("n2");
                    dgvitems.Rows.Clear();
                    string sDate = gForType.Equals("for Payment") ? "&transdate=" : "&transdate=" + dtFromDate.Value.ToString("yyyy-MM-dd");
                    JObject jsonObjectBody = new JObject();  
                    jsonObjectBody.Add("ids", jarrayBody);
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/sales/summary_trans?transtype=" +  gSalesType + sDate);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddParameter("application/json", jsonObjectBody, ParameterType.RequestBody);
                    request.Method = Method.PUT;
                    var response = client.Execute(request);
                    //Console.WriteLine("/api/sales/summary_trans?transtype=" + gSalesType + sDate);
                    //Console.WriteLine(jsonObjectBody);
                    if (response.ErrorMessage == null)
                    {
                        JObject jObject = new JObject();
                        jObject = JObject.Parse(response.Content.ToString());
                        bool isSuccess = false;
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("success"))
                            {
                                isSuccess = Convert.ToBoolean(x.Value.ToString());
                            }
                        }
                        if (isSuccess)
                        {
                            string forSAPAmountColumnName = (gForType.Equals("for SAP") ? "doctotal" : "amount_due");
                            foreach (var x in jObject)
                            {
                                if (x.Key.Equals("data"))
                                {
                                    if (x.Value.ToString() != "{}")
                                    {
                                        JObject jObjectData = JObject.Parse(x.Value.ToString());
                                        foreach (var y in jObjectData)
                                        {

                                            if (y.Key.Equals("header"))
                                            {
                                                JObject jObjectHeader = JObject.Parse(y.Value.ToString());
                                                foreach (var z in jObjectHeader)
                                                {
                                                    if (z.Key.Equals("gross"))
                                                    {
                                                        txtGrossPrice.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                    }
                                                    else if (z.Key.Equals("disc_amount"))
                                                    {
                                                        txtDiscountAmount.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                    }
                                                    else if (z.Key.Equals("disc_amount"))
                                                    {
                                                        txtDiscountAmount.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                    }
                                                    else if (z.Key.Equals(forSAPAmountColumnName))
                                                    {
                                                        txtlAmountPayable.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                    }
                                                    else if (z.Key.Equals("change"))
                                                    {
                                                        txtChange.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                    }
                                                }
                                            }
                                            else if (y.Key.Equals("row"))
                                            {
                                                JArray jArrayRow = JArray.Parse(y.Value.ToString());
                                                for (int i = 0; i < jArrayRow.Count(); i++)
                                                {
                                                    JObject data = JObject.Parse(jArrayRow[i].ToString());
                                                    String itemName = "";
                                                    double quantity = 0.00, price = 0.00, discountPercent = 0.00, totalPrice = 0.00, discamt = 0.00;
                                                    bool free = false;
                                                    foreach (var z in data)
                                                    {
                                                        if (z.Key.Equals("item_code"))
                                                        {
                                                            itemName = z.Value.ToString();
                                                        }
                                                        else if (z.Key.Equals("quantity"))
                                                        {
                                                            quantity = Convert.ToDouble(z.Value.ToString());
                                                        }
                                                        else if (z.Key.Equals("unit_price"))
                                                        {
                                                            price = Convert.ToDouble(z.Value.ToString());
                                                        }
                                                        else if (z.Key.Equals("discprcnt"))
                                                        {
                                                            discountPercent = Convert.ToDouble(z.Value.ToString());
                                                        }
                                                        else if (z.Key.Equals("linetotal"))
                                                        {
                                                            totalPrice = Convert.ToDouble(z.Value.ToString());
                                                        }
                                                        else if (z.Key.Equals("free"))
                                                        {
                                                            free = Convert.ToBoolean(z.Value.ToString());
                                                        }

                                                        else if (z.Key.Equals("disc_amount"))
                                                        {
                                                            discamt = Convert.ToDouble(z.Value.ToString());
                                                        }
                                                    }
                                                    dgvitems.Invoke(new Action(delegate ()
                                                    {
                                                        dgvitems.Rows.Add(itemName, Convert.ToDecimal(string.Format("{0:0.00}", quantity)), Convert.ToDecimal(string.Format("{0:0.00}", price)), Convert.ToDecimal(string.Format("{0:0.00}", discountPercent)), Convert.ToDecimal(string.Format("{0:0.00}", discamt)), Convert.ToDecimal(string.Format("{0:0.00}", totalPrice)), free);
                                                    }));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            lblItemsCount.Text = "Items (" + dgvitems.Rows.Count.ToString("N0") + ")";
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
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Your login session is expired. Please login again", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
            computeChange("POS","Cell Click");

            //int int_selectAll = 0;
            //for (int i = 0; i < dgvOrders.Rows.Count; i++)
            //{
            //    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
            //    {
            //        int_selectAll += 1;
            //    }
            //}
            //if (int_selectAll <= 0 && checkSelectAll.Checked)
            //{
            //    checkSelectAll.Checked = false;
            //}
        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrders.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0)
                {
                    selectOrders(false);
                }
                else if (e.ColumnIndex == 13)
                {
                    dgvOrders.CurrentCell = dgvOrders[0, dgvOrders.CurrentRow.Index];
                    dgvOrders.ClearSelection();
                    dgvOrders.CurrentRow.Cells["total_payment"].Selected = true;
                    EnterAmount frm = new EnterAmount();
                    EnterAmount.amount = 0.00;
                    EnterAmount.amount = Convert.ToDouble(dgvOrders.CurrentRow.Cells["total_payment"].Value.ToString());
                    frm.reference = dgvOrders.CurrentRow.Cells["ordernum"].Value.ToString();
                    frm.ShowDialog();
                    dgvOrders.CurrentRow.Cells["total_payment"].Value = Convert.ToDecimal(string.Format("{0:0.00}", EnterAmount.amount));
                    computeChange("POS","");
                }
            }
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgvOrders.Rows.Count <= 0)
            {
                MessageBox.Show("No order found", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int countSelected = 0;
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                    {
                        countSelected += 1;
                    }
                }
                if (countSelected <= 0)
                {
                    MessageBox.Show("No order selected", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to " + (gForType == "for Payment" ? "pay" : "confirm") + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (gForType == "for Payment")
                        {
                            if (insertTransaction(true, 0))
                            {
                                dtSelectedDeposit.Rows.Clear();
                                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                                await Task.Run(() => loadData(subURL));
                                counts();
                            }
                        }
                        else if (gForType == "for Confirmation")
                        {
                            confirmARSales();
                        }
                        else if (gForType == "for SAP")
                        {
                            string ids = "";
                            int int_hasSelected = 0;
                            for (int i = 0; i < dgvOrders.Rows.Count; i++)
                            {
                                if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                                {
                                    ids = ids + "," + dgvOrders.Rows[i].Cells["base_id"].Value.ToString();
                                    int_hasSelected += 1;
                                }
                            }
                            ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(1));
                            if(int_hasSelected > 0)
                            {
                                forSAPAR_SAPNumber forSAPAR_SAPNumber = new forSAPAR_SAPNumber();
                                forSAPAR_SAPNumber.ids = ids;
                                forSAPAR_SAPNumber.ShowDialog();
                                if (forSAPAR_SAPNumber.isSubmit)
                                {
                                    string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                                    await Task.Run(() => loadData(subURL));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void counts()
        {
            if (Login.jsonResult != null)
            {
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
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                   
                    var request = new RestRequest("/api/sales/count?transdate=" + dtFromDate.Value.ToString("yyyy-MM-dd"));
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.GET;

                    var response = client.Execute(request);

                    if (response.ErrorMessage == null)
                    {
                        JObject jObjectResponse = JObject.Parse(response.Content);
                        bool isSuccess = false;
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("success"))
                            {
                                isSuccess = true;
                                break;
                            }
                        }

                        string msg = "No message response found";
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(msg))
                        {
                            MessageBox.Show(msg, isSuccess ? "Success" : "Validation", MessageBoxButtons.OK, isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                        }
                        if (isSuccess)
                        {
                            int for_payment = 0, for_confirmation = 0;
                            foreach (var x in jObjectResponse)
                            {
                                if (x.Key.Equals("data"))
                                {
                                    if (x.Value.ToString() != "[]")
                                    {
                                        JObject data = JObject.Parse(x.Value.ToString());
                                        foreach (var q in data)
                                        {
                                            if (q.Key.Equals("for_payment"))
                                            {
                                                for_payment = Convert.ToInt32(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("for_confirmation"))
                                            {
                                                for_confirmation = Convert.ToInt32(q.Value.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            btnForPayment.Text = "For Payment (" + for_payment.ToString("N0") + ")";
                            btnForConfirmation.Text = "For AR Confirmation (" + for_confirmation.ToString("N0") + ")";
                        }
                    }
                    else
                    {
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        public async void confirmARSales()
        {
            if (dgvOrders.Rows.Count <= 0)
            {
                MessageBox.Show("No order found", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int countSelected = 0;
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                    {
                        countSelected += 1;
                    }
                }
                if (countSelected <= 0)
                {
                    MessageBox.Show("No order selected", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string ids = "";
                    for (int i = 0; i < dgvOrders.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                        {
                            //totalPendingAmount += Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString());
                            ids = ids + "," + dgvOrders.Rows[i].Cells["base_id"].Value.ToString();
                        }
                    }
                    ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(1));

                    if (Login.jsonResult != null)
                    {
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
                            var client = new RestClient(utilityc.URL);
                            client.Timeout = -1;
                            var request = new RestRequest("/api/sales/confirm?ids=%5B" + ids + "%5D");
                            request.AddHeader("Authorization", "Bearer " + token);
                            request.Method = Method.PUT;

                            var response = client.Execute(request);

                            if(response.ErrorMessage == null)
                            {
                                JObject jObjectResponse = JObject.Parse(response.Content);
                                bool isSuccess = false;
                                foreach (var x in jObjectResponse)
                                {
                                    if (x.Key.Equals("success"))
                                    {
                                        isSuccess = true;
                                        break;
                                    }
                                }

                                string msg = "No message response found";
                                foreach (var x in jObjectResponse)
                                {
                                    if (x.Key.Equals("message"))
                                    {
                                        msg = x.Value.ToString();
                                    }
                                }
                                MessageBox.Show(msg, isSuccess ? "Success" : "Validation", MessageBoxButtons.OK, isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                                if (isSuccess)
                                {
                                    string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                                    await Task.Run(() => loadData(subURL));
                                    counts();
                                }
                            }
                            else
                            {
                                MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
        }

        public bool insertTransaction(bool isMultipleTransaction, double ARSalesAmount)
        {
            bool result = false;
            Cursor.Current = Cursors.WaitCursor;
            if (Login.jsonResult != null)
            {
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
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/payment/new");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.POST;
                    JObject joBody = new JObject();
                    if (isMultipleTransaction)
                    {
                        DataTable dtSelectedDepositTEMP = new DataTable();
                        dtSelectedDepositTEMP.Columns.Add("id");
                        dtSelectedDepositTEMP.Columns.Add("amount");
                        dtSelectedDepositTEMP.Columns.Add("payment_type");
                        dtSelectedDepositTEMP.Columns.Add("sapnum");
                        dtSelectedDepositTEMP.Columns.Add("reference2");

                        foreach (DataRow row in dtSelectedDeposit.Rows)
                        {
                            dtSelectedDepositTEMP.Rows.Add(row["id"], row["amount"], row["payment_type"], row["sapnum"], row["reference2"]);
                        }
                        JObject joHeader = new JObject();
                        JArray jaPaymentRows = new JArray();
                        JArray jaSalesDetails = new JArray();
                        string headerCustCode = "";
                        //LOOP payment types
                        double paymentMethodAmount = 0.00;
                        foreach (DataRow row in dtSelectedDepositTEMP.Rows)
                        {
                            JObject joPaymentRows = new JObject();
                            if(row["id"].ToString() != "")
                            {
                                joPaymentRows.Add("deposit_id", Convert.ToInt32(row["id"].ToString()));
                            }
                            joPaymentRows.Add("payment_type", row["payment_type"].ToString());
                            double amt = string.IsNullOrEmpty(row["amount"].ToString()) ? 0.00 : Convert.ToDouble(row["amount"].ToString());

                            joPaymentRows.Add("amount", amt);
                            paymentMethodAmount += Convert.ToDouble(row["amount"].ToString());
                            if (string.IsNullOrEmpty(row["sapnum"].ToString()))
                            {
                                joPaymentRows.Add("sap_number", null);
                            }
                            else
                            {
                                joPaymentRows.Add("sap_number", Convert.ToInt32(row["sapnum"].ToString()));
                            }
                            joPaymentRows.Add("reference2", string.IsNullOrEmpty(row["reference2"].ToString()) ? null : row["reference2"].ToString());
                            jaPaymentRows.Add(joPaymentRows);
                        }
                        double totalAmount = 0.00;
                        int firstLoop = 0;
                        for (int i = 0; i < dgvOrders.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                            {
                                firstLoop += 1;
                                if (firstLoop == 1)
                                {
                                    //MessageBox.Show(dgvOrders.Rows[i].Cells["cust_code"].Value.ToString());
                                    headerCustCode = dgvOrders.Rows[i].Cells["cust_code"].Value.ToString();
                                }
                                JObject joSalesDetails = new JObject();
                                joSalesDetails.Add("sales_id", Convert.ToInt32(dgvOrders.Rows[i].Cells["base_id"].Value.ToString()));
                                joSalesDetails.Add("total_payment", Convert.ToDouble(dgvOrders.Rows[i].Cells["total_payment"].Value.ToString()));
                                totalAmount += Convert.ToDouble(dgvOrders.Rows[i].Cells["total_payment"].Value.ToString());
                                jaSalesDetails.Add(joSalesDetails);
                            }
                        }
                        joHeader.Add("cust_code", headerCustCode);
                        joHeader.Add("transdate", DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                        joHeader.Add("remarks", null);
                        joHeader.Add("reference", null);
                        joBody.Add("header", joHeader);
                        joBody.Add("payment_rows", jaPaymentRows);
                        joBody.Add("sales_details", jaSalesDetails);
                    }
                    Console.WriteLine(joBody);
                    request.AddParameter("application/json", joBody, ParameterType.RequestBody);
                    var response = client.Execute(request);
                    JObject jObjectResponse = JObject.Parse(response.Content);

                    foreach (var x in jObjectResponse)
                    {
                        if (x.Key.Equals("success"))
                        {
                            if (Convert.ToBoolean(x.Value.ToString()))
                            {
                                result = true;
                            }
                        }
                    }

                    if (!result)
                    {
                        string msg = "Object message key not found";
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }
                        if (msg.Equals("Token is invalid"))
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Your login session is expired. Please login again", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        string msg = "Object message key not found";
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
            return result;
        }

        private async void btnsearch_Click(object sender, EventArgs e)
        {
            string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
            await Task.Run(() => loadData(subURL));
        }

        private async void txtsearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter)) {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private async void cmbsales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cSales <= 0)
            {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private async void btnrefresh_Click(object sender, EventArgs e)
        {
            try
            {
                counts();
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void btnVoid_Click(object sender, EventArgs e)
        {
            if (dgvOrders.Rows.Count <= 0)
            {
                MessageBox.Show("No order found", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int countSelected = 0;
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                    {
                        countSelected += 1;
                    }
                }
                if (countSelected <= 0)
                {
                    MessageBox.Show("No order selected", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string ids = "";
                    int count = 0;
                    string selectedRef = "Selected #: " + Environment.NewLine;
                    for (int i = 0; i < dgvOrders.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                        {
                            //totalPendingAmount += Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString());
                            ids = ids + "," + dgvOrders.Rows[i].Cells["base_id"].Value.ToString();
                            selectedRef += dgvOrders.Rows[i].Cells["ordernum"].Value.ToString() + Environment.NewLine;
                            count += 1;
                        }
                    }
                    ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(1));

                    voidForm voidd = new voidForm();
                    //voidd.baseNum = Convert.ToInt32(dgvOrders.CurrentRow.Cells["transnumber"].Value.ToString());
                    voidd.lblOrderNumber.Text = "(" + count.ToString("N0") + ")";
                    voidd.selectedReference = selectedRef;
                    voidd.selectedID = ids;
                    voidd.ShowDialog();
                    if (voidForm.isSubmit)
                    {
                        string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                        await Task.Run(() => loadData(subURL));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectAdvancePayment selectAdvancePayment = new SelectAdvancePayment();
            selectAdvancePayment.ShowDialog();
            computeChange("POS","");
        }

        public void computeChange(string type, string type2)
        {
            //dtSelectedDeposit.Columns.Add("id");
            //dtSelectedDeposit.Columns.Add("amount");
            //dtSelectedDeposit.Columns.Add("payment_type");
            //dtSelectedDeposit.Columns.Add("sapnum");
            //dtSelectedDeposit.Columns.Add("reference2");
            DataTable dtTemp = dtSelectedDeposit;
            double selectedAmount = 0.00, totalAmtPayable = 0.00;
            if (type.Equals("POS"))
            {
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                    {
                        selectedAmount += string.IsNullOrEmpty(dgvOrders.Rows[i].Cells["tenderamount"].Value.ToString()) ? 0.00 : Convert.ToDouble(dgvOrders.Rows[i].Cells["tenderamount"].Value.ToString());
                        totalAmtPayable += string.IsNullOrEmpty(dgvOrders.Rows[i].Cells["total_payment"].Value.ToString()) ? 0.00 : Convert.ToDouble(dgvOrders.Rows[i].Cells["total_payment"].Value.ToString());
                    }
                }
            }
            else
            {
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                    {
                        totalAmtPayable += string.IsNullOrEmpty(dgvOrders.Rows[i].Cells["total_payment"].Value.ToString()) ? 0.00 : Convert.ToDouble(dgvOrders.Rows[i].Cells["total_payment"].Value.ToString());
                    }
                }
                foreach (DataRow row in dtTemp.Rows)
                {
                    if (row["type"].ToString().Equals("POS"))
                    {
                        selectedAmount += string.IsNullOrEmpty(row["amount"].ToString()) ? 0.00 : Convert.ToDouble(row["amount"].ToString());
                    }
                }
            }
            txtlAmountPayable.Text = totalAmtPayable.ToString("n2");
            double deletedAmount = 0.00;
            bool hasPOS = false;
            if(dtTemp.Rows.Count > 0)
            {
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    DataRow row = dtTemp.Rows[i];
                    if (row["type"].ToString().Equals("POS"))
                    {
                        deletedAmount = Convert.ToDouble(row["amount"].ToString());
                        dtSelectedDeposit.Rows.RemoveAt(i);
                        hasPOS = true;
                    }
                }
            }

            if(type2.Equals("Cell Click") && selectedAmount > 0)
            {
                dtSelectedDeposit.Rows.Add(null, selectedAmount, "CASH", "", "", "POS");
            }
            else if((selectedAmount > 0 &&  hasPOS))
            {
                selectedAmount -= deletedAmount == selectedAmount ? 0.00 : deletedAmount;
                dtSelectedDeposit.Rows.Add(null, selectedAmount, "CASH", "", "", "POS");
            }

            double totalPayment = 0.00;
            if (dtTemp.Rows.Count > 0)
            {
                foreach (DataRow row in dtTemp.Rows)
                {
                    totalPayment += (string.IsNullOrEmpty(row["amount"].ToString()) ? 0.00 : Convert.ToDouble(row["amount"].ToString()));
                }
            }
            txtTotalPayment.Text = totalPayment.ToString("n2");
            double num1 = (string.IsNullOrEmpty(txtTotalPayment.Text) ? 0.00 : Convert.ToDouble(txtTotalPayment.Text));
            double amountDue = (string.IsNullOrEmpty(txtlAmountPayable.Text) ? 0.00 : Convert.ToDouble(txtlAmountPayable.Text)); 
            double change = num1 - amountDue;
            txtChange.Text = (change > 0 ? change : 0.00).ToString("n2");
        }

        private async void cmbFromTime_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cFromTime <= 0)
            {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private async void cmbToTime_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cToTime <= 0)
            {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private void cmbBranches_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvitems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvitems.Rows.Count > 0 && dgvOrders.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    try
                    {
                        JArray jarrayBody = new JArray();
                        for (int i = 0; i < dgvOrders.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                            {
                                jarrayBody.Add(Convert.ToInt32(dgvOrders.Rows[i].Cells["base_id"].Value));
                            }
                        }

                        Cursor.Current = Cursors.WaitCursor;
                        if (Login.jsonResult != null)
                        {
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
                                var client = new RestClient(utilityc.URL);
                                client.Timeout = -1;
                                JObject jsonObjectBody = new JObject();
                                jsonObjectBody.Add("ids", jarrayBody);
                                jsonObjectBody.Add("discount", Convert.ToDouble(dgvitems.CurrentRow.Cells["discpercent"].Value.ToString()));
                                jsonObjectBody.Add("item_code", dgvitems.CurrentRow.Cells["item"].Value.ToString());
                                var request = new RestRequest("/api/sales/item/transaction/details");
                                request.AddHeader("Authorization", "Bearer " + token);
    
                                request.AddParameter("application/json", jsonObjectBody, ParameterType.RequestBody);
                                request.Method = Method.PUT;
                                var response = client.Execute(request);
                                ItemDiscount itemDisc = new ItemDiscount();
                                if (response.ErrorMessage == null)
                                {
                                    itemDisc.jsonResponse = response.Content.ToString();
                                }
                                else
                                {
                                    itemDisc.jsonResponse = response.ErrorMessage;
                                }
                                itemDisc.ShowDialog();
                                Cursor.Current = Cursors.Default;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private async void checkDate_CheckedChanged(object sender, EventArgs e)
        {
            if(cDate <= 0)
            {
                dtFromDate.Visible = checkDate.Checked;
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private void cmbFromTime_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void checkToDate_CheckedChanged(object sender, EventArgs e)
        {
           if(cToDate <= 0)
            {
                dtToDate.Visible = checkToDate.Checked;
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private void PendingOrder2_Leave(object sender, EventArgs e)
        {

        }

        private async void dtToDate_CloseUp(object sender, EventArgs e)
        {
            if (cToDate <= 0)
            {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private async void dtFromDate_CloseUp(object sender, EventArgs e)
        {
            if (cDate <= 0)
            {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private void dgvOrders_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        public bool IsNumeric(string text)
        {
            double test;
            return double.TryParse(text, out test);
        }

        private void dgvOrders_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                if (!IsNumeric(dgvOrders.CurrentRow.Cells["total_payment"].Value.ToString()))
                {
                    MessageBox.Show("Total Payment must be a number", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvOrders.CurrentRow.Cells["total_payment"].Value = "0.00";
                }
            }
        }

        private async void cmbCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cCustType <= 0)
            {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        private void cmbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchFilter();
        }

        public void pay(JObject jObjectPay, DataTable dtDeposit)
        {
            int id = 0;
            double amount = 0.00;
            foreach (var q in jObjectPay)
            {
                if (q.Key.Equals("id"))
                {
                    id = Convert.ToInt32(q.Value.ToString());
                }
                else if (q.Key.Equals("amount"))
                {
                    amount = Convert.ToDouble(q.Value.ToString());
                }
            }
            //DataTable dtRows = new DataTable();
            //dtRows.Columns.Add("id");
            //dtRows.Columns.Add("amount");
           
            while (amount > 0)
            {
                foreach(DataRow row in dtDeposit.Rows)
                {
                    if(Convert.ToDouble(row["amount"].ToString()) != 0.00)
                    {
                        double payment = 0.00;
                        if (Convert.ToDouble(row["amount"]) >= amount)
                        {
                            payment += amount;
                            row["amount"] = (Convert.ToDouble(row["amount"].ToString()) - amount);
                            dtPayment.Rows.Add(row["id"], payment,row["payment_type"], row["sapnum"], row["reference2"]);
                            amount = 0;
                            break;
                        }else if(Convert.ToDouble(row["amount"]) < amount)
                        {
                            payment += Convert.ToDouble(row["amount"]);
                            amount -= Convert.ToDouble(row["amount"].ToString());
                            dtPayment.Rows.Add(row["id"], payment, row["payment_type"], row["sapnum"], row["reference2"]);
                            row["amount"] = 0;
                        }
                    }
                }
                break;
            }
        }

        private async void cmbBranches_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cBranch <= 0)
            {
                await Task.Run(() => loadSalesAgent());
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                await Task.Run(() => loadData(subURL));
            }
        }

        public DataTable getSelectedRows()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("base_id");
            dt.Columns.Add("base_num");
            dt.Columns.Add("cust_code");
            dt.Columns.Add("amountdue");
            dt.Columns.Add("tenderamount");
            dt.Columns.Add("tendertype");
            for (int i = 0; i < dgvOrders.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                {
                    dt.Rows.Add(Convert.ToInt32(dgvOrders.Rows[i].Cells["base_id"].Value.ToString()), dgvOrders.Rows[i].Cells["transnumber"].Value.ToString(), dgvOrders.Rows[i].Cells["cust_code"].Value.ToString(), Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString()), Convert.ToDouble(dgvOrders.Rows[i].Cells["tenderamount"].Value.ToString()), dgvOrders.Rows[i].Cells["tendertype"].Value.ToString());
                }
            }
            return dt;
        }

        //private void btnForPayment_Click(object sender, EventArgs e)
        //{
        //    btnForPayment.ForeColor = Color.Black;
        //    btnForConfirmation.ForeColor = Color.White;
        //    counts();
        //    string subURL = btnForPayment.ForeColor == Color.Black ? "/api/payment/new" : "/api/sales/for_confirm";
        //    btnConfirm.Text = "PAY";
        //    button1.Visible = true;
        //    btnVoid.Visible = true;
        //    btnPaymentMethod.Visible=true;
        //    loadData(subURL);
        //}

        //private void btnForConfirmation_Click(object sender, EventArgs e)
        //{
        //    btnForPayment.ForeColor = Color.White;
        //    btnForConfirmation.ForeColor = Color.Black;
        //    counts();
        //    string subURL = btnForPayment.ForeColor == Color.Black ? "/api/payment/new" : "/api/sales/for_confirm";
        //    btnConfirm.Text = "CONFIRM";
        //    button1.Visible = false;
        //    btnVoid.Visible = false;
        //    btnPaymentMethod.Visible = false;
        //    loadData(subURL);
        //}

        private void btnPaymentMethod_Click(object sender, EventArgs e)
        {
            PaymentMethodList paymentMethodList = new PaymentMethodList();
            paymentMethodList.ShowDialog();
            computeChange("Payment Method","");
        }

        private void checkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvOrders.Rows.Count > 0)
            {
                //toggleSelectAll(checkSelectAll.Checked);
                //MessageBox.Show(cCheck.ToString());
                if (cCheck == 0)
                {
                    selectOrders(true);
                }
                else
                {
                    cCheck = 0;
                }
            }
        }
    }
}