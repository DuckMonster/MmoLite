#include "globals.hpp"

float game::delta				= 0.f;

net::server* network::server	= nullptr;

void network::sendTo( const net::packet & pkt, const player_id & player ) {
	server->send( pkt, player );
}

void network::sendTo( const net::packet & pkt, const player_list & players ) {
	for (player_list::const_iterator i = players.begin( ); i != players.end( ); i++)
		server->send( pkt, *i );
}
