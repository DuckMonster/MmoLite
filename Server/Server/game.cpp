#include "game.hpp"

#include <bstream.hpp>
#include <iostream>

#include "protocol.hpp"
#include "globals.hpp"

#include "playercontroller.hpp"
#include "npccontroller.hpp"

Game::Game( ) :
	playerList( ),
	actorArray( ),
	playerActorMap( ) {

	spawnActor( new Actor( ActorType::NPC, this ) );
}

void Game::logic( ) {
	for (actor_ptr& a : actorArray)
		if (a) a->logic( );
}

void Game::playerJoin( const size_t playerID ) {
	playerList.push_back( playerID );

	// Send player ID
	{
		bstream str;

		str.write<short>( protocol::PlayerID );
		str.write<short>( playerID );

		network::sendTo( net::packet( str.begin( ), str.size( ) ), playerID );
	}

	// Send all actors to the new player
	for (actor_ptr& a : actorArray)
		if (a)
			a->send( protocol::ActorJoin, playerID );

	// Spawn new actor
	actor_id newActor = spawnActor( new Actor( ActorType::Player, this ) );
	playerActorMap[playerID] = newActor;

	actorArray[newActor]->getController<PlayerController>( )->sendPossess( playerID );

	std::cout << playerID << " connected!\n";
}

void Game::playerLeave( const size_t playerID ) {
	std::vector<size_t>::iterator it = find( playerList.begin( ), playerList.end( ), playerID );

	if (it != playerList.end( ))
		playerList.erase( it );

	getPlayerActor( playerID )->send( protocol::ActorLeave );
	actorArray[playerActorMap[playerID]] = actor_ptr( );

	std::cout << playerID << " disconnected!\n";

}

void Game::handlePacket( const size_t playerID, const net::packet & pkt ) {
	bstream str( &pkt, pkt.size( ) );
	protocol::MmoProtocol protocol = (protocol::MmoProtocol)str.read<short>( );

	switch (protocol) {
		case protocol::Ping: {
			network::sendTo( pkt, playerID );
		} break;

		case protocol::ActorPosition: {
			float x = str.read<float>( ),
				y = str.read<float>( ),
				z = str.read<float>( );

			getPlayerActor( playerID )->setPosition( glm::vec3( x, y, z ) );
		} break;

		case protocol::ActorRotation: {
			float rotation = str.read<float>( );

			getPlayerActor( playerID )->setRotation( rotation );
		} break;

		case protocol::PlayerInput: {
			char input = str.read<char>( );

			getPlayerActor( playerID )->getController<PlayerController>( )->setInput( input );
		} break;
	}
}

actor_ptr Game::getPlayerActor( const player_id & playerID ) {
	std::map<player_id, actor_id>::iterator it = playerActorMap.find( playerID );

	if (it != playerActorMap.end( ))
		return actorArray[it->second];
	else
		return actor_ptr( );
}

void Game::sendToAll( const net::packet & pkt ) {
	for (size_t& id : playerList) {
		network::server->send( pkt, id );
	}
}

actor_id Game::emptyActor( ) {
	for (actor_id i = 0; i < actorArray.size( ); i++)
		if (!actorArray[i])
			return i;

	return -1;
}

actor_id Game::spawnActor( Actor* actor ) {
	actor_id id = emptyActor( );

	if (id != -1) {
		actorArray[id] = actor_ptr( actor );

		actor->setID( id );
		actor->send( protocol::ActorJoin );
	}

	return id;
}
