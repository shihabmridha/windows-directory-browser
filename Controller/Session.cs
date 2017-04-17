using Project.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Controller {
    public class Session {
        private string _Username;
        public static Session sec;
        string _SessionStartTime;

        public Session() {            
            _SessionStartTime = DateTime.Now.ToString("h:mm:ss tt");
        }
        public string SessionStartTime {
            get { return _SessionStartTime; }
        }
        public string UserName {
            get { return _Username; }
            set { _Username = value; }
        }

        public void StartSession() {
            SessionData.CreateSession(_Username,_SessionStartTime,1);
        }
        public string GetSession() {
            return SessionData.CurrentSession();
        }
        public void EndSession() {
            SessionData.EndSession();
        }
        public bool SessionExist() {
            return SessionData.HasSession();
        }
        public void UpdateSession() {
            SessionData.UpdateSession();

        }

    }
}
