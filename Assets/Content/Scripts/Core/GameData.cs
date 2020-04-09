using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameData/New GameData")]
public class GameData : ScriptableObject
{
    [Header("Reference")]
    public List<SceneSettings> _levels = new List<SceneSettings>();

    private static bool isCaching = false;
    private static GameData m_Data;
    public static GameData Instance
    {
        get
        {
            if (m_Data == null && !isCaching)
            {
                m_Data = Resources.Load("GameData", typeof(GameData)) as GameData;
            }
            return m_Data;
        }
    }
}