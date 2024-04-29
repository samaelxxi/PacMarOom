using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IResetable : MonoBehaviour
{
    public bool ActiveOnStart { get; set; } = true;

    public abstract void Reset();
}