using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class OverrideOffset : MonoBehaviour
{
    public bool changeOffset = false;
    [ShowIf("changeOffset")]
    public float newOffset = -1;
    public bool blockOffseIncrement;
    public bool blockOffsetDectrement;
}
