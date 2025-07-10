using UnityEngine;
using System.Collections.Generic;

public class SeagullSpawner : MonoBehaviour
{
    [SerializeField] private GameObject seagullPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float spawnRangeX = 8f;

    private float timer = 0f;
    private List<GameObject> activeSeagulls = new();

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && activeSeagulls.Count < 2)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), transform.position.y, 0f);
            GameObject newSeagull = Instantiate(seagullPrefab, spawnPos, Quaternion.identity);
            activeSeagulls.Add(newSeagull);

            // Cuando la gaviota se destruya, la quitamos de la lista
            Seagull seagullScript = newSeagull.GetComponent<Seagull>();
            if (seagullScript != null)
                seagullScript.OnSeagullDestroyed += () => activeSeagulls.Remove(newSeagull);

            timer = 0f;
        }
    }
}
