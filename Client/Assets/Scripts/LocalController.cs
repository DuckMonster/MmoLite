using UnityEngine;
using System.Collections;

public class LocalController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void Update( )
	{
		Vector2 movementInput = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical"));

		transform.Translate( new Vector3( movementInput.x, 0f, movementInput.y ) * Time.deltaTime );
	}
}
