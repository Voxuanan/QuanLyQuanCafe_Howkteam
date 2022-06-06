using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource tableList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();

            Loads();
        }
    

        #region methods

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDay.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListBillByDate(DateTime dateCheckIn, DateTime dateCheckOut)
        {
            dtgvBill.DataSource =  BillDAO.Instance.GetListBillByDate(dateCheckIn, dateCheckOut);
        }

        void Loads()
        {
            dtgvFood.DataSource = foodList;

            dtgvCategory.DataSource = categoryList;

            dtgvTable.DataSource = tableList;

            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDay.Value, (int)nmPage.Value, (int)nmPageSize.Value);

            LoadListFood();

            AddFoodBinding();

            LoadCategoryIntoComboBox();

            LoadListCategogy();

            AddCategoryBinding();

            LoadListTable();

            AddTableBinding();

            LoadAccount();

            AddAccountBinding();
        }

        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableList();
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void LoadListCategogy()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmStatusType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        
        void AddFoodBinding()
        {
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID",true, DataSourceUpdateMode.Never));
            txbFoodName.DataBindings.Add(new Binding("Text",dtgvFood.DataSource,"Name", true, DataSourceUpdateMode.Never));
     
            nmFoodPrice.DataBindings.Add(new Binding("Value",dtgvFood.DataSource,"Price", true, DataSourceUpdateMode.Never));
        }

        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbNameCategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name",true, DataSourceUpdateMode.Never));
            txbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoComboBox()
        {
            cbFoodCategory.DataSource = CategoryDAO.Instance.GetListCategory();
            cbFoodCategory.DisplayMember = "Name";
        }
        #endregion



        #region events
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDay.Value, (int)nmPage.Value, (int)nmPageSize.Value);
        }


        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0 && dtgvFood.SelectedCells[0].OwningRow.Cells["IdCategory"].Value != null)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["IdCategory"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    int index = -1;
                    int i = 0;

                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryId = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryId, price))
            {
                MessageBox.Show("Thêm món thành công", "Thông báo");
                LoadListFood();
                if (insertFood != null) insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm món thất bại", "Thông báo");
            }
        }
  

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công", "Thông báo");
                LoadListFood();
                if (deleteFood != null) deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa món thất bại", "Thông báo");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);
            string name = txbFoodName.Text;
            int categoryId = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;


            if (FoodDAO.Instance.UpdateFood(id, name, categoryId, price))
            {
                MessageBox.Show("Sửa món thành công", "Thông báo");
                LoadListFood();
                if (updateFood != null) updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Sửa món thất bại", "Thông báo");
            }
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }


        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbNameCategory.Text;
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công", "Thông báo");
                LoadListCategogy();
                if (insertCategory != null) insertCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại", "Thông báo");
            }
        }


        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {

            int id = Convert.ToInt32(txbCategoryID.Text);
            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công", "Thông báo");
                LoadListCategogy();
                if (deleteFood != null) deleteFood(this, new EventArgs());
                if (deleteCategory != null) deleteCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa danh mục thất bại", "Thông báo");
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txbNameCategory.Text;
            int id = Convert.ToInt32(txbCategoryID.Text);
            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa danh mục thành công", "Thông báo");
                LoadListCategogy();
                if (updateCategory != null) updateCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Sửa danh mục thất bại", "Thông báo");
            }
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategogy();
        }



        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công", "Thông báo");
                LoadListTable();
                if (insertTable != null) insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại", "Thông báo");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công", "Thông báo");
                LoadListTable();
                if (deleteTable != null) deleteTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Xóa bàn thất bại", "Thông báo");
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            int id = Convert.ToInt32(txbTableID.Text);
            if (TableDAO.Instance.UpdateTable(id,name))
            {
                MessageBox.Show("Sửa bàn thành công", "Thông báo");
                LoadListTable();
                if (updateTable != null) updateTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Sửa bàn thất bại", "Thông báo");
            }

        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }


        private void btnEditAccount_Click(object sender, EventArgs e)
        {   
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmStatusType.Value;
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công", "Thông báo");
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Sửa tài khoản thất bại", "Thông báo");
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Tên đăng nhập của tài khoản cần xóa không được trùng với tài khoản đang đăng nhập", "Thông báo");
                return;
            }
                if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công", "Thông báo");
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại", "Thông báo");
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txbUserName.Text;
                string displayName = txbDisplayName.Text;
                int type = (int)nmStatusType.Value;
                if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
                {
                    MessageBox.Show("Thêm tài khoản thành công", "Thông báo");
                    LoadAccount();
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại", "Thông báo");
                }
            } catch { MessageBox.Show("Tên đăng nhập đã được sử dụng vui lòng đặt lại tên khác", "Thông Báo"); }
        }

        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (AccountDAO.Instance.ResetAccountPassword(userName))
            {
                MessageBox.Show("Cập nhật mật khẩu thành công", "Thông báo");
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Cập nhật mật khẩu thất bại", "Thông báo");
            }
        }


        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }

        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }

        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }

        }


        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }

        }

        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }

        }

        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }

        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }

        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }

        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            nmPage.Value = 1;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetCountBillByDate(dtpkFromDate.Value, dtpkToDay.Value);
            int lastPage = sumRecord / (int)nmPageSize.Value;

            if (sumRecord % (int)nmPageSize.Value != 0) lastPage++;

            nmPage.Value = lastPage;
        }

        private void btnNext_Click(object sender, EventArgs e)
        { 
            int page = (int)nmPage.Value;
            int sumRecord = BillDAO.Instance.GetCountBillByDate(dtpkFromDate.Value, dtpkToDay.Value);
            int lastPage = sumRecord / (int)nmPageSize.Value;

            if (sumRecord % (int)nmPageSize.Value != 0) lastPage++;
            if ( page < lastPage )
            page++;
            nmPage.Value = page;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int page = (int)nmPage.Value;
            if (page > 1) page--;
            nmPage.Value = page; 
        }

        private void nmPage_ValueChanged(object sender, EventArgs e)
        {
            int page = (int)nmPage.Value;
            int sumRecord = BillDAO.Instance.GetCountBillByDate(dtpkFromDate.Value, dtpkToDay.Value);
            int lastPage = sumRecord / (int)nmPageSize.Value;

            if (sumRecord % (int)nmPageSize.Value != 0) lastPage++;
            if (page > lastPage) page = lastPage;
            if (page < 1) page = 1;
            nmPage.Value = page;
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDay.Value, (int)nmPage.Value, (int)nmPageSize.Value);
        }


        private void nmPageSize_ValueChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDay.Value, (int)nmPage.Value, (int)nmPageSize.Value);
        }

        #endregion

    }
}

