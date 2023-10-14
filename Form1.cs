using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using DevExpress.XtraEditors.Repository;


namespace WindowsFormsApptéatt
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog;
        public Form1()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Access Database Files|*.mdb;*.accdb";
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barEditItem1.Edit = new RepositoryItemComboBox();
            RepositoryItemComboBox repositoryItemComboBox1 = barEditItem1.Edit as RepositoryItemComboBox;

            barEditItem1.EditValue = null;
            repositoryItemComboBox1.Items.Clear();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string databasePath = openFileDialog.FileName;
                string connectionstring = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath}";

                using (OleDbConnection conn = new OleDbConnection(connectionstring))
                {
                    conn.Open();
                    DataTable tableSchema = conn.GetSchema("Tables");
                    foreach (DataRow row in tableSchema.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        if (!tableName.StartsWith("MSys")) // Bỏ qua các bảng hệ thống
                        {
                            repositoryItemComboBox1.Items.Add(tableName);
                        }
                    }
                }
            }
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            if (barEditItem1.EditValue != null)
            {
                string selectedTable = barEditItem1.EditValue.ToString();
                string databasePath = openFileDialog.FileName;
                string connectionstring = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath}";

                using (OleDbConnection conn = new OleDbConnection(connectionstring))
                {
                    conn.Open();
                    string query = $"SELECT * FROM [{selectedTable}]";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                        {
                            DataTable DataTable = new DataTable();
                            adapter.Fill(DataTable);
                            gridControl1.DataSource = DataTable;
                        }
                    }
                }
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
