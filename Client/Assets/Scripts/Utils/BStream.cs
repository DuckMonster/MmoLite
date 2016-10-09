using System;
using System.IO;

public class BStream
{
	byte[] data = new byte[1024];
	MemoryStream stream;
	BinaryReader reader;
	BinaryWriter writer;

	public BinaryReader Reader { get { return reader; } }
	public BinaryWriter Writer { get { return writer; } }

	public long Length { get { return stream.Position; } }
	public byte[] ToArray
	{
		get
		{
			byte[] arr = new byte[Length];
			Array.Copy( data, arr, Length );

			return arr;
		}
	}

	public BStream( byte[] data, int length )
	{
		this.data = new byte[length];
		Array.Copy( data, this.data, length );

		stream = new MemoryStream( data );

		reader = new BinaryReader( stream );
		writer = new BinaryWriter( stream );
	}
	public BStream( )
	{
		stream = new MemoryStream( data );

		reader = new BinaryReader( stream );
		writer = new BinaryWriter( stream );
	}
}