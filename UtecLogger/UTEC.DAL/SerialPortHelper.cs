/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 10/15/2011
 * Time: 6:34 PM
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.IO.Ports;

namespace UTEC.DAL
{
	/// <summary>
	/// Description of SerialPortHelper.
	/// </summary>
	public class SerialPortHelper
	{
		public static string[] GetPortNames()
		{
			return SerialPort.GetPortNames();
		}
	}
}
