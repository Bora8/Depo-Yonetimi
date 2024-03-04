using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;

namespace Depo_Yönetimi
{
    public partial class Ürün_Hareketleri : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True");
        public Ürün_Hareketleri()
        {
            InitializeComponent();
        }
        void ilkdurum()
        {
            label4.Visible = false;
            label5.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            
            label11.Visible = false;
            textBox7.Visible = false;
            textBox3.Visible = false;
            button15.Visible = false;
            label10.Visible = false;
            textBox6.Visible = false;
            
            button11.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            label6.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
        }
        void kacveri()
        {
            label6.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
            label6.Visible = true;
        }
        void kacveri1()
        {
            label7.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
            label7.Visible = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ana_Ekran aegeçiş = new Ana_Ekran();
            aegeçiş.Show();
            this.Hide();
        }

        private void Ürün_Hareketleri_Load(object sender, EventArgs e)
        {
            
            ilkdurum();
            SqlCommand komut = new SqlCommand("SELECT * FROM ÜrünTanımlama", baglanti);
            baglanti.Open();
            SqlDataReader dr;
            dr = komut.ExecuteReader();
            List<string> Liste = new List<string>();

            while (dr.Read())
            {
                if (!Liste.Contains(dr["ÜrünGrubu"]))
                {
                    comboBox1.Items.Add(dr["ÜrünGrubu"]);
                    Liste.Add(dr["ÜrünGrubu"].ToString());
                }

            }
            dr.Close();
        }

        void Databasegetir()
        {
            baglanti.Close();
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ÜrünHareketleri WHERE ÜrünGrubu like'" + comboBox1.Text + "'AND ÜrünAltGrubu like'" + comboBox2.Text + "' AND ÜrünKodu like'" + comboBox3.Text + "' ORDER BY Tarih ASC", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button10.Visible = false;
            button14.Visible = false;

            button11.Visible = true;
            button12.Visible = true;
            button13.Visible = true;
            button15.Visible = true;

            dataGridView1.DataSource = null;
            button3.Visible = false;
            button4.Visible = false;
            label4.Visible = true;
            label5.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;
            dataGridView1.Location = new Point(70, 121);

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                dataGridView1.DataSource = null;
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT ÜrünKodu, ÜrünGrubu From ÜrünHareketleri", baglanti);
                SqlDataReader dr;
                dr = komut.ExecuteReader();
                List<string> Liste = new List<string>();
                List<string> Liste1 = new List<string>();
                while (dr.Read())
                {
                    Liste.Add(dr["ÜrünKodu"].ToString());
                    Liste1.Add(dr["ÜrünGrubu"].ToString());
                }
                dr.Close();
                for (int i = 0; i < Liste.Count; i++)
                {
                    if (Liste[i] == comboBox3.Text && Liste1[i] == comboBox1.Text)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT ÜrünHareketleri.ÜrünGrubu, ÜrünHareketleri.ÜrünAltGrubu, ÜrünTanımlama.ÜrünAdı, ÜrünTanımlama.Marka, ÜrünTanımlama.Model, ÜrünHareketleri.ÜrünKodu, SUM(ÜrünHareketleri.StokMiktarı) AS ToplamStok FROM ÜrünHareketleri INNER JOIN ÜrünTanımlama ON ÜrünHareketleri.ÜrünKodu = ÜrünTanımlama.ÜrünKodu AND ÜrünHareketleri.ÜrünAltGrubu = ÜrünTanımlama.ÜrünAltGrubu WHERE ÜrünHareketleri.ÜrünKodu LIKE '" + comboBox3.Text + "' AND ÜrünHareketleri.ÜrünGrubu LIKE '" + comboBox1.Text + "' AND ÜrünHareketleri.ÜrünAltGrubu LIKE '" + comboBox2.Text + "' GROUP BY ÜrünHareketleri.ÜrünGrubu, ÜrünHareketleri.ÜrünAltGrubu, ÜrünTanımlama.ÜrünAdı, ÜrünTanımlama.Marka, ÜrünTanımlama.Model, ÜrünHareketleri.ÜrünKodu", baglanti);
                        DataTable tablo = new DataTable();
                        da.Fill(tablo);
                        dataGridView1.DataSource = tablo;

                        baglanti.Close();
                    }

                }
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1)
            {
                dataGridView1.DataSource = null;
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT ÜrünAltGrubu, ÜrünGrubu From ÜrünHareketleri", baglanti);
                SqlDataReader dr;
                dr = komut.ExecuteReader();
                List<string> Liste = new List<string>();
                List<string> Liste1 = new List<string>();
                while (dr.Read())
                {
                    Liste.Add(dr["ÜrünAltGrubu"].ToString());
                    Liste1.Add(dr["ÜrünGrubu"].ToString());
                }
                dr.Close();
                for (int i = 0; i < Liste.Count; i++)
                {
                    if (Liste[i] == comboBox2.Text && Liste1[i] == comboBox1.Text)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT ÜrünHareketleri.ÜrünGrubu, ÜrünHareketleri.ÜrünAltGrubu, ÜrünTanımlama.ÜrünAdı, ÜrünTanımlama.Marka, ÜrünTanımlama.Model, ÜrünHareketleri.ÜrünKodu, SUM(ÜrünHareketleri.StokMiktarı) AS ToplamStok FROM ÜrünHareketleri INNER JOIN ÜrünTanımlama ON ÜrünHareketleri.ÜrünKodu = ÜrünTanımlama.ÜrünKodu AND ÜrünHareketleri.ÜrünAltGrubu = ÜrünTanımlama.ÜrünAltGrubu WHERE ÜrünHareketleri.ÜrünGrubu LIKE '" + comboBox1.Text + "' AND ÜrünHareketleri.ÜrünAltGrubu LIKE '" + comboBox2.Text + "' GROUP BY ÜrünHareketleri.ÜrünGrubu, ÜrünHareketleri.ÜrünAltGrubu, ÜrünTanımlama.ÜrünAdı, ÜrünTanımlama.Marka, ÜrünTanımlama.Model, ÜrünHareketleri.ÜrünKodu", baglanti);
                        DataTable tablo = new DataTable();
                        da.Fill(tablo);
                        dataGridView1.DataSource = tablo;

                        baglanti.Close();
                    }

                }
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1)
            {
                dataGridView1.DataSource = null;
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT ÜrünGrubu From ÜrünHareketleri", baglanti);
                SqlDataReader dr;
                dr = komut.ExecuteReader();
                List<string> Liste = new List<string>();

                while (dr.Read())
                {
                    Liste.Add(dr["ÜrünGrubu"].ToString());
                }
                dr.Close();
                for (int i = 0; i < Liste.Count; i++)
                {
                    if (Liste[i] == comboBox1.Text)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT ÜrünHareketleri.ÜrünGrubu, ÜrünHareketleri.ÜrünAltGrubu, ÜrünTanımlama.ÜrünAdı, ÜrünTanımlama.Marka, ÜrünTanımlama.Model, ÜrünHareketleri.ÜrünKodu, SUM(ÜrünHareketleri.StokMiktarı) AS ToplamStok FROM ÜrünHareketleri INNER JOIN ÜrünTanımlama ON ÜrünHareketleri.ÜrünKodu = ÜrünTanımlama.ÜrünKodu AND ÜrünHareketleri.ÜrünAltGrubu = ÜrünTanımlama.ÜrünAltGrubu WHERE ÜrünHareketleri.ÜrünGrubu LIKE '" + comboBox1.Text + "' GROUP BY ÜrünHareketleri.ÜrünGrubu, ÜrünHareketleri.ÜrünAltGrubu, ÜrünTanımlama.ÜrünAdı, ÜrünTanımlama.Marka, ÜrünTanımlama.Model, ÜrünHareketleri.ÜrünKodu", baglanti);
                        DataTable tablo = new DataTable();
                        da.Fill(tablo);
                        dataGridView1.DataSource = tablo;

                        baglanti.Close();
                    }

                }
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1)
            {
                baglanti.Close();
                baglanti.Open();
                SqlDataAdapter da1 = new SqlDataAdapter("SELECT ÜrünHareketleri.ÜrünGrubu, ÜrünHareketleri.ÜrünAltGrubu, ÜrünTanımlama.ÜrünAdı, ÜrünTanımlama.Marka, ÜrünTanımlama.Model, ÜrünHareketleri.ÜrünKodu, SUM(ÜrünHareketleri.StokMiktarı) AS ToplamStok FROM ÜrünHareketleri INNER JOIN ÜrünTanımlama ON ÜrünHareketleri.ÜrünKodu = ÜrünTanımlama.ÜrünKodu AND ÜrünHareketleri.ÜrünAltGrubu = ÜrünTanımlama.ÜrünAltGrubu GROUP BY ÜrünHareketleri.ÜrünGrubu, ÜrünHareketleri.ÜrünAltGrubu, ÜrünTanımlama.ÜrünAdı, ÜrünTanımlama.Marka, ÜrünTanımlama.Model, ÜrünHareketleri.ÜrünKodu", baglanti);
                DataTable tablo1 = new DataTable();
                da1.Fill(tablo1);
                dataGridView1.DataSource = tablo1;

                baglanti.Close();
            }
            kacveri();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            ilkdurum();
            dataGridView1.Location = new Point(70, 83);
            button3.Visible = true;
            button4.Visible = true;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            
            button1.Visible = true;
            button2.Visible = true;
            button10.Visible = true;
            button14.Visible = true;
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox2.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                textBox1.Enabled = false;
            }
            else
            {
                textBox1.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            DateTime tarih = DateTime.Now;
            string tarihmetni = tarih.ToString("g");

            if (textBox1.Text.Length > 0)
            {
                textBox4.Text = "Giriş";
            }
            else if (textBox2.Text.Length > 0)
            {
                textBox4.Text = "Çıkış";
            }

            if (textBox2.Text.Length > 0)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT SUM(StokMiktarı) From ÜrünHareketleri WHERE ÜrünGrubu = '" + comboBox1.Text + "'AND ÜrünAltGrubu = '" + comboBox2.Text + "'AND ÜrünKodu = '" + comboBox3.Text + "'", baglanti);
                object sonuç = komut.ExecuteScalar();
                textBox3.Text = Convert.ToString(sonuç);
                baglanti.Close();
                if (Convert.ToInt32(textBox2.Text) > Convert.ToInt32(textBox3.Text))
                {
                    MessageBox.Show("Girdiğiniz çıkış miktarı ile birlikte stok miktarınız -li olmaktadır.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text) && !string.IsNullOrWhiteSpace(comboBox3.Text) && (!string.IsNullOrWhiteSpace(textBox1.Text) || !string.IsNullOrWhiteSpace(textBox2.Text)))
            {
                /*         try
                         {              */
                using (SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True"))

                {
                    SqlCommand komut2 = new SqlCommand("INSERT INTO ÜrünHareketleri(ÜrünGrubu, ÜrünAltGrubu, ÜrünKodu, StokMiktarı, Tarih, Açıklama) VALUES (@ÜrünGrubu, @ÜrünAltGrubu, @ÜrünKodu, @StokMiktarı, @Tarih, @Açıklama) ", baglanti);
                    komut2.Parameters.AddWithValue("@ÜrünGrubu", comboBox1.Text);
                    komut2.Parameters.AddWithValue("@ÜrünAltGrubu", comboBox2.Text);
                    komut2.Parameters.AddWithValue("@ÜrünKodu", comboBox3.Text);
                    if (textBox1.Text.Length > 0)
                    {
                        komut2.Parameters.AddWithValue("@StokMiktarı", textBox1.Text);
                    }
                    else if (textBox2.TextLength > 0)
                    {
                        komut2.Parameters.AddWithValue("@StokMiktarı", "-" + textBox2.Text);
                    }
                    komut2.Parameters.AddWithValue("@Tarih", tarih);
                    komut2.Parameters.AddWithValue("@Açıklama", textBox4.Text);
                    baglanti.Open();
                    komut2.ExecuteNonQuery();
                }
                baglanti.Close();

            }

            /*         catch (SqlException ex)
                     {
                         if (ex.Number == 2627)
                         {
                             MessageBox.Show("2 farklı ürünün Ürün Grubu, Ürün Alt Grubu ve Ürün Kodu aynı olamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                     }   */
            else
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Grubu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Alt Grubu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (comboBox3.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Kodu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (textBox1.Text == "" && textBox2.Text == "")
                {
                    MessageBox.Show("Ürün Giriş-Çıkış kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Databasegetir();
            dataGridView1.Columns[0].Visible = false;
            kacveri1();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Close();
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            List<string> Liste1 = new List<string>();
            SqlCommand komut1 = new SqlCommand("SELECT ÜrünAltGrubu FROM ÜrünTanımlama WHERE ÜrünGrubu = '" + comboBox1.Text + "'", baglanti);
            baglanti.Open();
            SqlDataReader dr1 = komut1.ExecuteReader();

            while (dr1.Read())
            {
                if (!Liste1.Contains(dr1["ÜrünAltGrubu"]))
                {
                    comboBox2.Items.Add(dr1["ÜrünAltGrubu"]);
                    Liste1.Add(dr1["ÜrünAltGrubu"].ToString());
                }

            }
            dr1.Close();
            baglanti.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Close();
            comboBox3.Text = "";
            comboBox3.Items.Clear();
            List<string> Liste2 = new List<string>();
            SqlCommand komut3 = new SqlCommand("SELECT ÜrünKodu FROM ÜrünTanımlama WHERE ÜrünAltGrubu = '" + comboBox2.Text + "'", baglanti);
            baglanti.Open();
            SqlDataReader dr3 = komut3.ExecuteReader();

            while (dr3.Read())
            {
                if (!Liste2.Contains(dr3["ÜrünKodu"]))
                {
                    comboBox3.Items.Add(dr3["ÜrünKodu"]);
                    Liste2.Add(dr3["ÜrünKodu"].ToString());
                }

            }
            dr3.Close();
            baglanti.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                dataGridView1.DataSource = null;
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT ÜrünKodu, ÜrünGrubu From ÜrünHareketleri ORDER BY Tarih DESC", baglanti);
                SqlDataReader dr;
                dr = komut.ExecuteReader();
                List<string> Liste = new List<string>();
                List<string> Liste1 = new List<string>();
                while (dr.Read())
                {
                    Liste.Add(dr["ÜrünKodu"].ToString());
                    Liste1.Add(dr["ÜrünGrubu"].ToString());
                }
                dr.Close();
                for (int i = 0; i < Liste.Count; i++)
                {
                    if (Liste[i] == comboBox3.Text && Liste1[i] == comboBox1.Text)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ÜrünHareketleri WHERE ÜrünKodu LIKE '" + comboBox3.Text + "' AND ÜrünGrubu LIKE '" + comboBox1.Text + "' AND ÜrünAltGrubu LIKE '" + comboBox2.Text + "' ", baglanti);
                        DataTable tablo = new DataTable();
                        da.Fill(tablo);
                        dataGridView1.DataSource = tablo;
                        dataGridView1.ClearSelection();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        baglanti.Close();
                    }

                }
                dataGridView1.Columns[0].Visible = false;
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1)
            {
                dataGridView1.DataSource = null;
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT ÜrünAltGrubu, ÜrünGrubu From ÜrünHareketleri ORDER BY Tarih DESC", baglanti);
                SqlDataReader dr;
                dr = komut.ExecuteReader();
                List<string> Liste = new List<string>();
                List<string> Liste1 = new List<string>();
                while (dr.Read())
                {
                    Liste.Add(dr["ÜrünAltGrubu"].ToString());
                    Liste1.Add(dr["ÜrünGrubu"].ToString());
                }
                dr.Close();
                for (int i = 0; i < Liste.Count; i++)
                {
                    if (Liste[i] == comboBox2.Text && Liste1[i] == comboBox1.Text)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ÜrünHareketleri WHERE ÜrünGrubu LIKE '" + comboBox1.Text + "' AND ÜrünAltGrubu LIKE '" + comboBox2.Text + "'", baglanti);
                        DataTable tablo = new DataTable();
                        da.Fill(tablo);
                        dataGridView1.DataSource = tablo;
                        dataGridView1.ClearSelection();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        baglanti.Close();
                    }

                }
                
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1)
            {
                dataGridView1.DataSource = null;
                baglanti.Close();
                baglanti.Open();
                SqlCommand komut = new SqlCommand("SELECT ÜrünGrubu From ÜrünHareketleri ORDER BY Tarih DESC", baglanti);
                SqlDataReader dr;
                dr = komut.ExecuteReader();
                List<string> Liste = new List<string>();

                while (dr.Read())
                {
                    Liste.Add(dr["ÜrünGrubu"].ToString());
                }
                dr.Close();
                for (int i = 0; i < Liste.Count; i++)
                {
                    if (Liste[i] == comboBox1.Text)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ÜrünHareketleri WHERE ÜrünGrubu LIKE '" + comboBox1.Text + "'", baglanti);
                        DataTable tablo = new DataTable();
                        da.Fill(tablo);
                        dataGridView1.DataSource = tablo;
                        dataGridView1.ClearSelection();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        baglanti.Close();
                    }

                }
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1)
            {
                baglanti.Close();
                baglanti.Open();
                SqlDataAdapter da1 = new SqlDataAdapter("SELECT * FROM ÜrünHareketleri ORDER BY Tarih DESC", baglanti);
                DataTable tablo1 = new DataTable();
                da1.Fill(tablo1);
                dataGridView1.DataSource = tablo1;
                dataGridView1.ClearSelection();
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                baglanti.Close();
            }
            dataGridView1.Columns[0].Visible = false;
            kacveri1();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DateTime tarih = DateTime.Now;
            string tarihmetni = tarih.ToString("g");

            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text) && !string.IsNullOrWhiteSpace(comboBox3.Text) && (!string.IsNullOrWhiteSpace(textBox1.Text) || !string.IsNullOrWhiteSpace(textBox2.Text)))
            {
                /*         try
                         {              */
                using (SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True"))

                {
                    SqlCommand komut2 = new SqlCommand("UPDATE ÜrünHareketleri SET ÜrünGrubu = @ÜrünGrubu, ÜrünAltGrubu = @ÜrünAltGrubu, ÜrünKodu = @ÜrünKodu, StokMiktarı = @StokMiktarı, Tarih = @Tarih, Açıklama = @Açıklama WHERE Sıra = @Sıra ", baglanti);
                    komut2.Parameters.AddWithValue("@ÜrünGrubu", comboBox1.Text);
                    komut2.Parameters.AddWithValue("@ÜrünAltGrubu", comboBox2.Text);
                    komut2.Parameters.AddWithValue("@ÜrünKodu", comboBox3.Text);
                    if (textBox1.Text.Length > 0)
                    {
                        komut2.Parameters.AddWithValue("@StokMiktarı", textBox1.Text);
                    }
                    else if (textBox2.TextLength > 0)
                    {
                        komut2.Parameters.AddWithValue("@StokMiktarı", "-" + textBox2.Text);
                    }
                    komut2.Parameters.AddWithValue("@Tarih", tarih);
                    komut2.Parameters.AddWithValue("@Açıklama", textBox4.Text);
                    komut2.Parameters.AddWithValue("@Sıra", textBox5.Text);
                    baglanti.Open();
                    komut2.ExecuteNonQuery();
                }
                baglanti.Close();

            }

            /*         catch (SqlException ex)
                     {
                         if (ex.Number == 2627)
                         {
                             MessageBox.Show("2 farklı ürünün Ürün Grubu, Ürün Alt Grubu ve Ürün Kodu aynı olamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                     }   */
            else
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Grubu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Alt Grubu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (comboBox3.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Kodu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Ürün Giriş-Çıkış kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Databasegetir();
            dataGridView1.Columns[0].Visible = false;
            kacveri1();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (button9.Visible == true)
            {
                if (Convert.ToInt32(dataGridView1.CurrentRow.Cells[4].Value) > 0)
                {
                    textBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    textBox2.Text = "";
                }
                else if (Convert.ToInt32(dataGridView1.CurrentRow.Cells[4].Value) < 0)
                {
                    textBox2.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    textBox1.Text = "";
                }


                textBox4.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                textBox5.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                comboBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                comboBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                comboBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {


            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text) && !string.IsNullOrWhiteSpace(comboBox3.Text) && (!string.IsNullOrWhiteSpace(textBox1.Text) || !string.IsNullOrWhiteSpace(textBox2.Text)))
            {
                {
                    SqlCommand komut = new SqlCommand("DELETE ÜrünHareketleri WHERE Sıra = @Sıra ", baglanti);
                    komut.Parameters.AddWithValue("@Sıra", textBox5.Text);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    Databasegetir();
                    dataGridView1.ClearSelection();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                    comboBox3.SelectedIndex = -1;
                    dataGridView1.Columns[0].Visible = false;

                }
                baglanti.Close();

            }

            else
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Grubu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Alt Grubu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (comboBox3.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Kodu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Ürün Giriş-Çıkış kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //    Databasegetir();
            //    dataGridView1.Columns[0].Visible = false;
            kacveri1();
        }

        private void comboBox1_MouseCaptureChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void comboBox2_MouseCaptureChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void comboBox3_MouseCaptureChanged(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = -1;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].Visible = false;
            if (label10.Visible == true)
            {
                string mailicerik = "<table width='100%' style='border:1px solid red;'>";

                mailicerik += "<tr>";
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    mailicerik += "<th style='color:white; background-color:blue; border: 2px solid red; text-align: middle; padding: 8px;'>" + column.HeaderText + "</th>";
                }
                mailicerik += "</tr>";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    bool stokmiktarı = false;
                    mailicerik += "<tr>";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellValue = cell.Value.ToString();
                        string cellStyle = "color:blue; border: 2px solid red; text-align: middle; padding: 8px;";

                        if (cell.OwningColumn.HeaderText == "ToplamStok" && !string.IsNullOrEmpty(cellValue) && int.Parse(cellValue) > 10)
                        {
                            stokmiktarı = true;
                        }

                        mailicerik += "<td style='" + cellStyle + "'>" + cellValue + "</td>";
                    }


                    string rowStyle = "color:blue; border: 2px solid red; text-align: middle; padding: 8px;";
                    if (stokmiktarı)
                    {
                        rowStyle += "background-color:green;";
                    }

                    mailicerik = mailicerik.Replace("<tr>", "<tr style='" + rowStyle + "'>");
                }

                mailicerik += "</table>";

                MailMessage mesaj = new MailMessage();
                mesaj.To.Add(textBox6.Text);
                mesaj.From = new MailAddress("dene8me@outlook.com", "Koel");
                mesaj.Subject = ("İstediğiniz Bilgiler");
                mesaj.IsBodyHtml = true;
                mesaj.Body = mailicerik;

                SmtpClient sc = new SmtpClient();
                sc.UseDefaultCredentials = true;
                sc.Credentials = new System.Net.NetworkCredential("dene8me@outlook.com", "koelkoel5656");
                sc.Host = "smtp.outlook.com";
                sc.Port = 587;
                sc.EnableSsl = true;
                sc.Send(mesaj);
                mesaj.Dispose();
                MessageBox.Show("Başarıyla Gönderildi.");
            }
            else
            {
                string mailicerik = "<table width='100%' style='border:1px solid red;'>";

                mailicerik += "<tr>";
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    mailicerik += "<th style='color:white; background-color:blue; border: 2px solid red; text-align: middle; padding: 8px;'>" + column.HeaderText + "</th>";
                }
                mailicerik += "</tr>";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    bool stokmiktarı = false;
                    mailicerik += "<tr>";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellValue = cell.Value.ToString();
                        string cellStyle = "color:blue; border: 2px solid red; text-align: middle; padding: 8px;";

                        if (cell.OwningColumn.HeaderText == "ToplamStok" && !string.IsNullOrEmpty(cellValue) && int.Parse(cellValue) > 10)
                        {
                            stokmiktarı = true;
                        }

                        mailicerik += "<td style='" + cellStyle + "'>" + cellValue + "</td>";
                    }


                    string rowStyle = "color:blue; border: 2px solid red; text-align: middle; padding: 8px;";
                    if (stokmiktarı)
                    {
                        rowStyle += "background-color:green;";
                    }

                    mailicerik = mailicerik.Replace("<tr>", "<tr style='" + rowStyle + "'>");
                }

                mailicerik += "</table>";

                MailMessage mesaj = new MailMessage();
                mesaj.To.Add("boraulker8@gmail.com");
                mesaj.From = new MailAddress("dene8me@outlook.com", "Koel");
                mesaj.Subject = ("İstediğiniz Bilgiler");
                mesaj.IsBodyHtml = true;
                mesaj.Body = mailicerik;

                SmtpClient sc = new SmtpClient();
                sc.UseDefaultCredentials = true;
                sc.Credentials = new System.Net.NetworkCredential("dene8me@outlook.com", "koelkoel5656");
                sc.Host = "smtp.outlook.com";
                sc.Port = 587;
                sc.EnableSsl = true;
                sc.Send(mesaj);
                mesaj.Dispose();
                MessageBox.Show("Başarıyla Gönderildi.");
            }
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
            if (label11.Visible == true)
            {
                string mailicerik = "<table width='100%' style='border:1px solid red;'>";

                mailicerik += "<tr>";
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    if (column.Index > 0)
                    {
                        mailicerik += "<th style='color:white; background-color:blue; border: 2px solid red; text-align: middle; padding: 8px;'>" + column.HeaderText + "</th>";
                    }
                }
                mailicerik += "</tr>";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    bool stokmiktarı = false;
                    mailicerik += "<tr>";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if(cell.ColumnIndex > 0)
                        {
                            string cellValue = cell.Value.ToString();
                            string cellStyle = "color:blue; border: 2px solid red; text-align: middle; padding: 8px;";

                            if (cell.OwningColumn.HeaderText == "ToplamStok" && !string.IsNullOrEmpty(cellValue) && int.Parse(cellValue) > 10)
                            {
                                stokmiktarı = true;
                            }

                            mailicerik += "<td style='" + cellStyle + "'>" + cellValue + "</td>";
                        }
                        
                    }


                    string rowStyle = "color:blue; border: 2px solid red; text-align: middle; padding: 8px;";
                    if (stokmiktarı)
                    {
                        rowStyle += "background-color:green;";
                    }

                    mailicerik = mailicerik.Replace("<tr>", "<tr style='" + rowStyle + "'>");
                }

                mailicerik += "</table>";

                MailMessage mesaj = new MailMessage();
                mesaj.To.Add(textBox7.Text);
                mesaj.From = new MailAddress("dene8me@outlook.com", "Koel");
                mesaj.Subject = ("İstediğiniz Bilgiler");
                mesaj.IsBodyHtml = true;
                mesaj.Body = mailicerik;

                SmtpClient sc = new SmtpClient();
                sc.UseDefaultCredentials = true;
                sc.Credentials = new System.Net.NetworkCredential("dene8me@outlook.com", "koelkoel5656");
                sc.Host = "smtp.outlook.com";
                sc.Port = 587;
                sc.EnableSsl = true;
                sc.Send(mesaj);
                mesaj.Dispose();
                MessageBox.Show("Başarıyla Gönderildi.");
            }
            else
            {
                string mailicerik = "<table width='100%' style='border:1px solid red;'>";

                mailicerik += "<tr>";
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    if(column.Index > 0)
                    {
                        mailicerik += "<th style='color:white; background-color:blue; border: 2px solid red; text-align: middle; padding: 8px;'>" + column.HeaderText + "</th>";
                    }

                }
                mailicerik += "</tr>";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {

                    bool stokmiktarı = false;
                    mailicerik += "<tr>";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.ColumnIndex > 0)
                        {
                            string cellValue = cell.Value.ToString();
                            string cellStyle = "color:blue; border: 2px solid red; text-align: middle; padding: 8px;";

                            if (cell.OwningColumn.HeaderText == "ToplamStok" && !string.IsNullOrEmpty(cellValue) && int.Parse(cellValue) > 10)
                            {
                                stokmiktarı = true;
                            }

                            mailicerik += "<td style='" + cellStyle + "'>" + cellValue + "</td>";
                        }

                    }

                    string rowStyle = "color:blue; border: 2px solid red; text-align: middle; padding: 8px;";
                    if (stokmiktarı)
                    {
                        rowStyle += "background-color:green;";
                    }

                    mailicerik = mailicerik.Replace("<tr>", "<tr style='" + rowStyle + "'>");
                }

                mailicerik += "</table>";

                MailMessage mesaj = new MailMessage();
                mesaj.To.Add("boraulker8@gmail.com");
                mesaj.From = new MailAddress("dene8me@outlook.com", "Koel");
                mesaj.Subject = ("İstediğiniz Bilgiler");
                mesaj.IsBodyHtml = true;
                mesaj.Body = mailicerik;

                SmtpClient sc = new SmtpClient();
                sc.UseDefaultCredentials = true;
                sc.Credentials = new System.Net.NetworkCredential("dene8me@outlook.com", "koelkoel5656");
                sc.Host = "smtp.outlook.com";
                sc.Port = 587;
                sc.EnableSsl = true;
                sc.Send(mesaj);
                mesaj.Dispose();
                MessageBox.Show("Başarıyla Gönderildi.");
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            Ana_Ekran aegeçiş = new Ana_Ekran();
            aegeçiş.Show();
            this.Hide();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            label10.Visible = true;
            textBox6.Visible = true;
            button14.Visible = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            label11.Visible = true;
            textBox7.Visible = true;
            button15.Visible = false;
        }
    }
}
