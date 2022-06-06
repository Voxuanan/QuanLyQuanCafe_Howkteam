using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class MenuFood
    {

        public MenuFood(DataRow row)
        {
            this.foodName = row["Name"].ToString();
            this.Count = (int)row["Count"];
            this.price = (float)Convert.ToDouble((row["price"]).ToString());
            this.totalPrice = (float)Convert.ToDouble((row["totalPrice"]).ToString());
        }

        public MenuFood(string foodName, int count, float price, float totalPrice = 0)
        {
            this.foodName = foodName;
            this.Count = count;
            this.price = price;
            this.totalPrice = totalPrice;
        }

        private float totalPrice;

        private float price;

        private string foodName;

        private int count;

        public string FoodName { get => foodName; set => foodName = value; }
        public int Count { get => count; set => count = value; }
        public float Price { get => price; set => price = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
