using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfo
    {
        public BillInfo(int id, int billID,int foodID, int count)
        {
            this.id = id;
            this.billID = billID;
            this.count = count;
            this.foodID = foodID;
        }


        public BillInfo(DataRow row)
        {
            this.id = (int)row["id"];
            this.billID = (int)row["idbill"];
            this.count = (int)row["count"];
            this.foodID = (int)row["idfood"];
        }

        private int billID;
        public int BillID { get => billID; set => billID = value; }

        private int id;
        public int Id { get => id; set => id = value; }

        private int count;
        public int Count { get => count; set => count = value; }

        private int foodID;
        public int FoodID { get => foodID; set => foodID = value; }


    }
}
