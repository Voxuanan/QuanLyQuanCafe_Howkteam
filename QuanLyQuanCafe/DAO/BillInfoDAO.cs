using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        private BillInfoDAO() { }

        public void DeleteBillInfoByFoodId(int id)
        {
            string query = "USP_DeleteBillInfoByFoodId @id";
            DataProvider.Instance.ExecuteQuery(query, new object[] { id });
        }

        public void DeleteBillInfoByCategoryId(int id)
        {
            string query = "USP_DeleteBillInfoByCategoryId @id";
            DataProvider.Instance.ExecuteQuery(query, new object[] { id });
        }

        public void DeleteBillInfoByTableId(int id)
        {
            string query = "USP_DeleteBillInfoByTableId @id";
            DataProvider.Instance.ExecuteQuery(query, new object[] { id });
        }
        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            string query = "EXEC USP_GetListBillInfoByBillID @BillID";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { id});

            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBillInfo @idBill , @idFood , @count", new object[]    { idBill , idFood , count });
        }
    }
}