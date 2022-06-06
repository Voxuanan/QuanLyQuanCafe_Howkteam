using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value;
                ChangeAccount(loginAccount);
            }
        }
        public fAccountProfile(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }

        void UpdateAccountInfo()
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            string password = txbPassWord.Text;
            string newPassword = txbNewPassWord.Text;
            string reEnterPassword = txbReEnterPassWord.Text;

            if (!newPassword.Equals(reEnterPassword)) MessageBox.Show("Mật khẩu mới và nhập lại nhật khẩu không giống nhau vui lòng nhập lại", "Thông báo");
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, password, newPassword))
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo");
                    if (updateAccount != null) updateAccount(this,new AccountEvent( AccountDAO.Instance.GetAccountByUserName(userName)));
                    this.Close();
                }
                else MessageBox.Show("Vui lòng điền đúng mật khẩu", "Thông báo");
            }
        }

        void ChangeAccount(Account acc)
        {
            txbUserName.Text = acc.UserName;
            txbDisplayName.Text = acc.DisplayName;

        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value;  }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }

    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc { get => acc; set => acc = value; }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
