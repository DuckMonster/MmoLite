#pragma once
#include <glm/vec3.hpp>
#include <net/packet.hpp>
#include <memory>
#include "types.hpp"
#include "playerinput.hpp"

class Game;
class Actor {
public:
	Actor( Game* game );

	void				setID( const actor_id& id ) { this->id = id; }
	const size_t		getID( ) { return id; }

	glm::vec3			getPosition( ) { return position; }
	void				setPosition( const glm::vec3& position );

	float				getRotation( ) { return rotation; }
	void				setRotation( const float rotation );

	PlayerInput&		getInput( ) { return input; }
	void				setInput( const PlayerInput& input );

	void				sendExistance( const player_id& player );
	void				sendExistance( const player_list& players );

	void				sendLeave( const player_id& player );
	void				sendLeave( const player_list& players );

	void				sendPossess( const player_id& player );

	void				sendPosition( const player_id& player );
	void				sendPosition( const player_list& players );

	void				sendRotation( const player_id& player );
	void				sendRotation( const player_list& players );

	void				sendInput( const player_id& player );
	void				sendInput( const player_list& players );

private:
	net::packet			existancePacket( );
	net::packet			leavePacket( );
	net::packet			possessPacket( );
	net::packet			positionPacket( );
	net::packet			rotationPacket( );
	net::packet			inputPacket( );

	Game* const			game;
	actor_id			id;

	glm::vec3			position;
	float				rotation;

	PlayerInput			input;
};

typedef std::shared_ptr<Actor> actor_ptr;
typedef std::array<actor_ptr, 512> actor_array;