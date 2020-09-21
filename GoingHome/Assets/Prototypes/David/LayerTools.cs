using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class LayerTools
{
    public static LayerMask SetLayer(this LayerMask mask, int layer)
    {
        mask |= layer;
        return mask;
    }

	public static LayerMask RemoveLayer(this LayerMask mask, int layer)
	{
		layer = ~layer;
		return mask & layer;
	}
}
