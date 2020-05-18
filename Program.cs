using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlyMore
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var configWorld = new World();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false); 
            Application.Run(new EnterForm(configWorld));
            Application.Run(new GameForm(configWorld));
        }
    }
}
