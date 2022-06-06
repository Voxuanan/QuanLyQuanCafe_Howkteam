using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance 
        {
            get { if (instance == null) return new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value;}
        }

        private TableDAO() { }

        public static int TableWidth = 130;
        public static int TableHeight = 130;

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable1 @IdTable1 , @IdTable2", new object[] { id1, id2 });
        }

        public List<Table> LoadTableList()
        {
            List<Table> TableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                TableList.Add(table);
            }

            return TableList;
        }

        public bool InsertTable(string name)
        {
            string query = "USP_InsertTable @name";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name});
            return result > 0;
        }

        public bool UpdateTable(int id, string name)
        {
            string query = "USP_UpdateTable2 @id , @name";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id , name });
            return result > 0;
        }

        public bool DeleteTable(int id)
        {
            BillInfoDAO.Instance.DeleteBillInfoByTableId(id);
            BillDAO.Instance.DeleteBillByTableId(id);
            string query = "USP_DeleteTable @id ";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id});
            return result > 0;
        }
    }
}
