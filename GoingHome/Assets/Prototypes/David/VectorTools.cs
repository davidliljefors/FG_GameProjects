using UnityEngine;

public static class VectorExtensions
{
	public static Vector3 Round(this Vector3 vector, float step = 1f)
	{
		vector.x = Mathf.Round(vector.x / step) * step;
		vector.y = Mathf.Round(vector.y / step) * step;
		vector.z = Mathf.Round(vector.z / step) * step;
		return vector;
	}

	public static Vector3 Between(this Vector3 lhs, Vector3 rhs, float t = 0.5f)
	{
		return lhs + t * (rhs - lhs);
	}

	public static Vector3 ClampX(this Vector3 vector, float min, float max)
	{
		vector.x = Mathf.Clamp(vector.x, min, max);
		return vector;
	}


	public static Vector3 ClampY(this Vector3 vector, float min, float max)
	{
		vector.y = Mathf.Clamp(vector.y, min, max);
		return vector;
	}

	public static Vector3 ClampZ(this Vector3 vector, float min, float max)
	{
		vector.z = Mathf.Clamp(vector.z, min, max);
		return vector;
	}

	/// <summary>
	/// Get vector with Y = 0;
	/// </summary>
	public static Vector3 Flatten(this Vector3 vector)
	{
		vector.y = 0;
		return vector;
	}

}
