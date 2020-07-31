using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.Diagnostics;
namespace SagensVision
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

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            //UserLookAndFeel.Default.SetSkinStyle("Blue");
            UserLookAndFeel.Default.SetSkinStyle("Black");
            Process Current = Process.GetCurrentProcess();
            Process[] Processs = Process.GetProcessesByName(Current.ProcessName);

            if (Processs.Length > 1)
            {
                MessageBox.Show("警告本机已打开胶路引导程序", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                Application.Run(new FormMain());
            }

            //Application.Run(new FormMain());
        }
    }
}
