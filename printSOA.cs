﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AB
{
    public partial class printSOA : Form
    {
        public printSOA()
        {
            InitializeComponent();
        }
        public DataTable dtResult = new DataTable();
        private void printSOA_Load(object sender, EventArgs e)
        {
            SOA_crystalReports report = new SOA_crystalReports();
            report.Database.Tables["soa"].SetDataSource(dtResult);
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        }
    }
}