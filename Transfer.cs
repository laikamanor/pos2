﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Transfer;
using Newtonsoft.Json.Linq;
using AB.API_Class.User;
namespace AB
{
    public partial class Transfer : Form
    {
        public Transfer()
        {
            InitializeComponent();
        }

        private void Transfer_Load(object sender, EventArgs e)
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                if (tp.Name.Equals("tabPage1"))
                {
                    if (this.Text == "Pullout Transactions")
                    {
                        tp.Text = "For Confirmation";
                    }
                    else
                    {
                        tp.Text = "Transactions";
                    }
                }
                if (tp.Name.Equals("tabPage2"))
                {
                    if (this.Text == "Received Transactions")
                    {
                        tp.Text = "For SAP";
                    }
                    else if (this.Text == "Pullout Transactions")
                    {
                        tp.Text = "For SAP IT and Transfer";
                    }
                    else
                    {
                        tp.Text = "Logs";
                    }
                }
            }

            Transfer2 transfer2 = new Transfer2("For Transactions");
            transfer2.Text = this.Text;
            showForm(panelTransactions, transfer2);
        }
        public void showForm(Panel panel, Form form)
        {
            panel.Controls.Clear();
            form.TopLevel = false;
            panel.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex.Equals(0))
            {
                Transfer2 transfer2 = new Transfer2("For Transactions");
                transfer2.Text = this.Text;
                showForm(panelTransactions, transfer2);
            }
            else if (tabControl1.SelectedIndex.Equals(1))
            {
                Transfer2 transfer2 = new Transfer2("For SAP");
                transfer2.Text = this.Text;
                showForm(panelSAP, transfer2);
            }
        }
    }
}
