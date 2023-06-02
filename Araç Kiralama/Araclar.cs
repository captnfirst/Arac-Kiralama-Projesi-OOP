using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Araç_Kiralama
{
    public partial class Araclar : Form
    {
        readonly MySqlConnection con = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';");
        MySqlCommand cmd;
        MySqlDataReader dr;

        public Araclar()
        {
            InitializeComponent();
            DisplayData();
        }

        private void Araclar_Load(object sender, EventArgs e)
        {
            DisplayData();
            this.dataGridView1.Columns["id"].Visible = false;
        }

        public void DisplayData()
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter sorgu = new MySqlDataAdapter($"select * from araclar;", con);
            con.Close();
            sorgu.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void PlakaToolStripButton_Click_1(object sender, EventArgs e)
        {
            DataTable dt2 = new DataTable();
            MySqlDataAdapter sorgu2 = new MySqlDataAdapter($"select * from araclar;", con);
            if (PlakaToolStripTextBox.Text == "")
            {
                sorgu2.Fill(dt2);
                dataGridView1.DataSource = dt2;
            }
            else
            {
                MySqlDataAdapter sorgu3 = new MySqlDataAdapter("select * from araclar where plaka like  '%" + PlakaToolStripTextBox.Text + "%';", con);
                dt2.Clear();
                sorgu3.Fill(dt2);
                dataGridView1.DataSource = dt2;
                con.Close();
            }
        }

        int ID = 0;
        private void ClearData()
        {
            plaka.Text = "";
            marka.Text = "";
            model.Text = "";
            yil.Text = "";
            renk.Text = "";
            km.Text = "";
            yakit.Text = "";
            sanziman.Text = "";
            g_fiyat.Text = "";
            h_fiyat.Text = "";
            a_fiyat.Text = "";
            ID = 0;
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ClearData();
            ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            plaka.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            marka.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            model.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            yil.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            renk.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            km.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            yakit.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            sanziman.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            g_fiyat.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            h_fiyat.Text = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            a_fiyat.Text = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
        }

        private void kaydet_Click(object sender, EventArgs e)
        {
            if (plaka.Text != string.Empty || km.Text != string.Empty)
            {
                cmd = new MySqlCommand("select * from araclar where plaka='" + plaka.Text + "'", con);
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    MessageBox.Show("Araç zaten mevcut, lütfen başka birini deneyin ", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dr.Close();
                    cmd = new MySqlCommand("insert into araclar (plaka,marka,model,yil,renk,km,yakit,sanziman,g_fiyat,h_fiyat,a_fiyat) values(@plaka,@marka,@model,@yil,@renk,@km,@yakit,@sanziman,@g_fiyat,@h_fiyat,@a_fiyat)", con);
                    cmd.Parameters.AddWithValue("@plaka", plaka.Text);
                    cmd.Parameters.AddWithValue("@marka", marka.Text);
                    cmd.Parameters.AddWithValue("@model", model.Text);
                    cmd.Parameters.AddWithValue("@yil", yil.Text);
                    cmd.Parameters.AddWithValue("@renk", renk.Text);
                    cmd.Parameters.AddWithValue("@km", km.Text);
                    cmd.Parameters.AddWithValue("@yakit", yakit.Text);
                    cmd.Parameters.AddWithValue("@sanziman", sanziman.Text);
                    cmd.Parameters.AddWithValue("@g_fiyat", g_fiyat.Text);
                    cmd.Parameters.AddWithValue("@h_fiyat", h_fiyat.Text);
                    cmd.Parameters.AddWithValue("@a_fiyat", a_fiyat.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Araç kaydetme başarılı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanlara değer giriniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DisplayData();
            ClearData();
            con.Close();
        }

        private void guncelle_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand("update araclar set plaka=@plaka,marka=@marka,model=@model,yil=@yil,renk=@renk,km=@km,yakit=@yakit,sanziman=@sanziman,g_fiyat=@g_fiyat,h_fiyat=@h_fiyat,a_fiyat=@a_fiyat where ID=@id", con);
            con.Open();
            cmd.Parameters.AddWithValue("@id", ID);
            cmd.Parameters.AddWithValue("@plaka", plaka.Text);
            cmd.Parameters.AddWithValue("@marka", marka.Text);
            cmd.Parameters.AddWithValue("@model", model.Text);
            cmd.Parameters.AddWithValue("@yil", yil.Text);
            cmd.Parameters.AddWithValue("@renk", renk.Text);
            cmd.Parameters.AddWithValue("@km", km.Text);
            cmd.Parameters.AddWithValue("@yakit", yakit.Text);
            cmd.Parameters.AddWithValue("@sanziman", sanziman.Text);
            cmd.Parameters.AddWithValue("@g_fiyat", g_fiyat.Text);
            cmd.Parameters.AddWithValue("@h_fiyat", h_fiyat.Text);
            cmd.Parameters.AddWithValue("@a_fiyat", a_fiyat.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Araç bilgileri güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();
            DisplayData();
            ClearData();
        }

        void KayıtSil(int id)
        {
            cmd = new MySqlCommand("DELETE FROM musteri WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void sil_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow drow in dataGridView1.SelectedRows)
            {
                int id = Convert.ToInt32(drow.Cells[0].Value);
                KayıtSil(id);
            }
            DisplayData();
            ClearData();
        }

        private void kira_donme_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand("update araclar set durum=0 where ID=@id", con);
            con.Open();
            cmd.Parameters.AddWithValue("@id", ID);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Seçilen Araç Kiradan Dönmüştür");
            con.Close();
            DisplayData();
        }

        private void anasayfa_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void temizle_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void sozlesme_Click(object sender, EventArgs e)
        {
            Sozlesme szl = new Sozlesme();
            szl.AracResimleriGetir();
            szl.Show();
        }

        private void PlakaToolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                PlakaToolStripButton.PerformClick();
            }
        }

        private void resim_ekle_Click(object sender, EventArgs e)
        {
            if (ID != 0) // Seçilen bir araç varsa devam edin
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string dosyaYolu = openFileDialog.FileName;
                    string resimAdi = Path.GetFileName(dosyaYolu);

                    byte[] resimBytes = File.ReadAllBytes(dosyaYolu);

                    // Veritabanına resim ekleme
                    ResimEkle(ID, resimAdi, resimBytes);
                }
            }
        }

        private void ResimEkle(int aracID, string resimAdi, byte[] resimBytes)
        {
            using (MySqlConnection connection = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';"))
            {
                connection.Open();

                // Resmi "resim" tablosuna ekleme
                string sql = "INSERT INTO resim (arac_id, resim_adi) VALUES (@aracID, @resimAdi)";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@aracID", aracID);
                    command.Parameters.AddWithValue("@resimAdi", resimAdi);
                    command.ExecuteNonQuery();
                }

                // Eklenen resmin resim_id'sini al
                string resimIDQuery = "SELECT LAST_INSERT_ID()";
                using (MySqlCommand command = new MySqlCommand(resimIDQuery, connection))
                {
                    int resimID = Convert.ToInt32(command.ExecuteScalar());

                    // Resim dosyasını diskte kaydetme
                    string kayitYeri = Path.Combine("Resimler", $"{resimID}_{resimAdi}");
                    File.WriteAllBytes(kayitYeri, resimBytes);
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Sadece "durum" sütununu düzenle
            if (dataGridView1.Columns[e.ColumnIndex].Name == "durum")
            {
                // Durum değerine göre metni güncelle
                if (e.Value != null && e.Value.ToString() == "0")
                {
                    e.Value = "Boş";
                }
                else if (e.Value != null && e.Value.ToString() == "1")
                {
                    e.Value = "Kirada";
                }
            }
        }


    }
}
