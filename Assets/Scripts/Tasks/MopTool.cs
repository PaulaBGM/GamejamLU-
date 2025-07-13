using UnityEngine;

public class MopTool : MountableTool
{
    [Header("Slippery Zone")]
    [SerializeField] private GameObject slipperyZonePrefab;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private Transform spawnpoint;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite mountedSprite;

    [Header("Mop Hitbox")]
    [SerializeField] private GameObject mopHitbox; // arrástralo en el Inspector

    private float spawnTimer;
    private SpriteRenderer playerSpriteRenderer;
    private Sprite originalSprite;
    private bool spriteChanged = false;

    public override void OnMounted()
    {
        spawnTimer = 0f;
        if (mopHitbox != null) mopHitbox.SetActive(true);

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
        if (mopHitbox != null) mopHitbox.SetActive(false);

        if (spriteChanged && playerSpriteRenderer != null && originalSprite != null)
        {
            playerSpriteRenderer.sprite = originalSprite;
            spriteChanged = false;
        }
    }
}
