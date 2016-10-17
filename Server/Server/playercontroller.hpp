#pragma once
#include <net/packet.hpp>

#include "actorcontroller.hpp"
#include "types.hpp"
#include "playerinput.hpp"

class PlayerController : public ActorController {
public:
	PlayerController( Actor* actor );
	~PlayerController( );

	PlayerInput&				getInput( ) { return input; }
	void						setInput( const PlayerInput& input );

	void						sendPossess( const player_id& player );

	void						sendInput( const player_id& player );
	void						sendInput( const player_list& players );

private:
	net::packet					possessPacket( );
	net::packet					inputPacket( );

	PlayerInput					input;
};