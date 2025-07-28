using UnityEngine;
using DG.Tweening;  

public class SunExplosion : MonoBehaviour
{
    [Header("Sun Settings")]
    [SerializeField] private GameObject sunObject;
    [SerializeField] private float explosionRadius = 100f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionDuration = 5f;

    void Update()
    {
        // Check for backspace key press to trigger the explosion
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ExplosionAnimation();
            TriggerExplosion();
        }
    }

    void ExplosionAnimation()
    {
        // Scale the sun down before expanding
        sunObject.transform.DOScale(Vector3.zero, explosionDuration / 2f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                // Scale the sun back up after shrinking
                sunObject.transform.DOScale(Vector3.one * 10f, explosionDuration / 2f)
                    .SetEase(Ease.OutBack);
            });

    }

    void TriggerExplosion()
    {
        // Get all objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Loop through all objects and apply explosion force
        foreach (GameObject obj in allObjects)
        {
            // Check if the object has a Rigidbody component
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate the distance from the sun to the object
                float distance = Vector3.Distance(sunObject.transform.position, obj.transform.position);
                // Check if the object is within the explosion radius
                if (distance <= explosionRadius)
                {
                    // Calculate explosion force based on distance
                    Vector3 direction = (obj.transform.position - sunObject.transform.position).normalized;
                    rb.AddExplosionForce(explosionForce, sunObject.transform.position, explosionRadius);
                }
            }
        }
        // Optionally, destroy the sun object after the explosion
        Destroy(sunObject, explosionDuration);
    }
}
