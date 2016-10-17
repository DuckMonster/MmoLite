#pragma once
#include <glm/vec3.hpp>
#include <net/packet.hpp>
#include <memory>

#include "types.hpp"
#include "protocol.hpp"
#include "actortype.hpp"
#include "playerinput.hpp"
#include "actorcontroller.hpp"

class Game;
class Actor {
public:
	Actor( ActorType type, Game* game );
	~Actor( );

	virtual void						logic( );

	Game* const							getGame( ) { return game; }

	void								setID( const actor_id& id ) { this->id = id; }
	const size_t						getID( ) { return id; }

	glm::vec3							getPosition( ) { return position; }
	void								setPosition( const glm::vec3& position );

	float								getRotation( ) { return rotation; }
	void								setRotation( const float rotation );

	const ActorType						getType( ) { return type; }
	ActorController*					getController( ) { return controller.get( ); }

	template<typename T>
	T*									getController( ) { return std::dynamic_pointer_cast<T>(controller).get( ); }

	void								send( protocol::MmoProtocol msg );
	void								send( protocol::MmoProtocol msg, const player_id& player );
	void								send( protocol::MmoProtocol msg, const player_list& players );

private:
	template<typename CollectionType>
	void								doSend( protocol::MmoProtocol msg, const CollectionType& players );

	net::packet							existancePacket( );
	net::packet							leavePacket( );
	net::packet							positionPacket( );
	net::packet							rotationPacket( );

	Game* const							game;
	actor_id							id;
	const ActorType						type;

	glm::vec3							position;
	float								rotation;

	std::shared_ptr<ActorController>	controller;
};

template<typename CollectionType>
inline void Actor::doSend( protocol::MmoProtocol msg, const CollectionType & players ) {
	switch (msg) {
		case protocol::ActorJoin:
			network::sendTo( existancePacket( ), players );
			send( protocol::ActorPosition, players );
			send( protocol::ActorRotation, players );

			break;

		case protocol::ActorPosition: network::sendTo( positionPacket( ), players ); break;
		case protocol::ActorRotation: network::sendTo( rotationPacket( ), players ); break;
		case protocol::ActorLeave: network::sendTo( leavePacket( ), players ); break;
	}
}

typedef std::shared_ptr<Actor> actor_ptr;
typedef std::array<actor_ptr, 512> actor_array;