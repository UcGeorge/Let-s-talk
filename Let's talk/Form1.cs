using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Let_s_talk
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void name_box_TextChanged(object sender, EventArgs e)
        {
            start_button.Enabled = true;
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            // Start networking stuffs
            ChatPeer peer = new ChatPeer();
        }
    }
}
