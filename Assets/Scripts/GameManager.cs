using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject enemyPrefab;
    private int numberOfEnemies = 5;

    [SerializeField]
    private Vector2[] enemyPositions;
    public MazeGenerator mazeGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        player.startingPosition = mazeGenerator.GetRandomRoomPosition();
        Debug.Log("Player starting position: " + player.startingPosition);
        enemyPositions = new Vector2[numberOfEnemies];
        for (int i = 0; i < numberOfEnemies; i++)
        {
            enemyPositions[i] = mazeGenerator.GetRandomRoomPosition(i, numberOfEnemies);
            while ((enemyPositions[i] - player.startingPosition).sqrMagnitude < 5f || 
                   (i > 0 && enemyPositions[i] == enemyPositions[i - 1]))
            {
                enemyPositions[i] = mazeGenerator.GetRandomRoomPosition(i, numberOfEnemies);
            }
            GameObject enemy = Instantiate(enemyPrefab, enemyPositions[i], Quaternion.identity);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.targetPosition = player.startingPosition;
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
