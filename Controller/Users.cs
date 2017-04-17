using Project.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Controller {
    class Users {
        protected string _FirstName, _LastName, _Username, _Email, _ShareCode;

        public Users() { }
        public Users(string fName, string lName, string Username, string Email, string ShareCode) {
            _FirstName = fName;
            _LastName = lName;
            _Username = Username;
            _Email = Email;
            _ShareCode = ShareCode;
        }
        public string FirstName {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string LastName {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public string Username {
            get { return _Username; }
            set { _Username = value; }
        }
        public string Email {
            get { return _Email; }
            set { _Email = value; }
        }
        public string ShareCode{
            get{ return _ShareCode; }
            set{ _ShareCode = value; }
        }
        public bool CheckIsEmailExist(string email) {
            UserAuth cred = new UserAuth();
            return cred.SelectEmail(email);
        }

        public void AddUser() {
            UserAuth add = new UserAuth();
            add.AddUserInfo(_FirstName, _LastName, _Username, _Email, _ShareCode);
        }
        public void UpdateUser() {
            UserAuth update = new UserAuth();
            update.UpdateUserInfo(_FirstName, _LastName, _Username, _Email, _ShareCode);
        }

    }
}
