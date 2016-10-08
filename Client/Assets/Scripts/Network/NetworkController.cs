using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class NetworkController : MonoBehaviour
{
	[SerializeField]
	GameObject playerPrefab = null;

	Timer tempTimer = new Timer(1f);
	NetworkWorker worker;

	void Start( )
	{
		worker = new NetworkWorker( new IPEndPoint( IPAddress.Parse( "127.0.0.1" ), 12345 ) );
	}

	void OnDestroy( )
	{
		worker.Shutdown( );
	}

	public void HandlePacket( Packet pkt )
	{
		BinaryReader reader = new BinaryReader(new MemoryStream(pkt.Data));

		Protocol p = (Protocol)reader.ReadInt16();

		switch (p)
		{
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

					GameObject newPlayer = (GameObject)Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
					newPlayer.GetComponent<PlayerController>( ).Init( id, id == Globals.MY_ID );
				}
				break;

			case Protocol.PlayerLeave:
				{
					int id = reader.ReadInt16();

					print( "Player " + id + " left!" );

					var players = FindObjectsOfType<PlayerController>();
					foreach (PlayerController c in players)
					{
						if (c.ID == id)
							Destroy( c.gameObject );
					}
				}
				break;
		}
	}

	void Update( )
	{
		PollWorker( );
	}

	void PollWorker( )
	{
		Packet pkt = null;
		while ((pkt = worker.Receive( )) != null)
			HandlePacket( pkt );
	}
}