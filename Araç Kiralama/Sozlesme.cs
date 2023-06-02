using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Araç_Kiralama
{
    public partial class Sozlesme : Form
    {
        private FlowLayoutPanel panel1;
        public Sozlesme()
        {
            InitializeComponent();
            panel1 = new FlowLayoutPanel();
            panel1.FlowDirection = FlowDirection.LeftToRight;
            panel1.Location = new Point(10, 10);
            panel1.Size = new Size(500, 300);
            Controls.Add(panel1);

            AracResimleriGetir();
        }
        
        public void AracResimleriGetir()
        {
            panel1.Controls.Clear();

            using (MySqlConnection connection = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';"))
            {
                connection.Open();
                string sql = "SELECT resim.arac_resim_id, resim.resim_adi, araclar.plaka, araclar.marka, araclar.model, araclar.km, araclar.sanziman, araclar.g_fiyat, araclar.h_fiyat, araclar.a_fiyat FROM resim INNER JOIN araclar ON resim.arac_id = araclar.id WHERE araclar.durum = 0";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int resimID = reader.GetInt32("arac_resim_id");
                            string resimAdi = reader.GetString("resim_adi");
                            string plaka = reader.GetString("plaka");
                            string marka = reader.GetString("marka");
                            string model = reader.GetString("model");
                            BigInteger km = reader.GetInt64("km");
                            string sanziman = reader.GetString("sanziman");
                            int g_fiyat = reader.GetInt32("g_fiyat");
                            int h_fiyat = reader.GetInt32("h_fiyat");
                            int a_fiyat = reader.GetInt32("a_fiyat");

                            Button resimButton = new Button();
                            resimButton.Name = "btnResim_" + resimID;
                            resimButton.Size = new Size(150, 150);

                            resimButton.BackgroundImage = Image.FromFile(Path.Combine("Resimler", $"{resimID}_{resimAdi}"));
                            resimButton.BackgroundImageLayout = ImageLayout.Stretch;

                            // Butona tıklandığında yeni bir form açılacak
                            resimButton.Click += (sender, e) =>
                            {
                                AracDetayForm aracDetayForm = new AracDetayForm(plaka, marka, model, km, sanziman, g_fiyat, h_fiyat, a_fiyat);
                                aracDetayForm.Text = plaka;
                                aracDetayForm.StartPosition = FormStartPosition.CenterScreen;
                                aracDetayForm.AutoSize = true;
                                aracDetayForm.ShowDialog();
                            };

                            panel1.Controls.Add(resimButton);
                        }
                    }
                }
            }
        }

        private void Sozlesme_FormClosed(object sender, FormClosedEventArgs e)
        {
            Araclar arac = (Araclar)Application.OpenForms["Araclar"];
            arac.DisplayData();
        }
    }
    public partial class AracDetayForm : Form
    {
        Araclar araclar = new Araclar();
        private TextBox txtPlaka;
        private TextBox txtMarka;
        private TextBox txtModel;
        private TextBox txtKm;
        private TextBox txtSanziman;
        private TextBox txtG_Fiyat;
        private TextBox txtH_Fiyat;
        private TextBox txtA_Fiyat;
        private TextBox txtAd;
        private TextBox txtSoyad;
        private TextBox txtTelefon;
        private TextBox txtSicilNo;
        private ComboBox comboBox1;
        private TextBox txtFiyat;
        private DateTimePicker dateTimePicker1;

        public AracDetayForm(string plaka, string marka, string model, BigInteger km, string sanziman, int g_fiyat, int h_fiyat, int a_fiyat)
        {
            InitializeComponent();
            MusterileriGetir();
            Text = plaka; // Formun ismini plaka ile değiştir

            txtPlaka.Text = plaka;
            txtMarka.Text = marka;
            txtModel.Text = model;
            txtKm.Text = km.ToString();
            txtSanziman.Text = sanziman;
            txtG_Fiyat.Text = g_fiyat.ToString();
            txtH_Fiyat.Text = h_fiyat.ToString();
            txtA_Fiyat.Text = a_fiyat.ToString();
        }

        private void MusterileriGetir()
        {
            using (MySqlConnection connection = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';"))
            {
                connection.Open();
                string sql = "SELECT tc FROM musteri";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BigInteger tc = reader.GetInt64("tc");
                            comboBox1.Items.Add(tc);
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTc = comboBox1.SelectedItem.ToString();
            GetMusteriBilgileri(selectedTc);
        }

        private void GetMusteriBilgileri(string tc)
        {
            using (MySqlConnection connection = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';"))
            {
                connection.Open();
                string sql = "SELECT ad, soyad, telefon, sicil_no FROM musteri WHERE tc = @tc";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tc", tc);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string ad = reader.GetString("ad");
                            string soyad = reader.GetString("soyad");
                            string telefon = reader.GetString("telefon");
                            string sicilNo = reader.GetString("sicil_no");

                            txtAd.Text = ad;
                            txtSoyad.Text = soyad;
                            txtTelefon.Text = telefon;
                            txtSicilNo.Text = sicilNo;
                        }
                    }
                }
            }
        }

        private int GetAracId(string plaka)
        {
            // Arac ID'sini veritabanından almak için gerekli kodu buraya ekleyin
            int aracId = 0;
            using (MySqlConnection connection = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';"))
            {
                connection.Open();
                string sql = "SELECT id FROM araclar WHERE plaka = @plaka";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@plaka", plaka);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        aracId = Convert.ToInt32(result);
                    }
                }
            }
            return aracId;
        }

        private int GetMusteriId(string tc)
        {
            // Müşteri ID'sini veritabanından almak için gerekli kodu buraya ekleyin
            int musteriId = 0;
            using (MySqlConnection connection = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';"))
            {
                connection.Open();
                string sql = "SELECT id FROM musteri WHERE tc = @tc";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tc", tc);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        musteriId = Convert.ToInt32(result);
                    }
                }
            }
            return musteriId;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string selectedTc = comboBox1.SelectedItem.ToString();
            string fiyat = txtFiyat.Text;
            DateTime tarih = dateTimePicker1.Value;

            // Veritabanına sözleşme bilgilerini eklemek için gerekli kodu buraya ekleyin
            // Örnek olarak MySQL kullanıldığı varsayılarak aşağıdaki gibi bir kod parçası oluşturulabilir:
            using (MySqlConnection connection = new MySqlConnection(@"Server=94.73.150.60;Database=u9176804_arac;Uid=u9176804_user06F;Pwd='sH8uP3-B1K6:xd@@';"))
            {
                connection.Open();
                string sql = "INSERT INTO sozlesme (musteri_id, arac_id, fiyat, tarih) VALUES (@musteri_id, @arac_id, @fiyat, @tarih)";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@musteri_id", GetMusteriId(selectedTc));
                    command.Parameters.AddWithValue("@arac_id", GetAracId(txtPlaka.Text));
                    command.Parameters.AddWithValue("@fiyat", fiyat);
                    command.Parameters.AddWithValue("@tarih", tarih);
                    command.ExecuteNonQuery();
                    araclar.DisplayData();
                }

                string sqlAracDurumGuncelle = "UPDATE araclar SET durum = 1 WHERE plaka = @plaka";
                using (MySqlCommand command = new MySqlCommand(sqlAracDurumGuncelle, connection))
                {
                    command.Parameters.AddWithValue("@plaka", txtPlaka.Text);
                    command.ExecuteNonQuery();
                    araclar.DisplayData();
                }
                connection.Close();
            }
            
            MessageBox.Show("Sözleşme bilgileri kaydedildi ve araç durumu güncellendi.");
        }


        private void InitializeComponent()
        {
            // Formun diğer özellikleri ve kontrolleri burada düzenlenir
            // Örneğin, TextBox'lar için konum, boyut, stil ayarları yapılır

            txtPlaka = new TextBox();
            txtMarka = new TextBox();
            txtModel = new TextBox();
            txtKm = new TextBox();
            txtSanziman = new TextBox();
            txtG_Fiyat = new TextBox();
            txtH_Fiyat = new TextBox();
            txtA_Fiyat = new TextBox();
            txtAd = new TextBox();
            txtSoyad = new TextBox();
            txtTelefon = new TextBox();
            txtSicilNo = new TextBox();
            comboBox1 = new ComboBox();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            txtFiyat = new TextBox();
            dateTimePicker1 = new DateTimePicker();

            // Diğer kontrolleri form üzerine ekleyin
            Controls.Add(txtFiyat);
            Controls.Add(dateTimePicker1);



            // TextBox'ları form üzerine ekleyin
            Controls.Add(txtPlaka);
            Controls.Add(txtMarka);
            Controls.Add(txtModel);
            Controls.Add(txtKm);
            Controls.Add(txtSanziman);
            Controls.Add(txtG_Fiyat);
            Controls.Add(txtH_Fiyat);
            Controls.Add(txtA_Fiyat);
            Controls.Add(comboBox1);
            Controls.Add(txtAd);
            Controls.Add(txtSoyad);
            Controls.Add(txtTelefon);
            Controls.Add(txtSicilNo);

            // TextBox'ların yerleştirme ayarları
            txtPlaka.Location = new Point(170, 12);
            txtPlaka.Enabled = false;
            txtMarka.Location = new Point(170, 42);
            txtMarka.Enabled = false;
            txtModel.Location = new Point(170, 72);
            txtModel.Enabled = false;
            txtKm.Location = new Point(170, 102);
            txtKm.Enabled = false;
            txtSanziman.Location = new Point(170, 132);
            txtSanziman.Enabled = false;
            txtG_Fiyat.Location = new Point(170, 162);
            txtG_Fiyat.Enabled = false;
            txtH_Fiyat.Location = new Point(170, 192);
            txtH_Fiyat.Enabled=false;
            txtA_Fiyat.Location = new Point(170, 222);
            txtA_Fiyat.Enabled = false;
            comboBox1.Location = new Point(400, 10);
            txtAd.Location = new Point(400, 40);
            txtAd.Enabled = false;
            txtSoyad.Location = new Point(400, 70);
            txtSoyad.Enabled = false;
            txtTelefon.Location = new Point(400, 100);
            txtTelefon.Enabled = false;
            txtSicilNo.Location = new Point(400, 130);
            txtSicilNo .Enabled = false;
            txtFiyat.Location = new Point(85,320);
            dateTimePicker1.Location = new Point(85,350);

            txtAd.Size = new Size(121, 20);
            txtSoyad.Size = new Size(121, 20);
            txtTelefon.Size = new Size(121, 20);
            txtSicilNo.Size = new Size(121, 20);

            // Butonu form üzerine ekleyin
            Button btnSave = new Button();
            btnSave.Text = "Kaydet";
            btnSave.Location = new Point(400, 350);
            btnSave.Click += btnSave_Click;
            Controls.Add(btnSave);

            Label lblTarih = new Label();
            lblTarih.AutoSize = true;
            lblTarih.BackColor = Color.Black;
            lblTarih.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTarih.ForeColor = SystemColors.Control;
            lblTarih.Location = new Point(10, 350);
            lblTarih.Name = "lblTarih";
            lblTarih.Size = new Size(69, 21);
            lblTarih.TabIndex = 0;
            lblTarih.TextAlign = ContentAlignment.MiddleCenter;
            lblTarih.UseWaitCursor = true;
            lblTarih.Text = "Tarih:";
            Controls.Add(lblTarih);

            Label lblFiyat = new Label();
            lblFiyat.AutoSize = true;
            lblFiyat.BackColor = Color.Black;
            lblFiyat.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblFiyat.ForeColor = SystemColors.Control;
            lblFiyat.Location = new Point(10, 320);
            lblFiyat.Name = "lblFiyat";
            lblFiyat.Size = new Size(69, 21);
            lblFiyat.TabIndex = 0;
            lblFiyat.TextAlign = ContentAlignment.MiddleCenter;
            lblFiyat.UseWaitCursor = true;
            lblFiyat.Text = "Fiyat:";
            Controls.Add(lblFiyat);

            Label lblSozlesme = new Label();
            lblSozlesme.AutoSize = true;
            lblSozlesme.BackColor = Color.Black;
            lblSozlesme.Font = new Font("Audiowide", 20F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblSozlesme.ForeColor = SystemColors.Control;
            lblSozlesme.Location = new Point(175, 270);
            lblSozlesme.Name = "lblSozlesme";
            lblSozlesme.Size = new Size(69, 21);
            lblSozlesme.TabIndex = 0;
            lblSozlesme.TextAlign = ContentAlignment.MiddleCenter;
            lblSozlesme.UseWaitCursor = true;
            lblSozlesme.Text = "SÖZLEŞME";
            Controls.Add(lblSozlesme);

            Label lblTc = new Label();
            lblTc.AutoSize = true;
            lblTc.BackColor = Color.Black;
            lblTc.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTc.ForeColor = SystemColors.Control;
            lblTc.Location = new Point(300, 10);
            lblTc.Name = "lblTc";
            lblTc.Size = new Size(69, 21);
            lblTc.TabIndex = 0;
            lblTc.TextAlign = ContentAlignment.MiddleCenter;
            lblTc.UseWaitCursor = true;
            lblTc.Text = "Tc:";
            Controls.Add(lblTc);

            Label lblAd = new Label();
            lblAd.AutoSize = true;
            lblAd.BackColor = Color.Black;
            lblAd.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblAd.ForeColor = SystemColors.Control;
            lblAd.Location = new Point(300, 40);
            lblAd.Name = "lblAd";
            lblAd.Size = new Size(69, 21);
            lblAd.TabIndex = 0;
            lblAd.TextAlign = ContentAlignment.MiddleCenter;
            lblAd.UseWaitCursor = true;
            lblAd.Text = "Ad:";
            Controls.Add(lblAd);

            Label lblSoyad = new Label();
            lblSoyad.AutoSize = true;
            lblSoyad.BackColor = Color.Black;
            lblSoyad.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblSoyad.ForeColor = SystemColors.Control;
            lblSoyad.Location = new Point(300, 70);
            lblSoyad.Name = "lblSoyad";
            lblSoyad.Size = new Size(69, 21);
            lblSoyad.TabIndex = 0;
            lblSoyad.TextAlign = ContentAlignment.MiddleCenter;
            lblSoyad.UseWaitCursor = true;
            lblSoyad.Text = "Soyad:";
            Controls.Add(lblSoyad);

            Label lblTelefon = new Label();
            lblTelefon.AutoSize = true;
            lblTelefon.BackColor = Color.Black;
            lblTelefon.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTelefon.ForeColor = SystemColors.Control;
            lblTelefon.Location = new Point(300, 100);
            lblTelefon.Name = "lblTelefon";
            lblTelefon.Size = new Size(69, 21);
            lblTelefon.TabIndex = 0;
            lblTelefon.TextAlign = ContentAlignment.MiddleCenter;
            lblTelefon.UseWaitCursor = true;
            lblTelefon.Text = "Telefon:";
            Controls.Add(lblTelefon);

            Label lblSicil = new Label();
            lblSicil.AutoSize = true;
            lblSicil.BackColor = Color.Black;
            lblSicil.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblSicil.ForeColor = SystemColors.Control;
            lblSicil.Location = new Point(300, 130);
            lblSicil.Name = "lblSicil";
            lblSicil.Size = new Size(69, 21);
            lblSicil.TabIndex = 0;
            lblSicil.TextAlign = ContentAlignment.MiddleCenter;
            lblSicil.UseWaitCursor = true;
            lblSicil.Text = "Sicil No:";
            Controls.Add(lblSicil);

            Label lblPlaka = new Label();
            lblPlaka.AutoSize = true;
            lblPlaka.BackColor = Color.Black;
            lblPlaka.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblPlaka.ForeColor = SystemColors.Control;
            lblPlaka.Location = new Point(10, 10);
            lblPlaka.Name = "lblPlaka";
            lblPlaka.Size = new Size(69, 21);
            lblPlaka.TabIndex = 0;
            lblPlaka.TextAlign = ContentAlignment.MiddleCenter;
            lblPlaka.UseWaitCursor = true;
            lblPlaka.Text = "Plaka:";
            Controls.Add(lblPlaka);

            Label lblMarka = new Label();
            lblMarka.AutoSize = true;
            lblMarka.BackColor = Color.Black;
            lblMarka.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblMarka.ForeColor = SystemColors.Control;
            lblMarka.Location = new Point(10, 40);
            lblMarka.Name = "lblMarka";
            lblMarka.Size = new Size(69, 21);
            lblMarka.TabIndex = 0;
            lblMarka.TextAlign = ContentAlignment.MiddleCenter;
            lblMarka.UseWaitCursor = true;
            lblMarka.Text = "Marka:";
            Controls.Add(lblMarka);

            Label lblModel = new Label();
            lblModel.AutoSize = true;
            lblModel.BackColor = Color.Black;
            lblModel.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblModel.ForeColor = SystemColors.Control;
            lblModel.Location = new Point(10, 70);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(69, 21);
            lblModel.TabIndex = 0;
            lblModel.TextAlign = ContentAlignment.MiddleCenter;
            lblModel.UseWaitCursor = true;
            lblModel.Text = "Model:";
            Controls.Add(lblModel);

            Label lblKm = new Label();
            lblKm.AutoSize = true;
            lblKm.BackColor = Color.Black;
            lblKm.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblKm.ForeColor = SystemColors.Control;
            lblKm.Location = new Point(10, 100);
            lblKm.Name = "lblKm";
            lblKm.Size = new Size(69, 21);
            lblKm.TabIndex = 0;
            lblKm.TextAlign = ContentAlignment.MiddleCenter;
            lblKm.UseWaitCursor = true;
            lblKm.Text = "Km:";
            Controls.Add(lblKm);

            Label lblSanziman = new Label();
            lblSanziman.AutoSize = true;
            lblSanziman.BackColor = Color.Black;
            lblSanziman.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblSanziman.ForeColor = SystemColors.Control;
            lblSanziman.Location = new Point(10, 130);
            lblSanziman.Name = "lblSanziman";
            lblSanziman.Size = new Size(69, 21);
            lblSanziman.TabIndex = 0;
            lblSanziman.TextAlign = ContentAlignment.MiddleCenter;
            lblSanziman.UseWaitCursor = true;
            lblSanziman.Text = "Şanzıman:";
            Controls.Add(lblSanziman);

            Label lblGFiyat = new Label();
            lblGFiyat.AutoSize = true;
            lblGFiyat.BackColor = Color.Black;
            lblGFiyat.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblGFiyat.ForeColor = SystemColors.Control;
            lblGFiyat.Location = new Point(10, 160);
            lblGFiyat.Name = "lblGFiyat";
            lblGFiyat.Size = new Size(69, 21);
            lblGFiyat.TabIndex = 0;
            lblGFiyat.TextAlign = ContentAlignment.MiddleCenter;
            lblGFiyat.UseWaitCursor = true;
            lblGFiyat.Text = "Günlük Fiyat:";
            Controls.Add(lblGFiyat);

            Label lblHFiyat = new Label();
            lblHFiyat.AutoSize = true;
            lblHFiyat.BackColor = Color.Black;
            lblHFiyat.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblHFiyat.ForeColor = SystemColors.Control;
            lblHFiyat.Location = new Point(10, 190);
            lblHFiyat.Name = "lblHFiyat";
            lblHFiyat.Size = new Size(69, 21);
            lblHFiyat.TabIndex = 0;
            lblHFiyat.TextAlign = ContentAlignment.MiddleCenter;
            lblHFiyat.UseWaitCursor = true;
            lblHFiyat.Text = "Haftalık Fiyat:";
            Controls.Add(lblHFiyat);

            Label lblAFiyat = new Label();
            lblAFiyat.AutoSize = true;
            lblAFiyat.BackColor = Color.Black;
            lblAFiyat.Font = new Font("Audiowide", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblAFiyat.ForeColor = SystemColors.Control;
            lblAFiyat.Location = new Point(10, 220);
            lblAFiyat.Name = "lblAFiyat";
            lblAFiyat.Size = new Size(69, 21);
            lblAFiyat.TabIndex = 0;
            lblAFiyat.TextAlign = ContentAlignment.MiddleCenter;
            lblAFiyat.UseWaitCursor = true;
            lblAFiyat.Text = "Aylık Fiyat:";
            Controls.Add(lblAFiyat);
        }
    }
}