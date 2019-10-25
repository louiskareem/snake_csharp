using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake_csharp
{
    public partial class Form2 : Form
    {
        public static string NameText = "";
        public Form2()
        {
            InitializeComponent();
        }

        private void submitbtn_Click(object sender, EventArgs e)
        {
            NameText = name_textbox.Text;

            Form1 frm1 = new Form1();
            frm1.Show();
        }
    }
}
