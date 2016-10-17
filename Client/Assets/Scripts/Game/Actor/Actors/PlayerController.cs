using UnityEngine;
using System.Collections;

public class PlayerController : ActorController
{

	[SerializeField]
	GameObject cameraSocketPrefab = null;

	PlayerInput playerInput = new PlayerInput();
	public PlayerInput PlayerInput { get { return playerInput; } set { playerInput = value; } }

	[SerializeField]
	float turnSpeed = 50f;

	public void Possess( )
	{
		gameObject.AddComponent<LocalInput>( ).Init( this );
		Instantiate( cameraSocketPrefab, transform, false );
	}

	public void ReceiveInput( byte input )
	{
		playerInput.Decode( input );
	}

	void FixedUpdate( )
	{
		Vector2 movement = new Vector2(
			PlayerInput.Strafe,
			PlayerInput.Forward
			).normalized;

		if (movement.magnitude > 0.1f)
			Movement.Move( movement * Time.fixedDeltaTime );

		if (Mathf.Abs( PlayerInput.Turn ) > 0.1f)
			Movement.Turn( turnSpeed * playerInput.Turn * Time.fixedDeltaTime );
	}
}
