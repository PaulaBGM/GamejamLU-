using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBroomController : MonoBehaviour
{
    private Rigidbody2D rb;
    private MountableTool currentTool;

    public Transform mountPoint; // GameObject que se activa al montar
    private GameObject toolObject; // Instancia de la herramienta
    private bool onTool = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (mountPoint != null)
            mountPoint.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Si hay una herramienta activa, delegamos su lógica
        if (onTool && currentTool != null)
        {
            currentTool.HandleMovement();

            // Desmontar con Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DismountTool();
            }
        }
    }

    public void MountBroom(GameObject toolPrefab, ParticleSystem particles)
    {
        if (onTool || toolPrefab == null) return;

        // Efecto visual de montaje
        if (particles != null)
        {
            Instantiate(particles, transform.position, Quaternion.identity).Play();
        }

        // Instanciamos la herramienta y la conectamos al jugador
        toolObject = Instantiate(toolPrefab, mountPoint.position, Quaternion.identity);
        currentTool = toolObject.GetComponent<MountableTool>();

        if (currentTool != null)
        {
            currentTool.Initialize(gameObject, mountPoint);
            currentTool.OnMounted();
        }

        if (mountPoint != null)
            mountPoint.gameObject.SetActive(true); // Activamos el punto de montaje

        onTool = true;
        Debug.Log("Montado en herramienta: " + toolObject.name);
    }

    public void DismountTool()
    {
        if (!onTool) return;

        if (currentTool != null)
        {
            currentTool.OnDismounted();
        }

        if (toolObject != null)
            Destroy(toolObject);

        if (mountPoint != null)
            mountPoint.gameObject.SetActive(false);

        currentTool = null;
        toolObject = null;
        onTool = false;

        Debug.Log("Desmontado de herramienta");
    }

    public bool IsOnBroom()
    {
        return onTool;
    }
}
