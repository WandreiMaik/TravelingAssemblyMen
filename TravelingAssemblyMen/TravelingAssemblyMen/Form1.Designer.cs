﻿namespace TravelingAssemblyMen
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxManageTAP = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGenerateTAP = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericCustomercount = new System.Windows.Forms.NumericUpDown();
            this.numericAssemblercount = new System.Windows.Forms.NumericUpDown();
            this.buttonOpenTAP = new System.Windows.Forms.Button();
            this.buttonSaveTAP = new System.Windows.Forms.Button();
            this.groupBoxSolveTAP = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSolveGreedy = new System.Windows.Forms.Button();
            this.buttonSolveRandomly = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelFitness = new System.Windows.Forms.Label();
            this.labelFitnessValue = new System.Windows.Forms.Label();
            this.panelSolvedGraph = new System.Windows.Forms.Panel();
            this.textBoxOvertimePenalty = new System.Windows.Forms.TextBox();
            this.textBoxOverallWorkload = new System.Windows.Forms.TextBox();
            this.mainLayoutPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBoxManageTAP.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCustomercount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAssemblercount)).BeginInit();
            this.groupBoxSolveTAP.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 3;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.mainLayoutPanel.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.panelSolvedGraph, 1, 0);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(1041, 695);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBoxManageTAP);
            this.flowLayoutPanel1.Controls.Add(this.groupBoxSolveTAP);
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(341, 341);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // groupBoxManageTAP
            // 
            this.groupBoxManageTAP.AutoSize = true;
            this.groupBoxManageTAP.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxManageTAP.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxManageTAP.Location = new System.Drawing.Point(3, 3);
            this.groupBoxManageTAP.Name = "groupBoxManageTAP";
            this.groupBoxManageTAP.Size = new System.Drawing.Size(330, 124);
            this.groupBoxManageTAP.TabIndex = 0;
            this.groupBoxManageTAP.TabStop = false;
            this.groupBoxManageTAP.Text = "Manage TAP";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.buttonGenerateTAP, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.numericCustomercount, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.numericAssemblercount, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonOpenTAP, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonSaveTAP, 2, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(324, 105);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // buttonGenerateTAP
            // 
            this.buttonGenerateTAP.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonGenerateTAP.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.buttonGenerateTAP, 2);
            this.buttonGenerateTAP.Location = new System.Drawing.Point(165, 40);
            this.buttonGenerateTAP.Margin = new System.Windows.Forms.Padding(5);
            this.buttonGenerateTAP.Name = "buttonGenerateTAP";
            this.buttonGenerateTAP.Size = new System.Drawing.Size(152, 24);
            this.buttonGenerateTAP.TabIndex = 0;
            this.buttonGenerateTAP.Text = "Generate TAP";
            this.buttonGenerateTAP.UseVisualStyleBackColor = true;
            this.buttonGenerateTAP.Click += new System.EventHandler(this.buttonGenerateTAP_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Customercount:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(163, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Assemblercount:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericCustomercount
            // 
            this.numericCustomercount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericCustomercount.Location = new System.Drawing.Point(90, 7);
            this.numericCustomercount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericCustomercount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericCustomercount.Name = "numericCustomercount";
            this.numericCustomercount.Size = new System.Drawing.Size(67, 20);
            this.numericCustomercount.TabIndex = 3;
            this.numericCustomercount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericCustomercount.ValueChanged += new System.EventHandler(this.numericCustomercount_ValueChanged);
            // 
            // numericAssemblercount
            // 
            this.numericAssemblercount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericAssemblercount.Location = new System.Drawing.Point(254, 7);
            this.numericAssemblercount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericAssemblercount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericAssemblercount.Name = "numericAssemblercount";
            this.numericAssemblercount.Size = new System.Drawing.Size(67, 20);
            this.numericAssemblercount.TabIndex = 4;
            this.numericAssemblercount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericAssemblercount.ValueChanged += new System.EventHandler(this.numericAssemblercount_ValueChanged);
            // 
            // buttonOpenTAP
            // 
            this.buttonOpenTAP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonOpenTAP.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.buttonOpenTAP, 2);
            this.buttonOpenTAP.Location = new System.Drawing.Point(5, 75);
            this.buttonOpenTAP.Margin = new System.Windows.Forms.Padding(5);
            this.buttonOpenTAP.Name = "buttonOpenTAP";
            this.buttonOpenTAP.Size = new System.Drawing.Size(150, 25);
            this.buttonOpenTAP.TabIndex = 5;
            this.buttonOpenTAP.Text = "Open TAP";
            this.buttonOpenTAP.UseVisualStyleBackColor = true;
            // 
            // buttonSaveTAP
            // 
            this.buttonSaveTAP.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonSaveTAP.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.buttonSaveTAP, 2);
            this.buttonSaveTAP.Location = new System.Drawing.Point(165, 75);
            this.buttonSaveTAP.Margin = new System.Windows.Forms.Padding(5);
            this.buttonSaveTAP.Name = "buttonSaveTAP";
            this.buttonSaveTAP.Size = new System.Drawing.Size(152, 25);
            this.buttonSaveTAP.TabIndex = 6;
            this.buttonSaveTAP.Text = "Save TAP";
            this.buttonSaveTAP.UseVisualStyleBackColor = true;
            // 
            // groupBoxSolveTAP
            // 
            this.groupBoxSolveTAP.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxSolveTAP.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSolveTAP.Location = new System.Drawing.Point(3, 133);
            this.groupBoxSolveTAP.Name = "groupBoxSolveTAP";
            this.groupBoxSolveTAP.Size = new System.Drawing.Size(330, 54);
            this.groupBoxSolveTAP.TabIndex = 1;
            this.groupBoxSolveTAP.TabStop = false;
            this.groupBoxSolveTAP.Text = "Solve TAP";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.buttonSolveGreedy, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSolveRandomly, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(324, 35);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonSolveGreedy
            // 
            this.buttonSolveGreedy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSolveGreedy.Location = new System.Drawing.Point(5, 5);
            this.buttonSolveGreedy.Margin = new System.Windows.Forms.Padding(5);
            this.buttonSolveGreedy.Name = "buttonSolveGreedy";
            this.buttonSolveGreedy.Size = new System.Drawing.Size(152, 25);
            this.buttonSolveGreedy.TabIndex = 0;
            this.buttonSolveGreedy.Text = "Greedy Solution";
            this.buttonSolveGreedy.UseVisualStyleBackColor = true;
            // 
            // buttonSolveRandomly
            // 
            this.buttonSolveRandomly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSolveRandomly.Location = new System.Drawing.Point(167, 5);
            this.buttonSolveRandomly.Margin = new System.Windows.Forms.Padding(5);
            this.buttonSolveRandomly.Name = "buttonSolveRandomly";
            this.buttonSolveRandomly.Size = new System.Drawing.Size(152, 25);
            this.buttonSolveRandomly.TabIndex = 1;
            this.buttonSolveRandomly.Text = "Random Solution";
            this.buttonSolveRandomly.UseVisualStyleBackColor = true;
            this.buttonSolveRandomly.Click += new System.EventHandler(this.buttonSolveRandomly_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 193);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 72);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weightings";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.67901F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.32099F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.textBoxOvertimePenalty, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.textBoxOverallWorkload, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(324, 53);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Overtime Penalty:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Over-All Workload:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 51);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.69136F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.30864F));
            this.tableLayoutPanel4.Controls.Add(this.labelFitness, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelFitnessValue, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(324, 32);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // labelFitness
            // 
            this.labelFitness.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelFitness.AutoSize = true;
            this.labelFitness.Location = new System.Drawing.Point(5, 9);
            this.labelFitness.Name = "labelFitness";
            this.labelFitness.Size = new System.Drawing.Size(69, 13);
            this.labelFitness.TabIndex = 1;
            this.labelFitness.Text = "Fitnessvalue:";
            // 
            // labelFitnessValue
            // 
            this.labelFitnessValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelFitnessValue.AutoSize = true;
            this.labelFitnessValue.Location = new System.Drawing.Point(83, 9);
            this.labelFitnessValue.Name = "labelFitnessValue";
            this.labelFitnessValue.Size = new System.Drawing.Size(13, 13);
            this.labelFitnessValue.TabIndex = 2;
            this.labelFitnessValue.Text = "0";
            // 
            // panelSolvedGraph
            // 
            this.panelSolvedGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainLayoutPanel.SetColumnSpan(this.panelSolvedGraph, 2);
            this.panelSolvedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSolvedGraph.Location = new System.Drawing.Point(350, 3);
            this.panelSolvedGraph.Name = "panelSolvedGraph";
            this.mainLayoutPanel.SetRowSpan(this.panelSolvedGraph, 2);
            this.panelSolvedGraph.Size = new System.Drawing.Size(688, 689);
            this.panelSolvedGraph.TabIndex = 3;
            this.panelSolvedGraph.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSolvedGraph_Paint);
            // 
            // textBoxOvertimePenalty
            // 
            this.textBoxOvertimePenalty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOvertimePenalty.Location = new System.Drawing.Point(150, 3);
            this.textBoxOvertimePenalty.Name = "textBoxOvertimePenalty";
            this.textBoxOvertimePenalty.Size = new System.Drawing.Size(171, 20);
            this.textBoxOvertimePenalty.TabIndex = 4;
            this.textBoxOvertimePenalty.Text = "1,0";
            this.textBoxOvertimePenalty.TextChanged += new System.EventHandler(this.textBoxOvertimePenalty_TextChanged);
            // 
            // textBoxOverallWorkload
            // 
            this.textBoxOverallWorkload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverallWorkload.Location = new System.Drawing.Point(150, 29);
            this.textBoxOverallWorkload.Name = "textBoxOverallWorkload";
            this.textBoxOverallWorkload.Size = new System.Drawing.Size(171, 20);
            this.textBoxOverallWorkload.TabIndex = 5;
            this.textBoxOverallWorkload.Text = "1,0";
            this.textBoxOverallWorkload.TextChanged += new System.EventHandler(this.textBoxOverallWorkload_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 695);
            this.Controls.Add(this.mainLayoutPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.mainLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBoxManageTAP.ResumeLayout(false);
            this.groupBoxManageTAP.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCustomercount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAssemblercount)).EndInit();
            this.groupBoxSolveTAP.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.GroupBox groupBoxManageTAP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonGenerateTAP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericCustomercount;
        private System.Windows.Forms.NumericUpDown numericAssemblercount;
        private System.Windows.Forms.Button buttonOpenTAP;
        private System.Windows.Forms.Button buttonSaveTAP;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxSolveTAP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonSolveGreedy;
        private System.Windows.Forms.Button buttonSolveRandomly;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelSolvedGraph;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label labelFitness;
        private System.Windows.Forms.Label labelFitnessValue;
        private System.Windows.Forms.TextBox textBoxOvertimePenalty;
        private System.Windows.Forms.TextBox textBoxOverallWorkload;
    }
}
