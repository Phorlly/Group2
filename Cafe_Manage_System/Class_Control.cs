using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace Cafe_Manage_System
{
    public class Class_Control : Class_Connection
    {
        public MySqlConnection cn;
        public MySqlCommand cm;
        public MySqlDataAdapter da;
        public MySqlDataReader dr;
        public DataTable dt;
        public DataSet ds;

        internal object LoadReport()
        {
            throw new NotImplementedException();
        }


        #region EXECUTEQUERY   
        // QUERY PARAMETERSdfdsf
        public List<MySqlParameter> Params = new List<MySqlParameter>();
        public int RecordCount;
        public string Exception;
        public string ReturnQuery;

        public void ExecQuery(String Query, Boolean ReturnIdentity = false)
        {
            //RESET QUERY STATS
            RecordCount = 0;
            Exception = "";

            using (cn = GetMySqlConnection())
            {
                try
                {
                    cn.Open();
                    // CREATE DB COMMAND
                    cm = new MySqlCommand(Query, cn);
                    // LOAD PARAMS INTO DB COMMAND
                    Params.ForEach(p => cm.Parameters.Add(p));
                    // CLEAR PARAM LIST
                    Params.Clear();
                    // EXECUTE COMMAND & FILL DATASET
                    dt = new DataTable();

                    da = new MySqlDataAdapter(cm);
                    RecordCount = da.Fill(dt);

                    if (ReturnIdentity == true)
                    {
                        ReturnQuery = "SELECT @@IDENTITY As LastID;";
                        // @@IDENTITY - SESSION
                        // SCOPE_IDENTITY() - SESSION & SCOPE
                        // IDENT_CURRENT(tablename) - LAST IDENT IN TABLE, ANY SCOPE, ANY SESSION
                        cm = new MySqlCommand(ReturnQuery, cn);
                        dt = new DataTable();
                        da = new MySqlDataAdapter(cm);
                        RecordCount = da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    // CAPTURE ERROR
                    Exception = "ExecQuery Error: \n" + ex.Message;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
        }

        // ADD PARAMS
        public void AddParam(String Name, Object Value)
        {
            MySqlParameter NewParam = new MySqlParameter(Name, Value);
            Params.Add(NewParam);
        }
        public Boolean HasException(Boolean Report = false)
        {
            if (String.IsNullOrEmpty(Exception))
            {
                return false;
            }
            if (Report == true)
            {
                MessageBox.Show(Exception, "Exception:");
            }
            return true;
        }

        #endregion

        #region Report
        public DataSet LoadReport(string qty, string table)
        {
            using (var cn = GetMySqlConnection())
            {
                da = new MySqlDataAdapter(qty, cn);
                DataSet ds = new DataSet();
                da.Fill(ds, table);
                return ds;
            }
            //CALL CRYSTALL REPORT
            //DataView dview = new DataView();
            //dview.Table = sql.LoadReport("score").Tables["score"];

            //Reports.CR_ERL myreport = new Reports.CR_ERL();
            //myreport.SetDataSource(dview);
            //crystalReportViewer1.ReportSource = myreport;
            ////_ = crystalReportViewer1.DataBindings;
        }

        #endregion

        public void Retrive(string qty, DataGridView dgv)
        {
            using (var cn = GetMySqlConnection())
            {
                cm = new MySqlCommand(qty, cn);
                da = new MySqlDataAdapter(cm);

                dt = new DataTable();

                da.Fill(dt);

                dgv.DataSource = dt;

            }
        }
        public string GetMaxID(string table, string field, int num, string pre, string defualt)

        {
            string id;
            using (var cn = GetMySqlConnection())
            {
                cn.Open();
                cm = new MySqlCommand("Select top 1 " + field + " From " + table + " order by " + field + " DESC", cn);
                dr = cm.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    string s = dr.GetString(0);
                    s = s.Substring(num, s.Length - 2);

                    int str = Convert.ToInt32(s) + 1;
                    id = pre + Convert.ToString(str).PadLeft(s.Length, '0');
                }
                else
                {
                    id = defualt;
                }

                dr.Close();

                return id;

                cn.Close();
            }
        }
        DataTable Pivot(DataTable dt, DataColumn pivotColumn, DataColumn pivotValue)
        {
            // find primary key columns 
            //(i.e. everything but pivot column and pivot value)
            DataTable temp = dt.Copy();
            temp.Columns.Remove(pivotColumn.ColumnName);
            temp.Columns.Remove(pivotValue.ColumnName);
            string[] pkColumnNames = temp.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToArray();

            // prep results table
            DataTable result = temp.DefaultView.ToTable(true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();
            dt.AsEnumerable()
                .Select(r => r[pivotColumn.ColumnName].ToString())
                .Distinct().ToList()
                .ForEach(c => result.Columns.Add(c, pivotColumn.DataType));

            // load it
            foreach (DataRow row in dt.Rows)
            {
                // find row to update
                DataRow aggRow = result.Rows.Find(
                    pkColumnNames
                        .Select(c => row[c])
                        .ToArray());
                // the aggregate used here is LATEST 
                // adjust the next line if you want (SUM, MAX, etc...)
                aggRow[row[pivotColumn.ColumnName].ToString()] = row[pivotValue.ColumnName];
            }

            return result;
        }

        public void BindGrid(DataGridView gvDetails, ComboBox cboX, ComboBox cboY, ComboBox cboZ)
        {

            ////string query = @"DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)
            //            DECLARE @ColumnName AS NVARCHAR(MAX)
            //            SELECT @ColumnName = ISNULL(@ColumnName + ',','')+ QUOTENAME(subject) FROM (SELECT DISTINCT subject FROM score) AS student
            //            SET @DynamicPivotQuery = ';WITH CTE AS(SELECT subject,student,score FROM score)
            //            SELECT student,'+@ColumnName+' FROM CTE
            //            PIVOT (MAX(score) FOR [subject] IN('+@ColumnName+')) p
            //            ORDER BY student DESC'
            //                EXEC(@DynamicPivotQuery)";

            using (MySqlConnection con = GetMySqlConnection())
            {
                using (MySqlCommand cm = new MySqlCommand())
                {
                    cm.CommandText = "select * from score";
                    cm.CommandType = CommandType.Text;

                    using (MySqlDataAdapter da = new MySqlDataAdapter())
                    {
                        cm.Connection = con;
                        da.SelectCommand = cm;
                        //using (DataTable dt = new DataTable())
                        //{
                        //gvDetails.AutoGenerateColumns = true;
                        //sda.Fill(dt);
                        //gvDetails.DataSource = dt;
                        //// gvDetails.DataBindings();


                        //SqlCommand cmd = new SqlCommand("select * from tbl_data", con);
                        //con.Open();
                        //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        con.Close();

                        gvDetails.DataSource = dt;

                        //}
                    }
                }
            }
        }
        public void REPORT()
        {

            using (cn = GetMySqlConnection())
            {

            }
        }

        public void cmbx(string qty, ComboBox cbx, string index, string value)
        {
            using (var cn = GetMySqlConnection())
            {
                //  cn.Open();
                da = new MySqlDataAdapter(qty, cn);
                dt = new DataTable();

                da.Fill(dt);

                cbx.DataSource = dt;
                cbx.DisplayMember = value;
                cbx.ValueMember = index;
            }
        }
        DataTable m_dataTable;
        DataTable table { get { return m_dataTable; } set { m_dataTable = value; } }


        private const string m_choiceCol = "Score";

        class Options
        {
            public int m_Index { get; set; }
            public string m_Text { get; set; }
        }

        public void retrive(string qty, DataGridView dgv)
        {
            using (var cn = GetMySqlConnection())
            {
                cm = new MySqlCommand(qty, cn);
                da = new MySqlDataAdapter(cm);

                dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;


                if (!dgv.Columns.Contains(m_choiceCol))
                {
                    DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                    txtCol.Name = m_choiceCol;
                    dgv.Columns.Add(txtCol);
                    dgv.Columns[1].ReadOnly = true;
                    dgv.Columns[0].ReadOnly = true;
                    dgv.Columns[2].ReadOnly = true;
                    dgv.Columns[3].ReadOnly = true;
                }

                //List<Options> oList = new List<Options>();
                //oList.Add(new Options() { m_Index = 0, m_Text = "None" });
                //for (int i = 1; i < 10; i++)
                //{
                //    oList.Add(new Options() { m_Index = i, m_Text = "Op" + i });
                //}

                //for (int i = 0; i < dgv.Rows.Count - 1; i += 2)
                //{
                // DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();

                //Setup A
                //c.DataSource = oList;
                //c.Value = oList[0].m_Text;
                //c.ValueMember = "m_Text";
                //c.DisplayMember = "m_Text";
                //c.ValueType = typeof(string);

                ////Setup B
                //c.DataSource = oList;
                //c.Value = 0;
                //c.ValueMember = "m_Index";
                //c.DisplayMember = "m_Text";
                //c.ValueType = typeof(int);

                //Result is the same A or B
                //dgv[m_choiceCol, i] = c;
                //}
                //ref: https://stackoverflow.com/questions/1814423/datagridview-how-to-set-a-cell-in-editing-mode
            }


        }

        //select data
        public void Retrive(DataGridView dgv)
        {
            using (var cn = GetMySqlConnection())
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tbstudent ORDER BY id  ASC ", cn);
                da = new MySqlDataAdapter(cm);

                dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;

                cn.Close();

            }
        }
        //insert data
        public void CmInsert(int id, string name, string pwd, string nameKh, string nameEn, string sex, string tele, string dob, string email, string add, string dest)
        {

            using (var cn = GetMySqlConnection())
            {

                cm.Connection = cn;
                cm.CommandText = "INSERT INTO tbstudent(id,name,passwords,nameKh,nameEn,sex,dob,telephone,email,address,description)VALUE(@id,@name,@passwords,@nameKh,@nameEn,@sex,@dob,@telephone,@email,@address,@description)";

                cn.Open();
                cm.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                cm.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                cm.Parameters.Add("@passwords", MySqlDbType.VarChar).Value = pwd;
                cm.Parameters.Add("@nameKh", MySqlDbType.VarChar).Value = nameKh;
                cm.Parameters.Add("@nameEn", MySqlDbType.VarChar).Value = nameEn;
                cm.Parameters.Add("@sex", MySqlDbType.VarChar).Value = sex;
                cm.Parameters.Add("@dob", MySqlDbType.VarChar).Value = dob;
                cm.Parameters.Add("@telephone", MySqlDbType.VarChar).Value = tele;
                cm.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cm.Parameters.Add("@address", MySqlDbType.VarChar).Value = add;
                cm.Parameters.Add("@description", MySqlDbType.VarChar).Value = dest;

                /*
                cm.Parameters.AddWithValue("@id", null);
                cm.Parameters.AddWithValue("@name", tb);
                cm.Parameters.AddWithValue("@passwords", null);
                cm.Parameters.AddWithValue("@nameKh", null);
                cm.Parameters.AddWithValue("@nameEn", null);
                cm.Parameters.AddWithValue("@dob", null);
                cm.Parameters.AddWithValue("@telephone", null);
                cm.Parameters.AddWithValue("@email", null);
                cm.Parameters.AddWithValue("@address", null);
                cm.Parameters.AddWithValue("@description", null);*/

                int i;
                i = cm.ExecuteNonQuery();
                if (i > 0)
                {
                    MessageBox.Show("Insert succeeded");
                }
                cn.Close();
            }
        }
    }
}
