using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FrontWork {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormConfigurationEditor());

            FormConfigurationEditor form = new FormConfigurationEditor();
            form.SetText("hehe");
            form.ShowDialog();
            //TestForm t = new TestForm();
            //t.ShowDialog();
            return;
        }
	}
}
