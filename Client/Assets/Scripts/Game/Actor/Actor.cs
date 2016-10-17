using UnityEngine;

public class Actor : MonoBehaviour
{
	const float POSITION_UPDATE_RANGE = 1f;
	const float ROTATION_UPDATE_RANGE = 40f;

	int id;
	public int ID { get { return id; } }

	ActorController controller;
	public ActorController Controller { get { return controller; } }

	float remoteRotation;
	Vector3 remotePosition;

	public float Rotation
	{
		get { return transform.rotation.eulerAngles.y; }
		set { transform.eulerAngles = new Vector3( 0f, value, 0f ); }
	}

	public float RemoteRotation { get { return remoteRotation; } }
	public Vector3 RemotePosition { get { return remotePosition; } }

	public Vector3 Forward
	{
		get
		{
			float rotation = Rotation * Mathf.Deg2Rad;
			return new Vector3( Mathf.Sin( rotation ), 0f, Mathf.Cos( rotation ) );
		}
	}
	public Vector3 Right
	{
		get
		{
			float rotation = Rotation * Mathf.Deg2Rad;
			return new Vector3( Mathf.Cos( rotation ), 0f, -Mathf.Sin( rotation ) );
		}
	}

	public void Init( int id, GameObject prefab )
	{
		this.id = id;
		GetComponentInChildren<TextMesh>( ).text = id.ToString( );

		if (prefab != null)
		{
			Instantiate( prefab, transform, false );
		}

		controller = GetComponentInChildren<ActorController>( );
	}

	public void ReceivePosition( Vector3 position )
	{
		transform.position = position;
		remotePosition = position;
	}
	public void ReceiveRotation( float rotation )
	{
		Rotation = rotation;
		remoteRotation = rotation;
	}
}