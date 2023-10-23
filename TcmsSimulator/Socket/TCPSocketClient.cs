///////////////////////////////////////////////////////////
//  Socket_Client.cs
//  Implementation of the Class SocketClient
//  Generated by Enterprise Architect
//  Created on:      07-8-2023 오후 5:27:24
//  Original author: 김의서
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcmsSimulator.Socket
{
	/// <summary>
	/// TCMS 테스트 프로그램
	/// </summary>
	public class TCPSocketClient
	{
		#region  변수 및 속성값 생성
		System.Net.Sockets.Socket _client = null;

		private ManualResetEvent connectDone = new ManualResetEvent(false);
		private ManualResetEvent sendDone = new ManualResetEvent(false);
		public ManualResetEvent ReceiveDone = new ManualResetEvent(false);

		public delegate void ReadCompleteDeletegate(byte[] receivedData);
		public ReadCompleteDeletegate NetworkReadHandler { get; set; }

		private int _bufferSize;
		private string _IP;
		private int _port;

		public int BufferSize
		{
			get { return _bufferSize; }
			set { _bufferSize = value; }
		}
		public string IP
		{
			get { return _IP; }
			set { _IP = value; }
		}
		public int port
		{
			get { return _port; }
			set { _port = value; }
		}
		#endregion 변수 및 속성값 생성

		public TCPSocketClient()
		{

		}
		public TCPSocketClient(int bufferSize, string IP, int port )
		{
			_bufferSize = bufferSize;
			_IP = IP;
			_port = port;
		}

		~TCPSocketClient()
		{

		}
		public void SetData(int bufferSize, string IP, int port)
		{
			_bufferSize = bufferSize;
			_IP = IP;
			_port = port;
		}

		/// 
		/// <param name="buffer"></param>
		public int Connect()
		{
			IPAddress ipAddress = IPAddress.Parse(_IP);
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, _port);
			if (TcmsSimulator.Util.Utils.IsPingTest(_IP) == false)
				return 0;

			if (_client != null)
			{
				Disconnect();
			}

			_client = new System.Net.Sockets.Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			// Connect to the remote endpoint.  
			_client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), _client);
			connectDone.WaitOne();

			return 0;
		}

		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.  
				//_client = (System.Net.Sockets.Socket)ar.AsyncState;
				// Complete the connection.  
				//_client.EndConnect(ar);

				// Signal that the connection has been made.  
				connectDone.Set();
			}
			catch (Exception e)
			{
				connectDone.Set();
				TcmsSimulator.Util.Utils.WriteError(e.Message);
			}
		}

		public void Send(byte[] byteData)
		{
            // Convert the string data to byte data using ASCII encoding.  
            //byte[] byteData = Encoding.ASCII.GetBytes(data);

            try
            {
				_client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), _client);
			}
			catch(Exception e)
            {
				Console.WriteLine(e.Message);
				TcmsSimulator.Util.Utils.WriteError(e.Message);
			}
			// Begin sending the data to the remote device.  
			
		}

		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.  
				System.Net.Sockets.Socket client = (System.Net.Sockets.Socket)ar.AsyncState;

				// Complete sending the data to the remote device.  
				int bytesSent = client.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to server.", bytesSent);

				// Signal that all bytes have been sent.  
				sendDone.Set();
			}
			catch (Exception e)
			{
				TcmsSimulator.Util.Utils.WriteError(e.Message);
			}
		}

		public virtual void Receive(System.Net.Sockets.Socket client)
		{
			try
			{
				// Create the state object.  
				StateObject state = new StateObject(_bufferSize);
				state.WorkSocket = client;

				// Begin receiving the data from the remote device.  
				client.BeginReceive(state.DataBuffer, 0, state.DataBufferSize, 0, new AsyncCallback(DataReadCallback), state);
			}
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				TcmsSimulator.Util.Utils.WriteError(e.Message);
			}
		}

		public virtual void DataReadCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the state object and the handler socket  
				// from the asynchronous state object.  
				StateObject state = (StateObject)ar.AsyncState;
				System.Net.Sockets.Socket handler = state.WorkSocket;

				if (!handler.Connected || handler.Available == 0)
				{
					Disconnect();
					return;
				}

				// Read data from the client socket.   
				int bytesRead = handler.EndReceive(ar);

				if(NetworkReadHandler != null)
					NetworkReadHandler(state.DataBuffer);
			}
			catch (Exception e)
			{
				//LogHelper.Instance.Error(e.Message);
				Disconnect();
				TcmsSimulator.Util.Utils.WriteError(e.Message);
			}
		}
		public void Disconnect()
		{
			if (_client != null)
			{
				try
				{
					ulong handle = (ulong)_client.Handle;
					ReceiveDone.Set();
					if (_client != null)
					{
						_client?.Shutdown(SocketShutdown.Both);
						_client?.Close();
						_client = null;
					}
				}
				catch (Exception e)
				{
					TcmsSimulator.Util.Utils.WriteError(e.Message);
				}
			}
			_client = null;
		}

	}//end SocketClient
}