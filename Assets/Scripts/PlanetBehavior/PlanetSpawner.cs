using UnityEngine;
using System.Collections.Generic;

public class PlanetSpawner : MonoBehaviour
{
    [Header("Planet Prefab")]
    [SerializeField] private GameObject planetPrefab;
    
    [Header("Spawn Settings")]
    [SerializeField] private int planetCount = 5;
    [SerializeField] private int maxSpawnAttempts = 100;
    private List<Vector3> spawnPositions = new List<Vector3>();

    void Start()
    {
        // Get the planet prefab from Resources
        planetPrefab = Resources.Load<GameObject>("Prefabs/PlanetPrefab");
        if (planetPrefab == null)
        {
            Debug.LogError("[PlanetSpawner] Planet prefab not found in Resources/Prefabs.");
            return;
        }

        // Spawn multiple planets at the start
        for (int i = 0; i < planetCount; i++)
            SpawnPlanet();
    }

    void SpawnPlanet()
    {
        // Find a suitable spawn position
        bool positionFound = false;
        int attempts = 0;

        while (!positionFound && attempts < maxSpawnAttempts)
        {
            attempts++;
            Vector3 spawnPosition = Random.onUnitSphere * Random.Range(30f, 120f);

            // Check if the position is too close to existing planets
            bool tooClose = false;
            foreach (Vector3 pos in spawnPositions)
            {
                if (Vector3.Distance(spawnPosition, pos) < 20f)
                {
                    tooClose = true;
                    break;
                }
            }   
            if (!tooClose)
            {
                spawnPositions.Add(spawnPosition);
                positionFound = true;
            }
        }
       
        // Spawn the planet
        GameObject planetInstance = Instantiate(planetPrefab);

        // Randomize planet color 
        Renderer planetRenderer = planetInstance.GetComponent<Renderer>();
        planetRenderer.material.color = new Color(Random.value, Random.value, Random.value);

        // Set color and shader properties
        MeshRenderer rend = planetInstance.GetComponent<MeshRenderer>();
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block);
        block.SetColor("_ObjectColor", planetRenderer.material.color);
        rend.SetPropertyBlock(block);

        // Randomize planet size
        float randomScale = Random.Range(1f, 5f);
        planetInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
    