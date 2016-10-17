#include "actor.hpp"

#include "game.hpp"
#include <bstream.hpp>
#include <iostream>

#include "protocol.hpp"
#include "globals.hpp"
#include "playercontroller.hpp"
#include "npccontroller.hpp"

Actor::Actor( ActorType type, Game * game ) :
	game( game ),
	id( -1 ),
	type( type ),

	position( glm::vec3( 23.02f, 13.06f, 32.18f ) ),
	rotation( 0.f ) {

	// Create controller
	switch (type) {
		case ActorType::Player:
			controller = std::shared_ptr<ActorController>( new PlayerController( this ) );
			break;

		case ActorType::NPC:
			controller = std::shared_ptr<ActorController>( new NpcController( this ) );
			break;
	}
}

Actor::~Actor( ) {
}

void Actor::logic( ) {
	getController( )->logic( );
}

void Actor::setPosition( const glm::vec3 & position ) {
	this->position = position;
	send( protocol::ActorPosition );
}

void Actor::setRotation( const float rotation ) {
	this->rotation = rotation;
	send( protocol::ActorRotation );
}

void Actor::send( protocol::MmoProtocol msg ) {
	doSend( msg, getGame( )->getPlayers( ) );
}
void Actor::send( protocol::MmoProtocol msg, const player_id& player ) {
	doSend( msg, player );
}
void Actor::send( protocol::MmoProtocol msg, const player_list& players ) {
	doSend( msg, players );
}


// PACKETS
net::packet Actor::existancePacket( ) {
	bstream str;

	str.write<short>( protocol::ActorJoin );
	str.write<short>( id );
	str.write<short>( type );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet Actor::leavePacket( ) {
	bstream str;

	str.write<short>( protocol::ActorLeave );
	str.write<short>( id );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet Actor::positionPacket( ) {
	bstream str;

	str.write<short>( protocol::ActorPosition );
	str.write<short>( id );

	str.write<float>( position.x );
	str.write<float>( position.y );
	str.write<float>( position.z );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet Actor::rotationPacket( ) {
	bstream str;

	str.write<short>( protocol::ActorRotation );
	str.write<short>( id );

	str.write<float>( rotation );

	return net::packet( str.begin( ), str.size( ) );
}