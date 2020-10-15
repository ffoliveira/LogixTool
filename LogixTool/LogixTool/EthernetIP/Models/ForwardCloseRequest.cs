using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LogixTool.EthernetIP
{
    /// <summary>
    /// ��������� ������� �� �������� ����������� � ��������� �����������.
    /// </summary>
	public class ForwardCloseRequest
    {
        #region [ PROPERTIES ]
        /* ============================================================================== */
        /// <summary>
        /// 
        /// </summary>
        public byte PriorityTimeTick { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte TimeOutTicks { get; set; }
        /// <summary>
        /// �������� ����� �����������.
        /// </summary>
        public ushort ConnectionSerialNumber { get; set; }
        /// <summary>
        /// Originator Vendor ID.
        /// </summary>
        public ushort OriginatorVendorID { get; set; }
        /// <summary>
        /// �������� ����� Originator-�.
        /// </summary>
        public uint OriginatorSerialNumber { get; set; }
        /// <summary>
        /// ���� �� �������������� �������.
        /// </summary>
        public EPath ConnectionPath { get; set; }
        /* ============================================================================== */
        #endregion

        /// <summary>
        /// ������� ��������� ������� �� �������� ����������� � ��������� �����������.
        /// </summary>
        public ForwardCloseRequest()
        {
            this.PriorityTimeTick = 0;
            this.TimeOutTicks = 0;
            this.ConnectionSerialNumber = 0;
            this.OriginatorVendorID = 0;
            this.OriginatorSerialNumber = 0;
            this.ConnectionPath = new EPath();
        }

        /// <summary>
        /// ��������������� ������ ������ � ����� ����.
        /// </summary>
        /// <returns></returns>
		public byte[] ToBytes()
		{
			List<byte> result = new List<byte>();

            // Priority/Time_tick
            result.Add(this.PriorityTimeTick);
            // Time-out_ticks
            result.Add(this.TimeOutTicks);
            // Connection Serial Number.
            result.AddRange(BitConverter.GetBytes(this.ConnectionSerialNumber));
            // Originator Vendor ID.
            result.AddRange(BitConverter.GetBytes(this.OriginatorVendorID));
            // Originator Serial Number.
            result.AddRange(BitConverter.GetBytes(this.OriginatorSerialNumber));
            // The number of 16 bit words in the Connection_Path field.
            result.Add(this.ConnectionPath.Size);
            // Reserved.
            result.Add(0);
            // Indicates the route to the Remote Target Device.
            result.AddRange(this.ConnectionPath.ToBytes(EPathToByteMethod.DataOnly));
			return result.ToArray();
		}
	}
}
