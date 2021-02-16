using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Branch;
using Newtonsoft.Json.Linq;
using AB.API_Class.Customer;
using AB.API_Class.SOA;
namespace AB
{
    public partial class ForSOA : Form
    {
        public ForSOA()
        {
            InitializeComponent();
        }
        branch_class branchc = new branch_class();
        customer_class customerc = new customer_class();
        soa_class soac = new soa_class();
        DataTable dtBranches = new DataTable();
        DataTable dtCustomer = new DataTable();
        DataTable dtForSOA = new DataTable();
        int cBranch = 1, cCustomer = 1, cCheckFromDate = 1, cCheckToDate=1, cFromDate = 1, cToDate = 1;
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

        public async Task loadCustomers()
        {
            cmbCustomers.Items.Clear();
            dtCustomer = await Task.Run(() => customerc.loadCustomers(""));
            if (dtCustomer.Rows.Count > 0)
            {
                cmbCustomers.Items.Add("All");
                foreach (DataRow row in dtCustomer.Rows)
                {
                    cmbCustomers.Items.Add(row["code"].ToString());
                }
                cmbCustomers.SelectedIndex = 0;
            }
        }

        public string findCode(DataTable dt, string compareName, string compareValue, string findValue)
        {
            string result = "";
            foreach(DataRow row in dt.Rows)
            {
                if(row[compareName].ToString() == compareValue)
                {
                    result = row[findValue].ToString();
                    break;
                }
            }
            return result;
        }

        private async void checkDate_CheckedChanged(object sender, EventArgs e)
        {
            if(cCheckFromDate <= 0)
            {
                dtFromDate.Visible = checkDate.Checked;
                await loadForSOA();
            }
        }

        private async void checkToDate_CheckedChanged(object sender, EventArgs e)
        {
            if (cCheckToDate <= 0)
            {
                dtToDate.Visible = checkToDate.Checked;
                await loadForSOA();
            }
        }

        private async void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            if(cFromDate <= 0)
            {
        
                await loadForSOA();
            }
        }

        private async void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            if (cToDate <= 0)
            {
                await loadForSOA();
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await loadForSOA();
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                await loadForSOA();
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void btnCreateSOA_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to create SOA?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                JObject joBody = new JObject();
                string custCode = "";
                JObject joHeader = new JObject();
                JArray jaRows = new JArray();
                int haveFirstCustomerCode = 0, haveDifferentCustomerCode = 0, intTemp = 0;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["chck"].Value.ToString()) && haveFirstCustomerCode <= 0){
                        custCode = dgv.Rows[i].Cells["cust_code"].Value.ToString();
                        haveFirstCustomerCode += 1;
                    }
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["chck"].Value.ToString()))
                    {
                        if (!string.IsNullOrEmpty(custCode.ToString().Trim())){
                            if (custCode != dgv.Rows[i].Cells["cust_code"].Value.ToString())
                            {
                                haveDifferentCustomerCode += 1;
                            }
                        }
                        JObject joRows = new JObject();
                        joRows.Add("base_id", int.TryParse(dgv.Rows[i].Cells["id"].Value.ToString(), out intTemp) ? Convert.ToInt32(dgv.Rows[i].Cells["id"].Value.ToString()) : 0);
                        joRows.Add("base_transdate", dgv.Rows[i].Cells["transdate"].Value.ToString());
                        joRows.Add("base_reference", dgv.Rows[i].Cells["reference"].Value.ToString());
                        joRows.Add("base_objtype", int.TryParse(dgv.Rows[i].Cells["objtype"].Value.ToString(), out intTemp) ? Convert.ToInt32(dgv.Rows[i].Cells["objtype"].Value.ToString()) : 0);
                        joRows.Add("sales_remarks", dgv.Rows[i].Cells["remarks"].Value.ToString());
                        joRows.Add("amount", Convert.ToDouble(dgv.Rows[i].Cells["doctotal"].Value.ToString()));
                        jaRows.Add(joRows);
                    }
                }
               
                joHeader.Add("transdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                joHeader.Add("cust_code", custCode);
                joBody.Add("header", joHeader);
                joBody.Add("rows", jaRows);
                Console.WriteLine(joBody);
                if(haveDifferentCustomerCode > 0)
                {
                    MessageBox.Show("You can't create SOA in different customer", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string response = soac.createSOA(joBody);
                    if (response.Substring(0, 1).Equals("{"))
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("reference");
                        dt.Columns.Add("transdate");
                        dt.Columns.Add("cust_code");
                        dt.Columns.Add("docstatus");
                        dt.Columns.Add("total_amount");
                        dt.Columns.Add("id");
                        dt.Columns.Add("doc_id");
                        dt.Columns.Add("base_transdate");
                        dt.Columns.Add("base_id");
                        dt.Columns.Add("base_reference");
                        dt.Columns.Add("base_objtype");
                        dt.Columns.Add("sales_remarks");
                        dt.Columns.Add("amount");

                        JObject joResponse = JObject.Parse(response);
                        Console.WriteLine("response: " + joResponse);
                        bool isSuccess = false;
                        string msg = "", reference = "", customerCode = "", docStatus = "";
                        DateTime dtTransDate = new DateTime();
                        double total = 0.00, doubleTemp = 0.00;
                        foreach (var q in joResponse)
                        {
                            if (q.Key.Equals("success"))
                            {
                                isSuccess = Convert.ToBoolean(q.Value.ToString());
                            }
                            else if (q.Key.Equals("message"))
                            {
                                msg = q.Value.ToString();
                            }
                            else if (q.Key.Equals("data"))
                            {
                                JObject joData = JObject.Parse(q.Value.ToString());
                                foreach (var x in joData)
                                {
                                    if (x.Key.Equals("reference"))
                                    {
                                        reference = x.Value.ToString();
                                    }
                                    else if (x.Key.Equals("transdate"))
                                    {
                                        string replaceT = x.Value.ToString().Replace("T", "");
                                        dtTransDate =string.IsNullOrEmpty(replaceT) ? new DateTime(): Convert.ToDateTime(replaceT);
                                    }
                                    else if (x.Key.Equals("cust_code"))
                                    {
                                        customerCode = x.Value.ToString();
                                    }
                                    else if (x.Key.Equals("total_amount"))
                                    {
                                        total = Convert.ToDouble(x.Value.ToString());
                                    }
                                    else if (x.Key.Equals("docstatus"))
                                    {
                                        docStatus = q.Value.ToString().Equals("O") ? "Open" : q.Value.ToString().Equals("C") ? "Closed" : q.Value.ToString().Equals("N") ? "Cancelled" : "";
                                    }
                                    else if (x.Key.Equals("soa_rows"))
                                    {
                                        if (x.Value.ToString() != "[]")
                                        {
                                            JArray jsonArray = JArray.Parse(x.Value.ToString());
                                            for (int i = 0; i < jsonArray.Count(); i++)
                                            {
                                                int soaID = 0, soaDocID = 0, soaBaseID = 0, soaBaseObjType = 0;
                                                double soaAmount = 0.00;
                                                string soaBaseReference = "", soaSalesRemarks = "";
                                                DateTime soaBaseTransDate = new DateTime();
                                                JObject joSoaRows = JObject.Parse(jsonArray[i].ToString());
                                                foreach (var y in joSoaRows)
                                                {
                                                    if (y.Key.Equals("id"))
                                                    {
                                                        soaID = Convert.ToInt32(y.Value.ToString());
                                                    }
                                                    else if (y.Key.Equals("doc_id"))
                                                    {
                                                        soaDocID = Convert.ToInt32(y.Value.ToString());
                                                    }

                                                    else if (y.Key.Equals("base_transdate"))
                                                    {
                                                        string replaceT = y.Value.ToString().Replace("T", "");
                                                        soaBaseTransDate = !string.IsNullOrEmpty(y.Value.ToString()) ? Convert.ToDateTime(replaceT) : new DateTime();
                                                    }
                                                    else if (y.Key.Equals("base_id"))
                                                    {
                                                        soaBaseID = Convert.ToInt32(y.Value.ToString());
                                                    }
                                                    else if (y.Key.Equals("base_reference"))
                                                    {
                                                        soaBaseReference = y.Value.ToString();
                                                    }
                                                    else if (y.Key.Equals("base_objtype"))
                                                    {
                                                        soaBaseObjType = Convert.ToInt32(y.Value.ToString());
                                                    }
                                                    else if (y.Key.Equals("sales_remarks"))
                                                    {
                                                        soaSalesRemarks = y.Value.ToString();
                                                    }
                                                    else if (y.Key.Equals("amount"))
                                                    {
                                                        soaAmount = Convert.ToDouble(y.Value.ToString());
                                                    }
                                                }

                                                dt.Rows.Add(reference, dtTransDate.ToString("MM/dd/yyyy"), customerCode, docStatus, total.ToString("n2"), soaID, soaDocID, soaBaseTransDate.ToString("MM/dd/yyyy"), soaBaseID, soaBaseReference, soaBaseObjType, soaSalesRemarks, soaAmount.ToString("n2"));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        MessageBox.Show(msg, isSuccess ? "Message" : "Validation", MessageBoxButtons.OK, isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                        if (isSuccess)
                        {
                            DialogResult dialogResult1 = MessageBox.Show("Do you want to print the report?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult1 == DialogResult.Yes)
                            {
                                printSOA frm = new printSOA();
                                frm.dtResult = dt;
                                frm.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(response, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }



        public async Task loadForSOA()
        {
            string branchParam = "?branch=" + findCode(dtBranches, "name", cmbBranches.Text, "code");
            string custParam = "&cust_code=" + findCode(dtCustomer, "name", cmbCustomers.Text, "code");
            string fromDateParam = "&from_date=" + (!checkDate.Checked ? "" : dtFromDate.Value.ToString("yyyy-MM-dd"));
            string toDateParam = "&to_date=" + (!checkToDate.Checked ? "" : dtToDate.Value.ToString("yyyy-MM-dd"));
            dtForSOA = await Task.Run(() => soac.getForSOA(branchParam + custParam + fromDateParam + toDateParam));
            AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
            dgv.Rows.Clear();
            if (dtForSOA.Rows.Count > 0)
            {
                foreach (DataRow row in dtForSOA.Rows)
                {
                    auto.Add(row["reference"].ToString());
                    if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
                    {
                        if (txtSearch.Text.ToString().Trim().ToLower().Contains(row["reference"].ToString().ToLower()))
                        {
                            dgv.Rows.Add(false, row["id"].ToString(), row["reference"].ToString(), row["cust_code"].ToString(), row["transdate"].ToString(), row["objtype"].ToString(), Convert.ToDecimal(string.Format("{0:0.00}", row["doctotal"].ToString())), row["remarks"].ToString(), row["docstatus"].ToString());
                        }
                    }
                    else
                    {
                        dgv.Rows.Add(false, row["id"].ToString(), row["reference"].ToString(), row["cust_code"].ToString(), row["transdate"].ToString(), row["objtype"].ToString(), Convert.ToDecimal(string.Format("{0:0.00}", row["doctotal"].ToString())), row["remarks"].ToString(), row["docstatus"].ToString());
                    }
                }
                txtSearch.AutoCompleteCustomSource = auto;
            }
            dgv.Columns["doctotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private async void cmbBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cBranch <= 0)
            {
                await loadForSOA();
            }
        }

        private async void cmbCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cCustomer <= 0)
            {
                await loadForSOA();
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await loadForSOA();
        }

        private async void ForSOA_Load(object sender, EventArgs e)
        {
            dtFromDate.Value = DateTime.Now;
            dtToDate.Value = DateTime.Now;
            checkDate.Checked = true;
            checkToDate.Checked = true;
            await loadBranches();
            await loadCustomers();
            await loadForSOA();
            cBranch = 0;
            cCustomer = 0;
            cCheckFromDate = 0;
            cCheckToDate = 0;
            cFromDate = 0;
            cToDate = 0;
        }
    }
}
