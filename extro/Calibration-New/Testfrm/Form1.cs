using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testfrm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Calibration.mainUtl mt = new Calibration.mainUtl();
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Controls.Add(mt);
        }

      
    }
}
