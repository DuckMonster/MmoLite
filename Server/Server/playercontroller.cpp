#include "playercontroller.hpp"
#include "actor.hpp"

#include <bstream.hpp>

#include "game.hpp"
#include "protocol.hpp"
#include "globals.hpp"

PlayerController::PlayerController( Actor * actor ) : ActorController( actor ) {
}

PlayerController::~PlayerController( ) {
}

void PlayerController::setInput( const PlayerInput & input ) {
	this->input = input;
	sendInput( getActor( )->getGame( )->getPlayers( ) );
}

void PlayerController::sendPossess( const player_id & player ) {
	network::sendTo( possessPacket( ), player );
}

void PlayerController::sendInput( const player_id & player ) {
	network::sendTo( inputPacket( ), player );
}

void PlayerController::sendInput( const player_list & players ) {
	network::sendTo( inputPacket( ), players );
}

net::packet PlayerController::possessPacket( ) {
	bstream str;

	str.write<short>( protocol::PlayerPossess );
	str.write<short>( getActor( )->getID( ) );

	return net::packet( str.begin( ), str.size( ) );
}

net::packet PlayerController::inputPacket( ) {
	bstream str;

	str.write<short>( protocol::PlayerInput );
	str.write<short>( getActor( )->getID( ) );

	str.write<char>( input.encode( ) );

	return net::packet( str.begin( ), str.size( ) );
}
