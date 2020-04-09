using UnityEngine;
using System.Collections;

public static class UtilityHelper
{
    public static bool isMobile
    {
        get
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}
