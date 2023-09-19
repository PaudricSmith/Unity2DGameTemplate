using UnityEngine;


[CreateAssetMenu(fileName = "NewLevel", menuName = "Scriptable Objects/Game Data/Level")]
public class LevelSO : GameSceneSO
{
    [Header("Level Number")]
    public int level = 1;

    [Header("Level Specific")]
    public bool hasGameStarted = false;
    // Add other saved game data fields here
}