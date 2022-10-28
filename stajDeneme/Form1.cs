using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace stajDeneme
{
    public partial class Form1 : Form
    {
        string connectionString;
        public SqlConnection sqlConnection = new SqlConnection();
        public Form1()
        {
            InitializeComponent();
        }
        int il_selected_id;
        int ilce_selected_id;
        int hold_first_ilce_id = 0;
        protected void openConnection(string connection)
        {
            if (sqlConnection == null)
            {
                try
                {
                    sqlConnection = new SqlConnection();
                }
                catch (Exception p)
                {
                    richTextBox1.Text += DateTime.Now.ToString() + "sqlConnection" + p.Message + Environment.NewLine;
                }
            }
            if (sqlConnection.State == ConnectionState.Closed)
            {
                try
                {
                    sqlConnection.ConnectionString = connection;
                    sqlConnection.Open();
                }
                catch (Exception ex)
                {
                    richTextBox1.Text += DateTime.Now.ToString() + "ConnectionString" + ex.Message + Environment.NewLine;
                }
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    MessageBox.Show("Veritabanına bağlanamadı", "Bağlantı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["suat"].ConnectionString;
                Console.WriteLine(connectionString);
            }
            catch (Exception a)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "Form1_Load" + a.Message + Environment.NewLine;
            }
            if (sqlConnection.State == ConnectionState.Closed)
            {
                openConnection(connectionString);
            }
            SqlCommand komut5 = null;
            string searchSQL = "SELECT id,adi FROM cinsiyet ";
            try
            {
                komut5 = new SqlCommand();
                komut5.Connection = sqlConnection;
                komut5.CommandType = CommandType.Text;
                komut5.CommandText = searchSQL;
                komut5.ExecuteNonQuery();
            }
            catch (Exception searc)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "Form1_Load SqlCommand cinsiyet" + searc.Message + Environment.NewLine;
                error(searc.Message, searc.StackTrace, "Form1_Load sqlcommand cinsiyet");
            }
            string cinsiyet_adi;
            int cinsiyet_id;
            try
            {
                using (oReader = komut5.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        try
                        {
                            cinsiyet_id = Convert.ToInt32(oReader["id"]);
                            cinsiyet_adi = oReader["adi"].ToString();
                            comboBox1.Items.Insert(cinsiyet_id - 1, cinsiyet_adi);
                        }
                        catch(Exception cins_deger)
                        {
                            error(cins_deger.Message, cins_deger.StackTrace, "Form1_Load cinsiyet deger alma");
                            richTextBox1.Text += DateTime.Now.ToString() + "Form1_Load oReader" + cins_deger.Message + Environment.NewLine;
                        }
                    }
                    oReader.Close();
                }
            }
            catch (Exception execute)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "Form1_Load oReader" + execute.Message + Environment.NewLine;
                error(execute.Message, execute.StackTrace, "Form1_Load oReader");
            }
            string il = "SELECT id,ad FROM il";
            SqlCommand komut7=null;
            try
            {
                komut7 = new SqlCommand();
                komut7.Connection = sqlConnection;
                komut7.CommandType = CommandType.Text;
                komut7.CommandText = il;
                komut7.ExecuteNonQuery();
            }
            catch (Exception searc)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "Form1_Load SqlCommand il" + searc.Message + Environment.NewLine;
                error(searc.Message, searc.StackTrace, "Form1_Load sqlcommand il");
            }
            string il_adi;
            int il_id;
            try
            {
                using (oReader = komut7.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        try
                        {
                            il_id = Convert.ToInt32(oReader["id"]);
                            il_adi = oReader["ad"].ToString();
                            comboBox2.Items.Insert(il_id - 1, il_adi);
                        }
                        catch (Exception cins_deger)
                        {
                            error(cins_deger.Message, cins_deger.StackTrace, "Form1_Load cins deger alma");
                            richTextBox1.Text += DateTime.Now.ToString() + "button2_click oRader" + cins_deger.Message + Environment.NewLine;
                        }
                    }
                    oReader.Close();
                }
            }
            catch (Exception execute)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button2_click oRader" + execute.Message + Environment.NewLine;
                error(execute.Message, execute.StackTrace, "button_click oReader");
            }
            sqlConnection.Close();
        }
        private void label1_Click(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void error(string hata, string adres, string metod)
        {
            try
            {
                string insertSQLhata = "INSERT INTO errors (adres,hataMesaji,metodAdi) " +
                   " VALUES ( '" + adres.Replace("'", " ") + "' , '" + hata.Replace("'", " ") + "','" + metod.Replace("'", " ") + "')";
                SqlCommand comm = new SqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = insertSQLhata;
                comm.ExecuteNonQuery();
            }
            catch (Exception err) { }
        }
        int selected_id;
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    openConnection(connectionString);
                }
            }
            catch (Exception b)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button1_Click_!" + b.Message + Environment.NewLine;
            }
            if ((textBox1.Text == "" || textBox2.Text == "") && sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Kayıt başarısız" + Environment.NewLine +
                "Hata  : isim veya soyisim değeri boş geçilemez", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string insertSQL = "INSERT INTO kisiler (cinsiyet_id,isim,soyisim,d_tarih,il_id,ilce_id) VALUES (" + selected_id + ",'" + textBox1.Text + "', '" + textBox2.Text +
                    "', " + " Convert(Date, '" + dateTimePicker2.Value.ToString("dd.MM.yyyy") + "', 104),"+il_selected_id+","+ilce_selected_id+")";
                try
                {
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = sqlConnection;
                    komut.CommandType = CommandType.Text;
                    komut.CommandText = insertSQL;
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Kayıt başarılı", "KAYIT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    richTextBox1.Text += DateTime.Now.ToString() + "button1_Click_1 SqlCommand" + ex.Message + Environment.NewLine;
                    error(ex.Message, ex.StackTrace, "button1_click_1 sqlcommand");
                    MessageBox.Show("Kayıt başarısız" + Environment.NewLine +
                        "Hata  : " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) { }
        string isim;
        string soyisim;
        string d_tarih;
        string id;
        SqlCommand komut2 = null;
        SqlDataReader oReader = null;
        int[] id_array=new int[20];
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    openConnection(connectionString);
                }
            }
            catch (Exception b)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button2_click" + b.Message + Environment.NewLine;
                error(b.Message, b.StackTrace, "button2_click sqlcommand");
            }
            /*string searchSQL = "SELECT isim,soyisim,d_tarih,id,cinsiyet_id FROM kisiler WHERE  (isim LIKE '%"
                + textBox1.Text + "%') and (soyisim LIKE '%" + textBox2.Text + "%')";*/
            string searchSQL = "SELECT kisiler.isim,kisiler.soyisim,kisiler.d_tarih,kisiler.id,cinsiyet.adi FROM kisiler INNER JOIN cinsiyet " +
                "ON cinsiyet.id=kisiler.cinsiyet_id WHERE (isim LIKE '%"
                + textBox1.Text + "%') AND (soyisim LIKE '%" + textBox2.Text + "%')";
            try
            {
                komut2 = new SqlCommand();
                komut2.Connection = sqlConnection;
                komut2.CommandType = CommandType.Text;
                komut2.CommandText = searchSQL;
                komut2.ExecuteNonQuery();
                dataGridView1.Rows.Clear();
            }
            catch (Exception searc)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button2_click SqlCommand" + searc.Message + Environment.NewLine;
                error(searc.Message, searc.StackTrace, "button2_click sqlcommand");
            }
            string cins_id;
            string cins_isim = null;
            int cins_id_convert;
            int counter = 0;
            try
            {
                using (oReader = komut2.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        isim = oReader["isim"].ToString();
                        soyisim = oReader["soyisim"].ToString();
                        d_tarih = oReader["d_tarih"].ToString();
                        id = oReader["id"].ToString();
                        id_array[counter] = Convert.ToInt32(id);
                        cins_isim = oReader["adi"].ToString();
                        counter++;
                    }
                    oReader.Close();
                }
            }
            catch (Exception execute)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button2_click oReader" + execute.Message + Environment.NewLine;
                error(execute.Message, execute.StackTrace, "button_click oReader");
            }
            SqlCommand komut9=null;
            string searchSQLil = "SELECT il.ad [iladi], ilce.ad [ilceadi] FROM kisiler INNER JOIN il "+
            "ON il.id = kisiler.il_id INNER JOIN ilce ON ilce.id = kisiler.ilce_id"+"  WHERE (isim LIKE '%"
            + textBox1.Text + "%') AND (soyisim LIKE '%" + textBox2.Text + "%')";
            try
            {
                komut9 = new SqlCommand();
                komut9.Connection = sqlConnection;
                komut9.CommandType = CommandType.Text;
                komut9.CommandText = searchSQLil;
                komut9.ExecuteNonQuery();
            }
            catch (Exception searc)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button2_click SqlCommand il" + searc.Message + Environment.NewLine;
                error(searc.Message, searc.StackTrace, "button2_click sqlcommand il");
            }
            string il_adi;
            string ilce_adi;
            try
            {
                using (oReader = komut9.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        il_adi = oReader["iladi"].ToString();
                        ilce_adi = oReader["ilceadi"].ToString();
                        dataGridView1.Rows.Add(isim, soyisim, d_tarih, cins_isim, id, il_adi, ilce_adi);
                    }
                    oReader.Close();
                }
            }
            catch (Exception execute)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button2_click oReader il" + execute.Message + Environment.NewLine;
                error(execute.Message, execute.StackTrace, "button_click oReader il");
            }
            sqlConnection.Close();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        int secili_id;
        private void Form1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                openConnection(connectionString);
            }
            int satir = Convert.ToInt32(e.RowIndex.ToString());
            int sayac = 0;
            secili_id = id_array[satir];
            sqlConnection.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                openConnection(connectionString);
            }
            string updateSQL = "UPDATE kisiler SET isim='" + textBox1.Text + "' , soyisim='" + textBox2.Text + "'," + "cinsiyet_id=" + selected_id +
                ",il_id="+il_selected_id+",ilce_id="+ilce_selected_id
                +"," + " d_tarih=Convert(Date" + ",'" + dateTimePicker2.Value.ToString("dd.MM.yyyy") + "', 104) WHERE id=" + secili_id;
            try
            {
                SqlCommand komut3 = new SqlCommand();
                komut3.Connection = sqlConnection;
                komut3.CommandType = CommandType.Text;
                komut3.CommandText = updateSQL;
                komut3.ExecuteNonQuery();
            }
            catch (Exception upd)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button3_click" + upd.Message + Environment.NewLine;
                error(upd.Message, upd.StackTrace, "button3_click sqlcommand");
            }
            finally
            {
                sqlConnection.Close();
            }
            button2_Click(sender, e);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                openConnection(connectionString);
            }
            try
            {
                string deleteSQL = "DELETE FROM kisiler WHERE id=" + secili_id;
                SqlCommand komut4 = new SqlCommand();
                komut4.Connection = sqlConnection;
                komut4.CommandText = deleteSQL;
                komut4.CommandType = CommandType.Text;
                komut4.ExecuteNonQuery();
            }
            catch (Exception dele)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "button4_click" + dele.Message + Environment.NewLine;
                error(dele.Message, dele.StackTrace, "button4_click sqlcommand");
            }
            finally
            {
                sqlConnection.Close();
            }
            button2_Click(sender, e);
        }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            selected_id = Convert.ToInt32(comboBox1.SelectedIndex) + 1;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker2.Value = DateTime.Today;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ilce_selected_id = Convert.ToInt32(comboBox3.SelectedIndex) + hold_first_ilce_id;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            if (sqlConnection.State == ConnectionState.Closed)
            {
                openConnection(connectionString);
            }
            comboBox3.SelectedIndex = -1;
            il_selected_id = Convert.ToInt32(comboBox2.SelectedIndex) + 1;
            Console.WriteLine(il_selected_id);
            string ilce = "SELECT id,ad FROM ilce WHERE il_id=" + il_selected_id;
            SqlCommand komut8 = null;
            try
            {
                komut8 = new SqlCommand();
                komut8.Connection = sqlConnection;
                komut8.CommandType = CommandType.Text;
                komut8.CommandText = ilce;
                komut8.ExecuteNonQuery();
            }
            catch (Exception searc)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "comboBox2_SelectionChangeCommitted SqlCommand ilce" + searc.Message + Environment.NewLine;
                error(searc.Message, searc.StackTrace, "comboBox2_SelectionChangeCommitted sqlcommand ilce");
            }
            string ilce_adi;
            int ilce_id;
            bool first = true;
            try
            {
                using (oReader = komut8.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        try
                        {
                            ilce_id = Convert.ToInt32(oReader["id"]);
                            if (first)
                            {
                                hold_first_ilce_id = ilce_id;
                                first = false;
                            }
                            ilce_adi = oReader["ad"].ToString();
                            comboBox3.Items.Insert(ilce_id - hold_first_ilce_id, ilce_adi);
                        }
                        catch (Exception cins_deger)
                        {
                            error(cins_deger.Message, cins_deger.StackTrace, "comboBox2_SelectionChangeCommitted ilce deger alma");
                            richTextBox1.Text += DateTime.Now.ToString() + "comboBox2_SelectionChangeCommitted oRader" + cins_deger.Message + Environment.NewLine;
                        }
                    }
                    oReader.Close();
                }
            }
            catch (Exception execute)
            {
                richTextBox1.Text += DateTime.Now.ToString() + "comboBox2_SelectionChangeCommitted oReader" + execute.Message + Environment.NewLine;
                error(execute.Message, execute.StackTrace, "comboBox2_SelectionChangeCommitted oReader");
            }
        }
    }
}