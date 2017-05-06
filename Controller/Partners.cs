using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project.Controller {
    class Partners {
        public bool IsContactAvailable(string IP) {
            FileSender fs = new FileSender();
            Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                clientSock.Connect(IP, 9050);
                if (fs.IsSocketConnected(clientSock)) {
                    return true; 
                }
                clientSock.Shutdown(SocketShutdown.Both);
                clientSock.Close();
            }
            catch (SocketException ex) {
                if (ex.NativeErrorCode.Equals(10035))
                    Console.WriteLine("Still Connected, but the Send would block");
                else {
                    string msg = "Unable to connect with partner. Error code " + ex.NativeErrorCode + "!";
                    Console.WriteLine(msg);
                }
            }
            return false;
        }
    }
}
