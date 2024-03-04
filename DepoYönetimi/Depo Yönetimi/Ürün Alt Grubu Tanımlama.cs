using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Depo_Yönetimi
{
    public partial class Ürün_Alt_Grubu_Tanımlama : Form
    {

        SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True");
        void Databasegetir()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ÜrünAltGrubuTanımlama", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
            label7.Visible = true;
            label7.Text = dataGridView1.Rows.Count.ToString()+" tane veriniz bulunmaktadır.";
            dataGridView1.ClearSelection();
            comboBox1.SelectedIndex = -1;
            textBox1.Text = "";
            textBox2.Text = "";
        }
        public Ürün_Alt_Grubu_Tanımlama()
        {
            InitializeComponent();
        }

        
        private void Ürün_Alt_Grubu_Tanımlama_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("SELECT * FROM ÜrünGrubuTanımlama",baglanti);
            baglanti.Open();
            SqlDataReader dr;
            dr = komut.ExecuteReader();
            while (dr.Read()) 
            {
                comboBox1.Items.Add(dr["Adı"]);
            }
            
            baglanti.Close();
           // Databasegetir();
           // dataGridView1.Columns[0].Visible = false;

            textBox3.Visible = false;
            textBox4.Visible = false;
            button7.Visible = false;
            label7.Visible = false;
            label11.Visible = false;
            textBox7.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                try
                {
                    using (SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True"))
                    {
                        SqlCommand komut2 = new SqlCommand("INSERT INTO ÜrünAltGrubuTanımlama(ÜrünAltGrubuKodu, ÜrünAltGrubuAdı, BağlıÜrünKodu, BağlıÜrünAdı) VALUES (@ÜrünAltGrubuKodu, @ÜrünAltGrubuAdı, @BağlıÜrünKodu, @BağlıÜrünAdı) ALTER TABLE ÜrünAltGrubuTanımlama ADD CONSTRAINT UrunAltGrubu1 UNIQUE (BağlıÜrünAdı, ÜrünAltGrubuKodu);", baglanti);
                        komut2.Parameters.AddWithValue("@ÜrünAltGrubuKodu", textBox1.Text);
                        komut2.Parameters.AddWithValue("@ÜrünAltGrubuAdı", textBox2.Text);
                        komut2.Parameters.AddWithValue("@BağlıÜrünKodu", textBox3.Text);
                        komut2.Parameters.AddWithValue("@BağlıÜrünAdı", comboBox1.Text);

                        baglanti.Open();
                        komut2.ExecuteNonQuery();
                        
                        

                    }
                    baglanti.Close();

                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("2 farklı ürünün alt grubu kodu ve bağlı olduğu ürün grubu aynı olamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                baglanti.Open();
                SqlDataAdapter da1 = new SqlDataAdapter("SELECT * FROM ÜrünAltGrubuTanımlama WHERE BağlıÜrünAdı like'" + comboBox1.Text + "'", baglanti);
                DataTable Tablo1 = new DataTable();
                da1.Fill(Tablo1);
                dataGridView1.DataSource = Tablo1;
                baglanti.Close();
                dataGridView1.Columns[0].Visible = false;
                label7.Visible = true;
                label7.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
                dataGridView1.ClearSelection();
                comboBox1.SelectedIndex = -1;
                textBox1.Text = "";
                textBox2.Text = "";
            }
            else
            {
                MessageBox.Show("Ürün Grubu, Ürün Alt Grubu, Ürün Adı, Ürün Kodu boş bırakılamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            SqlCommand komut1 = new SqlCommand("SELECT * FROM ÜrünGrubuTanımlama WHERE Adı like '"+comboBox1.Text+"'",baglanti); 
            baglanti.Close();
            baglanti.Open();
            SqlDataReader dr1 = komut1.ExecuteReader();
            while (dr1.Read())
            {
                textBox3.Text = dr1[1].ToString();
            }
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                
                        SqlCommand komut3 = new SqlCommand("Delete FROM ÜrünAltGrubuTanımlama WHERE ÜrünAltGrubuKodu = @ÜrünAltGrubuKodu AND BağlıÜrünAdı = @BağlıÜrünAdı", baglanti);
                        komut3.Parameters.AddWithValue("@ÜrünAltGrubuKodu", textBox1.Text);
                        komut3.Parameters.AddWithValue("@BağlıÜrünAdı", comboBox1.Text);
                        baglanti.Open();
                        komut3.ExecuteNonQuery();
                        baglanti.Close();
                        Databasegetir();
                        dataGridView1.ClearSelection();
                        comboBox1.SelectedIndex = -1;
                        textBox1.Text = "";
                        textBox2.Text = "";
                        dataGridView1.Columns[0].Visible = false;
                    
              }
            else
            {
                MessageBox.Show("Bağlı Olduğu Ürün Grubu ve Ürün Alt Grubu Kodu boşken veri silinemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
              try
              {
                 using (SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True"))
                  {
                       SqlCommand komut3 = new SqlCommand("UPDATE ÜrünAltGrubuTanımlama SET ÜrünAltGrubuKodu = @ÜrünAltGrubuKodu, ÜrünAltGrubuAdı = @ÜrünAltGrubuAdı, BağlıÜrünKodu = @BağlıÜrünKodu, BağlıÜrünAdı = @BağlıÜrünAdı WHERE Sıra = @Sıra ", baglanti);
                        komut3.Parameters.AddWithValue("@ÜrünAltGrubuKodu", textBox1.Text);
                        komut3.Parameters.AddWithValue("@ÜrünAltGrubuAdı", textBox2.Text);
                        komut3.Parameters.AddWithValue("@BağlıÜrünKodu", textBox3.Text);
                        komut3.Parameters.AddWithValue("@BağlıÜrünAdı", comboBox1.Text);
                        komut3.Parameters.AddWithValue("@Sıra", textBox4.Text);


                        baglanti.Open();
                        komut3.ExecuteNonQuery();
                        baglanti.Close();
                        baglanti.Open();
                        SqlDataAdapter da1 = new SqlDataAdapter("SELECT * FROM ÜrünAltGrubuTanımlama WHERE BağlıÜrünAdı like'" + comboBox1.Text + "'", baglanti);
                        DataTable Tablo1 = new DataTable();
                        da1.Fill(Tablo1);
                        dataGridView1.DataSource = Tablo1;
                        baglanti.Close();
                        dataGridView1.Columns[0].Visible = false;
                        label7.Visible = true;
                        label7.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
                        dataGridView1.ClearSelection();
                        comboBox1.SelectedIndex = -1;
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                    baglanti.Close();

                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("2 farklı ürünün Bağlı Olduğu Ürün Grubu ve Ürün Alt Grubu Kodu aynı olamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
            {
                MessageBox.Show("Bağlı Olduğu Ürün Grubu ve Ürün Alt Grubu Kodu boşken veri düzenlenemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Ana_Ekran aegeçiş = new Ana_Ekran();
            aegeçiş.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedIndex != -1)
            {
                baglanti.Open();
                SqlDataAdapter da1 = new SqlDataAdapter("SELECT * FROM ÜrünAltGrubuTanımlama WHERE BağlıÜrünAdı like'" + comboBox1.Text + "'", baglanti);
                DataTable Tablo1 = new DataTable();
                da1.Fill(Tablo1);
                dataGridView1.DataSource = Tablo1;
                baglanti.Close();
                label7.Visible = true;
                label7.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
                dataGridView1.ClearSelection();
                comboBox1.SelectedIndex = -1;
                textBox1.Text = "";
                textBox2.Text = "";

            }
            else
            {
                Databasegetir();
                comboBox1.SelectedIndex = -1;
                textBox1.Text = "";
                textBox2.Text = "";
                dataGridView1.Columns[0].Visible = false;
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("DELETE FROM ÜrünAltGrubuTanımlama", baglanti);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            Databasegetir();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(label11.Visible == false)
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
                    mailicerik += "<tr>";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.ColumnIndex > 0)
                        {
                            mailicerik += "<td style='color:blue; border: 2px solid red; text-align: middle; padding: 8px;'>" + cell.Value + "</td>";
                        }
                    }
                    mailicerik += "</tr>";
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
            else
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
                    mailicerik += "<tr>";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.ColumnIndex > 0)
                        {
                            mailicerik += "<td style='color:blue; border: 2px solid red; text-align: middle; padding: 8px;'>" + cell.Value + "</td>";
                        }
                    }
                    mailicerik += "</tr>";
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
           
        }

        private void button15_Click(object sender, EventArgs e)
        {
            label11.Visible = true;
            textBox7.Visible = true;
            button15.Visible = false;
        }
    }
}
