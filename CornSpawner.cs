using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CornSpawner : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    public GameObject cornPrefab;     // Prefab del maíz
    public int numberOfCorn = 25;     // Cuántos maíces generar

    private BoxCollider2D spawnArea;

    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();

        GenerateCorn();
    }

    void GenerateCorn()
    {
        Bounds bounds = spawnArea.bounds;

        for (int i = 0; i < numberOfCorn; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            Vector2 spawnPosition = new Vector2(randomX, randomY);

            Instantiate(cornPrefab, spawnPosition, Quaternion.identity);
        }
    }
}