using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading;

public class NetworkController : MonoBehaviour
{
	static NetworkController current;
	public static NetworkController Current { get { return current; } }

	Timer pingTimer = new Timer(0.4f);
	NetworkWorker worker;

	System.Diagnostics.Stopwatch pingWatch = new System.Diagnostics.Stopwatch();

	PacketManager packetManager;

	void Awake( )
	{
		current = this;
		packetManager = GetComponent<PacketManager>( );
	}

	void Start( )
	{
		//Application.targetFrameRate = 120;
		worker = new NetworkWorker( new IPEndPoint( IPAddress.Parse( "90.230.69.29" ), 15620 ) );
	}

	void OnDestroy( )
	{
		worker.Shutdown( );
	}

	public void SendToServer( BStream stream ) { SendToServer( stream.ToArray, (int)stream.Length ); }
	public void SendToServer( byte[] data, int size )
	{
		worker.Send( new Packet( data, size ) );
	}

	public void HandlePacket( Packet pkt )
	{
		BinaryReader reader = new BinaryReader(new MemoryStream(pkt.Data));

		Protocol p = (Protocol)reader.ReadInt16();

		switch (p)
		{
			case Protocol.Ping:
				{
					long ms = pingWatch.ElapsedMilliseconds;

					FindObjectOfType<UnityEngine.UI.Text>( ).text = "Ping: " + worker.Latency + " / " + pingWatch.ElapsedMilliseconds + " ms\n" +
						"FPS: " + (int)(1f / Time.deltaTime);
				}
				break;

			case Protocol.PlayerID:
				{
					Globals.MY_ID = reader.ReadInt16( );
					print( "My ID is " + Globals.MY_ID );
				}
				break;

			default:
				packetManager.HandlePacket( pkt );
				break;
		}
	}

	void Update( )
	{
		PollWorker( );

		pingTimer.Update( Time.deltaTime );
		if (pingTimer.Done)
		{
			pingTimer.Reset( );
			pingWatch.Reset( );
			pingWatch.Start( );

			BStream stream = new BStream();
			stream.Writer.Write( (short)Protocol.Ping );

			worker.Send( new Packet( stream.ToArray, (int)stream.Length ) );
		}
	}

	void PollWorker( )
	{
		Packet pkt = null;
		while ((pkt = worker.Receive( )) != null)
			HandlePacket( pkt );
	}
}