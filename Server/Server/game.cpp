#include "game.hpp"

#include <bstream.hpp>
#include <iostream>

#include "protocol.hpp"
#include "globals.hpp"

Game::Game( ) :
	playerList( ) {
}

void Game::logic( ) {
	// Poll the server
}

void Game::playerJoin( const size_t playerID ) {
	// Send ID
	{
		bstream str;

		str.write<short>( MmoProtocol::PlayerID );
		str.write<short>( playerID );

		network::server->send( net::packet( str.begin( ), str.size( ) ), playerID );
	}
	
	// Send join to other players
	{
		bstream str;

		str.write<short>( MmoProtocol::PlayerJoin );
		str.write<short>( playerID );

		net::packet pkt( str.begin( ), str.size( ) );
		sendToAll( pkt );
	}

	// Add player
	playerList.push_back( playerID );

	// Send all players to the new player
	for(size_t id : playerList)
	{
		bstream str;

		str.write<short>( MmoProtocol::PlayerJoin );
		str.write<short>( id );

		net::packet pkt( str.begin( ), str.size( ) );
		network::server->send( pkt, playerID );
	}

	std::cout << playerID << " connected!\n";
}

void Game::playerLeave( const size_t playerID ) {
	std::vector<size_t>::iterator it = find( playerList.begin( ), playerList.end( ), playerID );

	if (it != playerList.end( ))
		playerList.erase( it );

	// Send leave to all players
	{
		bstream str;

		str.write<short>( MmoProtocol::PlayerLeave );
		str.write<short>( playerID );

		net::packet pkt( str.begin( ), str.size( ) );
		sendToAll( pkt );
	}

	std::cout << playerID << " disconnected!\n";

}

void Game::handlePacket( const size_t playerID, const net::packet & pkt ) {
	std::cout << "Received " << playerID << " bytes from " << playerID << "!\n";
}

void Game::sendToAll( const net::packet & pkt ) {
	for (size_t id : playerList) {
		network::server->send( pkt, id );
	}
}
