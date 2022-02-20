using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

//branch 1 in member
//branch 2 in member
//branch 3 in member


namespace Cafe_Manage_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string server = "localhost";
        private string user = "root";
        private string pwd = "";
        private string db = "db_for_spi";
        private string cs;
        public MySqlConnection cn;

        private void bt_Show_Click(object sender, EventArgs e)
        {
            cs = @"server=localhost;user id=root ;persistsecurityinfo=true; database= db_for_spi; password= ";
            cn = new MySqlConnection(cs);
         
            try
            {
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {
                    tb_Data.Text = "Connected";
                    tb_Data.ForeColor = Color.Green;
                }
                else
                {
                    tb_Data.Text = "Not Connected";
                    tb_Data.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            cn.Close();
        }
        
          
            
        
    }
}
