using UnityEngine;
using System.Net.Sockets;
using System.Threading;

class NetworkWorker
{
	NetworkStream stream;
	bool Connected { get { return true; } }

	Thread outThread, inThread;

	public NetworkWorker( NetworkStream stream )
	{
		this.stream = stream;

		outThread = new Thread( OutThread );
		inThread = new Thread( InThread );

		outThread.Start( );
		inThread.Start( );
	}

	public void Shutdown( )
	{
		stream.Close( );

		outThread.Join( );
		inThread.Join( );
	}

	object lockobj = new object();

	void OutThread( )
	{
		while (Connected)
		{
			Thread.Sleep( 1000 );
			stream.Write( new byte[] { 0, 1, 2, 3, 4 }, 0, 5 );

			lock (lockobj)
			{
				Debug.Log( "Sent 5 bytes!" );
			}
		}
	}

	void InThread( )
	{
		byte[] readBuffer = new byte[1024];

		while (Connected)
		{
			int bytesReceived = stream.Read(readBuffer, 0, 1024);

			lock (lockobj)
			{
				Debug.Log( "Received " + bytesReceived + " bytes!" );
			}
		}
	}
}