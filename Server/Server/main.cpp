#include <iostream>
#include <net\server.hpp>

int main( ) {
	std::cout << "Hello world!\n";

	net::server s;
	s.startup( 12345 );

	while (s.active( )) {
		net::event e;
		s.pollEvent( e );

		switch (e.type( )) {
			case net::EventType::eConnect:
				std::cout << e.connect( ).id << " connected!\n";

				break;

			case net::EventType::eDisconnect:
				std::cout << e.disconnect( ).id << " disconnected!\n";

				break;

			case net::EventType::eError:
				std::cout << "Handled an event\n";

				break;

			case net::EventType::ePacket:
				std::cout << "Received " << e.packet( ).pkt.size( ) << " bytes from " << e.packet().id << "!\n";
				s.send( e.packet( ).pkt, e.packet( ).id );

				break;
		}

		std::this_thread::sleep_for( std::chrono::milliseconds( 1 ) );
	}
}