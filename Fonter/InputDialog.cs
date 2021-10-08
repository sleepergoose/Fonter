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
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
            this.Load += InputDialog_Load;
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {
            txtValue.Focus();
        }

        public string Value { get { return txtValue.Text; } set { txtValue.Text = value; } }
    }
}
