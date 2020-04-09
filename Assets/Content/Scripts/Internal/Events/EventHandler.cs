using UnityEngine;
using System;

public class EventHandler : MonoBehaviour
{
    //On Game Win
    public delegate void GameWin();
    public static GameWin onGameWin;
    //On Game Lose
    public delegate void GameLose();
    public static GameLose onGameLose;

    /// <summary>
    /// Call This when game is win
    /// </summary>
    public static void OnGameWinEvent()
    {
        if (onGameWin != null)
        {
            onGameWin();
        }
    }

    /// <summary>
    /// Call This when game is lose
    /// </summary>
    public static void OnGameLoseEvent()
    {
        if (onGameLose != null)
        {
            onGameLose();
        }
    }
}
