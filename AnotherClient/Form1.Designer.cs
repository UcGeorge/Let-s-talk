
namespace AnotherClient
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
            this.contactsView = new System.Windows.Forms.Panel();
            this.chatView.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hostBox
            // 
            this.hostBox.Location = new System.Drawing.Point(177, 8);
            this.hostBox.Name = "hostBox";
            this.hostBox.Size = new System.Drawing.Size(174, 20);
            this.hostBox.TabIndex = 0;
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(371, 8);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(64, 20);
            this.portBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Stencil Std", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(352, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = ":";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Swis721 Ex BT", 14.25F);
            this.label2.Location = new System.Drawing.Point(14, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Connect to";
            // 
            // severMsgBox
            // 
            this.severMsgBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.severMsgBox.Location = new System.Drawing.Point(11, 33);
            this.severMsgBox.Name = "severMsgBox";
            this.severMsgBox.ReadOnly = true;
            this.severMsgBox.Size = new System.Drawing.Size(665, 20);
            this.severMsgBox.TabIndex = 4;
            this.severMsgBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // connectButton
            // 
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(445, 8);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(99, 20);
            this.connectButton.TabIndex = 5;
            this.connectButton.Text = "connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Enabled = false;
            this.disconnectButton.Location = new System.Drawing.Point(549, 8);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(99, 20);
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
            this.label3.Font = new System.Drawing.Font("Swis721 Ex BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label3.Location = new System.Drawing.Point(8, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Contacts";
            // 
            // chatView
            // 
            this.chatView.Controls.Add(this.label4);
            this.chatView.Controls.Add(this.chatSendButton);
            this.chatView.Controls.Add(this.chatTextBox);
            this.chatView.Controls.Add(this.panel2);
            this.chatView.Location = new System.Drawing.Point(177, 75);
            this.chatView.Name = "chatView";
            this.chatView.Size = new System.Drawing.Size(498, 304);
            this.chatView.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Swis721 Ex BT", 18F, System.Drawing.FontStyle.Italic);
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(14, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(470, 29);
            this.label4.TabIndex = 16;
            this.label4.Text = "Select a contact to start messaging";
            // 
            // chatSendButton
            // 
            this.chatSendButton.Font = new System.Drawing.Font("Stylus BT", 14.25F);
            this.chatSendButton.Location = new System.Drawing.Point(420, 264);
            this.chatSendButton.Name = "chatSendButton";
            this.chatSendButton.Size = new System.Drawing.Size(77, 40);
            this.chatSendButton.TabIndex = 13;
            this.chatSendButton.Text = "Send";
            this.chatSendButton.UseVisualStyleBackColor = true;
            this.chatSendButton.Click += new System.EventHandler(this.chatSendButton_Click);
            // 
            // chatTextBox
            // 
            this.chatTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chatTextBox.Font = new System.Drawing.Font("Swis721 Ex BT", 11.25F);
            this.chatTextBox.Location = new System.Drawing.Point(-1, 264);
            this.chatTextBox.Name = "chatTextBox";
            this.chatTextBox.Size = new System.Drawing.Size(422, 40);
            this.chatTextBox.TabIndex = 12;
            this.chatTextBox.Text = "Write a message...";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel2.Controls.Add(this.chatIdLabel);
            this.panel2.Location = new System.Drawing.Point(-1, -1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(664, 31);
            this.panel2.TabIndex = 0;
            // 
            // chatIdLabel
            // 
            this.chatIdLabel.AutoSize = true;
            this.chatIdLabel.Font = new System.Drawing.Font("SuperFrench", 15.75F, System.Drawing.FontStyle.Bold);
            this.chatIdLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.chatIdLabel.Location = new System.Drawing.Point(2, 6);
            this.chatIdLabel.Name = "chatIdLabel";
            this.chatIdLabel.Size = new System.Drawing.Size(79, 21);
            this.chatIdLabel.TabIndex = 0;
            this.chatIdLabel.Text = "Jhonny";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(-1, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 30);
            this.panel1.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel3.Location = new System.Drawing.Point(176, 104);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 275);
            this.panel3.TabIndex = 15;
            // 
            // contactsView
            // 
            this.contactsView.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.contactsView.Location = new System.Drawing.Point(0, 105);
            this.contactsView.Name = "contactsView";
            this.contactsView.Size = new System.Drawing.Size(177, 274);
            this.contactsView.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 390);
            this.Controls.Add(this.contactsView);
            this.Controls.Add(this.panel3);
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
            this.MaximumSize = new System.Drawing.Size(702, 429);
            this.MinimumSize = new System.Drawing.Size(702, 429);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connected to";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.chatView.ResumeLayout(false);
            this.chatView.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.Panel contactsView;
    }
}

