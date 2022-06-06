using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }

        /// <summary>
        /// Thành công: bill ID
        /// thất bại: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUncheckBillIDByTableID(int id)
        {
            string query = "EXEC USP_GetUncheckBillIDByTableID @idTable , @status";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { id, 0 });
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
            }

            return -1;
        }

        public void CheckOut(int id, int discount, float totalprice)
        {
            string query = "USP_UpdateTable @id , @discount , @totalprice";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { id , discount, totalprice});
        }

        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @idTable", new object[] { id } );
        }

        public DataTable GetListBillByDate(DateTime datecheckin, DateTime datecheckout)
        {
            string query = "USP_GetListBillByDate @dateCheckIn , @dateCheckOut";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { datecheckin, datecheckout});
            return data;
        }

        public int GetCountBillByDate(DateTime datecheckin, DateTime datecheckout)
        {
            string query = "USP_GetCountBillByDate @dateCheckIN , @dateCheckOut";
            int temp = (int)DataProvider.Instance.ExecuteScalar(query, new object[] { datecheckin, datecheckout });
            return temp;
           
        }


        public DataTable GetListBillByDateAndPage(DateTime datecheckin, DateTime datecheckout, int page , int pageSize)
        {
            string query = "USP_GetListBillByDateAndPage @dateCheckIn , @dateCheckOut , @page , @pageSize";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { datecheckin, datecheckout, page, pageSize });
            return data;
        }

        public void DeleteBillByTableId(int id)
        {
            string query = "USP_DeleteBillByTableId @id";
            DataProvider.Instance.ExecuteQuery(query, new object[] { id });
        }

        public int GetMaxIdBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("USP_GetMaxIdBill");
            }
            catch
            {
                return 1;
            }
        }
    }
}

