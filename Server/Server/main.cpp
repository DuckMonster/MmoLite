#include <iostream>
#include <net\server.hpp>
#include "globals.hpp"
#include "game.hpp"

int main( ) {
	std::cout << "Hello world!\n";

	net::server s;
	s.startup( 12345 );
	network::server = &s;

	Game game;

	while (s.active( )) {
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