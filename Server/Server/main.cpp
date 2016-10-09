#include <iostream>
#include <net\server.hpp>
#include "globals.hpp"
#include "game.hpp"

typedef std::chrono::high_resolution_clock::time_point time_point;
const long MICRO_PER_SECOND = 1000000;

int main( ) {
	std::cout << "Hello world!\n";

	net::server s;
	s.startup( 15620 );
	network::server = &s;

	Game game;
	time_point prev = std::chrono::high_resolution_clock::now( );

	while (s.active( )) {
		time_point now = std::chrono::high_resolution_clock::now( );
		float delta = (float)std::chrono::duration_cast<std::chrono::microseconds>(now - prev).count( ) / MICRO_PER_SECOND;

		prev = now;

		net::event e;
		s.pollEvent( e );

		switch (e.type( )) {
			case net::EventType::eConnect:
				game.playerJoin( e.connect( ).id );

				break;

			case net::EventType::eDisconnect:
				game.playerLeave( e.disconnect( ).id );

				break;

			case net::EventType::eError:
				std::cout << "Handled an event\n";

				break;

			case net::EventType::ePacket:
				
				game.handlePacket( e.packet( ).id, e.packet( ).pkt );

				break;
		}

		game.logic( );

		std::this_thread::sleep_for( std::chrono::milliseconds( 1 ) );
	}
}