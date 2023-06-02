using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Araç_Kiralama
{
    public partial class Müsteri : Form
    {
        readonly MySqlConnection con = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';");
        MySqlCommand cmd;
        MySqlDataReader dr;

        public Müsteri()
        {
            InitializeComponent();
            DisplayData();
        }
        private void DisplayData()
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter sorgu = new MySqlDataAdapter($"select* from musteri;", con);
            con.Close();
            sorgu.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void TcNumaraToolStripButton_Click(object sender, EventArgs e)
        {
            DataTable dt2 = new DataTable();
            MySqlDataAdapter sorgu2 = new MySqlDataAdapter($"select * from musteri;", con);
            if (TcNumaraToolStripTextBox.Text=="")
            {
                sorgu2.Fill(dt2);
                dataGridView1.DataSource = dt2;
            }
            else
            {
                MySqlDataAdapter sorgu3 = new MySqlDataAdapter("select * from musteri where tc like  '%" + TcNumaraToolStripTextBox.Text + "%';", con);
                dt2.Clear();
                sorgu3.Fill(dt2);
                dataGridView1.DataSource = dt2;
                con.Close();
            }
        }
        private void Müsteri_Load(object sender, EventArgs e)
        {
            DisplayData();
            this.dataGridView1.Columns["id"].Visible = false;
        }

        private void kaydet_Click(object sender, EventArgs e)
        {
            string TCno = tc.Text;
            if (tc.Text != string.Empty || sicil_no.Text != string.Empty || telefon.Text != string.Empty)
            {
                cmd = new MySqlCommand("select * from musteri where tc='" + tc.Text + "'", con);
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    MessageBox.Show("Müşteri zaten mevcut, lütfen başka birini deneyin ", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int Algoritma_Adim_Kontrol = 0, TekBasamaklarToplami = 0, CiftBasamaklarToplami = 0, TumBasamaklarToplami = 0, Basamak_10 = 0, Basamak_11 = 0;

                    if (TCno.Length == 11) Algoritma_Adim_Kontrol = 1;

                    foreach (char chr in TCno) { if (Char.IsNumber(chr)) Algoritma_Adim_Kontrol = 2; }

                    if (TCno.Substring(0, 1) != "0") Algoritma_Adim_Kontrol = 3;

                    int[] arrTC = System.Text.RegularExpressions.Regex.Replace(TCno, "[^0-9]", "").Select(x => (int)Char.GetNumericValue(x)).ToArray();

                    for (int i = 0; i < TCno.Length; i++)
                    {
                        TumBasamaklarToplami += Convert.ToInt32(arrTC[i]);
                        if (((i + 1) % 2) == 0)
                        {
                            if (i + 1 != 10) CiftBasamaklarToplami += Convert.ToInt32(arrTC[i]);
                            else Basamak_10 = Convert.ToInt32(arrTC[i]);
                        }
                        else
                        {
                            if (i + 1 != 11) TekBasamaklarToplami += Convert.ToInt32(arrTC[i]);
                            else
                            {
                                Basamak_11 = Convert.ToInt32(arrTC[i]);
                                TumBasamaklarToplami = TumBasamaklarToplami - Basamak_11;
                            }
                        }
                    }

                    int ilkDeger = (TekBasamaklarToplami * 7) - CiftBasamaklarToplami;
                    int ilkDeger_mod10 = ilkDeger % 10;
                    if (Basamak_10 == ilkDeger_mod10) Algoritma_Adim_Kontrol = 4;

                    int ikinciDeger_mod10 = TumBasamaklarToplami % 10;
                    if (Basamak_11 == ikinciDeger_mod10) Algoritma_Adim_Kontrol = 5;

                    if (Algoritma_Adim_Kontrol == 5)
                    {
                        MessageBox.Show("TC Doğru Müşteri Kaydedildi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dr.Close();
                        cmd = new MySqlCommand("insert into musteri (tc,ad,soyad,cinsiyet,dogum_tarihi,telefon,eposta,adres,sicil_no,ehliyet_tarih,kan_grubu ) values(@tc,@ad,@soyad,@cinsiyet,@dogum_tarihi,@telefon,@eposta,@adres,@sicil_no,@ehliyet_tarih,@kan_grubu)", con);
                        cmd.Parameters.AddWithValue("@tc", tc.Text);
                        cmd.Parameters.AddWithValue("@ad", ad.Text);
                        cmd.Parameters.AddWithValue("@soyad", soyad.Text);
                        cmd.Parameters.AddWithValue("@cinsiyet", cinsiyet.Text);
                        cmd.Parameters.AddWithValue("@dogum_tarihi", dogum_tarihi.Value);
                        cmd.Parameters.AddWithValue("@telefon", telefon.Text);
                        cmd.Parameters.AddWithValue("@eposta", eposta.Text);
                        cmd.Parameters.AddWithValue("@adres", adres.Text);
                        cmd.Parameters.AddWithValue("@sicil_no", sicil_no.Text);
                        cmd.Parameters.AddWithValue("@ehliyet_tarih", ehliyet_tarih.Value);
                        cmd.Parameters.AddWithValue("@kan_grubu", kan_grubu.Text);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("TC Yanlış Müşteri Kaydedilmedi", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanlara değer giriniz", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DisplayData();
            ClearData();
            con.Close();
        }

        private void guncelle_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand("update musteri set tc=@tc, ad=@ad, soyad=@soyad, cinsiyet=@cinsiyet, dogum_tarihi=@dogum_tarihi, telefon=@telefon, eposta=@eposta, adres=@adres, sicil_no=@sicil_no, ehliyet_tarih=@ehliyet_tarih, kan_grubu=@kan_grubu where ID=@id", con);
            con.Open();
            cmd.Parameters.AddWithValue("@id", ID);
            cmd.Parameters.AddWithValue("@tc", tc.Text);
            cmd.Parameters.AddWithValue("@ad", ad.Text);
            cmd.Parameters.AddWithValue("@soyad", soyad.Text);
            cmd.Parameters.AddWithValue("@cinsiyet", cinsiyet.Text);
            cmd.Parameters.AddWithValue("@dogum_tarihi", dogum_tarihi.Value);
            cmd.Parameters.AddWithValue("@telefon", telefon.Text);
            cmd.Parameters.AddWithValue("@eposta", eposta.Text);
            cmd.Parameters.AddWithValue("@adres", adres.Text);
            cmd.Parameters.AddWithValue("@sicil_no", sicil_no.Text);
            cmd.Parameters.AddWithValue("@ehliyet_tarih", ehliyet_tarih.Value);
            cmd.Parameters.AddWithValue("@kan_grubu", kan_grubu.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Müşteri güncellendi.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ClearData();
            ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            tc.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            ad.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            soyad.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            cinsiyet.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            string dateValue = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            DateTime selectedDate;
            if (DateTime.TryParse(dateValue, out selectedDate))
            {
                dogum_tarihi.Value = selectedDate;
            }
            telefon.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            eposta.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            adres.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            sicil_no.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            string dataValue = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            if(DateTime.TryParse(dataValue,out selectedDate))
            {
                ehliyet_tarih.Value = selectedDate;
            }
            kan_grubu.Text = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
        }

        private void anasayfa_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        int ID = 0;
        private void ClearData()
        {
            tc.Text = "";
            ad.Text = "";
            soyad.Text = "";
            cinsiyet.Text = "";
            telefon.Text = "";
            eposta.Text = "";
            adres.Text = "";
            sicil_no.Text = "";
            kan_grubu.Text = "";
            ID = 0;
        }

        private void TcNumaraToolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                TcNumaraToolStripButton.PerformClick();
            }
        }

        private void temizle_Click(object sender, EventArgs e)
        {
            ClearData();
        }
    }
}
