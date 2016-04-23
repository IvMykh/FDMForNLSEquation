namespace FDMForNSE.Visualization
{
    partial class MainWindow
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
            this.sidePanel = new System.Windows.Forms.Panel();
            this.timeProgressBar = new System.Windows.Forms.ProgressBar();
            this.configGroupBox = new System.Windows.Forms.GroupBox();
            this.solutionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.resetConfigButton = new System.Windows.Forms.Button();
            this.tStepNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.xStepNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.tStepLabel = new System.Windows.Forms.Label();
            this.xStepLabel = new System.Windows.Forms.Label();
            this.durationNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.durationLabel = new System.Windows.Forms.Label();
            this.startStopButton = new System.Windows.Forms.Button();
            this.graphPanel = new System.Windows.Forms.Panel();
            this.sidePanel.SuspendLayout();
            this.configGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tStepNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xStepNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // sidePanel
            // 
            this.sidePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sidePanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.sidePanel.Controls.Add(this.timeProgressBar);
            this.sidePanel.Controls.Add(this.configGroupBox);
            this.sidePanel.Controls.Add(this.startStopButton);
            this.sidePanel.Location = new System.Drawing.Point(514, 12);
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(154, 411);
            this.sidePanel.TabIndex = 0;
            // 
            // timeProgressBar
            // 
            this.timeProgressBar.Location = new System.Drawing.Point(12, 19);
            this.timeProgressBar.Name = "timeProgressBar";
            this.timeProgressBar.Size = new System.Drawing.Size(121, 23);
            this.timeProgressBar.TabIndex = 2;
            // 
            // configGroupBox
            // 
            this.configGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configGroupBox.Controls.Add(this.solutionTypeComboBox);
            this.configGroupBox.Controls.Add(this.resetConfigButton);
            this.configGroupBox.Controls.Add(this.tStepNumericUpDown);
            this.configGroupBox.Controls.Add(this.xStepNumericUpDown);
            this.configGroupBox.Controls.Add(this.tStepLabel);
            this.configGroupBox.Controls.Add(this.xStepLabel);
            this.configGroupBox.Controls.Add(this.durationNumericUpDown);
            this.configGroupBox.Controls.Add(this.durationLabel);
            this.configGroupBox.Location = new System.Drawing.Point(3, 121);
            this.configGroupBox.Name = "configGroupBox";
            this.configGroupBox.Size = new System.Drawing.Size(147, 263);
            this.configGroupBox.TabIndex = 1;
            this.configGroupBox.TabStop = false;
            this.configGroupBox.Text = "Configuration";
            // 
            // solutionTypeComboBox
            // 
            this.solutionTypeComboBox.FormattingEnabled = true;
            this.solutionTypeComboBox.Location = new System.Drawing.Point(0, 185);
            this.solutionTypeComboBox.Name = "solutionTypeComboBox";
            this.solutionTypeComboBox.Size = new System.Drawing.Size(147, 21);
            this.solutionTypeComboBox.TabIndex = 5;
            this.solutionTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.solutionTypeComboBox_SelectedIndexChanged);
            // 
            // resetConfigButton
            // 
            this.resetConfigButton.Location = new System.Drawing.Point(9, 128);
            this.resetConfigButton.Name = "resetConfigButton";
            this.resetConfigButton.Size = new System.Drawing.Size(121, 32);
            this.resetConfigButton.TabIndex = 4;
            this.resetConfigButton.Text = "Set Default";
            this.resetConfigButton.UseVisualStyleBackColor = true;
            this.resetConfigButton.Click += new System.EventHandler(this.resetConfigButton_Click);
            // 
            // tStepNumericUpDown
            // 
            this.tStepNumericUpDown.DecimalPlaces = 4;
            this.tStepNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.tStepNumericUpDown.Location = new System.Drawing.Point(65, 83);
            this.tStepNumericUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.tStepNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.tStepNumericUpDown.Name = "tStepNumericUpDown";
            this.tStepNumericUpDown.Size = new System.Drawing.Size(76, 20);
            this.tStepNumericUpDown.TabIndex = 3;
            this.tStepNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            this.tStepNumericUpDown.ValueChanged += new System.EventHandler(this.tStepNumericUpDown_ValueChanged);
            // 
            // xStepNumericUpDown
            // 
            this.xStepNumericUpDown.DecimalPlaces = 2;
            this.xStepNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.xStepNumericUpDown.Location = new System.Drawing.Point(65, 57);
            this.xStepNumericUpDown.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.xStepNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.xStepNumericUpDown.Name = "xStepNumericUpDown";
            this.xStepNumericUpDown.Size = new System.Drawing.Size(76, 20);
            this.xStepNumericUpDown.TabIndex = 2;
            this.xStepNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.xStepNumericUpDown.ValueChanged += new System.EventHandler(this.xStepNumericUpDown_ValueChanged);
            // 
            // tStepLabel
            // 
            this.tStepLabel.AutoSize = true;
            this.tStepLabel.Location = new System.Drawing.Point(6, 85);
            this.tStepLabel.Name = "tStepLabel";
            this.tStepLabel.Size = new System.Drawing.Size(53, 13);
            this.tStepLabel.TabIndex = 3;
            this.tStepLabel.Text = "Step for t:";
            // 
            // xStepLabel
            // 
            this.xStepLabel.AutoSize = true;
            this.xStepLabel.Location = new System.Drawing.Point(6, 59);
            this.xStepLabel.Name = "xStepLabel";
            this.xStepLabel.Size = new System.Drawing.Size(55, 13);
            this.xStepLabel.TabIndex = 2;
            this.xStepLabel.Text = "Step for x:";
            // 
            // durationNumericUpDown
            // 
            this.durationNumericUpDown.Location = new System.Drawing.Point(65, 31);
            this.durationNumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.durationNumericUpDown.Name = "durationNumericUpDown";
            this.durationNumericUpDown.Size = new System.Drawing.Size(76, 20);
            this.durationNumericUpDown.TabIndex = 1;
            this.durationNumericUpDown.ValueChanged += new System.EventHandler(this.durationNumericUpDown_ValueChanged);
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.Location = new System.Drawing.Point(6, 33);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(50, 13);
            this.durationLabel.TabIndex = 0;
            this.durationLabel.Text = "Duration:";
            // 
            // startStopButton
            // 
            this.startStopButton.Location = new System.Drawing.Point(12, 64);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(121, 32);
            this.startStopButton.TabIndex = 0;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
            // 
            // graphPanel
            // 
            this.graphPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphPanel.Location = new System.Drawing.Point(12, 12);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(496, 411);
            this.graphPanel.TabIndex = 1;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 435);
            this.Controls.Add(this.graphPanel);
            this.Controls.Add(this.sidePanel);
            this.Name = "MainWindow";
            this.Text = "Graph Builder";
            this.sidePanel.ResumeLayout(false);
            this.configGroupBox.ResumeLayout(false);
            this.configGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tStepNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xStepNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel sidePanel;
        private System.Windows.Forms.Panel graphPanel;
        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.GroupBox configGroupBox;
        private System.Windows.Forms.Label tStepLabel;
        private System.Windows.Forms.Label xStepLabel;
        private System.Windows.Forms.NumericUpDown durationNumericUpDown;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.NumericUpDown tStepNumericUpDown;
        private System.Windows.Forms.NumericUpDown xStepNumericUpDown;
        private System.Windows.Forms.Button resetConfigButton;
        private System.Windows.Forms.ProgressBar timeProgressBar;
        private System.Windows.Forms.ComboBox solutionTypeComboBox;
    }
}

