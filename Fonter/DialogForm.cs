using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fonter
{
    public partial class DialogForm : Form
    {
        public string Title { get { return this.Text; } set { this.Text = value; } }
        public string Content { get { return this.lblContent.Text; } set { this.lblContent.Text = value; } }

        public int NewFontHeight {
            get
            {
                return (int)this.spinHeight.Value;
            }
            set
            {
                this.spinHeight.Value = value;
            }
        }
        public int NewFontWidth
        {
            get
            {
                return (int)this.spinWidth.Value;
            }
            set
            {
                this.spinWidth.Value = value;
            }
        }

        public DialogForm()
        {
            InitializeComponent();
            btnOK.Click += BtnOK_Click;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
