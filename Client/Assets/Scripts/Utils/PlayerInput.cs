public struct PlayerInput
{
	int forward;
	public float Forward
	{
		get { return FromInt( forward ); }
		set { forward = FromFloat( value ); }
	}

	int strafe;
	public float Strafe
	{
		get { return FromInt( strafe ); }
		set { strafe = FromFloat( value ); }
	}

	int turn;
	public float Turn
	{
		get { return FromInt( turn ); }
		set { turn = FromFloat( value ); }
	}

	public PlayerInput( byte data )
	{
		forward = 0;
		strafe = 0;
		turn = 0;

		Decode( data );
	}

	public byte Encode( )
	{
		// Encode the input in to a single byte
		// Each input is encoded into 2 bits
		// 0 is no input, 1 is positive input and 2 is negative input
		byte data = 0;

		data |= (byte)(forward & 3);
		data |= (byte)((strafe & 3) << 2);
		data |= (byte)((turn & 3) << 4);

		return data;
	}

	public void Decode( byte data )
	{
		forward = data & 0x3;
		strafe = (data >> 2) & 0x3;
		turn = (data >> 4) & 0x3;
	}

	int FromFloat( float val )
	{
		return val > 0.1f ? 1 : (val < -0.1f ? 2 : 0);
	}

	float FromInt( int val )
	{
		return val == 0 ? 0f : (val == 1 ? 1f : -1f);
	}

	public override bool Equals( object obj )
	{
		if (obj == null || !(obj is PlayerInput))
			return false;

		PlayerInput pi = (PlayerInput)obj;
		return this == pi;
	}

	public override int GetHashCode( )
	{
		return Encode();
	}

	public static bool operator !=(PlayerInput a, PlayerInput b)
	{
		return !(a == b);
	}
	public static bool operator ==(PlayerInput a, PlayerInput b)
	{
		return
			a.forward == b.forward &&
			a.strafe == b.strafe && 
			a.turn == b.turn;
	}
}