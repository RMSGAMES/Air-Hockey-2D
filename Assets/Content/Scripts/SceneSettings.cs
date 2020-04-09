using UnityEngine;

[CreateAssetMenu(menuName = "Game/Scene/New Scene Settings")]
public class SceneSettings : ScriptableObject
{
    [Header("Settings")]
    public bool isEnabled = true;
    public string showName;

    [Header("References")]
    public Object scene;
}