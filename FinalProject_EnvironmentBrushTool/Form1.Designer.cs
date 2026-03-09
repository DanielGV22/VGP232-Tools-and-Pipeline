namespace FinalProject_EnvironmentBrushTool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtBrushName = new TextBox();
            numRadius = new NumericUpDown();
            numDensity = new NumericUpDown();
            cmbDistribution = new ComboBox();
            chkRandomRotation = new CheckBox();
            chkRandomScale = new CheckBox();
            numMinScale = new NumericUpDown();
            numMaxScale = new NumericUpDown();
            numSeed = new NumericUpDown();
            btnGenerate = new Button();
            btnSave = new Button();
            btnLoad = new Button();
            btnExport = new Button();
            pnlPreview = new Panel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            toolTip1 = new ToolTip();
            lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)numRadius).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDensity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSeed).BeginInit();
            SuspendLayout();
            toolTip1.SetToolTip(txtBrushName, "Name of the current brush preset.");
            toolTip1.SetToolTip(cmbDistribution, "Controls how points are arranged inside the brush area.");
            toolTip1.SetToolTip(numRadius, "Size of the brush area.");
            toolTip1.SetToolTip(numDensity, "Number of points generated inside the brush.");
            toolTip1.SetToolTip(numMinScale, "Minimum random scale for generated objects.");
            toolTip1.SetToolTip(numMaxScale, "Maximum random scale for generated objects.");
            toolTip1.SetToolTip(numSeed, "Same seed generates the same pattern.");
            toolTip1.SetToolTip(chkRandomRotation, "Applies random Y-axis rotation to each point.");
            toolTip1.SetToolTip(chkRandomScale, "Applies random scale between Min Scale and Max Scale.");
            toolTip1.SetToolTip(btnGenerate, "Generate a new placement pattern.");
            toolTip1.SetToolTip(btnSave, "Save the current brush configuration to JSON.");
            toolTip1.SetToolTip(btnLoad, "Load a previously saved brush configuration.");
            toolTip1.SetToolTip(btnExport, "Export brush data for Unity.");
            toolTip1.SetToolTip(pnlPreview, "Preview of generated placement points inside the brush radius.");
            // 
            // txtBrushName
            // 
            txtBrushName.AccessibleName = "";
            txtBrushName.Location = new Point(34, 55);
            txtBrushName.Name = "txtBrushName";
            txtBrushName.Size = new Size(150, 31);
            txtBrushName.TabIndex = 0;
            // 
            // numRadius
            // 
            numRadius.DecimalPlaces = 1;
            numRadius.Location = new Point(30, 245);
            numRadius.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRadius.Name = "numRadius";
            numRadius.Size = new Size(180, 31);
            numRadius.TabIndex = 1;
            numRadius.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // numDensity
            // 
            numDensity.Location = new Point(30, 331);
            numDensity.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            numDensity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDensity.Name = "numDensity";
            numDensity.Size = new Size(180, 31);
            numDensity.TabIndex = 2;
            numDensity.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // cmbDistribution
            // 
            cmbDistribution.FormattingEnabled = true;
            cmbDistribution.Items.AddRange(new object[] { "Random", "Grid" });
            cmbDistribution.Location = new Point(30, 141);
            cmbDistribution.Name = "cmbDistribution";
            cmbDistribution.Size = new Size(182, 33);
            cmbDistribution.TabIndex = 3;
            // 
            // chkRandomRotation
            // 
            chkRandomRotation.AutoSize = true;
            chkRandomRotation.Location = new Point(316, 459);
            chkRandomRotation.Name = "chkRandomRotation";
            chkRandomRotation.Size = new Size(178, 29);
            chkRandomRotation.TabIndex = 4;
            chkRandomRotation.Text = "Random Rotation";
            chkRandomRotation.UseVisualStyleBackColor = true;
            // 
            // chkRandomScale
            // 
            chkRandomScale.AutoSize = true;
            chkRandomScale.Location = new Point(316, 554);
            chkRandomScale.Name = "chkRandomScale";
            chkRandomScale.Size = new Size(151, 29);
            chkRandomScale.TabIndex = 5;
            chkRandomScale.Text = "Random Scale";
            chkRandomScale.UseVisualStyleBackColor = true;
            // 
            // numMinScale
            // 
            numMinScale.DecimalPlaces = 2;
            numMinScale.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            numMinScale.Location = new Point(30, 427);
            numMinScale.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numMinScale.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numMinScale.Name = "numMinScale";
            numMinScale.Size = new Size(180, 31);
            numMinScale.TabIndex = 6;
            numMinScale.Value = new decimal(new int[] { 8, 0, 0, 65536 });
            // 
            // numMaxScale
            // 
            numMaxScale.DecimalPlaces = 2;
            numMaxScale.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            numMaxScale.Location = new Point(30, 518);
            numMaxScale.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numMaxScale.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            numMaxScale.Name = "numMaxScale";
            numMaxScale.Size = new Size(180, 31);
            numMaxScale.TabIndex = 7;
            numMaxScale.Value = new decimal(new int[] { 12, 0, 0, 65536 });
            // 
            // numSeed
            // 
            numSeed.Location = new Point(30, 601);
            numSeed.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numSeed.Name = "numSeed";
            numSeed.Size = new Size(180, 31);
            numSeed.TabIndex = 8;
            numSeed.Value = new decimal(new int[] { 12345, 0, 0, 0 });
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(622, 459);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(112, 34);
            btnGenerate.TabIndex = 9;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(622, 568);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 10;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(843, 568);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(112, 34);
            btnLoad.TabIndex = 11;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(843, 459);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(112, 34);
            btnExport.TabIndex = 12;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // pnlPreview
            // 
            pnlPreview.Location = new Point(326, 55);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new Size(629, 321);
            pnlPreview.TabIndex = 13;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 27);
            label1.Name = "label1";
            label1.Size = new Size(108, 25);
            label1.TabIndex = 14;
            label1.Text = "Brush Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 113);
            label2.Name = "label2";
            label2.Size = new Size(147, 25);
            label2.TabIndex = 15;
            label2.Text = "Distribution Type";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(30, 303);
            label3.Name = "label3";
            label3.Size = new Size(71, 25);
            label3.TabIndex = 16;
            label3.Text = "Density";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(30, 217);
            label4.Name = "label4";
            label4.Size = new Size(65, 25);
            label4.TabIndex = 17;
            label4.Text = "Radius";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(30, 488);
            label5.Name = "label5";
            label5.Size = new Size(90, 25);
            label5.TabIndex = 18;
            label5.Text = "Max Scale";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(30, 399);
            label6.Name = "label6";
            label6.Size = new Size(87, 25);
            label6.TabIndex = 19;
            label6.Text = "Min Scale";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(30, 573);
            label7.Name = "label7";
            label7.Size = new Size(51, 25);
            label7.TabIndex = 20;
            label7.Text = "Seed";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(613, 27);
            label8.Name = "label8";
            label8.Size = new Size(72, 25);
            label8.TabIndex = 21;
            label8.Text = "Preview";
            // lblStatus
            lblStatus.AutoSize = false;
            lblStatus.BorderStyle = BorderStyle.FixedSingle;
            lblStatus.Location = new Point(30, 640);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(925, 25);
            lblStatus.TabIndex = 22;
            lblStatus.Text = "Ready.";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1031, 710);
            Controls.Add(lblStatus);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pnlPreview);
            Controls.Add(btnExport);
            Controls.Add(btnLoad);
            Controls.Add(btnSave);
            Controls.Add(btnGenerate);
            Controls.Add(numSeed);
            Controls.Add(numMaxScale);
            Controls.Add(numMinScale);
            Controls.Add(chkRandomScale);
            Controls.Add(chkRandomRotation);
            Controls.Add(cmbDistribution);
            Controls.Add(numDensity);
            Controls.Add(numRadius);
            Controls.Add(txtBrushName);
            Name = "Form1";
            Text = "Environment Brush Tool";
            ((System.ComponentModel.ISupportInitialize)numRadius).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDensity).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSeed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBrushName;
        private NumericUpDown numRadius;
        private NumericUpDown numDensity;
        private ComboBox cmbDistribution;
        private CheckBox chkRandomRotation;
        private CheckBox chkRandomScale;
        private NumericUpDown numMinScale;
        private NumericUpDown numMaxScale;
        private NumericUpDown numSeed;
        private Button btnGenerate;
        private Button btnSave;
        private Button btnLoad;
        private Button btnExport;
        private Panel pnlPreview;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private ToolTip toolTip1;
        private Label lblStatus;
    }
}
