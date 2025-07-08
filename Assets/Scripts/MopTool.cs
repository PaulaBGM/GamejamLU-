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
            GameObject zone = Instantiate(slipperyZonePrefab, owner.transform.position, Quaternion.identity);
            spawnTimer = spawnInterval;
        }
    }
}
