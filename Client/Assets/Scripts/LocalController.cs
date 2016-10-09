using UnityEngine;
using System.Collections;
using System.IO;

public class LocalController : MonoBehaviour
{

	PlayerController Player { get { return GetComponent<PlayerController>( ); } }
	PlayerInput prevInput = new PlayerInput();

	// Use this for initialization
	void Start( )
	{

	}

	public void SendPosition( )
	{
		BStream str = new BStream();

		str.Writer.Write( (short)Protocol.PlayerPosition );
		str.Writer.Write( transform.position.x );
		str.Writer.Write( transform.position.y );
		str.Writer.Write( transform.position.z );

		Player.NetController.SendToServer( str );
	}

	public void SendRotation( )
	{
		BStream str = new BStream();

		str.Writer.Write( (short)Protocol.PlayerRotation );
		str.Writer.Write( transform.eulerAngles.y );

		Player.NetController.SendToServer( str );
	}

	public void SendInput( PlayerInput input )
	{
		SendPosition( );
		SendRotation( );

		BStream str = new BStream();

		str.Writer.Write( (short)Protocol.PlayerInput );
		str.Writer.Write( input.Encode( ) );

		Player.NetController.SendToServer( str );
	}

	void Update( )
	{
		PlayerInput input = new PlayerInput();

		input.Forward = Input.GetAxisRaw( "Vertical" );


		// Mouse turning
		if (Input.GetMouseButton( 1 ))
		{
			Cursor.lockState = CursorLockMode.Locked;

			input.Strafe = Input.GetAxisRaw( "Horizontal" ) + Input.GetAxisRaw( "Look" );

			float horizontal = Input.GetAxisRaw("Mouse X");

			transform.Rotate( new Vector3( 0f, horizontal, 0f ) );
			SendRotation( );
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;

			input.Strafe = Input.GetAxisRaw( "Horizontal" );
			input.Turn = Input.GetAxisRaw( "Look" );
		}

		if (input != prevInput)
		{
			SendInput( input );
			Player.PInput = input;
		}

		if (Input.GetKeyDown( KeyCode.Space ))
			Debug.Log( "Hello!" );

		prevInput = input;
	}
}
