using System;

public class Packet
{
	byte[] data;
	public byte[] Data { get { return data; } }

	int size;
	public int Size { get { return size; } }

	public Packet( byte[] data, int size )
	{
		this.size = size;
		this.data = new byte[size];

		Array.Copy( data, this.data, size );
	}
}