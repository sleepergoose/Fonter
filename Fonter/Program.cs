using System;
using System.Windows.Forms;
using Fonter.BL;

namespace Fonter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();
            Messager messager = new Messager();
            Logic logic = new Logic();

            Presenter presenter = new Presenter(mainForm, messager, logic);

            Application.Run(mainForm);
        }
    }
}
