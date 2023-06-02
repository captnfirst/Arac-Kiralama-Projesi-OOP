using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Araç_Kiralama
{
    public partial class Giris : Form
    {
        readonly MySqlConnection con = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';");
        MySqlCommand cmd;
        MySqlDataReader dr;
        public Giris()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sifre.Text != string.Empty || eposta.Text != string.Empty)
            {
                cmd = new MySqlCommand("select * from kullanici where eposta='" + eposta.Text + "' and sifre='" + sifre.Text + "'", con);
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    this.Hide();
                    Anasayfa home = new Anasayfa();
                    home.Show();
                    this.Hide();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("Bu kullanıcı adı ve şifre ile hesap yok ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alana değer girin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            con.Close();
        }

        private void Kayit_ol_Click(object sender, EventArgs e)
        {
            Kayit kayit = new Kayit();
            kayit.Show();
            this.Hide();
        }

        private void sifre_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                button1.PerformClick();
                sifre.Clear();
            }
        }
    }
}
