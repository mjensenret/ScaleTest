namespace ScaleIntegration_Server
{
    partial class frmServerForm
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
            this.lstClientCommand = new System.Windows.Forms.ListBox();
            this.lstServerResponse = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstClientCommand
            // 
            this.lstClientCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstClientCommand.FormattingEnabled = true;
            this.lstClientCommand.Location = new System.Drawing.Point(13, 13);
            this.lstClientCommand.Name = "lstClientCommand";
            this.lstClientCommand.Size = new System.Drawing.Size(259, 56);
            this.lstClientCommand.TabIndex = 0;
            // 
            // lstServerResponse
            // 
            this.lstServerResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstServerResponse.FormattingEnabled = true;
            this.lstServerResponse.Location = new System.Drawing.Point(13, 75);
            this.lstServerResponse.Name = "lstServerResponse";
            this.lstServerResponse.Size = new System.Drawing.Size(259, 56);
            this.lstServerResponse.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(197, 137);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // frmServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 172);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lstServerResponse);
            this.Controls.Add(this.lstClientCommand);
            this.Name = "frmServerForm";
            this.Text = "Server Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstClientCommand;
        private System.Windows.Forms.ListBox lstServerResponse;
        private System.Windows.Forms.Button btnStart;
    }
}

