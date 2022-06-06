using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance 
        {
            get { if (instance == null) instance = new FoodDAO(); return instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<Food> GetListFoodByCategoryID(int id)
        {
            List<Food> list = new List<Food>();
            string query = "EXEC USP_GetListFoodByCategoryID @categoryID";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { id });

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }

        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();
            string query = "USP_GetListFood";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }

        public void DeleteFoodByCategoryId(int id)
        {
            string query = "USP_DeleteFoodByCategoryId @id";
            DataProvider.Instance.ExecuteQuery(query, new object[] { id });
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();
            string query = "USP_SearchFoodByName @name";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { name });

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }

            return list;
        }

        public bool InsertFood(string name, int idcategory, float price)
        { 
            string query = "USP_InsertFood @name , @idCategory , @price";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, idcategory, price });
            return result > 0;
        }

        public bool UpdateFood(int id, string name, int idcategory, float price)
        {
            string query = "USP_UpdateFood @id , @name , @idCategory , @price";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id, name, idcategory, price });
            return result > 0;
        }

        public bool DeleteFood(int id)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodId(id);
            string query = "USP_DeleteFood @id";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id });
            return result > 0;
        }
    }
}
