#include "playerinput.hpp"

PlayerInput::PlayerInput( ) :
	forward( 0.f ),
	strafe( 0.f ),
	turn( 0.f ) {
}

PlayerInput::PlayerInput( char data ) : PlayerInput( ) {
	decode( data );
}

float PlayerInput::getForward( ) const {
	return toFloat( forward );
}

float PlayerInput::getStrafe( ) const {
	return toFloat( strafe );
}

float PlayerInput::getTurn( ) const {
	return toFloat( turn );
}

char PlayerInput::encode( ) const {
	// Encode the input in to a single byte
	// Each input is encoded into 2 bits
	// 0 is no input, 1 is positive input and 2 is negative input
	char data = 0;

	data |= (char)(forward & 3);
	data |= (char)((strafe & 3) << 2);
	data |= (char)((turn & 3) << 4);

	return data;
}

void PlayerInput::decode( char data ) {
	forward = data & 0x3;
	strafe = (data >> 2) & 0x3;
	turn = (data >> 4) & 0x3;
}

float PlayerInput::toFloat( int val ) const {
	return val > 0.1f ? 1 : (val < -0.1f ? 2 : 0);
}

int PlayerInput::toInt( float val ) const {
	return val == 0 ? 0.f : (val == 1 ? 1.f : -1.f);
}
