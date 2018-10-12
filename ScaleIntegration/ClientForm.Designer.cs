namespace ScaleIntegration_Client
{
    partial class frm920Simulation
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
            this.lstServerResponse = new System.Windows.Forms.ListBox();
            this.txtServerIpAddress = new System.Windows.Forms.TextBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblServerIpAddress = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblServerResponse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstServerResponse
            // 
            this.lstServerResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstServerResponse.FormattingEnabled = true;
            this.lstServerResponse.Location = new System.Drawing.Point(13, 134);
            this.lstServerResponse.Name = "lstServerResponse";
            this.lstServerResponse.Size = new System.Drawing.Size(297, 69);
            this.lstServerResponse.TabIndex = 0;
            this.lstServerResponse.TabStop = false;
            // 
            // txtServerIpAddress
            // 
            this.txtServerIpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerIpAddress.Location = new System.Drawing.Point(111, 12);
            this.txtServerIpAddress.Name = "txtServerIpAddress";
            this.txtServerIpAddress.Size = new System.Drawing.Size(199, 20);
            this.txtServerIpAddress.TabIndex = 1;
            // 
            // txtCommand
            // 
            this.txtCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommand.Location = new System.Drawing.Point(13, 70);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(297, 20);
            this.txtCommand.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(235, 96);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblServerIpAddress
            // 
            this.lblServerIpAddress.AutoSize = true;
            this.lblServerIpAddress.Location = new System.Drawing.Point(13, 15);
            this.lblServerIpAddress.Name = "lblServerIpAddress";
            this.lblServerIpAddress.Size = new System.Drawing.Size(92, 13);
            this.lblServerIpAddress.TabIndex = 5;
            this.lblServerIpAddress.Text = "Server IP Address";
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(235, 38);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Command";
            // 
            // lblServerResponse
            // 
            this.lblServerResponse.AutoSize = true;
            this.lblServerResponse.Location = new System.Drawing.Point(16, 118);
            this.lblServerResponse.Name = "lblServerResponse";
            this.lblServerResponse.Size = new System.Drawing.Size(89, 13);
            this.lblServerResponse.TabIndex = 8;
            this.lblServerResponse.Text = "Server Response";
            // 
            // frm920Simulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 215);
            this.Controls.Add(this.lblServerResponse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblServerIpAddress);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.txtServerIpAddress);
            this.Controls.Add(this.lstServerResponse);
            this.Name = "frm920Simulation";
            this.Text = "920 Simulation Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstServerResponse;
        private System.Windows.Forms.TextBox txtServerIpAddress;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblServerIpAddress;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblServerResponse;
    }
}

