using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    public GameObject player;
    private float speed = 5f;

    public Vector2 startingPosition {get; set;} 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = startingPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector2(0, 1) * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector2(0, -1) * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector2(-1, 0) * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * speed);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(player);
        }
        else if (other.CompareTag("Maze"))
        {
            Debug.Log("Maze");
        }
    }
}
