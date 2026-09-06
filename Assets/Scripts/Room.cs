using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] 
    private GameObject topWall;
    [SerializeField] 
    private GameObject bottomWall;
    [SerializeField] 
    private GameObject leftWall;
    [SerializeField] 
    private GameObject rightWall;

    // orientation of the walls
    public enum WallOrientation
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        NONE,
    }

    public Vector2Int index { get; set; }
    private bool visited { get; set; } = false;

    // setting the walls active or inactive based on the orientation
    private void SetWallActive(WallOrientation orientation, bool state)
    {
        switch (orientation)
        {
            case WallOrientation.TOP:
                topWall.SetActive(state);
                break;
            case WallOrientation.BOTTOM:
                bottomWall.SetActive(state);
                break;
            case WallOrientation.LEFT:
                leftWall.SetActive(state);
                break;
            case WallOrientation.RIGHT:
                rightWall.SetActive(state);
                break;
            default:
                break;
        }
    }

    public bool GetWallState(WallOrientation orientation)
    {
        switch (orientation)
        {
            case WallOrientation.TOP:
                return topWall.activeSelf;
            case WallOrientation.BOTTOM:
                return bottomWall.activeSelf;
            case WallOrientation.LEFT:
                return leftWall.activeSelf;
            case WallOrientation.RIGHT:
                return rightWall.activeSelf;
            default:
                return false;
        }
    }

    public void SetWallState(WallOrientation orientation, bool state)
    {
        this.SetWallActive(orientation, state);
    }

    public bool HasBeenVisited()
    {
        return this.visited;
    }
    public void SetVisited(bool state)
    {
        this.visited = state;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.SetWallActive(WallOrientation.TOP, true);
        this.SetWallActive(WallOrientation.BOTTOM, true);
        this.SetWallActive(WallOrientation.LEFT, true);
        this.SetWallActive(WallOrientation.RIGHT, true);

    }

    
}
