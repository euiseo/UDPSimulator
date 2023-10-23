///////////////////////////////////////////////////////////
//  TCMS_BaseData.cs
//  Implementation of the Interface TCMS_BaseData
//  Generated by Enterprise Architect
//  Created on:      07-8-2023 ���� 3:27:07
//  Original author: ���Ǽ�
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


public interface ITCMSBaseData
{
	/// 
	/// <param name="address"></param>
	/// <param name="sequenceCounter"></param>
	void SetData(string address, int sequenceCounter);

	/// 
	byte[] ToByte();
}//end TCMSBaseData