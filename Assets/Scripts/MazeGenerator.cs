using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject roomPrefab = null;

    [SerializeField]
    private int mazeHeight = 10;
    [SerializeField]
    private int mazeWidth = 15;
    private Room[,] rooms = new Room[15, 15];
    private float roomWidth = 1.0f;
    private float roomHeight = 1.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetRoomBounds();
        for (int i = 0; i < mazeWidth; ++i)
        {
            for (int j = 0; j < mazeHeight; ++j)
            {
                GameObject room = Instantiate(roomPrefab,
                  new Vector3(i * roomWidth, j * roomHeight, 0.0f),
                  Quaternion.identity);

                room.name = "Room_" + i.ToString() + "_" + j.ToString();
                rooms[i, j] = room.GetComponent<Room>();
                rooms[i, j].index = new Vector2Int(i, j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GetRoomBounds()
    {
        {
        SpriteRenderer[] spriteRenderers =
          roomPrefab.GetComponentsInChildren<SpriteRenderer>();

        Vector3 minBounds = Vector3.positiveInfinity;
        Vector3 maxBounds = Vector3.negativeInfinity;

        foreach (SpriteRenderer ren in spriteRenderers)
        {
            minBounds = Vector3.Min(
              minBounds,
              ren.bounds.min);

            maxBounds = Vector3.Max(
              maxBounds,
              ren.bounds.max);
        }

        roomWidth = maxBounds.x - minBounds.x;
        roomHeight = maxBounds.y - minBounds.y;
    }
    }
    private void GenerateMaze()
    {
        // Implement maze generation algorithm here
    }
}
