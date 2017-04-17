using Project.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Controller {
    public class UsersCred {
        protected string _Username, _Password;
        public UsersCred(string Username, string Password) {
            _Username = Username;
            _Password = Password;
        }
        public string Username {
            get { return _Username; }
            set { _Username = value; }
        }
        public string Password {
            get { return _Password; }
            set { _Password = value; }
        }
        public void AddCred() {
            UserAuth cred = new UserAuth();
            cred.AddUserCred(_Username, _Password);
        }
        public void UpdateCred() {
            UserAuth chk = new UserAuth();
            chk.UpdateUserCred(_Username, _Password);
        }
        public bool CheckIsUsernameExist() {
            UserAuth chk = new UserAuth();
            return chk.SelectUsername(_Username);
        }
        public bool CheckLoginValidity() {
            UserAuth chk = new UserAuth();
            return chk.CheckLoginValidity(_Username,_Password);
        }

    }
}
