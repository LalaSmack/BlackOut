using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject roomPrefab;

    
    private int mazeHeight = 15;
    private int mazeWidth = 20;
    private Room[,] rooms;
    private float roomWidth = 1.0f;
    private float roomHeight = 1.0f;

    //backtracking stack for the depth-first search algorithm
    private Stack<Room> backtrackingStack = new Stack<Room>();

    private bool generationComplete = false;
    private bool isGenerating = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (roomPrefab == null)
        {
            Debug.LogError("Room prefab is not assigned! Instance: " + gameObject.name);
            return;
            
        }
        rooms = new Room[mazeWidth, mazeHeight];
        GetRoomBounds();
        for (int i = 0; i < mazeWidth; ++i)
        {
            for (int j = 0; j < mazeHeight; ++j)
            {
                GameObject room = Instantiate(roomPrefab,
                  new Vector3(i * roomWidth, j * roomHeight, 0.0f),
                  Quaternion.identity);

                //room.name = "Room_" + i.ToString() + "_" + j.ToString();
                rooms[i, j] = room.GetComponent<Room>();
                rooms[i, j].index = new Vector2Int(i, j);
            }
        }

        
        Debug.Log("Maze generation started!");

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGenerating)
        {
            isGenerating = true;
            GenerateMaze();
        }

        if (generationComplete)
        {
            // remove loading screen 
        }
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

    private void RemoveWall(int i, int j, Room.WallOrientation direction)
    {
        if ( direction == Room.WallOrientation.NONE)
        {
            return;
        }

        Room currentRoom = rooms[i, j];
        //Debug.Log(currentRoom.name + " " + currentRoom.GetWallState(direction).ToString());
        Room.WallOrientation opp = Room.WallOrientation.NONE;

        if ((direction == Room.WallOrientation.TOP) && (j + 1 < mazeHeight))
        {
            opp = Room.WallOrientation.BOTTOM;
            j++;
        }
        else if ((direction == Room.WallOrientation.BOTTOM) && (j - 1 >= 0))
        {
            opp = Room.WallOrientation.TOP;
            j--;
        }
        else if ((direction == Room.WallOrientation.LEFT) && (i - 1 >= 0))
        {
            opp = Room.WallOrientation.RIGHT;
            i--;
        }
        else if ((direction == Room.WallOrientation.RIGHT) && (i + 1 < mazeWidth))
        {
            opp = Room.WallOrientation.LEFT;
            i++;
        }

        currentRoom.SetWallState(direction, false);
        rooms[i, j].SetWallState(opp, false);
    }


    // Generate the maze
    private void GenerateMaze()
    {
        // Implement maze generation algorithm here: Depth-first search.
        int startX = Random.Range(0, mazeWidth);
        int startY = Random.Range(0, mazeHeight);

        rooms[startX, startY].SetVisited(true);
        backtrackingStack.Push(rooms[startX, startY]);

        StartCoroutine(GenerateMazeCoroutine());


    }

    IEnumerator GenerateMazeCoroutine()
    {
        GenerateMazeDFS();
        

        yield return new WaitForSeconds(1f); // Wait for a short duration to visualize the maze generation
        Debug.Log("Maze generation complete!");
    }
    
    private void GenerateMazeDFS()
    {
        while (backtrackingStack.Count > 0)
        {
            Room currentRoom = backtrackingStack.Peek();
            int x = currentRoom.index.x;
            int y = currentRoom.index.y;

            int[] nextRoomCoords = GetUnvisitedNeighbor(x, y);
            int nextX = nextRoomCoords != null ? nextRoomCoords[0] : x;
            int nextY = nextRoomCoords != null ? nextRoomCoords[1] : y;
            Room nextRoom = null;

            if (nextRoomCoords != null)
            {
                nextRoom = rooms[nextX, nextY];
            }

            if (nextRoom != null)
            {
                RemoveWall(x, y, GetWallOrientation(currentRoom, nextRoom));

                // Mark the next room as visited and push it onto the stack
                nextRoom.SetVisited(true);
                backtrackingStack.Push(nextRoom);
            }
            else
            {
                // Backtrack to the previous room
                backtrackingStack.Pop();
            }
        }

        generationComplete = true;
    }

    private Room.WallOrientation GetWallOrientation(Room currentRoom, Room nextRoom)
    {
        int xDiff = nextRoom.index.x - currentRoom.index.x;
        int yDiff = nextRoom.index.y - currentRoom.index.y;

        if (xDiff == 1 && yDiff == 0)
        {
            return Room.WallOrientation.RIGHT;
        }
        else if (xDiff == -1 && yDiff == 0)
        {
            return Room.WallOrientation.LEFT;
        }
        else if (xDiff == 0 && yDiff == 1)
        {
            return Room.WallOrientation.TOP;
        }
        else if (xDiff == 0 && yDiff == -1)
        {
            return Room.WallOrientation.BOTTOM;
        }

        return Room.WallOrientation.NONE; // No valid orientation found
    }
    private int[] GetUnvisitedNeighbor(int x, int y)
    {
        // Check the four possible neighbors (top, bottom, left, right)
        List<int[]> unvisitedNeighbors = new List<int[]>();

        // Check the four possible neighbors (top, bottom, left, right)
        if (y + 1 < mazeHeight && !rooms[x, y + 1].HasBeenVisited())
        {
            unvisitedNeighbors.Add(new int[] { x, y + 1 });
        }
        if (y - 1 >= 0 && !rooms[x, y - 1].HasBeenVisited())
        {
            unvisitedNeighbors.Add(new int[] { x, y - 1 });
        }
        if (x - 1 >= 0 && !rooms[x - 1, y].HasBeenVisited())
        {
            unvisitedNeighbors.Add(new int[] { x - 1, y });
        }
        if (x + 1 < mazeWidth && !rooms[x + 1, y].HasBeenVisited())
        {
            unvisitedNeighbors.Add(new int[] { x + 1, y });
        }

        if (unvisitedNeighbors.Count > 0)
        {
            int randomIndex = Random.Range(0, unvisitedNeighbors.Count);
            return unvisitedNeighbors[randomIndex];
        }

        return null; // No unvisited neighbors
    }
}
