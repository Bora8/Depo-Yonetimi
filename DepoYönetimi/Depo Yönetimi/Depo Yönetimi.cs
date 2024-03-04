using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Depo_Yönetimi
{
    public partial class Ana_Ekran : Form
    {
        public Ana_Ekran()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e)
        {
            Ürün_Grubu_Tanımlama ügtgeçiş = new Ürün_Grubu_Tanımlama();
            ügtgeçiş.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Ürün_Alt_Grubu_Tanımlama üagtgeçiş = new Ürün_Alt_Grubu_Tanımlama();
            üagtgeçiş.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ürün_Tanımlama ütgeçiş = new Ürün_Tanımlama();
            ütgeçiş.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ürün_Hareketleri ühgeçiş = new Ürün_Hareketleri();
            ühgeçiş.Show();
            this.Hide() ;
        }
    }
}
