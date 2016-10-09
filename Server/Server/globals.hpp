#pragma once
#include <net/server.hpp>
#include "types.hpp"

namespace network {
	extern net::server*		server;
	void					sendTo( const net::packet& pkt, const player_id& player );
	void					sendTo( const net::packet& pkt, const player_list& players );
}