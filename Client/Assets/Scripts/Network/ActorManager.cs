using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
	[SerializeField]
	GameObject actorPrefab = null;

	[SerializeField]
	GameObject[] actorTypeList;
	Dictionary<string, GameObject> actorTypeDictionary = new Dictionary<string, GameObject>();

	Actor[] actorList = new Actor[1024];

	void Awake( )
	{
		// Insert each prefab into the dictionary
		foreach (GameObject o in actorTypeList)
			actorTypeDictionary[o.name] = o;
	}

	public Actor GetActor( short id )
	{
		if (id >= actorList.Length)
			return null;

		return actorList[id];
	}

	public Actor SpawnActor( short id, ActorType type )
	{
		string typeStr = "NPC";
		switch (type)
		{
			case ActorType.NPC: typeStr = "NPC"; break;
			case ActorType.Player: typeStr = "Player"; break;
		}

		actorList[id] = Instantiate( actorPrefab ).GetComponent<Actor>( );
		actorList[id].Init( id, actorTypeDictionary[typeStr] );

		return actorList[id];
	}

	public void DestroyActor( short id )
	{
		Actor a = GetActor(id);

		if (a)
			Destroy( a.gameObject );
	}

	public Actor this[short id]
	{
		get { return GetActor( id ); }
	}
}