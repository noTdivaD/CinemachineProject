using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [Header("Planet Prefab")]
    [SerializeField] private GameObject starPrefab;

    void Start()
    {
        // Get the planet prefab from Resources
        starPrefab = Resources.Load<GameObject>("Prefabs/StarPrefab");
        if (starPrefab == null)
        {
            Debug.LogError("[StarSpawner] Star prefab not found in Resources/Prefabs.");
            return;
        }

        // Spawn multiple planets at the start
        for (int i = 0; i < 2000; i++)
            SpawnStar();
    }

    void SpawnStar()
    {
        // Spawn the star
        GameObject starInstance = Instantiate(starPrefab);

        // Randomize star color 
        Renderer starRenderer = starInstance.GetComponent<Renderer>();
        starRenderer.material.color = new Color(Random.value, Random.value, Random.value);

        // Set color and shader properties
        MeshRenderer rend = starInstance.GetComponent<MeshRenderer>();
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block);
        block.SetColor("_ObjectColor", starRenderer.material.color);
        rend.SetPropertyBlock(block);

        // Randomize star size
        float randomScale = Random.Range(1f, 2f);
        starInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        // Set a random position in space
        Vector3 randomPosition = Random.onUnitSphere * Random.Range(500f, 2000f);
        starInstance.transform.position = randomPosition;
    }
}
