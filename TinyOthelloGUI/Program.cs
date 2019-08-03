using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace TinyOthello {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Trace.Listeners.Add(new ConsoleTraceListener());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GraphicUI.MainWindow());
        }
    }
}