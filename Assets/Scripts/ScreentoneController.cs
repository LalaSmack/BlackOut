using UnityEngine;

public class Screentone2DController : MonoBehaviour
{
    [Header("Dependencies")]
    public Material screentoneMaterial;
    public Camera mainCamera;
    [SerializeField]
    private float dotFrequency = 100f;
    
    [Header("Expansion Settings")]
    public float expansionSpeed = 4f;
    public float shrinkSpeed = 8f;
    public float maxRadius = 0.4f; // Normalized viewport range (0.5 reaches screen edges)

    [Header("Timing")]
    public float shrinkDuration = 0.5f;

    private float currentRadius = 0f;
    private Vector3 lastPosition;
    private float stopTimer = 0f;
    private bool isShrinking = false;

    void Start()
    {
        // Automatically find the main camera if left unassigned
        if (mainCamera == null) mainCamera = Camera.main;
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        // 1. Calculate physical movement distance in world space
        float movement = Vector2.Distance(transform.position, lastPosition);
        lastPosition = this.transform.position;

        if (movement > 0.001f) // Player is actively walking
        {
            stopTimer = 0f;
            isShrinking = false;

            // Expand the radius smoothly up to your max size
            currentRadius += expansionSpeed * Time.deltaTime;
            currentRadius = Mathf.Min(currentRadius, maxRadius);
        }
        else // Player is standing completely still
        {
            stopTimer += Time.deltaTime;

            // Wait exactly until the duration mark to start shrinking
            if (stopTimer >= shrinkDuration)
            {
                isShrinking = true;
            }

            if (isShrinking)
            {
                // Collapse the radius rapidly
                currentRadius -= shrinkSpeed * Time.deltaTime;

                // Create a sharp comic-book "pop" visual snap right at zero
                if (currentRadius <= 0.01f) 
                {
                    currentRadius = 0f;
                    isShrinking = false;
                }
            }
        }

        // 2. Map 2D world space coordinates to camera viewport screen data
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
        Vector2 screenPos = new Vector2(viewportPoint.x, viewportPoint.y);

        // 3. Update the material instance properties instantly
        if (screentoneMaterial != null)
        {
            screentoneMaterial.SetVector("_playerPos", screenPos);
            screentoneMaterial.SetFloat("_maskRadius", currentRadius);
            screentoneMaterial.SetFloat("_dotFrequency", dotFrequency);
        }
    }
}