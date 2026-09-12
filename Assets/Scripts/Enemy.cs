using Unity.VisualScripting;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public Vector2 targetPosition {get; set; } = new Vector2(0, 0);
    
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private Direction currentDirection;
    private bool movingTowardsPlayer = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDirection = (Direction)Random.Range(0, 4);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (movingTowardsPlayer)
    
        Movement();
            
            
    }

    

    private void Movement()
    {
        float speed = 2f; // Adjust if needed
        if (Random.value < 0.0005f) // slight chance to change direction each frame
        {
            ChangeDirection();
        }
        switch (currentDirection)
        {
            case Direction.Up:
                transform.Translate(new Vector2(0, 1) * speed * Time.deltaTime);
                break;
            case Direction.Down:
                transform.Translate(new Vector2(0, -1) * speed * Time.deltaTime);
                break;
            case Direction.Left:
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true; // Flip the sprite horizontally
                transform.Translate(new Vector2(-1, 0) * speed * Time.deltaTime);
                break;
            case Direction.Right:
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false; // Unflip the sprite horizontally
                transform.Translate(new Vector2(1, 0) * speed * Time.deltaTime);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy detected player");
            //ChangeDirectionTowardsPlayer();
        }
    }
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Enemy collided with: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Maze"))
        {
            //Debug.Log("Enemy collided with maze, changing direction.");
            MoveAwayFromWall();
            ChangeDirection();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Maze"))
        {
            //Debug.Log("Enemy is still colliding with maze, changing direction.");
            MoveAwayFromWall();
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        currentDirection = randomDirection(currentDirection);

        
    }

    private Direction randomDirection(Direction currentDirection)
    {
        Direction newDirection = currentDirection;
        while (newDirection == currentDirection)
        {
            newDirection = (Direction)Random.Range(0, 4);
        }
        return newDirection;
    }

    private void MoveAwayFromWall()
    {
        switch (currentDirection)
        {
            case Direction.Up:
                this.transform.Translate(new Vector2(0, -0.3f) * 0.1f);
                break;
            case Direction.Down:
                this.transform.Translate(new Vector2(0, 0.3f) * 0.1f);
                break;
            case Direction.Left:
                this.transform.Translate(new Vector2(-0.3f, 0) * 0.1f);
                break;
            case Direction.Right:
                this.transform.Translate(new Vector2(0.3f, 0) * 0.1f);
                break;
        }
    }
    private void ChangeDirectionTowardsPlayer()
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;
        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
        {
            currentDirection = directionToPlayer.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            currentDirection = directionToPlayer.y > 0 ? Direction.Up : Direction.Down;
        }
    }
}
