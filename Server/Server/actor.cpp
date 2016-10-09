#include "actor.hpp"

#include "game.hpp"
#include <bstream.hpp>
#include <iostream>

#include "protocol.hpp"
#include "globals.hpp"

Actor::Actor( Game * game ) :
	game( game ),
	id( -1 ),

	position( glm::vec3( 23.02f, 13.06f, 32.18f ) ),
	rotation( 0.f ),

	input( ) {
}

void Actor::setPosition( const glm::vec3 & position ) {
	this->position = position;
	sendPosition( game->getPlayers( ) );
}

void Actor::setRotation( const float rotation ) {
	this->rotation = rotation;
	sendRotation( game->getPlayers( ) );
}

void Actor::setInput( const PlayerInput & input ) {
	this->input = input;
	sendInput( game->getPlayers( ) );
}

void Actor::sendExistance( const player_id & player ) {
	network::sendTo( existancePacket( ), player );
	sendPosition( player );
	sendRotation( player );
}

void Actor::sendExistance( const player_list & players ) {
	network::sendTo( existancePacket( ), players );
	sendPosition( players );
	sendRotation( players );
}

void Actor::sendLeave( const player_id & player ) {
	network::sendTo( leavePacket( ), player );
}

void Actor::sendLeave( const player_list & players ) {
	network::sendTo( leavePacket( ), players );
}

void Actor::sendPossess( const player_id & player ) {
	network::sendTo( possessPacket( ), player );
}

void Actor::sendPosition( const player_id & player ) {
	network::sendTo( positionPacket( ), player );
}

void Actor::sendPosition( const player_list & players ) {
	network::sendTo( positionPacket( ), players );
}

void Actor::sendRotation( const player_id & player ) {
	network::sendTo( rotationPacket( ), player );
}

void Actor::sendRotation( const player_list & players ) {
	network::sendTo( rotationPacket( ), players );
}

void Actor::sendInput( const player_id & player ) {
	network::sendTo( inputPacket( ), player );
}

void Actor::sendInput( const player_list & players ) {
	network::sendTo( inputPacket( ), players );
}


// PACKETS
net::packet Actor::existancePacket( ) {
	bstream str;

	str.write<short>( protocol::PlayerJoin );
	str.write<short>( id );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet Actor::leavePacket( ) {
	bstream str;

	str.write<short>( protocol::PlayerLeave );
	str.write<short>( id );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet Actor::possessPacket( ) {
	bstream str;

	str.write<short>( protocol::PlayerPossess );
	str.write<short>( id );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet Actor::positionPacket( ) {
	bstream str;

	str.write<short>( protocol::PlayerPosition );
	str.write<short>( id );

	str.write<float>( position.x );
	str.write<float>( position.y );
	str.write<float>( position.z );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet Actor::rotationPacket( ) {
	bstream str;

	str.write<short>( protocol::PlayerRotation );
	str.write<short>( id );

	str.write<float>( rotation );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet Actor::inputPacket( ) {
	bstream str;

	str.write<short>( protocol::PlayerInput );
	str.write<short>( id );

	str.write<char>( input.encode( ) );

	return net::packet( str.begin( ), str.size( ) );
}
