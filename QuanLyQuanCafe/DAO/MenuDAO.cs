using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set { MenuDAO.instance = value; }
        }

        private MenuDAO() { }

        public List<MenuFood> GetListMenuByTable(int id)
        {
            List<MenuFood> listMenu = new List<MenuFood>();

            String query = "EXEC USP_GetListMenuByTable @idTable";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { id });

            foreach (DataRow item in data.Rows)
            {
                MenuFood menu = new MenuFood(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
