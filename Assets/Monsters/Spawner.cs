using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The GameObject to instantiate.
    public Transform platforms;
    public GameObject monsterToSpawn;
    public GameObject monsterLeftBoundarySpawn;
    public GameObject monsterRightBoundarySpawn;

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
        int currentIndex = 0;

        for (int i = 0; i < spawnManager.numberOfPrefabsToCreate; i++)
        {
            //Find the platform for the monster to spawn on.
            Transform currentPlatform = platforms.GetChild(currentIndex);

            //Creates an instance of left boundary.
            currentLeftBoundary = Instantiate(monsterLeftBoundarySpawn, spawnManager.leftBoundary_SpawnPoints[currentIndex], Quaternion.identity, currentPlatform);
            currentLeftBoundary.name = spawnManager.leftBoundary + instanceNumber;
            currentLeftBoundary.SetActive(true);
            
            //Creates an instance of right boundary.
            currentRightBoundary = Instantiate(monsterRightBoundarySpawn, spawnManager.rightBoundary_SpawnPoints[currentIndex], Quaternion.identity, currentPlatform);
            currentRightBoundary.name = spawnManager.rightBoundary + instanceNumber;
            currentRightBoundary.SetActive(true);

            // Creates an instance of the monster.
            currentEntity = Instantiate(monsterToSpawn, spawnManager.monster_SpawnPoints[currentIndex], Quaternion.identity, currentPlatform);

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            currentEntity.name = spawnManager.prefabName + instanceNumber;
            CPU_Movement cpu_movement = currentEntity.GetComponent<CPU_Movement>();
            cpu_movement.leftMonsterBoundaryGameObject = currentLeftBoundary;
            cpu_movement.rightMonsterBoundaryGameObject = currentRightBoundary;
            cpu_movement.max_X_AxisControlForBoundaryObjects = spawnManager.maxXAxis;
            cpu_movement.min_X_AxisControlForBoundaryObjects = spawnManager.minXAxis;
            cpu_movement.yAxisControlForBoundaryObjects = spawnManager.leftBoundary_SpawnPoints[currentIndex].y;

            // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
            currentIndex = (currentIndex + 1) % spawnManager.monster_SpawnPoints.Length;

            instanceNumber++;
        }
    }
}