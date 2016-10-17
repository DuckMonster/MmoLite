using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System;
using System.Diagnostics;
using System.Linq;

class NetworkWorker
{
	Socket socket;

	bool connected = false;
	public bool Connected { get { return connected; } }

	Thread outThread, inThread;

	Queue<Packet> inQueue = new Queue<Packet>();
	Queue<Packet> outQueue = new Queue<Packet>();

	Stopwatch pingWatch;
	long latency = -1;
	public long Latency { get { return latency; } }

	public NetworkWorker( IPEndPoint endpoint )
	{
		socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
		socket.Connect( endpoint );
		socket.NoDelay = true;

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

		Application.Quit( );
	}

	public void Shutdown( )
	{
		Disconnect( );

		lock (outQueue)
		{
			lock (inQueue)
			{
				Monitor.PulseAll( outQueue );
			}
		}

		outThread.Join( );
		inThread.Join( );
	}

	public Packet Receive( )
	{
		Packet pkt = null;

		// Try to lock
		lock (inQueue)
		{
			if (inQueue.Count > 0)
				pkt = inQueue.Dequeue( );
		}

		return pkt;
	}

	public void Send( Packet pkt )
	{
		lock (outQueue)
		{
			outQueue.Enqueue( pkt );
			Monitor.PulseAll( outQueue );
		}
	}

	void OutThread( )
	{
		while (Connected)
		{
			lock (outQueue)
			{
				while (outQueue.Count > 0)
				{
					Packet pkt = outQueue.Dequeue();

					byte[] headerData = BitConverter.GetBytes((short)pkt.Size);

					// Intercept ping protocol
					if (PacketIsPing( pkt ))
						pingWatch = Stopwatch.StartNew( );

					// Header
					socket.Send( headerData );

					// Send data
					socket.Send( pkt.Data );

					//UnityEngine.Debug.Log("Sent in " + lockWatch.ElapsedMilliseconds);
				}

				Monitor.Wait( outQueue );
			}
		}
	}

	void InThread( )
	{
		byte[] sizeBuffer = new byte[2];
		byte[] readBuffer = new byte[1024];

		NetworkStream stream = new NetworkStream(socket);

		while (Connected)
		{
			int bytesRead = 0;

			// Read a 2 byte size header
			do
			{
				bytesRead += socket.Receive( sizeBuffer, 2 - bytesRead, SocketFlags.None );
			} while (bytesRead != 2 && bytesRead != 0);

			short packetSize = BitConverter.ToInt16(sizeBuffer, 0);

			// Read actual data
			bytesRead = 0;
			do
			{
				bytesRead += socket.Receive( readBuffer, packetSize - bytesRead, SocketFlags.None );
			} while (bytesRead != packetSize && bytesRead != 0);

			// Error handling
			if (bytesRead == 0)
				Disconnect( );

			Packet pkt = new Packet(readBuffer, packetSize);

			// Received ping
			if (PacketIsPing( pkt ) && pingWatch != null)
				latency = pingWatch.ElapsedMilliseconds;

			lock (inQueue)
			{
				inQueue.Enqueue( pkt );
			}
		}
	}

	bool PacketIsPing( Packet pkt )
	{
		return pkt.Size == 2 && Enumerable.SequenceEqual( pkt.Data, BitConverter.GetBytes( (short)Protocol.Ping ) );
	}
}