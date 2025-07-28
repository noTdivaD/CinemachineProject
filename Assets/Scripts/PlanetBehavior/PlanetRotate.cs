using UnityEngine;
using Unity.Cinemachine;

public class PlanetRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float orbitRadius;
    [SerializeField] private Vector3 orbitAxis;

    [Header("Camera Prefab")]
    [SerializeField] private GameObject planetCameraPrefab;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private GameObject cameraLookTarget;

    [Header("Sun Object")]
    [SerializeField] private GameObject sunObject;
    [SerializeField] private Vector3 sunPosition;

    [Header("Orbit Settings")]
    [SerializeField] private LineRenderer orbitLineRenderer;
    private LineRenderer orbitInstance;
    private int orbitSegments = 100;

    [Header("Planet Collider")]
    [SerializeField] private MeshCollider planetCollider;

    void Start()
    {
        // Get the sun object by tag
        sunObject = GameObject.FindGameObjectWithTag("Sun");
        orbitLineRenderer = Resources.Load<LineRenderer>("Prefabs/OrbitLineRenderer");
        planetCameraPrefab = Resources.Load<GameObject>("Prefabs/PlanetCamera");

        if (sunObject == null || orbitLineRenderer == null || planetCameraPrefab == null)
        {
            Debug.LogError("[PlanetRotateAroundSun] Error at start. Check for sun tags and prefabs.");
            return;
        }

        // Get the position of the sun
        sunPosition = sunObject.transform.position;

        // Randomize rotation settings
        rotationSpeed = Random.Range(10f, 200f);
        orbitRadius = Random.Range(30f, 120f);
        orbitAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // Set the initial position of the planet
        Vector3 startPosition = sunPosition + new Vector3(orbitRadius, 0, 0);
        startPosition.y = sunPosition.y;
        transform.position = startPosition;

        // Instantiate a new orbit line renderer for this planet
        orbitInstance = Instantiate(orbitLineRenderer);
        orbitInstance.positionCount = orbitSegments + 1;
        orbitInstance.loop = true;

        // Draw the orbit path
        DrawOrbit();

        // Spawn Cinemachine Virtual Camera
        GameObject cameraInstance = Instantiate(planetCameraPrefab);

        // Set the camera
        cinemachineCamera = cameraInstance.GetComponent<CinemachineCamera>();
        cameraLookTarget = new GameObject($"{gameObject.name}_CameraLookTarget");
        cameraLookTarget.transform.position = transform.position + transform.forward;
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = transform;
            cinemachineCamera.LookAt = cameraLookTarget.transform;
        }else
        {
            Debug.LogError("[PlanetRotate] CinemachineCamera component not found on the camera prefab.");
            return;
        }

        // Set the MeshCollider
        planetCollider = GetComponent<MeshCollider>();
        if(planetCollider == null)
        {
            Debug.LogError("[PlanetRotate] MeshCollider component not found on the planet.");
            return;
        }
    }

    void Update()
    {
        // Calculate orbital speed based on scientific stuff
        float orbitalSpeed = Mathf.Sqrt(1f / orbitRadius) * rotationSpeed;

        // Calculate spin speed based on size
        float planetSize = transform.localScale.magnitude;
        float spinSpeed = 50f / planetSize;

        // Orbit around the sun
        transform.RotateAround(sunPosition, Vector3.up, orbitalSpeed * Time.deltaTime);

        // Spin on itself
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    void DrawOrbit()
    {
        // Draw the orbit
        for (int i = 0; i <= orbitSegments; i++)
        {
            float angle = (float)i / orbitSegments * 2 * Mathf.PI;

            // Calculate point on a circle in local space
            Vector3 pointOnCircle = new Vector3(
              Mathf.Cos(angle) * orbitRadius,
              0,
              Mathf.Sin(angle) * orbitRadius
            );

            // Rotate point to match the orbit axis
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
            Vector3 rotatedPoint = rotation * pointOnCircle;

            // Set position in world space relative to the sun
            Vector3 orbitPos = sunPosition + pointOnCircle;
            orbitPos.y = sunPosition.y;
            orbitInstance.SetPosition(i, orbitPos);
        }
    }

    void OnMouseDown()
    {
        Debug.Log("[PlanetRotate] Camera switch triggered");

        // Increase this planet's camera priority
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Priority = 20;
        }

        // Lower priority of all other virtual cameras
        foreach (var cam in FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
        {
            if (cam != cinemachineCamera)
                cam.Priority = 1;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sun"))
        {
            Debug.Log("[PlanetRotate] Planet collided with the sun. Destroying planet.");
            Destroy(gameObject);
        }
    }
   
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sun"))
        {
            Debug.Log("[PlanetRotate] Planet is still colliding with the sun.");
            Destroy(gameObject);
        }
    }
}