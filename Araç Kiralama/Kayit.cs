using Microsoft.Win32;
using MySql.Data.MySqlClient;
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
    public partial class Kayit : Form
    {
        readonly MySqlConnection con = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';");
        MySqlCommand cmd;
        MySqlDataReader dr;
        public Kayit()
        {
            InitializeComponent();
        }

        private void Kayit_ol_Click(object sender, EventArgs e)
        {
            if (sifre.Text != string.Empty || eposta.Text != string.Empty)
            {
                cmd = new MySqlCommand("select * from kullanici where eposta='" + eposta.Text + "'", con);
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    MessageBox.Show("Kullanıcı adı Zaten mevcut, lütfen başka birini deneyin ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dr.Close();
                    cmd = new MySqlCommand("insert into kullanici (ad,soyad,eposta,sifre) values(@ad,@soyad,@eposta,@sifre)", con);
                    cmd.Parameters.AddWithValue("ad", ad.Text);
                    cmd.Parameters.AddWithValue("soyad", soyad.Text);
                    cmd.Parameters.AddWithValue("eposta", eposta.Text);
                    cmd.Parameters.AddWithValue("sifre", sifre.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Hesabınız oluşturuldu. Lütfen şimdi giriş yapın.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Giris giris = new Giris();
                    giris.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanlara değer giriniz", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Giris giris = new Giris();
            giris.Show();
            this.Hide();
        }

        private void sifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                Kayit_ol.PerformClick();
                sifre.Clear();
            }
        }
    }
}
