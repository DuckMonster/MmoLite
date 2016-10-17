#pragma once
#include <glm\glm.hpp>

namespace math {
	// Rotate a vector around axis
	glm::vec3	rotateVector( glm::vec3 vector, glm::vec3 axis, float angle );

	// Great Circle navigation
	///<summary>Find the tangent of the Great Circle that connects two points on a sphere
	///<param="a">First point</param>
	///<param="b">Second point</param>
	///</symmary>
	glm::vec3	circleTangent( glm::vec3 a, glm::vec3 b );

	///<summary>Convert speed to how many degrees (radians) of a circle</summary>
	float		speedToAngle( float speed, float radius );


	///<summary>2D Vector to angle (Radians)</summary>
	float		vecToAng( glm::vec2 vector );
	///<summary>2D Vector to angle (Degrees)</summary>
	float		vecToAngD( glm::vec2 vector );

	///<summary>Angle to 2D vector (Radians)</summary>
	glm::vec2	angToVec( float angle );
	///<summary>Angle to 2D vector (Degrees)</summary>
	glm::vec2	angToVecD( float angle );

	///<summary>Distance between two directions on a sphere</summary>
	float		distance(glm::vec3 a, glm::vec3 b, float radius);

    ///<summary>Linear interpolation between two vectors</summar>
    glm::vec2   lerp( glm::vec2 a, glm::vec2 b, float f );

    ///<summary>Linear interpolation between two vectors</summar>
    glm::vec3   lerp( glm::vec3 a, glm::vec3 b, float f );
}

namespace random {
	// RANDOM
	///<summary>Random float between 0 and 1</summary>
	float		frand( );
	///<summary>Random float between -1 and 1</summary>
	float		ufrand( );
	///<summary>Random float between 0 and max</summary>
	float		frand( float max );
	///<summary>Random float between min and max</summary>
	float		frand( float min, float max );

	///<summary>Random int between 0 and maximum int [inclusive]</summary>
	int		    rand( );
	///<summary>Random int between 0 and max [inclusive]</summary>
    int		    rand( int max );
	///<summary>Random int between min and max [inclusive]</summary>
    int		    rand( int min, int max );

    ///<summary>Random chance</summary>
    bool        chance( float percent );

    ///<summary>Random ivec2 within a set rectangle</summary>
    glm::ivec2  vrand( glm::ivec2 min, glm::ivec2 max );

	///<summary>Random point within unit circle</summary>
	glm::vec2	circle( );

	///<summary>Random point within unit sphere</summary>
	glm::vec3	sphere( );
}