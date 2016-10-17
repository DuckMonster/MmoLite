using UnityEngine;
using System.Collections;

public class ActorMovement : MonoBehaviour
{

	[SerializeField]
	float movementSpeed = 4f;
	[SerializeField]
	float maxClimbAngle = 45f;

	Actor actor;

	void Awake( )
	{
		actor = GetComponent<Actor>( );
	}

	bool FindGround( Vector3 position, Vector3 delta, out Vector3 ground, out float angle )
	{
		Ray ray = new Ray(transform.position + delta + new Vector3(0f, 10f, 0f), Vector3.down);
		RaycastHit hit;

		if (Physics.Raycast( ray, out hit, 100f, 1 << 8 ))
		{
			ground = hit.point;

			float distance = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(ground.x, ground.z));
			float height = ground.y - position.y;

			angle = Mathf.Atan( height / distance ) * Mathf.Rad2Deg;

			return true;
		}

		ground = position;
		angle = 0f;

		return false;
	}

	public void Turn( float delta )
	{
		actor.Rotation += delta;
	}

	public void Move( Vector2 movement )
	{
		Vector3 delta = (actor.Forward * movement.y + actor.Right * movement.x) * movementSpeed;

		Vector3 newPosition;
		float angle;

		FindGround( transform.position, delta, out newPosition, out angle );

		if (angle <= maxClimbAngle)
			transform.position = newPosition;
	}
}
