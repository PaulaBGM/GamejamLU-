using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerToolRider : MonoBehaviour
{
    [SerializeField] private Transform mountPoint;
    [SerializeField] private ParticleSystem mountParticles;
    [SerializeField] private Sprite mopSprite;

    private MountableTool currentTool;
    private Animator animator;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Sprite originalSprite;
    public bool isOnBroom = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (playerSpriteRenderer != null)
            originalSprite = playerSpriteRenderer.sprite;
    }

    private void Update()
    {
        if (currentTool != null)
        {
            currentTool.HandleMovement();

            if (Input.GetKeyDown(KeyCode.Q))
                DismountTool();
        }
    }

    public void MountTool(GameObject toolPrefab)
    {
        if (currentTool != null) return;

        GameObject toolInstance = Instantiate(toolPrefab, mountPoint.position, Quaternion.identity);
        currentTool = toolInstance.GetComponent<MountableTool>();
        currentTool.Initialize(gameObject, mountPoint);
        currentTool.OnMounted();

        string tag = toolInstance.tag;

        if (animator != null)
        {
            animator.SetBool("Broom", tag == "Broom");
            animator.SetBool("Mop", tag == "Mop");
        }

        if (tag == "Mop" && playerSpriteRenderer != null && mopSprite != null)
        {
            playerSpriteRenderer.sprite = mopSprite;
        }
        else if (currentTool is BroomTool)
        { isOnBroom = true; }
           


        // Si es una fregona, cambiar el sprite de la herramienta (no del jugador)
        if (currentTool is MopTool && mopSprite != null)
        {
            SpriteRenderer mopRenderer = currentTool.GetComponentInChildren<SpriteRenderer>();
            if (mopRenderer != null)
            {
                mopRenderer.sprite = mopSprite;
            }
        }

        if (mountParticles != null)
            Instantiate(mountParticles, transform.position, Quaternion.identity).Play();
    }

    public void DismountTool()
    {
        if (currentTool == null) return;

        if (animator != null)
        {
            animator.SetBool("Broom", false);
            animator.SetBool("Mop", false);
        }

        if (playerSpriteRenderer != null && originalSprite != null)
        {
            playerSpriteRenderer.sprite = originalSprite;
        }

        currentTool.OnDismounted();
        Destroy(currentTool.gameObject);
        currentTool = null;

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public bool IsMounted() => currentTool != null;
}
