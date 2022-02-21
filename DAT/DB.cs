namespace POSSystem02.DAT
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class DB
    {
        public static string userid = "";
        public static MySqlConnection DBConnection;
        private MySqlCommand DBCommand;
        private MySqlDataAdapter DBAdapter;
        private MySqlDataReader _DBReader;
        public Functions _Functions = new Functions();
        public string Query;
        public string Message;

        public object Data(string query)
        {
            object obj2;
            DataTable table = this.Data_Table(query);
            if (table.Rows.Count == 0)
            {
                obj2 = null;
            }
            else
            {
                obj2 = table.Rows[0].ItemArray.GetValue(0).ToString();
                if (obj2.ToString() == "")
                {
                    return null;
                }
            }
            return obj2;
        }

        public DataTable Data_Table(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                this.OpenConnection();
                this.DBCommand = new MySqlCommand(query, DBConnection);
                this.DBCommand.CommandTimeout = 0xbb8;
                this.DBAdapter = new MySqlDataAdapter();
                this.DBAdapter.SelectCommand = this.DBCommand;
                this.DBAdapter.Fill(dataTable);
                DBConnection.Close();
            }
            catch (Exception exception)
            {
                this._Functions.ShowMessage(exception.Message, MessageType.Error);
            }
            return dataTable;
        }

        public MySqlDataReader DBReader(string query)
        {
            try
            {
                this.OpenConnection();
                this.DBCommand = new MySqlCommand(query, DBConnection);
                this._DBReader = this.DBCommand.ExecuteReader();
                return this._DBReader;
            }
            catch (Exception exception)
            {
                this._Functions.ShowMessage(exception.Message, MessageType.Error);
                return null;
            }
        }

        public bool Delete(string table, string objField, object objData)
        {
            try
            {
                string[] textArray1 = new string[7];
                textArray1[0] = "DELETE FROM ";
                textArray1[1] = table;
                textArray1[2] = " WHERE ";
                textArray1[3] = objField;
                textArray1[4] = "='";
                textArray1[5] = objData?.ToString();
                string[] local1 = textArray1;
                local1[6] = "'";
                this.Query = string.Concat(local1);
                return this.Execute();
            }
            catch (Exception exception)
            {
                this.Message = exception.Message;
                return false;
            }
        }

        private bool Execute()
        {
            bool flag2;
            if (!this.OpenConnection())
            {
                flag2 = false;
            }
            else
            {
                try
                {
                    this.DBCommand = new MySqlCommand(this.Query, DBConnection);
                    this.DBCommand.ExecuteNonQuery();
                    DBConnection.Close();
                }
                catch (Exception exception)
                {
                    this.Message = exception.Message;
                    DBConnection.Close();
                    return false;
                }
                flag2 = true;
            }
            return flag2;
        }

        public bool ExecuteQuery(string query)
        {
            bool flag2;
            if (!this.OpenConnection())
            {
                flag2 = false;
            }
            else
            {
                try
                {
                    this.DBCommand = new MySqlCommand(query, DBConnection);
                    this.DBCommand.ExecuteNonQuery();
                    DBConnection.Close();
                }
                catch (Exception exception)
                {
                    this._Functions.ShowMessage(exception.Message, MessageType.Error);
                    DBConnection.Close();
                    return false;
                }
                flag2 = true;
            }
            return flag2;
        }

        public void From(List<string> tables)
        {
            this.Query = this.Query + "FROM ";
            foreach (string str in tables)
            {
                this.Query = this.Query + str + ",";
            }
            this.Query = this.Query.Remove(this.Query.Length - 1);
            this.Query = this.Query + " ";
        }

        public void From(string table)
        {
            this.Query = this.Query + "FROM " + table + " ";
        }

        public string Generate_Id(string extention, string exist_id)
        {
            string str = "";
            if (exist_id == "")
            {
                str = (extention != "") ? (extention + "-100001") : "100001";
            }
            else if (extention == "")
            {
                str = (int.Parse(exist_id) + 1).ToString();
            }
            else
            {
                char[] separator = new char[] { '-' };
                str = extention + "-" + (int.Parse(exist_id.Split(separator)[1]) + 1).ToString();
            }
            return str;
        }

        public DataTable Get()
        {
            DataTable dataTable = new DataTable();
            try
            {
                this.OpenConnection();
                this.DBAdapter = new MySqlDataAdapter(this.Query, DBConnection);
                this.DBAdapter.Fill(dataTable);
                DBConnection.Close();
            }
            catch (Exception exception)
            {
                this.Message = exception.Message;
            }
            return dataTable;
        }

        public void Group_By(string field)
        {
            this.Query = this.Query + "GROUP BY " + field + " ";
        }

        public bool Insert(string table, Dictionary<string, object> datas)
        {
            try
            {
                this.Query = "INSERT INTO " + table;
                string str = " (";
                string str2 = "VALUES(";
                foreach (KeyValuePair<string, object> pair in datas)
                {
                    string key = pair.Key;
                    object obj2 = pair.Value;
                    str = str + key + ",";
                    str2 = str2 + "'" + obj2?.ToString() + "',";
                }
                str = str.Remove(str.Length - 1) + ")";
                str2 = str2.Remove(str2.Length - 1) + ")";
                this.Query = this.Query + str + " " + str2;
                return this.Execute();
            }
            catch (Exception exception)
            {
                this.Message = exception.Message;
                return false;
            }
        }

        public void Limit(int limit)
        {
            this.Query = this.Query + "LIMIT " + limit.ToString();
        }

        public void MySql(string query)
        {
            this.Query = query;
        }

        public bool OpenConnection()
        {
            DBConnection = new MySqlConnection(Config.ConnectionString);
            if (DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
            }
            try
            {
                DBConnection.Open();
            }
            catch (Exception exception)
            {
                this._Functions.ShowMessage(exception.Message, MessageType.Error);
                DBConnection.Close();
                return false;
            }
            return true;
        }

        public void Order_By(string value, string order = "ASC")
        {
            string[] textArray1 = new string[] { this.Query, "ORDER BY ", value, " ", order, " " };
            this.Query = string.Concat(textArray1);
        }

        public void Select(List<string> values)
        {
            this.Query = "SELECT ";
            foreach (string str in values)
            {
                this.Query = this.Query + str + ",";
            }
            this.Query = this.Query.Remove(this.Query.Length - 1);
            this.Query = this.Query + " ";
        }

        public void Select(string value)
        {
            this.Query = "SELECT " + value + " ";
        }

        public void Select_Max(string field, string as_value = null)
        {
            if (ReferenceEquals(as_value, null))
            {
                as_value = field;
            }
            string[] textArray1 = new string[] { "SELECT MAX(", field, ") as ", as_value, " " };
            this.Query = string.Concat(textArray1);
        }

        public void Select_Sum(string field, string as_value = null)
        {
            if (ReferenceEquals(as_value, null))
            {
                as_value = field;
            }
            string[] textArray1 = new string[] { "SELECT SUM(", field, ") as ", as_value, " " };
            this.Query = string.Concat(textArray1);
        }

        public bool Update(string table, Dictionary<string, object> objects, Dictionary<string, object> wheres)
        {
            try
            {
                this.Query = "UPDATE " + table + " SET ";
                foreach (KeyValuePair<string, object> pair in objects)
                {
                    string key = pair.Key;
                    object obj2 = pair.Value;
                    string[] textArray1 = new string[5];
                    textArray1[0] = this.Query;
                    textArray1[1] = key;
                    textArray1[2] = "='";
                    textArray1[3] = obj2?.ToString();
                    string[] local1 = textArray1;
                    local1[4] = "',";
                    this.Query = string.Concat(local1);
                }
                this.Query = this.Query.Remove(this.Query.Length - 1);
                if (wheres.Count > 1)
                {
                    this.Query = this.Query + " WHERE ";
                }
                foreach (KeyValuePair<string, object> pair2 in wheres)
                {
                    string key = pair2.Key;
                    object obj3 = pair2.Value;
                    if (wheres.Count <= 1)
                    {
                        string[] textArray3 = new string[6];
                        textArray3[0] = this.Query;
                        textArray3[1] = " WHERE ";
                        textArray3[2] = key;
                        textArray3[3] = "='";
                        textArray3[4] = obj3?.ToString();
                        string[] local3 = textArray3;
                        local3[5] = "'";
                        this.Query = string.Concat(local3);
                        break;
                    }
                    string[] textArray2 = new string[5];
                    textArray2[0] = this.Query;
                    textArray2[1] = key;
                    textArray2[2] = "='";
                    textArray2[3] = obj3?.ToString();
                    string[] local2 = textArray2;
                    local2[4] = "' AND ";
                    this.Query = string.Concat(local2);
                }
                if (wheres.Count > 1)
                {
                    this.Query = this.Query.Remove(this.Query.Length - 4);
                }
                return this.Execute();
            }
            catch (Exception exception)
            {
                this.Message = exception.Message;
                return false;
            }
        }

        public void Where(Dictionary<string, object> values)
        {
            this.Query = !this.Query.Contains("WHERE") ? (this.Query + "WHERE ") : (this.Query + "AND ");
            foreach (KeyValuePair<string, object> pair in values)
            {
                string text1;
                string[] textArray1 = new string[5];
                textArray1[0] = this.Query;
                textArray1[1] = pair.Key;
                textArray1[2] = "='";
                object local1 = pair.Value;
                string[] textArray2 = textArray1;
                if (local1 != null)
                {
                    text1 = local1.ToString();
                }
                else
                {
                    object local2 = local1;
                    text1 = null;
                }
                textArray1[3] = text1;
                string[] local3 = textArray1;
                local3[4] = "' AND ";
                this.Query = string.Concat(local3);
            }
            this.Query = this.Query.Remove(this.Query.Length - 4);
            this.Query = this.Query + " ";
        }

        public void Where(string query, bool where_check = true)
        {
            this.Query = !where_check ? (this.Query + "WHERE " + query + " ") : (!this.Query.Contains("WHERE") ? (this.Query + "WHERE " + query + " ") : (this.Query + "AND " + query + " "));
        }

        public void Where(string field, object data, bool where_check = true)
        {
            if (!where_check)
            {
                string[] textArray3 = new string[6];
                textArray3[0] = this.Query;
                textArray3[1] = "WHERE ";
                textArray3[2] = field;
                textArray3[3] = " = '";
                textArray3[4] = data?.ToString();
                string[] local3 = textArray3;
                local3[5] = "' ";
                this.Query = string.Concat(local3);
            }
            else if (this.Query.Contains("WHERE"))
            {
                string[] textArray1 = new string[6];
                textArray1[0] = this.Query;
                textArray1[1] = "AND ";
                textArray1[2] = field;
                textArray1[3] = " = '";
                textArray1[4] = data?.ToString();
                string[] local1 = textArray1;
                local1[5] = "' ";
                this.Query = string.Concat(local1);
            }
            else
            {
                string[] textArray2 = new string[6];
                textArray2[0] = this.Query;
                textArray2[1] = "WHERE ";
                textArray2[2] = field;
                textArray2[3] = " = '";
                textArray2[4] = data?.ToString();
                string[] local2 = textArray2;
                local2[5] = "' ";
                this.Query = string.Concat(local2);
            }
        }

        public void Where_And(string field, object data)
        {
            string[] textArray1 = new string[6];
            textArray1[0] = this.Query;
            textArray1[1] = "AND ";
            textArray1[2] = field;
            textArray1[3] = " = '";
            textArray1[4] = data?.ToString();
            string[] local1 = textArray1;
            local1[5] = "' ";
            this.Query = string.Concat(local1);
        }

        public static string UserId
        {
            get =>
                userid;
            set =>
                userid = value;
        }
    }
}

