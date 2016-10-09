using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{

	const float POSITION_UPDATE_RANGE = 0f;
	const float ROTATION_UPDATE_RANGE = 0f;

	[SerializeField]
	GameObject cameraSocketPrefab = null;

	NetworkController netController = null;
	public NetworkController NetController
	{
		get { return netController; }
	}

	PlayerInput pinput = new PlayerInput();
	public PlayerInput PInput
	{
		get { return pinput; }
		set { pinput = value; }
	}

	int id;
	public int ID { get { return id; } }
	public bool IsLocal { get { return id == Globals.MY_ID; } }

	float movementSpeed = 4f;
	float rotateSpeed = 90f;

	Vector3 serverPositionOffset = Vector3.zero;
	float serverRotationOffset = 0f;

	void Start( )
	{
	}

	public void Init( int id, NetworkController controller )
	{
		this.id = id;
		this.netController = controller;
		GetComponentInChildren<TextMesh>( ).text = id.ToString( );
	}

	public void Possess( )
	{
		gameObject.AddComponent<LocalController>( );
		Instantiate( cameraSocketPrefab, transform, false );
	}

	public void ReceivePosition( Vector3 position )
	{
		serverPositionOffset = position - transform.position;
	}
	public void ReceiveRotation( float rotation )
	{
		serverRotationOffset = Mathf.DeltaAngle( transform.eulerAngles.y, rotation );
	}
	public void ReceiveInput( byte input )
	{
		PlayerInput newInput = new PlayerInput(input);
		if (newInput.Forward > 0f)
			GetComponentInChildren<Renderer>( ).material.color = Color.red;
		else
			GetComponentInChildren<Renderer>( ).material.color = Color.white;

		if (!IsLocal)
			pinput.Decode( input );
	}

	void Update( )
	{
		Vector3 posDelta = serverPositionOffset * 15f * Time.deltaTime;
		float angleDelta = serverRotationOffset * 15f * Time.deltaTime;

		transform.position = transform.position + posDelta;
		transform.eulerAngles = transform.eulerAngles + new Vector3( 0f, angleDelta, 0f );

		serverPositionOffset -= posDelta;
		serverRotationOffset -= angleDelta;
	}

	void FixedUpdate( )
	{
		Vector2 movement = new Vector2(
			pinput.Strafe,
			pinput.Forward
			);

		if (movement.magnitude > 0.1f)
		{
			movement.Normalize( );
			transform.Translate( (Vector3.forward * movement.y + Vector3.right * movement.x) * movementSpeed * Time.fixedDeltaTime );
			FindGround();
		}

		if (Mathf.Abs( pinput.Turn ) > 0.1f)
			transform.Rotate( new Vector3( 0f, pinput.Turn, 0f ) * rotateSpeed * Time.fixedDeltaTime );
	}

	void FindGround( )
	{
		Ray ray = new Ray(transform.position + new Vector3(0f, 10f, 0f), Vector3.down);
		RaycastHit hit;

		if (Physics.Raycast( ray, out hit, 100f, 1 << 8 ))
		{
			print( hit.collider.name );
			transform.position = hit.point;
		}
	}
}
