using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserProfileEntry.DataProvides;

namespace UserProfileEntry
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
            Application.Run(new UserProfileEntryForm(new UserProfileUserProfileDataProvider(), new BranchesDataProvider(), new PermissionsDataProvider()));
        }
    }
}
