using UnityEngine;

public class MopTool : MountableTool
{
    public GameObject slipperyZonePrefab;
    public float spawnInterval = 0.5f;

    private float spawnTimer;

    public override void OnMounted()
    {
        spawnTimer = 0f;
    }

    public override void HandleMovement()
    {
        base.HandleMovement();

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            Vector3 spawnPos = owner.transform.position;
            spawnPos.y = 28.23f; // Asegura que se instale en el plano del suelo

            Instantiate(slipperyZonePrefab, spawnPos, Quaternion.identity);
            spawnTimer = spawnInterval;
        }
    }

}