using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLibrary
{
    public static bool IsLayerInMask(LayerMask lm, int layer)
    {
        return (lm == (lm | (1 << layer)));
    }
}
