using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POSSystem02.DAT
{
    public class DataAccessLayer : function_
    {
        public Functions _Functions = new Functions();
        public static MySqlConnection sqlconnection;
        public static DataTable comtable;
        public function_ fun = new function_();
        //public DataAccessLayer()
        //{
        //   //sqlconnection = new MySqlConnection(@"server=127.0.0.1;user id=root;database=hardwear");
        //   sqlconnection = new MySqlConnection(Config.ConnectionString);

        //   // sqlconnection = new MySqlConnection(@"database='hardwear'; datasource='192.168.8.15'; username='root'; password='12345'");
        //}

        public bool OpenConnection()
        {
            sqlconnection = new MySqlConnection(Config.ConnectionString);

            if (sqlconnection.State == ConnectionState.Open)
            {
                sqlconnection.Close();
            }
            try
            {
                sqlconnection.Open();
            }
            catch (Exception exception)
            {
                this._Functions.ShowMessage(exception.Message, MessageType.Error);
                sqlconnection.Close();
                return false;
            }
            return true;
        }



        public DataTable SelectData(string stored_procedure, MySqlParameter[] para)
        {

            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandTimeout = 1000;
            sqlcmd.CommandText = stored_procedure;
            sqlcmd.Connection = sqlconnection;

            if (para != null)
            {
                for (Int32 i = 0; i < para.Length; i++)
                {
                    sqlcmd.Parameters.Add(para[i]);

                }

            }

            MySqlDataAdapter da = new MySqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            return dt;

        }

        public void ExecuteCommand(string stored_procedure, MySqlParameter[] para)
        {
            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = stored_procedure;
            sqlcmd.Connection = sqlconnection;

            if (para != null)
            {

                sqlcmd.Parameters.AddRange(para);
            }
            sqlcmd.ExecuteNonQuery();
        }

        public void ExecuteCommand1(string stored_procedure, MySqlParameter[] para, List<string> listitem)
        {
            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = stored_procedure;
            sqlcmd.Connection = sqlconnection;

            if (para != null)
            {

                sqlcmd.Parameters.AddRange(para);
                sqlcmd.Parameters.Add(new SqlParameter("@MyCodes", listitem));

            }
            sqlcmd.ExecuteNonQuery();
        }

        public void Fill_Combobox_Desplaymember_Valauemenber(string store_producer, ComboBox com, string id, string name)
        {
            com.DataSource = SelectData(store_producer, null);
            com.DisplayMember = name;
            com.ValueMember = id;
            com.Text = "";

        }


        //DisplayAllCategory

    }
}
