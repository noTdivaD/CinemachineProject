using UnityEngine;
using Unity.Cinemachine;

public class CinemachineThirdPerson : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private CinemachineThirdPersonFollow cinemachineThirdPerson;

    // Initialize components
    void Start()
    {
        cinemachineThirdPerson = GetComponent<CinemachineThirdPersonFollow>();
        if (cinemachineThirdPerson == null)
            Debug.LogError("[CinemachineThirdPerson] CinemachineThirdPersonFollow component not found.");
    }

    // Camera movement
    void Update()
    {
        if (cinemachineThirdPerson != null)
        {
            // Get input
            float verticalInput = Input.GetAxis("Vertical");

            // Move the camera
            cinemachineThirdPerson.VerticalArmLength += verticalInput * rotationSpeed * Time.deltaTime;

            // Clamp vertical rotation to prevent flipping
            cinemachineThirdPerson.VerticalArmLength = Mathf.Clamp(cinemachineThirdPerson.VerticalArmLength, -90f, 90f);

            // Zoom in and out with mouse scroll wheel
            float zoomInput = Input.GetAxis("Mouse ScrollWheel");
            if (zoomInput != 0f)
            {
                cinemachineThirdPerson.CameraDistance += zoomInput * rotationSpeed * Time.deltaTime;
                cinemachineThirdPerson.CameraDistance = Mathf.Clamp(cinemachineThirdPerson.CameraDistance, 1f, 130f);
            }

            // Zoom in and out with A and E keys
            if (Input.GetKey(KeyCode.Q))
            {
                cinemachineThirdPerson.CameraDistance -= rotationSpeed * Time.deltaTime;
                cinemachineThirdPerson.CameraDistance = Mathf.Clamp(cinemachineThirdPerson.CameraDistance, 1f, 130f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                cinemachineThirdPerson.CameraDistance += rotationSpeed * Time.deltaTime;
                cinemachineThirdPerson.CameraDistance = Mathf.Clamp(cinemachineThirdPerson.CameraDistance, 1f, 130f);
            }
        }
    }
}
