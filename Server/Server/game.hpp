#pragma once
#include <vector>
#include <map>
#include <net/server.hpp>
#include <net/packet.hpp>
#include "types.hpp"
#include "actor.hpp"

class Game {
public:
	Game( );

	void							logic( );

	void							playerJoin( const size_t playerID );
	void							playerLeave( const size_t playerID );
	void							handlePacket( const size_t playerID, const net::packet& pkt );

	const player_list&				getPlayers( ) { return playerList; };
	actor_ptr						getPlayerActor( const player_id& playerID );

private:
	void							sendToAll( const net::packet& pkt );

	actor_id						emptyActor( );
	actor_id						spawnActor( Actor* actor );

	player_list						playerList;
	actor_array						actorArray;
	std::map<player_id, actor_id>	playerActorMap;
};