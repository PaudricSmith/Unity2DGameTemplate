using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "EnemyDataList", menuName = "Scriptable Objects/Game Data/Enemy Data List", order = 1)]
public class EnemyDataListSO : ScriptableObject
{

    [SerializeField] private List<EnemyDataSO> enemyDataList;



    public List<EnemyDataSO> EnemyDataList { get => enemyDataList; set => enemyDataList = value; }
}