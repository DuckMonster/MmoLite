#include "timer.hpp"

#include <glm/glm.hpp>

Timer::Timer( float length, float value ) :
	value( value ),
	length( length ) {
}

bool Timer::isDone( ) {
	return value >= 1.f;
}

void Timer::reset( ) { reset( length ); }
void Timer::reset( float length ) {
	this->length = length;
	value = 0.f;
}

void Timer::update( float delta ) {
	value = glm::clamp( value + delta / length, 0.f, 1.f );
}