using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	void Start()
	{

	}

	void Update()
	{
		Vector2 movementInput = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical"));

		transform.Translate(new Vector3(movementInput.x, 0f, movementInput.y) * Time.deltaTime);
	}
}
