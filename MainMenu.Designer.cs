﻿namespace AB
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.branchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warehouseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priceListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uomGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gLAccountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.productionOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.issueForProductionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.receiptFromProductionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRequestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.receivedTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transferTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pulloutTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cashVarianceTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustmentInToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustmentOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancePaymentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pendingOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addActualCashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pOSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.salesReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSalesReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cashTransactionReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printableReportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventorySummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesReportsWoInventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifications0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelchildform = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.masterDataToolStripMenuItem,
            this.productionToolStripMenuItem1,
            this.inventoryTransactionToolStripMenuItem,
            this.orderToolStripMenuItem,
            this.reportToolStripMenuItem,
            this.notifications0ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(700, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutToolStripMenuItem});
            this.settingsToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // masterDataToolStripMenuItem
            // 
            this.masterDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemsToolStripMenuItem,
            this.usersToolStripMenuItem,
            this.branchesToolStripMenuItem,
            this.warehouseToolStripMenuItem,
            this.customersToolStripMenuItem,
            this.objectTypeToolStripMenuItem,
            this.seriesToolStripMenuItem,
            this.priceListToolStripMenuItem,
            this.uomToolStripMenuItem,
            this.uomGroupToolStripMenuItem,
            this.gLAccountsToolStripMenuItem});
            this.masterDataToolStripMenuItem.Name = "masterDataToolStripMenuItem";
            this.masterDataToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.masterDataToolStripMenuItem.Text = "Master Data";
            // 
            // itemsToolStripMenuItem
            // 
            this.itemsToolStripMenuItem.Name = "itemsToolStripMenuItem";
            this.itemsToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.itemsToolStripMenuItem.Text = "Items";
            this.itemsToolStripMenuItem.Click += new System.EventHandler(this.itemsToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.usersToolStripMenuItem.Text = "Users";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // branchesToolStripMenuItem
            // 
            this.branchesToolStripMenuItem.Name = "branchesToolStripMenuItem";
            this.branchesToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.branchesToolStripMenuItem.Text = "Branches";
            this.branchesToolStripMenuItem.Click += new System.EventHandler(this.branchesToolStripMenuItem_Click);
            // 
            // warehouseToolStripMenuItem
            // 
            this.warehouseToolStripMenuItem.Name = "warehouseToolStripMenuItem";
            this.warehouseToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.warehouseToolStripMenuItem.Text = "Warehouse";
            this.warehouseToolStripMenuItem.Click += new System.EventHandler(this.warehouseToolStripMenuItem_Click);
            // 
            // customersToolStripMenuItem
            // 
            this.customersToolStripMenuItem.Name = "customersToolStripMenuItem";
            this.customersToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.customersToolStripMenuItem.Text = "Customers";
            this.customersToolStripMenuItem.Click += new System.EventHandler(this.customersToolStripMenuItem_Click);
            // 
            // objectTypeToolStripMenuItem
            // 
            this.objectTypeToolStripMenuItem.Name = "objectTypeToolStripMenuItem";
            this.objectTypeToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.objectTypeToolStripMenuItem.Text = "Object Type";
            this.objectTypeToolStripMenuItem.Click += new System.EventHandler(this.objectTypeToolStripMenuItem_Click);
            // 
            // seriesToolStripMenuItem
            // 
            this.seriesToolStripMenuItem.Name = "seriesToolStripMenuItem";
            this.seriesToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.seriesToolStripMenuItem.Text = "Series";
            this.seriesToolStripMenuItem.Click += new System.EventHandler(this.seriesToolStripMenuItem_Click);
            // 
            // priceListToolStripMenuItem
            // 
            this.priceListToolStripMenuItem.Name = "priceListToolStripMenuItem";
            this.priceListToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.priceListToolStripMenuItem.Text = "Price List";
            this.priceListToolStripMenuItem.Click += new System.EventHandler(this.priceListToolStripMenuItem_Click);
            // 
            // uomToolStripMenuItem
            // 
            this.uomToolStripMenuItem.Name = "uomToolStripMenuItem";
            this.uomToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.uomToolStripMenuItem.Text = "Uom";
            this.uomToolStripMenuItem.Click += new System.EventHandler(this.uomToolStripMenuItem_Click);
            // 
            // uomGroupToolStripMenuItem
            // 
            this.uomGroupToolStripMenuItem.Name = "uomGroupToolStripMenuItem";
            this.uomGroupToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.uomGroupToolStripMenuItem.Text = "Uom Group";
            this.uomGroupToolStripMenuItem.Click += new System.EventHandler(this.uomGroupToolStripMenuItem_Click);
            // 
            // gLAccountsToolStripMenuItem
            // 
            this.gLAccountsToolStripMenuItem.Name = "gLAccountsToolStripMenuItem";
            this.gLAccountsToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.gLAccountsToolStripMenuItem.Text = "GL Accounts";
            this.gLAccountsToolStripMenuItem.Click += new System.EventHandler(this.gLAccountsToolStripMenuItem_Click);
            // 
            // productionToolStripMenuItem1
            // 
            this.productionToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productionOrderToolStripMenuItem,
            this.issueForProductionToolStripMenuItem,
            this.receiptFromProductionToolStripMenuItem});
            this.productionToolStripMenuItem1.Name = "productionToolStripMenuItem1";
            this.productionToolStripMenuItem1.Size = new System.Drawing.Size(78, 20);
            this.productionToolStripMenuItem1.Text = "Production";
            this.productionToolStripMenuItem1.Click += new System.EventHandler(this.productionToolStripMenuItem1_Click);
            // 
            // productionOrderToolStripMenuItem
            // 
            this.productionOrderToolStripMenuItem.Name = "productionOrderToolStripMenuItem";
            this.productionOrderToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.productionOrderToolStripMenuItem.Text = "Production Order";
            this.productionOrderToolStripMenuItem.Click += new System.EventHandler(this.productionOrderToolStripMenuItem_Click);
            // 
            // issueForProductionToolStripMenuItem
            // 
            this.issueForProductionToolStripMenuItem.Name = "issueForProductionToolStripMenuItem";
            this.issueForProductionToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.issueForProductionToolStripMenuItem.Text = "Issue for Production";
            this.issueForProductionToolStripMenuItem.Click += new System.EventHandler(this.issueForProductionToolStripMenuItem_Click);
            // 
            // receiptFromProductionToolStripMenuItem
            // 
            this.receiptFromProductionToolStripMenuItem.Name = "receiptFromProductionToolStripMenuItem";
            this.receiptFromProductionToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.receiptFromProductionToolStripMenuItem.Text = "Receipt from Production";
            this.receiptFromProductionToolStripMenuItem.Click += new System.EventHandler(this.receiptFromProductionToolStripMenuItem_Click);
            // 
            // inventoryTransactionToolStripMenuItem
            // 
            this.inventoryTransactionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemRequestToolStripMenuItem,
            this.receivedTransactionsToolStripMenuItem,
            this.transferTransactionsToolStripMenuItem,
            this.pulloutTransactionsToolStripMenuItem,
            this.cashVarianceTransactionsToolStripMenuItem,
            this.salesTransactionsToolStripMenuItem,
            this.adjustmentsToolStripMenuItem});
            this.inventoryTransactionToolStripMenuItem.Name = "inventoryTransactionToolStripMenuItem";
            this.inventoryTransactionToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.inventoryTransactionToolStripMenuItem.Text = "Transactions";
            // 
            // itemRequestToolStripMenuItem
            // 
            this.itemRequestToolStripMenuItem.Name = "itemRequestToolStripMenuItem";
            this.itemRequestToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.itemRequestToolStripMenuItem.Text = "Item Request";
            this.itemRequestToolStripMenuItem.Click += new System.EventHandler(this.itemRequestToolStripMenuItem_Click);
            // 
            // receivedTransactionsToolStripMenuItem
            // 
            this.receivedTransactionsToolStripMenuItem.Name = "receivedTransactionsToolStripMenuItem";
            this.receivedTransactionsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.receivedTransactionsToolStripMenuItem.Text = "Received Transactions";
            this.receivedTransactionsToolStripMenuItem.Click += new System.EventHandler(this.receivedTransactionsToolStripMenuItem_Click);
            // 
            // transferTransactionsToolStripMenuItem
            // 
            this.transferTransactionsToolStripMenuItem.Name = "transferTransactionsToolStripMenuItem";
            this.transferTransactionsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.transferTransactionsToolStripMenuItem.Text = "Transfer Transactions";
            this.transferTransactionsToolStripMenuItem.Click += new System.EventHandler(this.transferTransactionsToolStripMenuItem_Click);
            // 
            // pulloutTransactionsToolStripMenuItem
            // 
            this.pulloutTransactionsToolStripMenuItem.Name = "pulloutTransactionsToolStripMenuItem";
            this.pulloutTransactionsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.pulloutTransactionsToolStripMenuItem.Text = "Pullout Transactions";
            this.pulloutTransactionsToolStripMenuItem.Click += new System.EventHandler(this.pulloutTransactionsToolStripMenuItem_Click);
            // 
            // cashVarianceTransactionsToolStripMenuItem
            // 
            this.cashVarianceTransactionsToolStripMenuItem.Name = "cashVarianceTransactionsToolStripMenuItem";
            this.cashVarianceTransactionsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.cashVarianceTransactionsToolStripMenuItem.Text = "Cash Variance Transactions";
            this.cashVarianceTransactionsToolStripMenuItem.Click += new System.EventHandler(this.cashVarianceTransactionsToolStripMenuItem_Click);
            // 
            // salesTransactionsToolStripMenuItem
            // 
            this.salesTransactionsToolStripMenuItem.Name = "salesTransactionsToolStripMenuItem";
            this.salesTransactionsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.salesTransactionsToolStripMenuItem.Text = "Sales Transactions";
            this.salesTransactionsToolStripMenuItem.Click += new System.EventHandler(this.salesTransactionsToolStripMenuItem_Click);
            // 
            // adjustmentsToolStripMenuItem
            // 
            this.adjustmentsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adjustmentInToolStripMenuItem1,
            this.adjustmentOutToolStripMenuItem});
            this.adjustmentsToolStripMenuItem.Name = "adjustmentsToolStripMenuItem";
            this.adjustmentsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.adjustmentsToolStripMenuItem.Text = "Adjustments";
            // 
            // adjustmentInToolStripMenuItem1
            // 
            this.adjustmentInToolStripMenuItem1.Name = "adjustmentInToolStripMenuItem1";
            this.adjustmentInToolStripMenuItem1.Size = new System.Drawing.Size(159, 22);
            this.adjustmentInToolStripMenuItem1.Text = "Adjustment In";
            this.adjustmentInToolStripMenuItem1.Click += new System.EventHandler(this.adjustmentInToolStripMenuItem1_Click);
            // 
            // adjustmentOutToolStripMenuItem
            // 
            this.adjustmentOutToolStripMenuItem.Name = "adjustmentOutToolStripMenuItem";
            this.adjustmentOutToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.adjustmentOutToolStripMenuItem.Text = "Adjustment Out";
            this.adjustmentOutToolStripMenuItem.Click += new System.EventHandler(this.adjustmentOutToolStripMenuItem_Click);
            // 
            // orderToolStripMenuItem
            // 
            this.orderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.advancePaymentToolStripMenuItem,
            this.pendingOrdersToolStripMenuItem,
            this.addActualCashToolStripMenuItem,
            this.pOSToolStripMenuItem});
            this.orderToolStripMenuItem.Name = "orderToolStripMenuItem";
            this.orderToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.orderToolStripMenuItem.Text = "Sales";
            // 
            // advancePaymentToolStripMenuItem
            // 
            this.advancePaymentToolStripMenuItem.Name = "advancePaymentToolStripMenuItem";
            this.advancePaymentToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.advancePaymentToolStripMenuItem.Text = "Deposit";
            this.advancePaymentToolStripMenuItem.Click += new System.EventHandler(this.advancePaymentToolStripMenuItem_Click);
            // 
            // pendingOrdersToolStripMenuItem
            // 
            this.pendingOrdersToolStripMenuItem.Name = "pendingOrdersToolStripMenuItem";
            this.pendingOrdersToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.pendingOrdersToolStripMenuItem.Text = "Sales";
            this.pendingOrdersToolStripMenuItem.Click += new System.EventHandler(this.pendingOrdersToolStripMenuItem_Click);
            // 
            // addActualCashToolStripMenuItem
            // 
            this.addActualCashToolStripMenuItem.Name = "addActualCashToolStripMenuItem";
            this.addActualCashToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.addActualCashToolStripMenuItem.Text = "Add Actual Cash";
            this.addActualCashToolStripMenuItem.Click += new System.EventHandler(this.addActualCashToolStripMenuItem_Click);
            // 
            // pOSToolStripMenuItem
            // 
            this.pOSToolStripMenuItem.Name = "pOSToolStripMenuItem";
            this.pOSToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.pOSToolStripMenuItem.Text = "POS";
            this.pOSToolStripMenuItem.Click += new System.EventHandler(this.pOSToolStripMenuItem_Click);
            // 
            // reportToolStripMenuItem
            // 
            this.reportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryToolStripMenuItem1,
            this.salesReportToolStripMenuItem,
            this.itemSalesReportToolStripMenuItem,
            this.cashTransactionReportToolStripMenuItem,
            this.printableReportsToolStripMenuItem});
            this.reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            this.reportToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportToolStripMenuItem.Text = "Reports";
            // 
            // inventoryToolStripMenuItem1
            // 
            this.inventoryToolStripMenuItem1.Name = "inventoryToolStripMenuItem1";
            this.inventoryToolStripMenuItem1.Size = new System.Drawing.Size(201, 22);
            this.inventoryToolStripMenuItem1.Text = "Inventory";
            this.inventoryToolStripMenuItem1.Click += new System.EventHandler(this.inventoryToolStripMenuItem1_Click);
            // 
            // salesReportToolStripMenuItem
            // 
            this.salesReportToolStripMenuItem.Name = "salesReportToolStripMenuItem";
            this.salesReportToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.salesReportToolStripMenuItem.Text = "Sales Report";
            this.salesReportToolStripMenuItem.Click += new System.EventHandler(this.salesReportToolStripMenuItem_Click);
            // 
            // itemSalesReportToolStripMenuItem
            // 
            this.itemSalesReportToolStripMenuItem.Name = "itemSalesReportToolStripMenuItem";
            this.itemSalesReportToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.itemSalesReportToolStripMenuItem.Text = "Item Sales Report";
            this.itemSalesReportToolStripMenuItem.Click += new System.EventHandler(this.itemSalesReportToolStripMenuItem_Click);
            // 
            // cashTransactionReportToolStripMenuItem
            // 
            this.cashTransactionReportToolStripMenuItem.Name = "cashTransactionReportToolStripMenuItem";
            this.cashTransactionReportToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.cashTransactionReportToolStripMenuItem.Text = "Cash Transaction Report";
            this.cashTransactionReportToolStripMenuItem.Click += new System.EventHandler(this.cashTransactionReportToolStripMenuItem_Click);
            // 
            // printableReportsToolStripMenuItem
            // 
            this.printableReportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryCountToolStripMenuItem,
            this.inventorySummaryToolStripMenuItem,
            this.salesReportsWoInventoryToolStripMenuItem});
            this.printableReportsToolStripMenuItem.Name = "printableReportsToolStripMenuItem";
            this.printableReportsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.printableReportsToolStripMenuItem.Text = "Printable Reports";
            // 
            // inventoryCountToolStripMenuItem
            // 
            this.inventoryCountToolStripMenuItem.Name = "inventoryCountToolStripMenuItem";
            this.inventoryCountToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.inventoryCountToolStripMenuItem.Text = "Inventory Count Summary";
            this.inventoryCountToolStripMenuItem.Click += new System.EventHandler(this.inventoryCountToolStripMenuItem_Click);
            // 
            // inventorySummaryToolStripMenuItem
            // 
            this.inventorySummaryToolStripMenuItem.Name = "inventorySummaryToolStripMenuItem";
            this.inventorySummaryToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.inventorySummaryToolStripMenuItem.Text = "Sales and Inventory Report ";
            this.inventorySummaryToolStripMenuItem.Click += new System.EventHandler(this.inventorySummaryToolStripMenuItem_Click);
            // 
            // salesReportsWoInventoryToolStripMenuItem
            // 
            this.salesReportsWoInventoryToolStripMenuItem.Name = "salesReportsWoInventoryToolStripMenuItem";
            this.salesReportsWoInventoryToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.salesReportsWoInventoryToolStripMenuItem.Text = "Sales Reports w/o inventory";
            this.salesReportsWoInventoryToolStripMenuItem.Click += new System.EventHandler(this.salesReportsWoInventoryToolStripMenuItem_Click);
            // 
            // notifications0ToolStripMenuItem
            // 
            this.notifications0ToolStripMenuItem.Name = "notifications0ToolStripMenuItem";
            this.notifications0ToolStripMenuItem.Size = new System.Drawing.Size(104, 20);
            this.notifications0ToolStripMenuItem.Text = "Notifications (0)";
            this.notifications0ToolStripMenuItem.Click += new System.EventHandler(this.notifications0ToolStripMenuItem_Click);
            // 
            // panelchildform
            // 
            this.panelchildform.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelchildform.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelchildform.Location = new System.Drawing.Point(12, 27);
            this.panelchildform.Name = "panelchildform";
            this.panelchildform.Size = new System.Drawing.Size(676, 399);
            this.panelchildform.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 30000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(700, 429);
            this.Controls.Add(this.panelchildform);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Atlantic Bakery";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainMenu_FormClosing);
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pendingOrdersToolStripMenuItem;
        private System.Windows.Forms.Panel panelchildform;
        private System.Windows.Forms.ToolStripMenuItem masterDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancePaymentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem salesReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryTransactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemRequestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem receivedTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transferTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salesTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cashTransactionReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem branchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printableReportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryCountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventorySummaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pulloutTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adjustmentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adjustmentInToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem adjustmentOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem warehouseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cashVarianceTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemSalesReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priceListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addActualCashToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salesReportsWoInventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notifications0ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productionToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pOSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uomGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productionOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem issueForProductionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem receiptFromProductionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gLAccountsToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}