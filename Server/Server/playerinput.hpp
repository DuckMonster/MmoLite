#pragma once
class PlayerInput {
public:
	PlayerInput( );
	PlayerInput( char data );

	float			getForward( ) const;
	float			getStrafe( ) const;
	float			getTurn( ) const;

	char			encode( ) const;
	void			decode( char data );

private:
	float			toFloat( int val ) const;
	int				toInt( float val ) const;

	int				forward;
	int				strafe;
	int				turn;
};