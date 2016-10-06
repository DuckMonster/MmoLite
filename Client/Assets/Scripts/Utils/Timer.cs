using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Timer
{
	float length;
	public float Length { get { return length; } }

	float value;
	public float Value { get { return value; } }

	public bool Done { get { return value >= 1f; } }

	public Timer(float length, float value = 0f)
	{
		this.length = length;
		this.value = value;
	}

	public void Update(float delta)
	{
		value = Mathf.Clamp(value + delta / length, 0f, 1f);
	}

	public void Reset() { Reset(length); }
	public void Reset(float newLength)
	{
		length = newLength;
		value = 0;
	}
}