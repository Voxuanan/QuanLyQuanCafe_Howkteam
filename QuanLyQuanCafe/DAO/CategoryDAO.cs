using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance 
        { 
            get { if (instance == null) instance = new CategoryDAO(); return instance;} 
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();
            string query = "EXEC USP_SelectCategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }

            return list;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;
            string query = "USP_GetCategoryByID @categoryId";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { id });

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }


            return category;
        }

        public bool InsertCategory(string name  )
        {
            string query = "USP_InsertCategory @name";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name });
            return result > 0;
        }

        public bool UpdateCategory(int id, string name)
        {
            string query = "USP_UpdateCategory @id , @name";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id , name });
            return result > 0;
        }

        public bool DeleteCategory(int id)
        {
            BillInfoDAO.Instance.DeleteBillInfoByCategoryId(id);
            FoodDAO.Instance.DeleteFoodByCategoryId(id);
            string query = "USP_DeleteCategory @id";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id });
            return result > 0;
        }
    }
}
