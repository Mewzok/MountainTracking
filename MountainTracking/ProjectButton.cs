using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp1
{
    class ProjectButton : Button
    {
        public string filePath { get; set; }
        public bool completed { get; set; }

        public ProjectButton()
        {
            filePath = "";
            completed = false;
            MessageBox.Show("Did the constructor run? Probably not.");
        }

    }
}
