﻿namespace AB
{
    partial class ReceiptFromProduction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiptFromProduction));
            this.tcProd = new System.Windows.Forms.TabControl();
            this.tpReceiptFromProduction = new System.Windows.Forms.TabPage();
            this.panelIssueProdOrder = new System.Windows.Forms.Panel();
            this.tpForSAP = new System.Windows.Forms.TabPage();
            this.panelForSAP = new System.Windows.Forms.Panel();
            this.tcProd.SuspendLayout();
            this.tpReceiptFromProduction.SuspendLayout();
            this.tpForSAP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcProd
            // 
            this.tcProd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcProd.Controls.Add(this.tpReceiptFromProduction);
            this.tcProd.Controls.Add(this.tpForSAP);
            this.tcProd.Location = new System.Drawing.Point(12, 9);
            this.tcProd.Name = "tcProd";
            this.tcProd.SelectedIndex = 0;
            this.tcProd.Size = new System.Drawing.Size(803, 483);
            this.tcProd.TabIndex = 3;
            this.tcProd.SelectedIndexChanged += new System.EventHandler(this.tcProd_SelectedIndexChanged);
            // 
            // tpReceiptFromProduction
            // 
            this.tpReceiptFromProduction.Controls.Add(this.panelIssueProdOrder);
            this.tpReceiptFromProduction.Location = new System.Drawing.Point(4, 22);
            this.tpReceiptFromProduction.Name = "tpReceiptFromProduction";
            this.tpReceiptFromProduction.Size = new System.Drawing.Size(795, 457);
            this.tpReceiptFromProduction.TabIndex = 0;
            this.tpReceiptFromProduction.Text = "Receipt from Production";
            this.tpReceiptFromProduction.UseVisualStyleBackColor = true;
            // 
            // panelIssueProdOrder
            // 
            this.panelIssueProdOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelIssueProdOrder.AutoScroll = true;
            this.panelIssueProdOrder.Location = new System.Drawing.Point(3, 6);
            this.panelIssueProdOrder.Name = "panelIssueProdOrder";
            this.panelIssueProdOrder.Size = new System.Drawing.Size(789, 445);
            this.panelIssueProdOrder.TabIndex = 1;
            // 
            // tpForSAP
            // 
            this.tpForSAP.Controls.Add(this.panelForSAP);
            this.tpForSAP.Location = new System.Drawing.Point(4, 22);
            this.tpForSAP.Name = "tpForSAP";
            this.tpForSAP.Size = new System.Drawing.Size(795, 457);
            this.tpForSAP.TabIndex = 1;
            this.tpForSAP.Text = "For SAP";
            this.tpForSAP.UseVisualStyleBackColor = true;
            // 
            // panelForSAP
            // 
            this.panelForSAP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelForSAP.AutoScroll = true;
            this.panelForSAP.Location = new System.Drawing.Point(3, 6);
            this.panelForSAP.Name = "panelForSAP";
            this.panelForSAP.Size = new System.Drawing.Size(789, 445);
            this.panelForSAP.TabIndex = 2;
            // 
            // ReceiptFromProduction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(827, 501);
            this.Controls.Add(this.tcProd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReceiptFromProduction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receipt From Production";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ReceiptFromProduction_Load);
            this.tcProd.ResumeLayout(false);
            this.tpReceiptFromProduction.ResumeLayout(false);
            this.tpForSAP.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcProd;
        private System.Windows.Forms.TabPage tpReceiptFromProduction;
        private System.Windows.Forms.Panel panelIssueProdOrder;
        private System.Windows.Forms.TabPage tpForSAP;
        private System.Windows.Forms.Panel panelForSAP;
    }
}