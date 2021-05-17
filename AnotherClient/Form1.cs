using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherClient
{
    public partial class Form1 : Form
    {
        TcpClient myclient;
        private int port_Number = 1234;
        private Button contact;
        private Button prevButton;
        private string currentChatID;
        private ChatClient client;
        private int prev_y = 264;
        private string clientName;

        public Form1(string clientName)
        {
            this.clientName = clientName;
            InitializeComponent();
        }

        void ShowContacts()
        {
            int x = 0;
            int y = 104;
            for (int i = 1; i <= 5; i++)
            {
                contact = new Button();
                contact.Location = new System.Drawing.Point(x, y);
                contact.Size = new System.Drawing.Size(178, 35);
                contact.Text = "Contact " + i;
                this.Controls.Add(this.contact);
                contact.Click += new System.EventHandler(contactButtonHandler);
                y += 33;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hostBox.Enabled = true;
            connectButton.Enabled = true;
            portBox.Text = port_Number.ToString();
            portBox.Enabled = false;
            hostBox.Text = "0.0.0.0";
            hostBox.Focus();
            severMsgBox.Text = "Enter host address";
            ShowContacts();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            severMsgBox.Text = "Connecting to server";

            if (hostBox.Text == "" || portBox.Text == "")
            {
                severMsgBox.Text = "Please provide host IP address and port number.";
                hostBox.Focus();
                return;
            }
            try
            {
                hostBox.Enabled = false;
                connectButton.Enabled = false;
                myclient = new TcpClient(hostBox.Text, port_Number);

                if (myclient.Connected)
                {
                    client = new ChatClient(myclient);
                    HandleConnection();
                    hostBox.Enabled = false;
                    connectButton.Enabled = false;
                    disconnectButton.Visible = true;
                    disconnectButton.Enabled = true;
                    severMsgBox.Text = "Connected to server at " + hostBox.Text + " : " + portBox.Text;
                    label2.Text = "Connected to";
                }
                else
                {
                    severMsgBox.Text = "Failed to Connect to server at " + hostBox.Text + " : " + portBox.Text;
                }
            }
            catch (Exception k)
            {
                hostBox.Enabled = true;
                connectButton.Enabled = true;
                hostBox.Focus();
                severMsgBox.ForeColor = Color.Red;
                severMsgBox.Text = "An error occoured. Check the console for more info.";
                Console.WriteLine(k.ToString());
                return;
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            hostBox.Enabled = true;
            connectButton.Enabled = true;
            disconnectButton.Visible = false;
            hostBox.Focus();
            severMsgBox.Text = "Enter host address";
            label2.Text = "Connect to";
            client = null;
        }

        private void contactButtonHandler(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;
            currentButton.Enabled = false;
            if (prevButton != null)
            {
                prevButton.Enabled = true;
                prevButton = currentButton;
            }
            else
            {
                prevButton = currentButton;
            }
            currentChatID = currentButton.Text;
            label4.Visible = false;
            chatIdLabel.Text = currentChatID;
            chatIdLabel.Visible = true;
            chatTextBox.Visible = true;
            chatTextBox.Focus();
            chatSendButton.Visible = true;
        }

        private void HandleConnection()
        {
            (bool, string) response = client.sendMessage(clientName, "Admin");
            currentChatID = response.Item2.Split('~')[0];
            receiveBubble(response.Item2.Split('~')[1]);
            sendeBubble("My name is " + clientName);
            response = client.sendMessage("Thank you...", "Admin");
            sendeBubble("Thank you...");
            /*currentChatID = response.Item2.Split('~')[0];
            receiveBubble(response.Item2.Split('~')[1]);
            sendeBubble("My name is " + clientName);*/

            label4.Visible = false;
            chatIdLabel.Text = currentChatID;
            chatIdLabel.Visible = true;
            chatTextBox.Visible = true;
            chatSendButton.Visible = true;
            chatTextBox.Focus();
        }

        private void receiveBubble(string text)
        {
            Panel bubble = new Panel();
            chatView.Controls.Add(bubble);
            Label chatText = new Label();
            bubble.Controls.Add(chatText);

            chatText.AutoSize = true;
            chatText.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chatText.ForeColor = System.Drawing.SystemColors.HighlightText;
            chatText.Location = new System.Drawing.Point(7, 7);
            chatText.Name = "chatText";
            chatText.Text = text;

            bubble.BackColor = System.Drawing.Color.Teal;
            bubble.Location = new System.Drawing.Point(7, prev_y - 42);
            bubble.Name = "panel" + (prev_y - 42).ToString();
            bubble.Size = new System.Drawing.Size(chatText.Size.Width + 13, 36);
            prev_y -= 42;

        }

        private void sendeBubble(string text)
        {
            Panel bubble = new Panel();
            chatView.Controls.Add(bubble);
            Label chatText = new Label();
            bubble.Controls.Add(chatText);

            chatText.AutoSize = true;
            chatText.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chatText.ForeColor = System.Drawing.SystemColors.ControlText;
            chatText.Location = new System.Drawing.Point(7, 7);
            chatText.Name = "chatText";
            chatText.Text = text;

            bubble.BackColor = System.Drawing.Color.LightCyan;
            bubble.Location = new System.Drawing.Point(chatView.Size.Width - chatText.Size.Width - 13 - 7, prev_y - 42);
            bubble.Name = "panel" + (prev_y - 42).ToString();
            bubble.Size = new System.Drawing.Size(chatText.Size.Width + 13, 36);
            prev_y -= 42;
        }

        private void chatSendButton_Click(object sender, EventArgs e)
        {
            if (chatTextBox.Text == "")
            {
                severMsgBox.Text = "Please enter a message!";
                severMsgBox.Focus();
                return;
            }
            if (client.sendMessage(chatTextBox.Text, currentChatID).Item1)
            {
                sendeBubble(chatTextBox.Text);
            }
            else
            {
                severMsgBox.Text = "There was an error sending messages";
            }
        }
    }
}
