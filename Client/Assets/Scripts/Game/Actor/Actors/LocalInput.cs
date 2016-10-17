using UnityEngine;
using System.Collections;
using System.IO;

public class LocalInput : MonoBehaviour
{

	PlayerInput prevInput = new PlayerInput();
	PlayerController playerController;

	public void Init( PlayerController controller )
	{
		playerController = controller;
	}

	public void SendPosition( )
	{
		BStream str = new BStream();

		str.Writer.Write( (short)Protocol.ActorPosition );
		str.Writer.Write( transform.position.x );
		str.Writer.Write( transform.position.y );
		str.Writer.Write( transform.position.z );

		NetworkController.Current.SendToServer( str );
	}

	public void SendRotation( )
	{
		BStream str = new BStream();

		str.Writer.Write( (short)Protocol.ActorRotation );
		str.Writer.Write( transform.eulerAngles.y );

		NetworkController.Current.SendToServer( str );
	}

	public void SendInput( PlayerInput input )
	{
		SendPosition( );
		SendRotation( );

		BStream str = new BStream();

		str.Writer.Write( (short)Protocol.PlayerInput );
		str.Writer.Write( input.Encode( ) );

		NetworkController.Current.SendToServer( str );
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

			playerController.Movement.Turn( horizontal );
			if (Mathf.Abs( Mathf.DeltaAngle( playerController.Actor.Rotation, playerController.Actor.RemoteRotation ) ) > 2f)
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
			playerController.PlayerInput = input;
		}

		if (Input.GetKeyDown( KeyCode.Space ))
			Debug.Log( "Hello!" );

		prevInput = input;
	}
}
