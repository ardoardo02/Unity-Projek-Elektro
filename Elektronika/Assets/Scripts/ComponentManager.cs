using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    [System.Serializable]
    public enum ComponentType
    {
        IC_Type1,
        Type2,
        _74LS148,
        _7447,
        Others
    }

    public enum SlotType
    {
        Normal,
        Locked
    }
}
