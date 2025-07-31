using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerToolRider : MonoBehaviour
{
    [SerializeField] private Transform mountPoint;
    [SerializeField] private Transform broomtoolMountZone; // Nuevo: zona donde dejar herramientas
    [SerializeField] private Transform moptoolMountZone; // Nuevo: zona donde dejar herramientas
    [SerializeField] private ParticleSystem mountParticles;
    [SerializeField] private Sprite mopSprite;

    private MountableTool currentTool;
    private GameObject currentToolPrefab; // Nuevo: para recordar qué prefab se montó
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

            if (InputManager.Instance != null && InputManager.Instance.CurrentInput != null)
            {
                if (InputManager.Instance.CurrentInput.DismountPressed())
                    DismountTool();
            }
        }
    }


    public void MountTool(GameObject toolPrefab)
    {
        if (currentTool != null) return;

        currentToolPrefab = toolPrefab; // Guardamos referencia al prefab original
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
            isOnBroom = true;
            playerSpriteRenderer.sprite = mopSprite;
        }
        else if (currentTool is BroomTool)
        {
            isOnBroom = true;
        }

        if (currentTool is MopTool && mopSprite != null)
        {
            SpriteRenderer mopRenderer = currentTool.GetComponentInChildren<SpriteRenderer>();
            if (mopRenderer != null)
                mopRenderer.sprite = mopSprite;
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

        // Guardar la rotación del objeto actual (por si es importante)
        Quaternion toolRotation = currentTool.transform.rotation;

        currentTool.OnDismounted();
        Destroy(currentTool.gameObject);
        currentTool = null;

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // Respawnear herramienta desmontada en toolMountZone
        if (broomtoolMountZone != null && currentToolPrefab != null)
        {
            if (currentTool is BroomTool) 
            {
                Instantiate(currentToolPrefab, broomtoolMountZone.position, toolRotation);
                currentToolPrefab = null; // Limpiar referencia si solo se monta una vez
            }

            if (currentTool is MopTool)
            {
                Instantiate(currentToolPrefab, moptoolMountZone.position, toolRotation);
                currentToolPrefab = null; // Limpiar referencia si solo se monta una vez
            }
            
        }

        isOnBroom = false;
    }

    public bool IsMounted() => currentTool != null;
}
