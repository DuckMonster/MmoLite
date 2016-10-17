#pragma once
class Actor;

class ActorController {
public:
	ActorController( Actor* actor );
	virtual ~ActorController( );

	Actor* const		getActor( ) { return actor; }

	virtual void		logic( );

private:
	Actor* const		actor;
};