using UnityEngine;

public class MopTool : MountableTool
{
    [Header("Slippery Zone")]
    [SerializeField] private GameObject slipperyZonePrefab;
    [SerializeField] private float spawnInterval = 0.5f;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite mountedSprite;

    private float spawnTimer;
    private SpriteRenderer playerSpriteRenderer;
    private Sprite originalSprite;
    private bool spriteChanged = false;

    public override void OnMounted()
    {
        spawnTimer = 0f;

        if (!spriteChanged && owner != null)
        {
            if (playerSpriteRenderer == null)
                playerSpriteRenderer = owner.GetComponentInChildren<SpriteRenderer>();

            if (playerSpriteRenderer != null && mountedSprite != null)
            {
                originalSprite = playerSpriteRenderer.sprite;
                playerSpriteRenderer.sprite = mountedSprite;
                spriteChanged = true;
            }
        }
    }

    public override void OnDismounted()
    {
        if (spriteChanged && playerSpriteRenderer != null && originalSprite != null)
        {
            playerSpriteRenderer.sprite = originalSprite;
            spriteChanged = false;
        }
    }

    public override void HandleMovement()
    {
        base.HandleMovement();

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            Vector3 spawnPos = owner.transform.position;

            Instantiate(slipperyZonePrefab, spawnPos, Quaternion.identity);
            spawnTimer = spawnInterval;
        }
    }
}
