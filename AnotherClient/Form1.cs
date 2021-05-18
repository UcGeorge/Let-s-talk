using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        private string clientName;
        private Dictionary<string, List<Message>> chatsByContact = new Dictionary<string, List<Message>>();


        public Form1(string clientName)
        {
            this.clientName = clientName;
            InitializeComponent();
        }

        void ShowContacts(string contacts)
        {
            contactsView.Controls.Clear();
            int x = 0;
            int y = 0;
            foreach (string coontact in contacts.Split('+'))
            {
                if (!chatsByContact.Keys.Contains(coontact))
                {
                    chatsByContact.Add(coontact, new List<Message>());
                }
                if(coontact == clientName)
                {
                    continue;
                }
                else
                {
                    contact = new Button();
                    contact.Location = new System.Drawing.Point(x, y);
                    contact.Size = new System.Drawing.Size(178, 35);
                    contact.Text = coontact;
                    contactsView.Controls.Add(this.contact);
                    contact.Click += new System.EventHandler(contactButtonHandler);
                    y += 33;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = clientName;
            hostBox.Enabled = true;
            connectButton.Enabled = true;
            portBox.Text = port_Number.ToString();
            portBox.Enabled = false;
            chatTextBox.Visible = false;
            chatSendButton.Visible = false;
            hostBox.Text = "localhost";
            hostBox.Focus();
            severMsgBox.Text = "Enter host address";
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
                    InitializeConnection();
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
            chatIdLabel.Text = currentChatID;
            chatIdLabel.Visible = true;
            chatTextBox.Visible = true;
            chatTextBox.Focus();
            chatSendButton.Visible = true;
            BuildChatBubbles(currentChatID);
        }

        private void InitializeConnection()
        {
            client.sendMessage(clientName, "Admin");
            currentChatID = "Admin";
            chatIdLabel.Text = currentChatID;
            chatTextBox.Visible = true;
            chatSendButton.Visible = true;
            chatTextBox.Focus();
            Thread listener = new Thread(listenForMessages);
            listener.Start();
        }

        private void BuildChatBubbles(string chatID)
        {
            chatsRegion.Controls.Clear();
            int y = 7;
            foreach (Message message in chatsByContact[chatID])
            {
                Panel bubble = new Panel();
                chatsRegion.Controls.Add(bubble);
                Label chatText = new Label();
                bubble.Controls.Add(chatText);

                if(message.messageType == MessageType.Incoming)
                {
                    chatText.AutoSize = true;
                    chatText.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    chatText.ForeColor = SystemColors.HighlightText;
                    chatText.Location = new Point(7, 7);
                    chatText.Name = "chatText";
                    chatText.Text = message.message;

                    bubble.BackColor = Color.Teal;
                    bubble.Name = "panel " + message.message;
                    bubble.Size = new Size(chatText.Size.Width + 13, 36);
                    bubble.Location = new Point(7, y);
                    y += 42;
                }
                else
                {
                    chatText.AutoSize = true;
                    chatText.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                    chatText.ForeColor = System.Drawing.SystemColors.ControlText;
                    chatText.Location = new System.Drawing.Point(7, 7);
                    chatText.Name = "chatText";
                    chatText.Text = message.message;

                    bubble.BackColor = System.Drawing.Color.LightCyan;
                    bubble.Name = "panel " + message.message;
                    bubble.Size = new System.Drawing.Size(chatText.Size.Width + 13, 36);
                    bubble.Location = new System.Drawing.Point(chatView.Size.Width - chatText.Size.Width - 13 - 27, y);
                    y += 42;
                }
                if(chatsByContact[chatID].Last() == message)
                {
                    bubble.Focus();
                }
            }
        }

        private void listenForMessages()
        {
            while (true)
            {
                try
                {
                    (string, string) message = client.receiveMessage();
                    switch (message.Item1)
                    {
                        case "Contacts":
                            Invoke(new Action(() =>
                            {
                                ShowContacts(message.Item2);
                            }));
                            break;
                        default:
                            if (currentChatID == message.Item1)
                            {
                                Invoke(new Action(() =>
                                {
                                    if (!chatsByContact.Keys.Contains(message.Item1))
                                    {
                                        chatsByContact.Add(message.Item1, new List<Message>());
                                    }
                                    chatsByContact[message.Item1].Add(new Message(MessageType.Incoming, message.Item2));
                                    BuildChatBubbles(message.Item1);
                                }));
                            }
                            else
                            {
                                Invoke(new Action(() =>
                                {
                                    if (!chatsByContact.Keys.Contains(message.Item1))
                                    {
                                        chatsByContact.Add(message.Item1, new List<Message>());
                                    }
                                    chatsByContact[message.Item1].Add(new Message(MessageType.Incoming, message.Item2));
                                }));
                            }
                            break;
                    }
                }catch(Exception e)
                {
                    Invoke(new Action(() =>
                    {
                        severMsgBox.Text = "An error occoured. Check the console for more info.";
                    }));
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private void chatSendButton_Click(object sender, EventArgs e)
        {
            if (chatTextBox.Text == "")
            {
                severMsgBox.Text = "Please enter a message!";
                severMsgBox.Focus();
                return;
            }
            if (client.sendMessage(chatTextBox.Text, currentChatID))
            {
                chatsByContact[currentChatID].Add(new Message(MessageType.Outgoing, chatTextBox.Text));
                BuildChatBubbles(currentChatID);
            }
            else
            {
                severMsgBox.Text = "There was an error sending messages";
            }
        }
    }

    class Message
    {
        public readonly MessageType messageType;
        public readonly string message;

        public Message(MessageType messageType, string message)
        {
            this.messageType = messageType;
            this.message = message;
        }
    }

    enum MessageType
    {
        Outgoing,
        Incoming
    }
}
