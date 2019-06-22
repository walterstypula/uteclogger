/*
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

	public class ZT2WidebandReader : WidebandReader
	{

		private Object locker = new Object();

		int sampleBytePacketIndex = 0; // this is only used for testing.
		
		public ZT2WidebandReader(String comPortName)
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
			comPort.Handshake = Handshake.None;

		}


		/*
		 * This method is only used for testing
		 */
		private byte GetByteFromSamplePacket()
		{
			byte[] packet = new byte[] {
				( 0 ), // [0] always 0
				( 1 ), // [1] always 1
				( 2 ), // [2] always 2
				( 147 ), // [3] AFR
				( 0 ), // [4] EGT Low
				( 0 ), // [5] EGT High
				( 0 ), // [6] RPM Low
				( 0 ), // [7] RPM High
				( 0 ), // [8] MAP Low
				( 0 ), // [9] MAP High
				( 0 ), // [10] TPS
				( 0 ), // [11] USER1
				( 0 ), // [12] Config Register1
				( 0 ), // [13] Config Register2
			};

			if (sampleBytePacketIndex >= packet.Length)
			{
				sampleBytePacketIndex = 0;
				Thread.Sleep(TimeSpan.FromMilliseconds(80));
			}

			return packet[sampleBytePacketIndex++];

		}

		protected override void InitiateReading()
		{

			List<byte> buffer = new List<byte>();

			bool packetStarted = false;

			while (continueRunning)
			{
				try
				{
					byte aByte = 0;

					if (testMode)
					{
						aByte = GetByteFromSamplePacket(); // test packet
					}
					else
					{
						aByte = (byte)comPort.ReadByte(); // to read from the serial port
					}

					if (buffer.Count >= 2 && 0x02 == aByte && 0x01 == buffer[buffer.Count - 1] && 0x00 == buffer[buffer.Count - 2])
					{
						packetStarted = true;
						buffer.Clear();
						buffer.Add(0x00);
						buffer.Add(0x01);
						buffer.Add(aByte);
					}
					else
					{
						if (true == packetStarted && buffer.Count <= 14)
						{

							buffer.Add(aByte);

							switch (buffer.Count)
							{
								case 4:
									{
										latestReading = buffer[3] / 10d;
									}
									break;
								case 14:
									{
										buffer.Clear();
										packetStarted = false;
									}
									break;
							}
						}
						else
						{
							buffer.Add(aByte);
							packetStarted = false;
						}
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
