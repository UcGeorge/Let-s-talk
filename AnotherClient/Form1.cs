using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private List<ChatFile> chatFiles = new List<ChatFile>();


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
                if (coontact == clientName)
                {
                    continue;
                }
                else
                {
                    contact = new Button();
                    contact.Location = new Point(x, y);
                    contact.Size = new Size(178, 35);
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
            fileButton.Visible = false;
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
            if (currentButton.Text == "Admin" || currentButton.Text == "Broadcast")
            {
                fileButton.Visible = false;
                BuildChatBubbles(currentChatID);
            }
            else
            {
                fileButton.Visible = true;
                BuildChatBubbles(currentChatID);
            }
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
            int inc = 0;
            foreach (Message message in chatsByContact[chatID])
            {
                if (message.messageType == MessageType.Incoming)
                {
                    Panel bubble = new Panel();
                    chatsRegion.Controls.Add(bubble);
                    Label chatText = new Label();
                    bubble.Controls.Add(chatText);

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
                    inc = bubble.Height + 6;
                    y += inc;
                    if (chatsByContact[chatID].Last() == message)
                    {
                        bubble.Focus();
                    }
                }
                else if (message.messageType == MessageType.File)
                {
                    Panel fileBubble = new Panel();
                    chatsRegion.Controls.Add(fileBubble);
                    Label fileBubbleSender = new Label();
                    Label fileBubbleName = new Label();
                    Button fileBubbleButton = new Button();
                    Label fileBubbleSize = new Label();
                    // 
                    // fileBubble
                    // 
                    fileBubble.BackColor = Color.Teal;
                    fileBubble.Controls.Add(fileBubbleSize);
                    fileBubble.Controls.Add(fileBubbleButton);
                    fileBubble.Controls.Add(fileBubbleName);
                    fileBubble.Controls.Add(fileBubbleSender);
                    fileBubble.Location = new Point(7, y);
                    fileBubble.Name = "fileBubble";
                    fileBubble.Size = new Size(485, 75);
                    fileBubble.TabIndex = 0;
                    // 
                    // fileBubbleSender
                    // 
                    fileBubbleSender.AutoSize = true;
                    fileBubbleSender.Font = new Font("News701 BT", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    fileBubbleSender.ForeColor = SystemColors.ControlLightLight;
                    fileBubbleSender.Location = new Point(5, 7);
                    fileBubbleSender.Name = "fileBubbleSender";
                    fileBubbleSender.Size = new Size(69, 20);
                    fileBubbleSender.TabIndex = 0;
                    fileBubbleSender.Text = message.chatFile.Sender;
                    // 
                    // fileBubbleName
                    // 
                    fileBubbleName.AutoSize = true;
                    fileBubbleName.Font = new Font("News701 BT", 12F, FontStyle.Bold);
                    fileBubbleName.ForeColor = SystemColors.ControlLightLight;
                    fileBubbleName.Location = new Point(5, 28);
                    fileBubbleName.Name = "fileBubbleName";
                    fileBubbleName.Size = new Size(218, 20);
                    fileBubbleName.TabIndex = 1;
                    fileBubbleName.Text = message.chatFile.Name.Split('\\').Last();
                    // 
                    // fileBubbleButton
                    // 
                    fileBubbleButton.BackColor = Color.PaleTurquoise;
                    fileBubbleButton.FlatStyle = FlatStyle.Popup;
                    fileBubbleButton.Font = new Font("News701 BT", 12F, ((FontStyle)((FontStyle.Bold | FontStyle.Italic))), GraphicsUnit.Point, ((byte)(0)));
                    fileBubbleButton.ForeColor = Color.Teal;
                    fileBubbleButton.Location = new Point(402, 3);
                    fileBubbleButton.Name = message.chatFile.Name;
                    fileBubbleButton.Size = new Size(80, 69);
                    fileBubbleButton.TabIndex = 2;
                    fileBubbleButton.Text = "save as";
                    fileBubbleButton.UseVisualStyleBackColor = false;
                    fileBubbleButton.Click += new EventHandler(SaveFile);
                    // 
                    // fileBubbleSize
                    // 
                    fileBubbleSize.AutoSize = true;
                    fileBubbleSize.Font = new Font("News701 BT", 12F, FontStyle.Bold);
                    fileBubbleSize.ForeColor = SystemColors.ControlLightLight;
                    fileBubbleSize.Location = new Point(5, 51);
                    fileBubbleSize.Name = "fileBubbleSize";
                    fileBubbleSize.Size = new Size(92, 20);
                    fileBubbleSize.TabIndex = 3;
                    fileBubbleSize.Text = message.chatFile.Size + " bytes";
                    inc = fileBubble.Height + 6;
                    y += inc;
                    if (chatsByContact[chatID].Last() == message)
                    {
                        fileBubble.Focus();
                    }
                }
                else
                {
                    Panel bubble = new Panel();
                    chatsRegion.Controls.Add(bubble);
                    Label chatText = new Label();
                    bubble.Controls.Add(chatText);

                    chatText.AutoSize = true;
                    chatText.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point);
                    chatText.ForeColor = SystemColors.ControlText;
                    chatText.Location = new Point(7, 7);
                    chatText.Name = "chatText";
                    chatText.Text = message.message;

                    bubble.BackColor = Color.LightCyan;
                    bubble.Name = "panel " + message.message;
                    bubble.Size = new Size(chatText.Size.Width + 13, 36);
                    bubble.Location = new Point(chatView.Size.Width - chatText.Size.Width - 13 - 27, y);
                    inc = bubble.Height + 6;
                    y += inc;
                    if (chatsByContact[chatID].Last() == message)
                    {
                        bubble.Focus();
                    }
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

                        case "File":
                            if(message.Item2 != null && message.Item2 != "")
                            {
                                Invoke(new Action(() =>
                                {
                                    if (!chatsByContact.Keys.Contains(message.Item2.Split('|')[0]))
                                    {
                                        chatsByContact.Add(message.Item2.Split('|')[0], new List<Message>());
                                    }
                                    chatsByContact[message.Item2.Split('|')[0]].Add(new Message(MessageType.File, "", new ChatFile(message.Item2.Split('|')[1], message.Item2.Split('|')[2], message.Item2.Split('|')[0])));
                                }));

                                if (currentChatID == message.Item2.Split('|')[0])
                                {
                                    Invoke(new Action(() =>
                                    {
                                        BuildChatBubbles(message.Item2.Split('|')[0]);
                                    }));
                                }
                            }
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
                }
                catch (Exception e)
                {
                    try
                    {
                        Invoke(new Action(() =>
                        {
                            severMsgBox.Text = "An error occoured. Check the console for more info.";
                        }));
                    }
                    catch(Exception f)
                    {
                        return;
                    }
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
                chatTextBox.Text = "";
                chatTextBox.Focus();
            }
            else
            {
                severMsgBox.Text = "There was an error sending messages";
            }
        }

        private void fileButton_Click(object sender, EventArgs e)
        {
            string fileName = "";
            byte[] content = null;
            int size = -1;

            var t = new Thread(() =>
            {
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    fileName = openFileDialog1.FileName;
                    try
                    {
                        content = File.ReadAllBytes(fileName);
                        size = content.Length;
                    }
                    catch (IOException)
                    {
                    }
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            Console.WriteLine(String.Format("File: {0}\nSize: {1} bytes", fileName, size));
            var t2 = new Thread(() =>
            {
                Invoke(new Action(() =>
                {
                    severMsgBox.Text = "Sending a file: " + fileName;
                }));

                if (!client.sendFile(content, currentChatID, fileName))
                {
                    Invoke(new Action(() =>
                    {
                        severMsgBox.Text = "There was an error sending the file";
                    }));
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        chatsByContact[currentChatID].Add(new Message(MessageType.Outgoing, String.Format("[FILE] {0}, {1} bytes", fileName, size)));
                        BuildChatBubbles(currentChatID);
                    }));
                }

                Invoke(new Action(() =>
                {
                    severMsgBox.Text = "";
                }));
            });
            t2.SetApartmentState(ApartmentState.STA);
            t2.Start();
        }

        private void SaveFile(object sender, EventArgs e)
        {
            Button saveButton = (Button)sender;
            string fileName = null;
            byte[] receivedFile = null;
            Thread t = new Thread(() =>
            {
                fileName = (saveButton).Name;
                receivedFile = client.receiveFile(fileName, clientName, hostBox.Text);
                Console.WriteLine(String.Format("Received File: {0}\nSize: {1} bytes", fileName.Split('\\').Last(), receivedFile.Length));
            });
            t.Start();
            t.Join();

            saveFileDialog1.FileName = fileName.Split('\\').Last();
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                File.WriteAllBytes(saveFileDialog1.FileName, receivedFile);
                saveButton.Name = saveFileDialog1.FileName;
            }

            saveButton.Text = "open";
            saveButton.Click -= new EventHandler(SaveFile);
            saveButton.Click += new EventHandler((object s, EventArgs se) => {
                Process.Start(saveFileDialog1.FileName);
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //close all streams...
            client.sendMessage("close", "Close");
            client.Close();
        }
    }

    class Message
    {
        public readonly MessageType messageType;
        public readonly string message;
        public readonly ChatFile chatFile;

        public Message(MessageType messageType, string message, ChatFile chatFile = null)
        {
            this.messageType = messageType;
            this.message = message;
            this.chatFile = chatFile;
        }
    }

    class ChatFile
    {
        private string fileName;
        private string size;
        private string sender;

        public ChatFile(string fileName, string size, string sender)
        {
            this.size = size;
            this.sender = sender;
            this.fileName = fileName;
        }

        public string Name { get => fileName; }
        public string Size { get => size; }
        public string Sender { get => sender; }

        public static List<ChatFile> Map<T>(List<T> e, Func<T, ChatFile> func)
        {
            List<ChatFile> chatFiles = new List<ChatFile>();

            foreach(T i in e)
            {
                chatFiles.Add(func(i));
            }

            return chatFiles;
        }
    }

    enum MessageType
    {
        Outgoing,
        Incoming,
        File
    }
}
