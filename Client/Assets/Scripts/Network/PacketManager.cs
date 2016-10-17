using System.IO;
using UnityEngine;

public class PacketManager : MonoBehaviour
{
	ActorManager actorManager;

	void Awake( )
	{
		actorManager = GetComponent<ActorManager>( );
	}

	public void HandlePacket( Packet pkt )
	{
		BinaryReader reader = new BinaryReader(new MemoryStream(pkt.Data));

		Protocol p = (Protocol)reader.ReadInt16();

		switch (p)
		{
			case Protocol.ActorJoin:
				{
					short id = reader.ReadInt16();
					ActorType type = (ActorType)reader.ReadInt16();
					actorManager.SpawnActor( id, type );

					print( "Player " + id + " joined!" );
				}
				break;

			case Protocol.PlayerPossess:
				{
					short id = reader.ReadInt16();

					if (actorManager[id] != null)
						(actorManager[id].Controller as PlayerController).Possess( );
				}
				break;

			case Protocol.ActorLeave:
				{
					short id = reader.ReadInt16();
					actorManager.DestroyActor( id );

					print( "Player " + id + " left!" );
				}
				break;

			case Protocol.ActorPosition:
				{
					short id = reader.ReadInt16();

					Vector3 pos = new Vector3(
						reader.ReadSingle(),
						reader.ReadSingle(),
						reader.ReadSingle());

					if (actorManager[id] != null)
						actorManager[id].ReceivePosition( pos );
				}
				break;

			case Protocol.ActorRotation:
				{
					short id = reader.ReadInt16();

					float rotation = reader.ReadSingle();

					if (actorManager[id] != null)
						actorManager[id].ReceiveRotation( rotation );
				}
				break;

			case Protocol.PlayerInput:
				{
					short id = reader.ReadInt16();

					byte input = reader.ReadByte();

					if (actorManager[id] != null)
						(actorManager[id].Controller as PlayerController).ReceiveInput( input );
				}
				break;
		}
	}
}