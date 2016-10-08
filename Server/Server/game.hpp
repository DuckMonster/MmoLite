#pragma once
#include <vector>
#include <net/server.hpp>
#include <net/packet.hpp>

class Game {
public:
	Game( );

	void				logic( );

	void				playerJoin( const size_t playerID );
	void				playerLeave( const size_t playerID );
	void				handlePacket( const size_t playerID, const net::packet& pkt );

private:
	void				sendToAll( const net::packet& pkt );

	std::vector<size_t>	playerList;
};