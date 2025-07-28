using UnityEngine;
using Unity.Cinemachine;

public class CinemachineRotateOrbital : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private CinemachineOrbitalFollow cinemachineOrbital;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    // Initialize components
    void Start()
    {
        cinemachineOrbital = GetComponent<CinemachineOrbitalFollow>();
        if (cinemachineOrbital == null)
            Debug.LogError("[CinemachineOrbital] CinemachineOrbitalFollow component not found.");

        cinemachineCamera = GetComponent<CinemachineCamera>();
        if (cinemachineCamera == null)
            Debug.LogError("[CinemachineOrbital] CinemachineCamera component not found.");
    }

    // Camera movement
    void Update()
    {
        if (cinemachineOrbital != null)
        {
            // Get input
            float horizontalInput = -Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Rotate the camera
            cinemachineOrbital.HorizontalAxis.Value += horizontalInput * rotationSpeed * Time.deltaTime;
            cinemachineOrbital.VerticalAxis.Value += verticalInput * rotationSpeed * Time.deltaTime;

            // Clamp vertical rotation to prevent flipping
            cinemachineOrbital.VerticalAxis.Value = Mathf.Clamp(cinemachineOrbital.VerticalAxis.Value, -90f, 90f);

            // Zoom in and out with mouse scroll wheel
            float zoomInput = Input.GetAxis("Mouse ScrollWheel");
            if (zoomInput != 0f)
            {
                cinemachineOrbital.Radius += zoomInput * rotationSpeed * Time.deltaTime;
                cinemachineOrbital.Radius = Mathf.Clamp(cinemachineOrbital.Radius, 1f, 130f);
            }

            // Zoom in and out with A and E keys
            if (Input.GetKey(KeyCode.Q))
            {
                cinemachineOrbital.Radius -= rotationSpeed * Time.deltaTime;
                cinemachineOrbital.Radius = Mathf.Clamp(cinemachineOrbital.Radius, 1f, 130f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                cinemachineOrbital.Radius += rotationSpeed * Time.deltaTime;
                cinemachineOrbital.Radius = Mathf.Clamp(cinemachineOrbital.Radius, 1f, 130f);
            }
        }

        // Center camera on the target when pressing the space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
    }
}
