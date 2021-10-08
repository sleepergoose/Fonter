using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fonter.BL
{
    public interface IMessager
    {
        void ShowError(string error);
        void ShowExclamation(string exclamation);
        void ShowMessage(string message);
    }

    public class Messager : IMessager
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Message - Fonter", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        public void ShowError(string error)
        {
            MessageBox.Show(error, "Error - Fonter", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public void ShowExclamation(string exclamation)
        {
            MessageBox.Show(exclamation, "Exclamation - Fonter", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }
    }
}
