using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Araç_Kiralama
{
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Müsteri mst = new Müsteri();
            mst.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Araclar arc= new Araclar();
            arc.Show();
        }
    }
}
