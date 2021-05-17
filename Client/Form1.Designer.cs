
namespace Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.hostBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.severMsgBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.chatView = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.chatSendButton = new System.Windows.Forms.Button();
            this.chatTextBox = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chatIdLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chatView.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // hostBox
            // 
            this.hostBox.Location = new System.Drawing.Point(207, 9);
            this.hostBox.Name = "hostBox";
            this.hostBox.Size = new System.Drawing.Size(202, 23);
            this.hostBox.TabIndex = 0;
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(431, 9);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(74, 23);
            this.portBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Stencil Std", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(411, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = ":";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Swis721 Ex BT", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(37, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Connect to";
            // 
            // severMsgBox
            // 
            this.severMsgBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.severMsgBox.Location = new System.Drawing.Point(13, 38);
            this.severMsgBox.Name = "severMsgBox";
            this.severMsgBox.ReadOnly = true;
            this.severMsgBox.Size = new System.Drawing.Size(775, 23);
            this.severMsgBox.TabIndex = 4;
            this.severMsgBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // connectButton
            // 
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(519, 9);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(116, 23);
            this.connectButton.TabIndex = 5;
            this.connectButton.Text = "connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Enabled = false;
            this.disconnectButton.Location = new System.Drawing.Point(641, 9);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(116, 23);
            this.disconnectButton.TabIndex = 6;
            this.disconnectButton.Text = "disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Visible = false;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label3.Font = new System.Drawing.Font("Swis721 Ex BT", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label3.Location = new System.Drawing.Point(4, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 22);
            this.label3.TabIndex = 8;
            this.label3.Text = "Contacts";
            // 
            // chatView
            // 
            this.chatView.Controls.Add(this.label4);
            this.chatView.Controls.Add(this.chatSendButton);
            this.chatView.Controls.Add(this.chatTextBox);
            this.chatView.Controls.Add(this.panel2);
            this.chatView.Location = new System.Drawing.Point(207, 87);
            this.chatView.Name = "chatView";
            this.chatView.Size = new System.Drawing.Size(581, 351);
            this.chatView.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Swis721 Ex BT", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(62, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(470, 29);
            this.label4.TabIndex = 16;
            this.label4.Text = "Select a contact to start messaging";
            // 
            // chatSendButton
            // 
            this.chatSendButton.Font = new System.Drawing.Font("Stylus BT", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chatSendButton.Location = new System.Drawing.Point(490, 305);
            this.chatSendButton.Name = "chatSendButton";
            this.chatSendButton.Size = new System.Drawing.Size(90, 46);
            this.chatSendButton.TabIndex = 13;
            this.chatSendButton.Text = "Send";
            this.chatSendButton.UseVisualStyleBackColor = true;
            this.chatSendButton.Click += new System.EventHandler(this.chatSendButton_Click);
            // 
            // chatTextBox
            // 
            this.chatTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chatTextBox.Font = new System.Drawing.Font("Swis721 Ex BT", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chatTextBox.Location = new System.Drawing.Point(-1, 305);
            this.chatTextBox.Name = "chatTextBox";
            this.chatTextBox.Size = new System.Drawing.Size(492, 45);
            this.chatTextBox.TabIndex = 12;
            this.chatTextBox.Text = "Write a message...";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel2.Controls.Add(this.chatIdLabel);
            this.panel2.Location = new System.Drawing.Point(-1, -1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(775, 36);
            this.panel2.TabIndex = 0;
            // 
            // chatIdLabel
            // 
            this.chatIdLabel.AutoSize = true;
            this.chatIdLabel.Font = new System.Drawing.Font("SuperFrench", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chatIdLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.chatIdLabel.Location = new System.Drawing.Point(2, 7);
            this.chatIdLabel.Name = "chatIdLabel";
            this.chatIdLabel.Size = new System.Drawing.Size(79, 21);
            this.chatIdLabel.TabIndex = 0;
            this.chatIdLabel.Text = "Jhonny";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel1.Location = new System.Drawing.Point(-1, 87);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(209, 35);
            this.panel1.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel3.Location = new System.Drawing.Point(205, 120);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 317);
            this.panel3.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.severMsgBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.hostBox);
            this.Controls.Add(this.chatView);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(816, 489);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "Form1";
            this.Text = "Connected to";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.chatView.ResumeLayout(false);
            this.chatView.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox hostBox;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox severMsgBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel chatView;
        private System.Windows.Forms.Button chatSendButton;
        private System.Windows.Forms.RichTextBox chatTextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label chatIdLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
    }
}

