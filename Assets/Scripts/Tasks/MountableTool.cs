using UnityEngine;

/// <summary>
/// Clase base abstracta para herramientas montables como escoba o fregona.
/// No modifica el movimiento del jugador.
/// Cada herramienta puede definir su comportamiento espec�fico.
/// </summary>
public abstract class MountableTool : MonoBehaviour
{
    public float speed = 6f;

    [Header("Configuraci�n de montaje")]
    public Vector3 customOffset = Vector3.zero;     // Offset personalizado por herramienta
    public Vector3 playerOffset = Vector3.zero;     // Offset para ajustar la posici�n del sprite del jugador
    public GameObject mountPointObject;             // Este objeto se activar� al montar

    protected Transform mountPoint;                 // Referencia al punto de montaje del jugador
    protected Rigidbody2D rb;
    protected GameObject owner;

    /// <summary>
    /// Inicializa la herramienta montable.
    /// </summary>
    public virtual void Initialize(GameObject owner, Transform mountPoint)
    {
        this.owner = owner;
        this.mountPoint = mountPoint;
        rb = owner.GetComponent<Rigidbody2D>();

        // Padres e inicializamos posiciones relativas
        transform.SetParent(owner.transform);
        transform.localPosition = customOffset;

        // Aplicamos el offset al jugador
        if (mountPoint != null)
        {
            mountPoint.localPosition = playerOffset;
            mountPoint.gameObject.SetActive(true);
        }

        // Activamos el objeto visual si existe
        if (mountPointObject != null)
            mountPointObject.SetActive(true);

        OnMounted();
    }

    /// <summary>
    /// Se llama al desmontar la herramienta.
    /// </summary>
    public virtual void OnDismounted()
    {
        // Restaurar el estado inicial
        if (mountPoint != null)
            mountPoint.gameObject.SetActive(false);

        if (mountPointObject != null)
            mountPointObject.SetActive(false);

        transform.SetParent(null);
    }

    /// <summary>
    /// Sobreescribible: cada herramienta puede aplicar su l�gica sin afectar el movimiento.
    /// </summary>
    public virtual void HandleMovement()
    {
        // Este m�todo puede ser sobreescrito por herramientas para comportamientos espec�ficos.
    }

    /// <summary>
    /// Se llama autom�ticamente al montar.
    /// </summary>
    protected virtual void OnMounted()
    {
        // Puedes poner l�gica visual o de sonido base aqu� si lo deseas.
    }
}
