using Project.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Controller {
    class ProfileController{
        public Users GetProfileInfo() {
            Users usr = ProfileData.GetUserInformation();      
            return usr;
        }
        public void UpdateProfileInfo(){
            throw new NotImplementedException();
        }
    }
}
