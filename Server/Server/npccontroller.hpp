#pragma once
#include "actorcontroller.hpp"
#include "timer.hpp"

class NpcController : public ActorController {
public:
	NpcController( Actor* actor );
	~NpcController( );

	void			logic( ) override;

private:
	Timer			rotateTimer;
};