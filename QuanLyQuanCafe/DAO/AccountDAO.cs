using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set {instance = value;}
        }
        private AccountDAO() { }

        public bool Login(string userName, string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hasdData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hashPass = "";
            foreach (Byte item in hasdData)
            {
                hashPass += item;
            }
            //var list = hasdData.ToString();
            //list.Reverse(); 
            

            string query = "EXEC USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query,new object[] {userName, hashPass /*list*/});
            return result.Rows.Count > 0;
        }

        public Account GetAccountByUserName(string username)
        {
            string query = "USP_GetAccountByUserName @userName";
            DataTable data = DataProvider.Instance.ExecuteQuery(query,new object[] { username });

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }

        public DataTable GetListAccount()
        {
            string query = "USP_GetListAccount";
            return DataProvider.Instance.ExecuteQuery(query);
        }


        public bool UpdateAccount(string username, string displayname, string password, string newpassword)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hasdData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hashPass = "";
            foreach (Byte item in hasdData)
            {
                hashPass += item;
            }

            byte[] temp2 = ASCIIEncoding.ASCII.GetBytes(newpassword);
            byte[] hasdData2 = new MD5CryptoServiceProvider().ComputeHash(temp2);
            string hashPass2 = "";
            foreach (Byte item in hasdData2)
            {
                hashPass2 += item;
            }
            string query = "USP_UpdateAccount @userName , @displayName , @password , @newPassword";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { username, displayname, hashPass, hashPass2 });
            return (result > 0);
        }

        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = "USP_InsertAccount @userName , @displayName , @type";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, displayName, type });
            return result > 0;
        }

        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = "USP_UpdateAccount2 @userName , @displayName , @type";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, displayName, type });
            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = "USP_DeleteAccount @userName";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name});
            return result > 0;
        }

        public bool ResetAccountPassword(string name)
        {
            string query = "USP_ResetAccountPassword @userName";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name });
            return result > 0;
        }
    }
}
