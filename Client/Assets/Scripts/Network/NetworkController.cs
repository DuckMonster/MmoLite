using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Generic;

public class NetworkController : MonoBehaviour
{
	[SerializeField]
	GameObject playerPrefab = null;
	[SerializeField]
	Transform playerSpawn;

	Timer tempTimer = new Timer(1f);
	NetworkWorker worker;

	Dictionary<int, PlayerController> playerList = new Dictionary<int, PlayerController>();

	System.Diagnostics.Stopwatch pingWatch = new System.Diagnostics.Stopwatch();

	void Start( )
	{
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
					print( "Ping: " + pingWatch.ElapsedMilliseconds );
				}
				break;

			case Protocol.PlayerID:
				{
					Globals.MY_ID = reader.ReadInt16( );
					print( "My ID is " + Globals.MY_ID );
				}
				break;

			case Protocol.PlayerJoin:
				{
					int id = reader.ReadInt16();

					print( "Player " + id + " joined!" );

					GameObject newPlayer = (GameObject)Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);

					playerList[id] = newPlayer.GetComponent<PlayerController>( );
					playerList[id].Init( id, this );
				}
				break;

			case Protocol.PlayerPossess:
				{
					int id = reader.ReadInt16();

					if (playerList.ContainsKey( id ))
						playerList[id].Possess( );
				}
				break;

			case Protocol.PlayerLeave:
				{
					int id = reader.ReadInt16();

					print( "Player " + id + " left!" );

					if (playerList.ContainsKey( id ))
						Destroy( playerList[id].gameObject );
				}
				break;

			case Protocol.PlayerPosition:
				{
					int id = reader.ReadInt16();

					Vector3 pos = new Vector3(
						reader.ReadSingle(),
						reader.ReadSingle(),
						reader.ReadSingle());

					if (playerList.ContainsKey( id ))
						playerList[id].ReceivePosition( pos );
				}
				break;

			case Protocol.PlayerRotation:
				{
					int id = reader.ReadInt16();

					float rotation = reader.ReadSingle();

					if (playerList.ContainsKey( id ))
						playerList[id].ReceiveRotation( rotation );
				}
				break;

			case Protocol.PlayerInput:
				{
					int id = reader.ReadInt16();

					byte input = reader.ReadByte();

					if (playerList.ContainsKey( id ))
						playerList[id].ReceiveInput( input );
				}
				break;
		}
	}

	void Update( )
	{
		PollWorker( );
		tempTimer.Update( Time.deltaTime );

		if (tempTimer.Done)
		{
			BStream stream = new BStream();
			stream.Writer.Write( (short)Protocol.Ping );

			worker.Send( new Packet( stream.ToArray, (int)stream.Length ) );

			tempTimer.Reset( );

			pingWatch.Reset( );
			pingWatch.Start( );
		}
	}

	void PollWorker( )
	{
		Packet pkt = null;
		while ((pkt = worker.Receive( )) != null)
			HandlePacket( pkt );
	}
}