using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour
{
	Actor actor;
	public Actor Actor { get { return actor; } }

	ActorMovement movement;
	public ActorMovement Movement { get { return movement; } }

	void Awake( )
	{
		actor = FindComponent<Actor>( );
		movement = FindComponent<ActorMovement>( );
	}

	T FindComponent<T>( ) where T : class
	{
		T comp = null;
		Transform t = transform;

		do
		{
			comp = t.GetComponent<T>( );
			t = t.parent;
		} while (comp == null && t != null);

		return comp;
	}
}
