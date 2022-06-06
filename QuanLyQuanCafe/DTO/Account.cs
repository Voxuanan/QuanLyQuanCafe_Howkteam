using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        public Account(string username, string displayname, int type, string password = null)
        {
            this.userName = username;
            this.displayName = displayname;
            this.type = type;
            this.passWord = password;
        }

        public Account(DataRow row)
        {
            this.userName = row["username"].ToString();
            this.displayName = row["displayname"].ToString();
            this.type = (int)row["type"];
            this.passWord = row["password"].ToString();
        }

        private string userName;

        private string displayName;

        private string passWord;

        private int type;

        public string UserName { get => userName; set => userName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        public int Type { get => type; set => type = value; }
    }
}
