using UnityEngine;
using DG.Tweening;  

public class SunExplosion : MonoBehaviour
{
    [Header("Sun Settings")]
    [SerializeField] private GameObject sunObject;
    [SerializeField] private float explosionRadius = 100f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionDuration = 30f;

    void Start()
    {
        sunObject = GameObject.FindGameObjectWithTag("Sun");
        if (sunObject == null)
        {
            Debug.LogError("Sun object not found! Please ensure it has the tag 'Sun'.");
            return;
        }
    }

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
        // Scale the sun down before expanding. Turn it white when scaling down then blue when expanding
        sunObject.transform.DOScale(Vector3.one * 0.5f, 5f)
            .OnStart(() => sunObject.GetComponent<Renderer>().material.color = Color.white)
            .OnComplete(() => 
            {
                sunObject.transform.DOScale(Vector3.one * 10f, 10f)
                    .OnStart(() => sunObject.GetComponent<Renderer>().material.color = Color.blue);
            });
    }

    void TriggerExplosion()
    {
        // Get all objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        // Loop through all objects and kill them all
        foreach (GameObject obj in allObjects)
        {
            if(obj != sunObject) // Avoid affecting the sun itself
            {
                
            }
        }
    }
}
