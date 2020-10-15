using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogixTool.EthernetIP
{
    /// <summary>
    /// CIP ������.
    /// </summary>
	public class MessageRouterRequest
	{
        /// <summary>
        /// ����� �������������� ���������� ����.
        /// </summary>
        public byte ServiceCode { get; set; }
        /// <summary>
        /// ������������� ����.
        /// </summary>
        public EPath RequestPath { get; set; }
        /// <summary>
        /// ������������ ������.
        /// </summary>
        public List<byte> RequestData { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public MessageRouterRequest()
		{
            this.ServiceCode = 0;
            this.RequestPath = new EPath();
            this.RequestData = new List<byte>();
		}

        /// <summary>
        /// ��������������� ������ ������ � ����� ����.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            List<byte> list = new List<byte>();
            list.Add(this.ServiceCode);

            if (this.RequestPath != null)
            {
                list.AddRange(this.RequestPath.ToBytes(EPathToByteMethod.Complete));
            }

            if (this.RequestData != null)
            {
                list.AddRange(this.RequestData);
            }
            return list.ToArray();
        }
	}
}
