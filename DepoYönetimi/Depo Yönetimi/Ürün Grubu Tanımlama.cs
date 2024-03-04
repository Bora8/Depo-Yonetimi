using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using System.Net.Mail;

namespace Depo_Yönetimi
{
    public partial class Ürün_Grubu_Tanımlama : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True");

        void Databasegetir()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ÜrünGrubuTanımlama", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
            dataGridView1.ClearSelection();
            textBox1.Text = "";
            textBox2.Text = "";
            label5.Visible = true;
            label5.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
        }
        public Ürün_Grubu_Tanımlama()
        {
            InitializeComponent();
        }

        private void Ürün_Grubu_Tanımlama_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Ürün_Grubu_Tanımlama_Load(object sender, EventArgs e)
        {
            button4.Visible = false;
            Databasegetir();
            dataGridView1.Columns[0].Visible = false;
            textBox3.Visible = false;
            label11.Visible = false;
            textBox7.Visible = false;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
           if (!string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
           {
                try
              {
                using (SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True"))
                {
                    SqlCommand komut = new SqlCommand("INSERT INTO ÜrünGrubuTanımlama(Kodu, Adı) VALUES (@Kodu, @Adı)", baglanti);
                    komut.Parameters.AddWithValue("@Kodu", textBox1.Text);
                    komut.Parameters.AddWithValue("@Adı", textBox2.Text);

                    baglanti.Open();
                    komut.ExecuteNonQuery();
                }
                baglanti.Close();
                Databasegetir();
              }
              catch (SqlException ex)
              {
                if (ex.Number == 2627) 
                {
                    MessageBox.Show("Aynı Kod değeri zaten mevcut","Depo Yönetimi",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
               }
            dataGridView1.ClearSelection();
            textBox1.Text = "";
            textBox2.Text = "";
           }
           else
           {
               MessageBox.Show("Ürün Grubu ve Ürün Adı boşken veri eklenemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("DELETE FROM ÜrünGrubuTanımlama", baglanti);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            Databasegetir();
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                SqlCommand komut = new SqlCommand("Delete FROM ÜrünGrubuTanımlama WHERE @Kodu = Kodu", baglanti);
                komut.Parameters.AddWithValue("@Kodu", textBox1.Text);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                Databasegetir();
                dataGridView1.ClearSelection();
                textBox1.Text = "";
                textBox2.Text = "";
            }
            else
            {
                MessageBox.Show("Ürün Grubu ve Ürün Adı boşken veri silinemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                try
                {
                    using (SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True"))

                    {
                        SqlCommand komut = new SqlCommand("UPDATE ÜrünGrubuTanımlama SET Kodu = @Kodu, Adı = @Adı WHERE Sıra = @Sıra", baglanti);
                        komut.Parameters.AddWithValue("@Kodu", textBox1.Text);
                        komut.Parameters.AddWithValue("@Adı", textBox2.Text);
                        komut.Parameters.AddWithValue("@Sıra", textBox3.Text);
                        baglanti.Open();
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        Databasegetir();
                        dataGridView1.ClearSelection();
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("2 farklı ürünün Ürün Kodu aynı naynı olamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
            {
                MessageBox.Show("Ürün Grubu ve Ürün Adı boşken veri düzenlenemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void button7_Click(object sender, EventArgs e)
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
            textBox7.Visible = true;
            label11.Visible = true;
            button15.Visible = false;
        }
    }
}
