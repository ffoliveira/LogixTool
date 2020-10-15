using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LogixTool.EthernetIP
{
    /// <summary>
    /// ��������� ������ �� �������� ����������� � ��������� �����������.
    /// </summary>
	public class ForwardOpenResponse
	{
        #region [ PROPERTIES ]
        /* ============================================================================== */
        /// <summary>
        /// ���������� ��� �� ����� ��������.
        /// </summary>
        public bool? IsSuccessful { get; set; }
        /// <summary>
        /// ID ����������� Originator -> Target.
        /// </summary>
        public uint OtoTConnectionID { get; set; }
        /// <summary>
        /// ID ����������� Target -> Originator.
        /// </summary>
        public uint TtoOConnectionID { get; set; }
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
        /// ��� ���� ������������ � ������ ������ ������������� � ���������� ����� ���� � ������������ ���� ��������
        /// ������� ����� ������������� � ������������� ��� ������.
        /// </summary>
        public byte? RemainingPathSize { get; set; }
        /// <summary>
        /// ���������� ������ ������� �������� ������� (RPI) Originator -> Target.
        /// </summary>
        public uint OtoTActualPacketInterval { get; set; }
        /// <summary>
        /// ���������� ������ ������� �������� ������� (RPI) Target -> Originator.
        /// </summary>
        public uint TtoOActualPacketInterval { get; set; }
        /// <summary>
        /// ����������� ����� �� ����������.
        /// </summary>
        public List<byte> ApplicationReply { get; set; }
        /* ============================================================================== */
        #endregion

        /// <summary>
        /// ������� ��������� ������ �� �������� ����������� � ��������� �����������.
        /// ������������ ��� ������������� �������.
        /// </summary>
        public ForwardOpenResponse()
        {
            this.IsSuccessful = null;
            this.OtoTConnectionID = 0;
            this.TtoOConnectionID = 0;
            this.ConnectionSerialNumber = 0;
            this.OriginatorVendorID = 0;
            this.OriginatorSerialNumber = 0;
            this.RemainingPathSize = null;
            this.OtoTActualPacketInterval = 0;
            this.TtoOActualPacketInterval = 0;
            this.ApplicationReply = new List<byte>();
        }
        /// <summary>
        /// ��������� ������������������ ���� � ������ �� ���������� �� ������ ������������������.
        /// � ������ �������� ���������, ����� ��� ������ ���������� �������� null.
        /// </summary>
        /// <param name="responce">����� �� ���������� �������.</param>
        public static ForwardOpenResponse Parse(MessageRouterResponse responce)
        {
            ForwardOpenResponse forwardOpenResponse = new ForwardOpenResponse();

            if (responce == null || responce.ResponseData == null)
            {
                return null;
            }

            List<byte> bytes = responce.ResponseData;

            if (responce.GeneralStatus == 0)
            {
                if (bytes.Count >= 26)
                {
                    forwardOpenResponse.OtoTConnectionID = (uint)(bytes[0] | bytes[1] << 8 | bytes[2] << 16 | bytes[3] << 24);
                    forwardOpenResponse.TtoOConnectionID = (uint)(bytes[4] | bytes[5] << 8 | bytes[6] << 16 | bytes[7] << 24);
                    forwardOpenResponse.ConnectionSerialNumber = (ushort)(bytes[8] | bytes[9] << 8);
                    forwardOpenResponse.OriginatorVendorID = (ushort)(bytes[10] | bytes[11] << 8);
                    forwardOpenResponse.OriginatorSerialNumber = (uint)(bytes[12] | bytes[13] << 8 | bytes[14] << 16 | bytes[15] << 24);
                    forwardOpenResponse.OtoTActualPacketInterval = (uint)(bytes[16] | bytes[17] << 8 | bytes[18] << 16 | bytes[19] << 24);
                    forwardOpenResponse.TtoOActualPacketInterval = (uint)(bytes[20] | bytes[21] << 8 | bytes[22] << 16 | bytes[23] << 24);
                    int applicationReplySize = bytes[24];

                    for (int ix = 0; ix < applicationReplySize; ix += 2)
                    {
                        int index = ix + 26;
                        if (index >= bytes.Count)
                        {
                            return null;
                        }
                        forwardOpenResponse.ApplicationReply.Add(bytes[index]);
                        forwardOpenResponse.ApplicationReply.Add(bytes[index + 1]);
                    }
                    forwardOpenResponse.IsSuccessful = true;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (bytes.Count == 10)
                {
                    forwardOpenResponse.ConnectionSerialNumber = (ushort)(bytes[0] | bytes[1] << 8);
                    forwardOpenResponse.OriginatorVendorID = (ushort)(bytes[2] | bytes[3] << 8);
                    forwardOpenResponse.OriginatorSerialNumber = (uint)(bytes[4] | bytes[5] << 8 | bytes[6] << 16 | bytes[7] << 24);
                    forwardOpenResponse.RemainingPathSize = bytes[8];
                    forwardOpenResponse.IsSuccessful = false;
                }
                else
                {
                    return null;
                }
            }

            return forwardOpenResponse;
        }

        /// <summary>
        /// ��������������� ������ ������ � ����� ����.
        /// </summary>
        /// <returns></returns>
		public byte[] ToBytes()
		{
			List<byte> result = new List<byte>();

            // Connection ID: O to T.
            result.AddRange(BitConverter.GetBytes(this.OtoTConnectionID));
            // Connection ID: T to O.
            result.AddRange(BitConverter.GetBytes(this.TtoOConnectionID));
            // Connection Serial Number.
            result.AddRange(BitConverter.GetBytes(this.ConnectionSerialNumber));
            // Originator Vendor ID.
            result.AddRange(BitConverter.GetBytes(this.OriginatorVendorID));
            // Originator Serial Number.
            result.AddRange(BitConverter.GetBytes(this.OriginatorSerialNumber));
            // Requested Packet Rate O->T in Microseconds
            result.AddRange(BitConverter.GetBytes(this.OtoTActualPacketInterval));
            // Requested Packet Rate T->O in Microseconds
            result.AddRange(BitConverter.GetBytes(this.TtoOActualPacketInterval));
            // Number of 16 bit words in the Application Reply field.
            result.Add((byte)ApplicationReply.Count);
            // Reserved
            result.Add(0);
            // Application specific data
            foreach (byte appReply in this.ApplicationReply)
            {
                result.Add(appReply);
            }
			return result.ToArray();
		}
	}
}
