#pragma once
class Timer {
public:
	Timer( float length, float value = 0.f );

	bool			isDone( );

	void			reset( );
	void			reset( float length );

	void			update( float delta );

	float value;

private:
	float length;
};