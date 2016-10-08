using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System;

class NetworkWorker
{
	Socket socket;

	bool connected = false;
	bool Connected { get { return connected; } }

	Thread outThread, inThread;

	Queue<Packet> inQueue = new Queue<Packet>();
	Queue<Packet>[] outQueues = { new Queue<Packet>(), new Queue<Packet>() };
	int outQueueIndex = 0;

	Queue<Packet> FrontOutQueue { get { return outQueues[outQueueIndex % outQueues.Length]; } }
	void SwapOutQueue( ) { outQueueIndex++; }

	public NetworkWorker( IPEndPoint endpoint )
	{
		socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
		socket.Connect( endpoint );

		connected = true;

		outThread = new Thread( OutThread );
		inThread = new Thread( InThread );

		outThread.Start( );
		inThread.Start( );
	}

	public void Disconnect( )
	{
		if (!connected)
			return;

		socket.Shutdown( SocketShutdown.Both );
		socket.Close( );

		connected = false;
	}

	public void Shutdown( )
	{
		Disconnect( );

		outThread.Join( );
		inThread.Join( );
	}

	public Packet Receive( )
	{
		Packet pkt = null;

		// Try to lock
		if (Monitor.TryEnter( inQueue ))
		{
			if (inQueue.Count > 0)
				pkt = inQueue.Dequeue( );

			Monitor.Exit( inQueue );
		}

		return pkt;
	}

	public void Send( Packet pkt )
	{
		lock (FrontOutQueue)
		{
			FrontOutQueue.Enqueue( pkt );
		}
	}

	void OutThread( )
	{
		while (Connected)
		{
			Queue<Packet> pktQueue = null;

			// See if the front queue has data in it
			lock (FrontOutQueue)
			{
				if (FrontOutQueue.Count > 0)
				{
					pktQueue = FrontOutQueue;
					SwapOutQueue( );
				}
			}

			if (pktQueue != null)
			{
				while (pktQueue.Count > 0)
				{
					Packet pkt = pktQueue.Dequeue();
					socket.Send( pkt.Data, pkt.Size, SocketFlags.None );
				}
			}

			Thread.Sleep( 1 );
		}
	}

	void InThread( )
	{
		byte[] sizeBuffer = new byte[2];
		byte[] readBuffer = new byte[1024];

		while (Connected)
		{
			int bytesRead = 0;

			// Read a 2 byte size header
			do
			{
				bytesRead += socket.Receive( sizeBuffer );
			} while (bytesRead != 2 && bytesRead != 0);

			short packetSize = BitConverter.ToInt16(sizeBuffer, 0);

			// Read actual data
			bytesRead = 0;
			do
			{
				bytesRead += socket.Receive( readBuffer, packetSize, SocketFlags.None );
			} while (bytesRead != packetSize && bytesRead != 0);

			// Error handling
			if (bytesRead == 0)
				Disconnect( );

			Packet pkt = new Packet(readBuffer, packetSize);

			lock (inQueue)
			{
				inQueue.Enqueue( pkt );
				Debug.Log( "Received " + packetSize );
			}
		}
	}
}