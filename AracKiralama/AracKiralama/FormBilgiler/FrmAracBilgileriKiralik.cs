﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AracKiralama
{
    public partial class FrmAracBilgileriKiralik : Form
    {
        int mov;
        int movX;
        int movY;
        public FrmKira _frmKira;
        static Baglanti baglanti = new Baglanti();
        public string connectionString = $"Data Source={baglanti.Adres};Version=3;";
        public FrmAracBilgileriKiralik(FrmKira frmKira)
        {
            InitializeComponent();
            _frmKira = frmKira;

        }

        private void FrmAracBilgileriKiralik_Load(object sender, EventArgs e)
        {
            this.Location = Screen.AllScreens[0].WorkingArea.Location;
            Rectangle screenRectangle = Screen.PrimaryScreen.Bounds;
            int x = (screenRectangle.Width - this.Width) / 2;
            int y = (screenRectangle.Height - this.Height) / 2;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(x, y);

            string plaka = _frmKira.dataGridViewKiralik.SelectedRows[0].Cells["plakaDataGridViewTextBoxColumn1"].Value.ToString();

            if (string.IsNullOrEmpty(plaka))
            {
                DialogResult result = MessageBox.Show("Araç Bulunamadı!", "Uyarı", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    this.Close();
                }
            }


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM araclar WHERE plaka = @Plaka";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Plaka", plaka);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["plaka"].ToString();
                            textBox2.Text = reader["marka"].ToString();
                            textBox3.Text = reader["model"].ToString();
                            textBox4.Text = reader["yil"].ToString();
                            textBox8.Text = reader["yakit"].ToString();
                            textBox9.Text = reader["vites"].ToString();
                            textBox5.Text = reader["kilometre"].ToString();
                            textBox6.Text = reader["motor_gucu"].ToString();
                            textBox11.Text = reader["kasa_tipi"].ToString();
                            textBox7.Text = reader["renk"].ToString();
                            textBox10.Text = reader["hasar"].ToString();
                            DateTime baslangicTarihi = DateTime.Parse(reader["baslangic"].ToString());
                            DateTime bitisTarihi = DateTime.Parse(reader["bitis"].ToString());
                            textBox12.Text = baslangicTarihi.ToString("dd.MM.yyyy");
                            textBox13.Text = bitisTarihi.ToString("dd.MM.yyyy");
                            textBox14.Text = reader["tc_no"].ToString();
                            string gorselYolu = reader["gorsel"].ToString();

                            if (!string.IsNullOrEmpty(gorselYolu))
                            {
                                pictureBox1.ImageLocation = gorselYolu;
                                reader.Close();
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelMove_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void panelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panelMove_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }
    }
}
