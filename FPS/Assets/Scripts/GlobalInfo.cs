using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalInfo
{
    private static bool isGround=true;
    private static bool wallRun = false;
    private static bool Podkat= false;

    public static bool CheckGround()
    {
        return isGround;
    }
    public static void ChangeGround(bool change)
    {
        isGround = change;
        Debug.Log("isGround=" + isGround);
    }
    public static bool CheckWallRun()
    {
        return wallRun;
    }
    public static void ChangeWallRun(bool change)
    {
        wallRun = change;
    }
    public static bool ChecPodkat()
    {
        return Podkat;
    }
    public static void ChangePodkat(bool change)
    {
        Podkat = change;
    }
}
