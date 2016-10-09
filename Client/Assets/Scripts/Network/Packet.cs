using System;

public class Packet
{
	byte[] data;
	byte[] header;
	public byte[] Data { get { return data; } }
	public byte[] HeaderData
	{
		get
		{
			byte[] hdrData = new byte[Size + 2];
			Array.Copy(header, hdrData, 2);
			Array.Copy(data, 0, hdrData, 2, data.Length);

			return hdrData;
		}
	}

	int size;
	public int Size { get { return size; } }

	public Packet( byte[] data, int size )
	{
		this.size = size;
		this.data = new byte[size];

		Array.Copy( data, this.data, size );
		header = BitConverter.GetBytes( (short)size );
	}
}