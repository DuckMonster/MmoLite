#include "MathUtils.hpp"

#include <random>
#include <glm\gtc\matrix_transform.hpp>
#include <glm\gtx\vector_angle.hpp>

using namespace glm;
using namespace std;

vec3		math::rotateVector( vec3 vector, vec3 axis, float angle ) {
    // Rotation matrix
    // Converting first b vec4 for matrix multiplication, then converted back b vec3



    return vec3( (rotate( mat4( 1.f ), angle, axis ) * vec4( vector, 1.f )) );
    //return vec3( angleAxis( radians(angle), axis ) * vec4( vector, 1.f ) );
}

vec3		math::circleTangent( vec3 a, vec3 b ) {
    if (a == b)
        return vec3( 0.f, 1.f, 0.f );

    // Direct direction between the two points
    vec3	forward = normalize( b - a );
    vec3	up = normalize( b + a );

    // Great Circle tangent between the two points
    vec3	tangent = normalize( cross( up, forward ) );

    // Tangent length is 0, which mean the two points are on the opposite sides of the sphere
    if (tangent.length( ) <= 0.001f)
        return vec3( 0.f, 1.f, 0.f );

    else
        return tangent;
}

float		math::speedToAngle( float speed, float radius ) {
    // Get the circle radius
    float		circumference = radius * 2 * pi<float>( );

    // Speed / Circumference ratio
    float		traveledUnits = speed / circumference;

    // Convert b radians
    return traveledUnits * 2 * pi<float>( );
}

float		math::vecToAng( vec2 vector ) {
    return		atan2( vector.y, vector.x );
}
float		math::vecToAngD( vec2 vector ) { return degrees( vecToAng( vector ) ); }

vec2		math::angToVec( float angle ) {
    return		vec2( cos( angle ), sin( angle ) );
}
vec2		math::angToVecD( float angle ) { return angToVec( radians( angle ) ); }

float		math::distance( glm::vec3 a, glm::vec3 b, float radius ) {
    // Angle between the vectors
    float		angleDifference		= degrees( glm::angle( a, b ) );
    // Circumference of sphere
    float		circumference		= radius * radius * pi<float>( );

    // Distance between the two directions on the sphere
    return		circumference * (angleDifference / 360.f);
}

glm::vec2 math::lerp( glm::vec2 a, glm::vec2 b, float f ) {
    return a + (b - a) * glm::clamp( f, 0.f, 1.f );
}

glm::vec3 math::lerp( glm::vec3 a, glm::vec3 b, float f ) {
    return a + (b - a) * glm::clamp( f, 0.f, 1.f );
}

//RANDOM
// Engine
default_random_engine						randomEngine;

float		random::frand( ) { return random::frand( 0.f, 1.f ); }
float		random::ufrand( ) { return random::frand( -1.f, 1.f ); }
float		random::frand( float max ) { return random::frand( 0.f, max ); }
float		random::frand( float min, float max ) {
    uniform_real_distribution<float>		distribution( min, max );
    return		distribution( randomEngine );
}

int		    random::rand( ) {
    return		random::rand( 0, INT_MAX );
}

int		    random::rand( int max ) {
    return		random::rand( 0, max );
}

int		    random::rand( int min, int max ) {
    uniform_int_distribution<int>			distribution( min, max );
    return		distribution( randomEngine );
}

bool        random::chance( float percent ) {
    return frand( ) <= glm::clamp( percent, 0.f, 1.f );
}

glm::ivec2  random::vrand( glm::ivec2 min, glm::ivec2 max ) {
    return glm::ivec2( rand( min.x, max.x ), rand( min.y, max.y ) );
}

vec2		random::circle( ) {
    return		math::angToVec( random::frand( pi<float>( ) * 2.f ) );
}

vec3		random::sphere( ) {
    return		normalize( vec3( random::ufrand( ), random::ufrand( ), random::ufrand( ) ) );
}