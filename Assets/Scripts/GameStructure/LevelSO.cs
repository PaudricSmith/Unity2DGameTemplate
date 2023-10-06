using UnityEngine;


[CreateAssetMenu(fileName = "NewLevel", menuName = "Scriptable Objects/Game Data/Level")]
public class LevelSO : GameSceneSO
{

    [Header("Level Number")]
    [SerializeField] private int level = 1;

    [SerializeField] private bool hasLevelStarted = false;



    public int Level { get => level; set => level = value; }
    public bool HasLevelStarted { get => hasLevelStarted; set => hasLevelStarted = value; }
}