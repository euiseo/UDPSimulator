///////////////////////////////////////////////////////////
//  TCMSInterfaceManager.cs
//  Implementation of the Class TCMSInterfaceManager
//  Generated by Enterprise Architect
//  Created on:      07-8-2023 오후 3:27:07
//  Original author: 김의서
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TcmsSimulator.Socket;
using System.Net.Sockets;
using System.Configuration;

public class TCMSInterfaceManager
{
	#region  변수 및 속성값 생성
	//private Queue<TCMSBaseData> _operationDataQueue;
	private UDPSocketClient _statusSender = null;
	private UDPSocketServer _TCMSReceiver = null;

	public delegate void ReadCompleteDeletegate(byte[] receivedData);
	public ReadCompleteDeletegate ReadComplete { get; set; }

	public UDPSocketClient StatusSender
	{
		get { return _statusSender; }
	}
	public UDPSocketServer TCMSReceiver
	{
		get { return _TCMSReceiver; }
	}
	#endregion

	public TCMSInterfaceManager()
	{
		InitData();
		InitEvent();
	}

	~TCMSInterfaceManager(){

	}

	private void InitData()
	{
		int bufferSize;
		int OperationPort;
		int StatusPort;
		int SimulatorPort; 
		ProtocolType protocolType;
		SocketType socketType;
		
		try
		{
			//초기값 세팅
			string SimulatorIP = ConfigurationManager.AppSettings["SimulatorIP"];
			int.TryParse(ConfigurationManager.AppSettings["BufferSize"], out bufferSize);
			int.TryParse(ConfigurationManager.AppSettings["OperationPort"], out OperationPort);
			int.TryParse(ConfigurationManager.AppSettings["StatusPort"], out StatusPort);
			int.TryParse(ConfigurationManager.AppSettings["SimulatorPort"], out SimulatorPort);
			protocolType = TcmsSimulator.Util.EnumUtil<ProtocolType>.Parse(ConfigurationManager.AppSettings["ProtocolType"]);
			socketType = TcmsSimulator.Util.EnumUtil<SocketType>.Parse(ConfigurationManager.AppSettings["SocketType"]);

			_TCMSReceiver = new UDPSocketServer(bufferSize, SimulatorIP, OperationPort);
			_statusSender = new UDPSocketClient(bufferSize, SimulatorIP, StatusPort);
		}
		catch(Exception e)
        {
			TcmsSimulator.Util.Utils.WriteError(e.Message);
		}
	}

	private void InitEvent()
	{
		_TCMSReceiver.NetworkReadHandler += ReadCompleteCallback;
		_statusSender.NetworkReadHandler += ReadCompleteCallback;
	}
	public void ReadCompleteCallback(byte[] receivedData)
    {
		ReadComplete(receivedData);
	}
	/// 
	/// <param name="second"></param>
	public void DataResetTimer(int second){

	}

	public bool ReadConfig(){

		return false;
	}

	/// 
	/// <param name="bufferSize"></param>
	/// <param name="IP"></param>
	/// <param name="port"></param>
	public bool SetClientConfig(int bufferSize, string IP, int port){

		return false;
	}

	/// 
	/// <param name="bufferSize"></param>
	/// <param name="IP"></param>
	/// <param name="port"></param>
	/// <param name="protocol"></param>
	/// <param name="socketType"></param>
	public bool SetServerConfig(int bufferSize, string IP, int port, ProtocolType protocol, SocketType socketType){

		return false;
	}

	public bool SetConfig(){

		return false;
	}

}//end TCMSInterfaceManager