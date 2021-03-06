﻿using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project.Controller {
    class FileReceiver {
        public Thread t1;
        int flag = 0;
        string receivedPath;
        ProgressDialogController ReceiveDialog;
        public FileReceiver(string path, ProgressDialogController pr) {
            t1 = new Thread(new ThreadStart(StartListening));
            t1.Start();
            receivedPath = path;
            ReceiveDialog = pr;
        }

        public class StateObject {
            // Client socket.
            public Socket workSocket = null;

            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
        }

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public void StartListening() {
            byte[] bytes = new Byte[1024];
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 9050);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                listener.Bind(ipEnd);
                listener.Listen(100);
                while (true) {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                    Console.WriteLine("Started recieving file.");
                }
            } catch (Exception ex) {
                Console.WriteLine("StartListening Error: " + ex.Message);
            }

        }
        public void AcceptCallback(IAsyncResult ar) {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            flag = 0;
        }

        public void ReadCallback(IAsyncResult ar) {

            int fileNameLen = 1;
            String content = String.Empty;
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            SocketError errorCode;
            int bytesRead = handler.EndReceive(ar, out errorCode);
            if (errorCode != SocketError.Success) {
                bytesRead = 0;
            }
            else {
                if (bytesRead > 0) {
                    try {
                        if (flag == 0) {
                            fileNameLen = BitConverter.ToInt32(state.buffer, 0);
                            string fileName = Encoding.UTF8.GetString(state.buffer, 4, fileNameLen);
                            receivedPath += fileName;
                            flag++;
                        }
                        if (flag >= 1) {
                            BinaryWriter writer = new BinaryWriter(File.Open(receivedPath, FileMode.Append));
                            if (flag == 1) {
                                writer.Write(state.buffer, 4 + fileNameLen, bytesRead - (4 + fileNameLen));
                                flag++;
                            }
                            else {
                                writer.Write(state.buffer, 0, bytesRead);
                            }
                            writer.Close();
                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("ReadCallback Error: " + e.Message);
                    }
                }
                else {
                    FinishTheJob();
                }
            }
            
        }

        async void FinishTheJob() {
            ReceiveDialog.SetProgress(1);
            await ReceiveDialog.CloseAsync();            
            t1.Abort();
        }

    }
}
