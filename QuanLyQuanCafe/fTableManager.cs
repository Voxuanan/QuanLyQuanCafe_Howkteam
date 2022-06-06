using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount 
        {
            get { return loginAccount; }
            set { loginAccount = value;
                ChangeAccount(loginAccount.Type);
            }
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();

            LoadCategory();

            LoadComboBoxTable(cbSwitchTable);
        }


        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = (type == 1);
            thôngTinTàiKhoảnToolStripMenuItem.Text += " ("+loginAccount.DisplayName+")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCategoryID(id);
            if (listFood.Count == 0)
            {
                cbFood.Text = "";
            }
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList =TableDAO.Instance.LoadTableList();
            
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += Btn_Click;
                btn.Tag = item;
                
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.Lime;
                        break;
                }
                flpTable.Controls.Add(btn);
            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<MenuFood> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);

            float toltalPrice = 0;

            foreach(MenuFood item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName);
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                toltalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            
            }
            CultureInfo culture = new CultureInfo("vi-VN");

            Thread.CurrentThread.CurrentCulture = culture;

            txbTotalPrice.Text = toltalPrice.ToString("c");
        }

        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion



        #region Event

        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            nmDiscount.Value = 0;
            nmFoodCount.Value = 1;
            ShowBill(tableID);  
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateAccount += F_UpdateAccount;
            f.ShowDialog();
        }

        private void F_UpdateAccount(object sender,AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản ("+ e.Acc.DisplayName +")";
            loginAccount = e.Acc;
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.InsertCategory += F_InsertCategory;
            f.UpdateCategory += F_UpdateCategory;
            f.DeleteCategory += F_DeleteCategory;
            f.InsertTable += F_InsertTable;
            f.UpdateTable += F_UpdateTable;
            f.DeleteTable += F_DeleteTable;
            f.ShowDialog();
        }

        private void F_DeleteTable(object sender, EventArgs e)
        {
            LoadTable();
            LoadComboBoxTable(cbSwitchTable);
        }

        private void F_UpdateTable(object sender, EventArgs e)
        {
            LoadTable();
            LoadComboBoxTable(cbSwitchTable);
        }

        private void F_InsertTable(object sender, EventArgs e)
        {
            LoadTable();
            LoadComboBoxTable(cbSwitchTable);
        }

        private void F_DeleteCategory(object sender, EventArgs e)
        {
            LoadCategory();
            LoadFoodByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_UpdateCategory(object sender, EventArgs e)
        {
            LoadCategory();
            LoadFoodByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_InsertCategory(object sender, EventArgs e)
        {
            LoadCategory();
            LoadFoodByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null) return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodByCategoryID(id);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table != null)
            {

                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int idFood = (cbFood.SelectedItem as Food).ID;
                int count = (int)nmFoodCount.Value;

                if (idBill == -1)
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), idFood, count);
                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
                }
                ShowBill(table.ID);
                LoadTable();

            }
            else MessageBox.Show("Vui lòng chọn bàn trước khi thêm món", "Thông báo");
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table != null)
            {

                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int discount = (int)nmDiscount.Value;

                float totalPrice = float.Parse(txbTotalPrice.Text.Split(',')[0]);
                float final = totalPrice - (totalPrice / 100) * discount;

                if (idBill != -1)
                {
                    if (MessageBox.Show(string.Format("Bạn có thực sự muốn thanh toán hóa đơn cho {0}\n " +
                        "Tổng tiền - ( Tổng tiền / 100 ) * Giảm  giá  :\n" +
                        " {1} - ( {1} / 100 ) * {2} = {3}", table.Name, totalPrice, discount, final), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        BillDAO.Instance.CheckOut(idBill, discount,final);
                        ShowBill(table.ID);

                        LoadTable();
                    }
                }
            }
            else MessageBox.Show("Vui lòng chọn bàn trước thanh toán", "Thông báo");
        }


        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            try
            {
                if (lsvBill.Tag != null)
                {
                    int id1 = (lsvBill.Tag as Table).ID;

                    int id2 = (cbSwitchTable.SelectedItem as Table).ID;

                    if ((lsvBill.Tag as Table).Status == "Trống")
                        MessageBox.Show("Bàn cần chuyển không được trống", "Thông báo");
                    else
                        if (MessageBox.Show(string.Format("Bạn có thực sự muốn chuyển {0} sang {1} ", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        TableDAO.Instance.SwitchTable(id1, id2);

                        LoadTable();
                    }
                }
                else MessageBox.Show("Vui lòng chọn bàn trước khi chuyển bàn", "Thông báo");
            } catch { }
        }
        #endregion

    }
}
