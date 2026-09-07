using UnityEngine;

public class ScreentoneController : MonoBehaviour
{// Rename this to public to make it explicitly clear
    public Material screentoneMaterial;
    public Camera mainCamera;
    
    [Header("Expansion Settings")]
    public float expansionSpeed = 4f;
    public float shrinkSpeed = 8f;
    public float maxRadius = 0.4f; 

    [Header("Timing")]
    public float shrinkDuration = 0.5f;

    private float currentRadius = 0f;
    private Vector3 lastPosition;
    private float stopTimer = 0f;
    private bool isShrinking = false;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        lastPosition = transform.position;

        // FORCE CHECK: If your Renderer Feature uses this material, 
        // we must make sure we are editing the exact asset instance.
        if (screentoneMaterial == null)
        {
            Debug.LogError("Screentone Material is missing from the Player Script!");
        }
    }
    void Update()
    {
        // 1. Calculate how much the player moved in world space
        float movement = Vector2.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if (movement > 0.001f) // Player is moving
        {
            stopTimer = 0f;
            isShrinking = false;

            // Expand the radius smoothly up to your max size
            currentRadius += expansionSpeed * Time.deltaTime;
            currentRadius = Mathf.Min(currentRadius, maxRadius);
        }
        else // Player has stopped moving
        {
            stopTimer += Time.deltaTime;

            // Wait exactly until the 0.5s mark to start shrinking
            if (stopTimer >= shrinkDuration)
            {
                isShrinking = true;
            }

            if (isShrinking)
            {
                // Collapse the radius rapidly
                currentRadius -= shrinkSpeed * Time.deltaTime;

                // Sharp "POP" visual snap when it hits zero
                if (currentRadius <= 0.01f) 
                {
                    currentRadius = 0f;
                    isShrinking = false;
                }
            }
        }

        // 2. Map 2D world position directly to 0-1 Viewport Space coordinates
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
        Vector2 screenPos = new Vector2(viewportPoint.x, viewportPoint.y);

        // 3. Update Fullscreen Material properties instantly
        screentoneMaterial.SetVector("_playerPos", screenPos);
        screentoneMaterial.SetFloat("_maskRadius", currentRadius);
    }
}
