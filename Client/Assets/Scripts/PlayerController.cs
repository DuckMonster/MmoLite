using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	GameObject cameraSocketPrefab = null;

	int id;
	public int ID { get { return id; } }

	void Start()
	{

	}

	public void Init(int id, bool isLocal)
	{
		this.id = id;
		GetComponentInChildren<TextMesh>().text = id.ToString();

		if (isLocal)
		{
			gameObject.AddComponent<LocalController>();
			Instantiate(cameraSocketPrefab, transform);
		}
	}
}
