using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The GameObject to instantiate.
    public GameObject entityToSpawn;
    public GameObject leftBoundarySpawn;
    public GameObject rightBoundarySpawn;

    public GameObject currentEntity;
    public GameObject currentLeftBoundary;
    public GameObject currentRightBoundary;


    // An instance of the ScriptableObject defined above.
    public SpawnManager spawnManager;

    // This will be appended to the name of the created entities and increment when each is created.
    public int instanceNumber = 1;

    void Start()
    {
        SpawnEntities();
    }

    void SpawnEntities()
    {
        int currentMonsterSpawnPointIndex = 0;
        int currentLeftBoundarySpawnPointIndex = 0;
        int currentRightBoundarySpawnPointIndex = 0;

        for (int i = 0; i < spawnManager.numberOfPrefabsToCreate; i++)
        {
            currentLeftBoundary = Instantiate(leftBoundarySpawn, spawnManager.leftBoundary_SpawnPoints[currentLeftBoundarySpawnPointIndex], Quaternion.identity);
            currentLeftBoundary.name = spawnManager.leftBoundary + instanceNumber;
            currentLeftBoundary.SetActive(true);
            //currentEntity.Find(spawnManager.leftBoundary + instanceNumber);

            currentRightBoundary = Instantiate(rightBoundarySpawn, spawnManager.rightBoundary_SpawnPoints[currentRightBoundarySpawnPointIndex], Quaternion.identity);
            currentRightBoundary.name = spawnManager.rightBoundary + instanceNumber;
            currentRightBoundary.SetActive(true);

            // Creates an instance of the prefab at the current spawn point.
            currentEntity = Instantiate(entityToSpawn, spawnManager.monster_SpawnPoints[currentMonsterSpawnPointIndex], Quaternion.identity);

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            currentEntity.name = spawnManager.prefabName + instanceNumber;
            CPU_Movement cpu_movement = currentEntity.GetComponent<CPU_Movement>();
            cpu_movement.leftLimitGameObject = currentLeftBoundary;
            cpu_movement.rightLimitGameObject = currentRightBoundary;
            cpu_movement.xAxisControlForLimitObjects = spawnManager.maxXAxis;
            cpu_movement.yAxisControlForLimitObjects = spawnManager.leftBoundary_SpawnPoints[currentLeftBoundarySpawnPointIndex].y;

            // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
            currentMonsterSpawnPointIndex = (currentMonsterSpawnPointIndex + 1) % spawnManager.monster_SpawnPoints.Length;
            currentLeftBoundarySpawnPointIndex = (currentLeftBoundarySpawnPointIndex + 1) % spawnManager.leftBoundary_SpawnPoints.Length;
            currentRightBoundarySpawnPointIndex = (currentRightBoundarySpawnPointIndex + 1) % spawnManager.rightBoundary_SpawnPoints.Length;

            instanceNumber++;
        }
    }
}