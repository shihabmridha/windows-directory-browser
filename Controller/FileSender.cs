using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project.Controller {
    class FileSender {
        bool IsSocketConnected(Socket s) {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        public Socket clientSock;

        public void SendFile(string FileDir, string IP) {

            clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] m_clientData;
            FileProperties file = new FileProperties(FileDir);
            byte[] fileName = Encoding.UTF8.GetBytes(file.GetFileName()); //file name
            byte[] fileData = File.ReadAllBytes(FileDir); //file
            byte[] fileNameLen = BitConverter.GetBytes(fileName.Length); //lenght of file name
            m_clientData = new byte[4 + fileName.Length + fileData.Length];

            fileNameLen.CopyTo(m_clientData, 0);
            fileName.CopyTo(m_clientData, 4);
            fileData.CopyTo(m_clientData, 4 + fileName.Length);

            try {
                clientSock.Connect(IP, 9050); //target machine's ip address and the port number
                if (IsSocketConnected(clientSock)) {
                    clientSock.Send(m_clientData);
                    clientSock.Shutdown(SocketShutdown.Both);
                    clientSock.Close();
                    Console.WriteLine("Shit Connected!");
                } else {
                    Console.WriteLine("Shit Not Connected!");
                }                
            } catch (SocketException ex) {
                if (ex.NativeErrorCode.Equals(10035))
                    Console.WriteLine("Still Connected, but the Send would block");
                else {
                    string msg = "Unable to connect with partner. Error code " + ex.NativeErrorCode + "!";
                    Console.WriteLine(msg);
                }
            }

        }
    }
}
