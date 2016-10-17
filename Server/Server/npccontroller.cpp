#include "npccontroller.hpp"

#include "actor.hpp"

#include "globals.hpp"
#include "mathutils.hpp"

NpcController::NpcController( Actor * actor ) :
	ActorController( actor ),
	rotateTimer( 1.f ) {
}

NpcController::~NpcController( ) {
}

void NpcController::logic( ) {
	rotateTimer.update( game::delta );
	if (rotateTimer.isDone( )) {
		rotateTimer.reset( );
		getActor( )->setRotation( random::frand( 0.f, 360.f ) );
	}
}