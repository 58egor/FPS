using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalInfo
{
    private static bool isGround=true;
    public static bool ChechGround()
    {
        return isGround;
    }
    public static void ChangeGround(bool change)
    {
        isGround = change;
        Debug.Log("isGround=" + isGround);
    }
}
