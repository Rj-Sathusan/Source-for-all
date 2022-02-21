namespace POSSystem02.DAT
{

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class Functions
    {
        public void ChangeDefault(DataGridView dgv)
        {
            dgv.CurrentRow.Cells["cl_itemcode"].Style.BackColor = Color.White;
            dgv.CurrentRow.Cells["cl_itemname"].Style.BackColor = Color.White;
        }

        public void ChangeRed(DataGridView dgv)
        {
            dgv.CurrentRow.Cells["cl_check"].Value = false;
            dgv.CurrentRow.Cells["cl_itemcode"].Style.BackColor = Color.Red;
            dgv.CurrentRow.Cells["cl_itemname"].Style.BackColor = Color.Red;
        }

        public void CheckDouble(KeyPressEventArgs e, TextBox txt)
        {
            char keyChar = e.KeyChar;
            if ((keyChar == '.') && (txt.Text.IndexOf(".") != -1))
            {
                e.Handled = true;
            }
            if ((!char.IsDigit(keyChar) && (keyChar != '\b')) && (keyChar != '.'))
            {
                e.Handled = true;
            }
        }

        public string ConvertDate(string date) =>
            DateTime.Parse(date).ToString("yyyy-MM-dd");

        public string ConvertTime(string date) =>
            DateTime.Parse(date).ToString("HH:mm:ss");

        public string ConvertTimeStamp(string date) =>
            DateTime.Parse(date).ToString("yyyy-MM-dd HH:mm:ss");

        public DataTable Create(params string[] columns)
        {
            DataTable table = new DataTable();
            foreach (string str in columns)
            {
                table.Columns.Add(str);
            }
            return table;
        }

        public string DeleteQuery(string table, string objField, object objData)
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
                return string.Concat(local1);
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message, MessageType.Error);
                return null;
            }
        }

        public void ExportExcel(DataGridView dgv, string filename)
        {
            /*
            Microsoft.Office.Interop.Excel.Application application = (Microsoft.Office.Interop.Excel.Application) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
            application.Application.Workbooks.Add(System.Type.Missing);
            application.Columns.ColumnWidth = 0x19;
            int num = 1;
            while (true)
            {
                if (num >= (dgv.ColumnCount + 1))
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 >= dgv.RowCount)
                        {
                            if (!Directory.Exists(@"C:\POS_REPORTS"))
                            {
                                Directory.CreateDirectory(@"C:\POS_REPORTS");
                            }
                            string str = @"C:\POS_REPORTS\" + filename + this.PathTime + ".xlsx";
                            application.ActiveWorkbook.SaveAs(str, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);
                            application.ActiveWorkbook.Saved = true;
                            Process.Start(str);
                            return;
                        }
                        int num3 = 0;
                        while (true)
                        {
                            if (num3 >= dgv.ColumnCount)
                            {
                                num2++;
                                break;
                            }
                            if (dgv[num3, num2].Value != null)
                            {
                                application.Cells[num2 + 2, num3 + 1] = dgv[num3, num2].Value.ToString();
                            }
                            num3++;
                        }
                    }
                }
                application.Cells[1, num] = dgv.Columns[num - 1].HeaderText;
                num++;
            }*/
        }

        public void Focus(Control c)
        {
            c.BackColor = Color.GhostWhite;
        }

        public void HideColumn(params DataGridViewColumn[] columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.Visible = false;
            }
        }

        public string InsertQuery(string table, Dictionary<string, object> objects)
        {
            try
            {
                string str = "INSERT INTO " + table;
                string str2 = " (";
                string str3 = "VALUES(";
                foreach (KeyValuePair<string, object> pair in objects)
                {
                    string key = pair.Key;
                    object obj2 = pair.Value;
                    str2 = str2 + key + ",";
                    str3 = str3 + "'" + obj2?.ToString() + "',";
                }
                str3 = str3.Remove(str3.Length - 1) + ")";
                return (str + (str2.Remove(str2.Length - 1) + ")") + " " + str3);
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message, MessageType.Error);
                return null;
            }
        }

        public void Leave(Control c)
        {
            c.BackColor = Color.White;
        }

        public void RemoveRow(DataGridView dgv, DataGridViewCellEventArgs e, int cIndex)
        {
            if (e.ColumnIndex == cIndex)
            {
                dgv.Rows.RemoveAt(e.RowIndex);
            }
        }

        public string SelectQuery(string table)
        {
            try
            {
                return ("SELECT * FROM " + table);
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message, MessageType.Error);
                return null;
            }
        }

        public string SelectQuery(List<string> fields, List<string> tables)
        {
            try
            {
                string str = "SELECT ";
                foreach (string str2 in fields)
                {
                    str = str + str2 + ",";
                }
                str = str.Remove(str.Length - 1) + " FROM ";
                foreach (string str3 in tables)
                {
                    str = str + str3 + ",";
                }
                return (str.Remove(str.Length - 1) + " ");
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message, MessageType.Error);
                return null;
            }
        }

        public void SetAlignCenter(params DataGridViewColumn[] columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        public void SetAlignRight(params DataGridViewColumn[] columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        public void SetFormat(params DataGridViewColumn[] columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.DefaultCellStyle.Format = "0,0.000";
            }
        }

        public void SetReadOnly(params DataGridViewColumn[] columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.ReadOnly = true;
            }
        }

        public void SetReadOnly(DataGridViewColumnCollection columns)
        {
            foreach (DataGridViewColumn column in columns)
            {
                column.ReadOnly = true;
            }
        }

        public void SetWidth(DataGridViewColumn column, int width)
        {
            column.Width = width;
        }

        public bool ShowMessage(string message, MessageType messageType)
        {
            if (messageType != MessageType.Confirm)
            {
                if (messageType == MessageType.Warning)
                {
                    MessageBox.Show(message, Config.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (messageType == MessageType.Information)
                {
                    MessageBox.Show(message, Config.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (messageType == MessageType.Error)
                {
                    MessageBox.Show(message, Config.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else if (MessageBox.Show(message, Config.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }

        public double SumCell(DataGridView dgv, string cellname) =>
            (from row in dgv.Rows.Cast<DataGridViewRow>()
             where row.Cells[cellname].FormattedValue.ToString() != string.Empty
             select double.Parse(row.Cells[cellname].FormattedValue.ToString())).ToArray().Sum();

        public string UpdateQuery(string table, Dictionary<string, object> objects, Dictionary<string, object> wheres)
        {
            try
            {
                string str = "UPDATE " + table + " SET ";
                foreach (KeyValuePair<string, object> pair in objects)
                {
                    string key = pair.Key;
                    object obj2 = pair.Value;
                    string[] textArray1 = new string[5];
                    textArray1[0] = str;
                    textArray1[1] = key;
                    textArray1[2] = "='";
                    textArray1[3] = obj2?.ToString();
                    string[] local1 = textArray1;
                    local1[4] = "',";
                    str = string.Concat(local1);
                }
                str = str.Remove(str.Length - 1);
                using (Dictionary<string, object>.Enumerator enumerator2 = wheres.GetEnumerator())
                {
                    if (enumerator2.MoveNext())
                    {
                        KeyValuePair<string, object> current = enumerator2.Current;
                        string key = current.Key;
                        object obj3 = current.Value;
                        string[] textArray2 = new string[6];
                        textArray2[0] = str;
                        textArray2[1] = " WHERE ";
                        textArray2[2] = key;
                        textArray2[3] = "='";
                        textArray2[4] = obj3?.ToString();
                        string[] local2 = textArray2;
                        local2[5] = "'";
                        str = string.Concat(local2);
                    }
                }
                return str;
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message, MessageType.Error);
                return null;
            }
        }

        public string CurrentTime =>
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public string PathTime
        {
            get
            {
                DateTime now = DateTime.Now;
                string[] textArray1 = new string[] { now.Year.ToString(), now.Month.ToString(), now.Day.ToString(), now.Hour.ToString(), now.Minute.ToString(), now.Second.ToString() };
                return string.Concat(textArray1).ToString();
            }
        }

        public string Today =>
            DateTime.Now.ToString("yyyy-MM-dd");

        public string ThisWeek
        {
            get
            {
                string str = "SELECT WEEK(date('" + this.Today + "'))";
                return "";
            }
        }

        public string ThisMonth =>
            DateTime.Now.Month.ToString();

        public string ThisYear =>
            DateTime.Now.Year.ToString();
    }
}

