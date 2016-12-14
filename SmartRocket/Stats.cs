using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartRocket
{
    public partial class Stats : Form
    {
        public Stats()
        {
            InitializeComponent();
        }
        public void AddItemToList(String s)
        {
            listBox1.Items.Add(s);
        }
    }
}
