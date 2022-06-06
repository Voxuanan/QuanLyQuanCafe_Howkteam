using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut,int status,int discount)
        {
            this.id = id;
            this.dateCheckIn = dateCheckIn;
            this.dateCheckOut = dateCheckOut;
            this.status = status;
            this.discount = discount;
        }

        public Bill(DataRow row)
        {
            this.id = (int)row["id"];
            this.dateCheckIn = (DateTime)row["DateCheckIn"];

            var dateCheckOutTemp = row["DateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
                this.dateCheckOut = (DateTime)row["DateCheckOut"];


            this.status = (int)row["Status"];
            this.discount = (int)row["Discount"];
        }

        private int id;

        private int discount;
        public int Id { get => id; set => id = value;}
    
     
        private DateTime? dateCheckIn;
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }

        private DateTime? dateCheckOut;
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }

        private int status;
        public int Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }
    }
}
