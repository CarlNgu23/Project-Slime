using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManager", order = 1)]
public class SpawnManager : ScriptableObject
{
    public string prefabName;
    public string leftBoundary;
    public string rightBoundary;
    public float maxXAxis;
    public int numberOfPrefabsToCreate;
    public Vector3[] monster_SpawnPoints;
    public Vector3[] leftBoundary_SpawnPoints;
    public Vector3[] rightBoundary_SpawnPoints;
}