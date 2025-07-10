using UnityEngine;

// Este script genera cacas a intervalos desde una posición superior
public class PoopSpawner : MonoBehaviour
{
    public GameObject poopPrefab;      // Prefab de la caca
    public float spawnInterval = 2f;   // Tiempo entre spawns
    public float spawnRangeX = 6f;     // Rango horizontal aleatorio de aparición

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime; // Acumulamos tiempo
        if (timer >= spawnInterval)
        {
            // Posición aleatoria en el eje X dentro del rango
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, 0);
            Instantiate(poopPrefab, spawnPos, Quaternion.identity); // Creamos la caca
            timer = 0f; // Reiniciamos el temporizador
        }
    }
}
