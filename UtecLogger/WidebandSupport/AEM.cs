﻿/*
 * Copyright 2009 George Daswani
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace WidebandSupport
{

	public class AEMWidebandReader : WidebandReader
	{
		int sampleLinePacketIndex = 0; // this is only used for testing.

		public AEMWidebandReader(String comPortName)
		{

			if (false == IsSerialPortNameValid(comPortName))
			{
				throw new ArgumentException(comPortName + ", is invalid.");
			}

			init(comPortName);

		}

		private void init(String comPortName)
		{
			comPort = new SerialPort();
			comPort.PortName = comPortName;
			comPort.BaudRate = 9600;
			comPort.DataBits = 8;
			comPort.Parity = Parity.None;
			comPort.StopBits = StopBits.One;
			comPort.Handshake = Handshake.RequestToSend;
		}


		/*
		 * This method is only used for testing
		 */
		private String GetLineFromSamplePacket()
		{
			String[] lines = {
				"AEM Inc. 2003",
				"AFR Gauge",
				"Version 3",
				"",
				"00.0",
				"10.0",
				"11.0",
				"12.0",
				"13.0",
				"14.0",
				"15.0"
			};

			if (sampleLinePacketIndex >= lines.Length)
			{
				sampleLinePacketIndex = 0;
			}

			Thread.Sleep(TimeSpan.FromMilliseconds(10));

			return lines[sampleLinePacketIndex++];

		}

		protected  override void InitiateReading()
		{


			while (continueRunning)
			{

				try
				{
					String line = null;
					double afrValue;

					if (testMode)
					{
						line = GetLineFromSamplePacket();
					}
					else
					{
						line = comPort.ReadLine();
					}

					if (line != null && 0 != line.Trim().Length && true == Double.TryParse(line, out afrValue))
					{
						latestReading = afrValue;
					}
				}
				catch (ThreadInterruptedException)
				{
					// nothing
				}
				catch(Exception ex)
				{
					OnErrorReceived(ex);
				}

			}


		}

	}
}
