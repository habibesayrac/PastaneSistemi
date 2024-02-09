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

namespace PastaneSistemi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestMaliyet;Integrated Security=True");
        void malzemelistele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBLMALZEMELER", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void urunlistele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBLURUNLER", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        void kasa()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBLKASA", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void urunler()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBLURUNLER", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbUrun.ValueMember = "URUNID";
            cmbUrun.DisplayMember = "AD";
            cmbUrun.DataSource = dt;
            connection.Close();
        }
        void malzemeler()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBLMALZEMELER", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbMalzeme.ValueMember = "MALZEMEID";
            cmbMalzeme.DisplayMember = "AD";
            cmbMalzeme.DataSource = dt;
            connection.Close(); ;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            malzemelistele();
            urunler();
            malzemeler();
        }

        private void BtnUrunListesi_Click(object sender, EventArgs e)
        {
            urunlistele();

        }

        private void BtnMalzemeListesi_Click(object sender, EventArgs e)
        {
            malzemelistele();

        }

        private void BtnKasa_Click(object sender, EventArgs e)
        {
            kasa();
        }

        private void BtnMalzemeEkle_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("insert into TBLMALZEMELER (AD,STOK,FIYAT,NOTLAR) values (@p1,@p2,@p3,@p4)", connection);
            command.Parameters.AddWithValue("@p1", TxtMalzemeAd.Text);
            command.Parameters.AddWithValue("@p2", TxtMalzemStok.Text);
            command.Parameters.AddWithValue("@p3", TxtMalzemeFiyat.Text);
            command.Parameters.AddWithValue("@p4", TxtMalzemeNotlar.Text);
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Malzeme eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            malzemelistele();
        }

        private void BtnUrunEkle_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("insert into TBLURUNLER (Ad,MFIYAT,SFIYAT,STOK) values (@p1,@p2,@p3,@p4)", connection);
            command.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
            command.Parameters.AddWithValue("@p2", TxtMFiyat.Text);
            command.Parameters.AddWithValue("@p3", TxtSFiyat.Text);
            command.Parameters.AddWithValue("@p4", TxtUrunStok.Text);
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Ürün eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            urunlistele();
        }

        private void BtnUrunOlustur_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("insert into TBLFIRIN (URUNID,MALZEMEID,MIKTAR,MALIYET) values (@p1,@p2,@p3,@p4)", connection);
            command.Parameters.AddWithValue("@p1", cmbUrun.SelectedValue);
            command.Parameters.AddWithValue("@p2", cmbMalzeme.SelectedValue);
            command.Parameters.AddWithValue("@p3",  decimal.Parse(TxtMiktar.Text));
            command.Parameters.AddWithValue("@p4", decimal.Parse(TxtMaliyet.Text));
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Malzeme eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            listBox1.Items.Add(cmbMalzeme.Text + "-" + TxtMaliyet.Text);
        }

        private void TxtMiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;
            if (TxtMiktar.Text == " ")
            {
                TxtMiktar.Text = "0";
            }

            connection.Open();
            SqlCommand command = new SqlCommand("select * from TBLMALZEMELER where MALZEMEID=@p1", connection);
            command.Parameters.AddWithValue("@p1", cmbMalzeme.SelectedValue);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                TxtMaliyet.Text = dr[3].ToString();

            }
            connection.Close();
            maliyet = Convert.ToDouble(TxtMaliyet.Text) / 1000 * Convert.ToDouble(TxtMiktar.Text);
            TxtMaliyet.Text = maliyet.ToString();

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtUrunId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();    
            TxtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();

            connection.Open();
            SqlCommand command = new SqlCommand("Select sum(MALIYET) from TBLFIRIN where URUNID=@p1", connection);
            command.Parameters.AddWithValue("@p1",TxtUrunId.Text);
            SqlDataReader dr = command.ExecuteReader();
            while(dr.Read())
            {
                TxtMFiyat.Text = dr[0].ToString();
            }
            connection.Close();

        }

        private void cmbMalzeme_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}