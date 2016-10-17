#include <iostream>
#include <net\server.hpp>
#include "globals.hpp"
#include "game.hpp"

typedef std::chrono::high_resolution_clock::time_point time_point;
const long MICRO_PER_SECOND = 1000000;

int main( ) {
	std::cout << "Server started at port 15620.\n";

	net::server s;
	s.startup( 15620 );
	network::server = &s;

	Game game;
	time_point prev = std::chrono::high_resolution_clock::now( );

	float t = 0.f;

	while (s.active( )) {
		time_point now = std::chrono::high_resolution_clock::now( );
		float delta = (float)std::chrono::duration_cast<std::chrono::microseconds>(now - prev).count( ) / MICRO_PER_SECOND;
		game::delta = delta;

		prev = now;

		t += delta;
		if (t >= 2.f) {
			t = 0.f;
			std::cout << delta * 1000.f << " ms\n";
		}

		net::event e;
		int n = 0;

		while (s.pollEvent( e )) {
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
		}

		game.logic( );

		std::this_thread::sleep_for( std::chrono::milliseconds( 1 ) );
	}
}