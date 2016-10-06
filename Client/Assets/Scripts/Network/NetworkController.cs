using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;

public class NetworkController : MonoBehaviour
{

	TcpClient client;
	Timer tempTimer = new Timer(1f);
	NetworkWorker worker;

	void Start( )
	{
		client = new TcpClient( );

		print( "Connecting..." );
		client.Connect( new IPEndPoint( IPAddress.Parse( "127.0.0.1" ), 12345 ) );
		print( "Connected!" );

		worker = new NetworkWorker( client.GetStream( ) );
	}

	void OnDestroy( )
	{
		client.Close( );
		worker.Shutdown( );
	}

	void Update( )
	{
	}
}