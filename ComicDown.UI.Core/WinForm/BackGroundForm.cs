using System;
using System.Windows.Forms;

namespace ComicDown.UI.Core
{
    public partial class BackGroundForm : Form
    {
        private int _c = 0;
        public event Action TimerTick;
        public BackGroundForm()
        {
            InitializeComponent();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TimerTick != null)
            {
                TimerTick();
            }
        }
    }
}
