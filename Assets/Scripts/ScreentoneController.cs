using UnityEngine;

public class ScreentoneController : MonoBehaviour
{
    public Material screentoneMaterial;
    public Camera mainCamera;
    
    [Header("Expansion Settings")]
    public float expansionSpeed = 1f;
    public float shrinkSpeed = 8f;
    public float maxRadius = 0.8f; 

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

    }
    void Update()
    {
        // 1. Calculate how much the player moved in world space
        float movement = Vector2.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if (movement > 0.001f) 
        {
            stopTimer = 0f;
            isShrinking = false;

            // Expand the radius smoothly up to max size
            currentRadius += expansionSpeed * Time.deltaTime;
            currentRadius = Mathf.Min(currentRadius, maxRadius);
        }
        else 
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

                if (currentRadius <= 0.01f) 
                {
                    currentRadius = 0f;
                    isShrinking = false;
                }
            }
        }

        //  Map 2D world position directly to 0-1 Viewport Space coordinates
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
        Vector2 screenPos = new Vector2(viewportPoint.x, viewportPoint.y);

        // Update Fullscreen Material properties instantly
        screentoneMaterial.SetVector("_playerPos", screenPos);
        screentoneMaterial.SetFloat("_maskRadius", currentRadius);
    }
}
