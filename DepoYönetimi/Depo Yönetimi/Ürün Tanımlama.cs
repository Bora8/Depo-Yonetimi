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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Depo_Yönetimi
{
    public partial class Ürün_Tanımlama : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True");
        public Ürün_Tanımlama()
        {
            InitializeComponent();
        }
        void Databasegetir()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ÜrünTanımlama", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
            dataGridView1.ClearSelection();
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox3.Text = "";
            textBox4.Text = "";
            richTextBox1.Text = "";
            label15.Text = dataGridView1.Rows.Count.ToString()+" tane veriniz bulunmaktadır.";
            label15.Visible = true;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                try
                {
                    using (SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True"))
                    
                    {
                            SqlCommand komut2 = new SqlCommand("INSERT INTO ÜrünTanımlama(ÜrünKodu, ÜrünAdı, Marka, Model, ÜrünAltGrubu, ÜrünGrubu, DetayBilgiler) VALUES (@ÜrünKodu, @ÜrünAdı, @Marka, @Model, @ÜrünAltGrubu, @ÜrünGrubu, @DetayBilgiler) ALTER TABLE ÜrünTanımlama ADD CONSTRAINT UrunTanımlama1 UNIQUE (ÜrünGrubu, ÜrünAltGrubu, ÜrünKodu);", baglanti);
                            komut2.Parameters.AddWithValue("@ÜrünKodu", textBox2.Text);
                            komut2.Parameters.AddWithValue("@ÜrünAdı", textBox1.Text);
                            komut2.Parameters.AddWithValue("@Marka", textBox3.Text);
                            komut2.Parameters.AddWithValue("@Model", textBox4.Text);
                            komut2.Parameters.AddWithValue("@ÜrünAltGrubu", comboBox2.Text);
                            komut2.Parameters.AddWithValue("@ÜrünGrubu", comboBox1.Text);
                            komut2.Parameters.AddWithValue("@DetayBilgiler", richTextBox1.Text);
                            baglanti.Open();
                            komut2.ExecuteNonQuery();}
                            baglanti.Close();

                    }

                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("2 farklı ürünün Ürün Grubu, Ürün Alt Grubu ve Ürün Kodu aynı olamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                Databasegetir();
                dataGridView1.ClearSelection();
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                richTextBox1.Text = "";
                dataGridView1.Columns[0].Visible = false;
            }
            else
            {
                if(comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Grubu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if(comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Alt Grubu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Ürün Adı kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Ürün Kodu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    

        private void Ürün_Tanımlama_Load(object sender, EventArgs e)
        {
            label15.Visible = false;
            button5.Visible = false;
            textBox5.Visible = false;
            label16.Visible = false;
            textBox6.Visible = false;
            SqlCommand komut = new SqlCommand("SELECT * FROM ÜrünAltGrubuTanımlama", baglanti);
            baglanti.Open();
            SqlDataReader dr;
            dr = komut.ExecuteReader();
            List<string> Liste = new List<string>();
           
            while (dr.Read())
            {
                if (!Liste.Contains(dr["BağlıÜrünAdı"]))
                {
                    comboBox1.Items.Add(dr["BağlıÜrünAdı"]);
                    Liste.Add(dr["BağlıÜrünAdı"].ToString());
                }
                
            }

            baglanti.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Close();
            comboBox2.Text = "";
            comboBox2.Items.Clear();
            List<string> Liste1 = new List<string>();
            SqlCommand komut1 = new SqlCommand("SELECT ÜrünAltGrubuAdı FROM ÜrünAltGrubuTanımlama WHERE BağlıÜrünAdı = '" + comboBox1.Text + "'", baglanti);
            baglanti.Open();
            SqlDataReader dr1 = komut1.ExecuteReader();
            
            while (dr1.Read())
            {
                if (!Liste1.Contains(dr1["ÜrünAltGrubuAdı"]))
                {
                    comboBox2.Items.Add(dr1["ÜrünAltGrubuAdı"]);
                    Liste1.Add(dr1["ÜrünAltGrubuAdı"].ToString());
                }

            }
            dr1.Close();
            baglanti.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && textBox1.Text == "")
            {
                Databasegetir();
                dataGridView1.Columns[0].Visible = false;
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox3.Text = "";
                textBox4.Text = "";
                richTextBox1.Text = "";
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && textBox1.Text == "")
            {
                baglanti.Open();
                SqlDataAdapter da1 = new SqlDataAdapter("SELECT * FROM ÜrünTanımlama WHERE ÜrünGrubu like'" + comboBox1.Text+ "'", baglanti);
                DataTable table1 = new DataTable();
                da1.Fill(table1);
                dataGridView1.DataSource = table1;
                baglanti.Close();
                dataGridView1.ClearSelection();
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox3.Text = "";
                textBox4.Text = "";
                richTextBox1.Text = "";
                label15.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
                label15.Visible = true;
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && textBox1.Text == "")
            {
                baglanti.Open();
                SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM ÜrünTanımlama WHERE ÜrünGrubu LIKE '%" + comboBox1.Text + "%' AND ÜrünAltGrubu LIKE '%" + comboBox2.Text + "%'", baglanti);
                DataTable table2 = new DataTable();
                da2.Fill(table2);
                dataGridView1.DataSource = table2;
                baglanti.Close();
                dataGridView1.ClearSelection();
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox3.Text = "";
                textBox4.Text = "";
                richTextBox1.Text = "";
                label15.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
                label15.Visible = true;
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && textBox1.Text != "")
            {
                baglanti.Open();
                SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM ÜrünTanımlama WHERE ÜrünGrubu LIKE '%" + comboBox1.Text + "%' AND ÜrünAltGrubu LIKE '%" + comboBox2.Text + "%'", baglanti);
                DataTable table2 = new DataTable();
                da2.Fill(table2);
                dataGridView1.DataSource = table2;
                baglanti.Close();
                dataGridView1.ClearSelection();
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox3.Text = "";
                textBox4.Text = "";
                richTextBox1.Text = "";
                label15.Text = dataGridView1.Rows.Count.ToString() + " tane veriniz bulunmaktadır.";
                label15.Visible = true;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            richTextBox1.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                SqlCommand komut = new SqlCommand("Delete FROM ÜrünTanımlama WHERE @ÜrünKodu = ÜrünKodu", baglanti);
                komut.Parameters.AddWithValue("@ÜrünKodu", textBox2.Text);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                Databasegetir();
                dataGridView1.ClearSelection();
                textBox1.Text = "";
                textBox2.Text = "";
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox3.Text = "";
                textBox4.Text = "";
                richTextBox1.Text = "";
                
            }
            else
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Grubu kısmı boşken silinemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("Ürün Alt Grubu kısmı boşken silinemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Ürün Adı kısmı boşken silinemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Ürün Kodu kısmı boşken silinemez.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("DELETE FROM ÜrünTanımlama", baglanti);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            Databasegetir();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Ana_Ekran aegeçiş = new Ana_Ekran();
            aegeçiş.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                try
                {
                    using (SqlConnection baglanti = new SqlConnection("Data Source=KOEL-PC1\\SQLEXPRESS;Initial Catalog=DepoYönetimi;Integrated Security=True"))

                    {
                        SqlCommand komut3 = new SqlCommand("UPDATE ÜrünTanımlama SET ÜrünKodu = @ÜrünKodu, ÜrünAdı = @ÜrünAdı, Marka = @Marka, Model = @Model, ÜrünAltGrubu = @ÜrünAltGrubu, ÜrünGrubu = @ÜrünGrubu, DetayBilgiler = @DetayBilgiler WHERE Sıra = @Sıra ", baglanti);
                        komut3.Parameters.AddWithValue("@ÜrünKodu", textBox2.Text);
                        komut3.Parameters.AddWithValue("@ÜrünAdı", textBox1.Text);
                        komut3.Parameters.AddWithValue("@Marka", textBox3.Text);
                        komut3.Parameters.AddWithValue("@Model", textBox4.Text);
                        komut3.Parameters.AddWithValue("@ÜrünAltGrubu", comboBox2.Text);
                        komut3.Parameters.AddWithValue("@ÜrünGrubu", comboBox1.Text);
                        komut3.Parameters.AddWithValue("@DetayBilgiler", richTextBox1.Text);
                        komut3.Parameters.AddWithValue("@Sıra", textBox5.Text);


                        baglanti.Open();
                        komut3.ExecuteNonQuery();
                        baglanti.Close();
                        Databasegetir();
                        dataGridView1.ClearSelection();
                        comboBox1.SelectedIndex = -1;
                        textBox1.Text = "";
                        textBox2.Text = "";
                        dataGridView1.Columns[0].Visible = false;
                        baglanti.Close();
                    }
                }

                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        MessageBox.Show("2 farklı ürünün Ürün Grubu, Ürün Alt Grubu ve Ürün Kodu aynı olamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                Databasegetir();
                dataGridView1.ClearSelection();
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                richTextBox1.Text = "";
                dataGridView1.Columns[0].Visible = false;
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
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Ürün Adı kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Ürün Kodu kısmı boş kalamaz.", "Depo Yönetimi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(label16.Visible == false)
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
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            label16.Visible = true;
            textBox6.Visible = true;
            button14.Visible = false;
        }
    }
}

