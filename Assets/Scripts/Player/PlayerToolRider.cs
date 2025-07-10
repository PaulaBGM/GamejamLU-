using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerToolRider : MonoBehaviour
{
    public Transform mountPoint;
    public ParticleSystem mountParticles;

    private MountableTool currentTool;

    void Update()
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

        if (mountParticles != null)
            Instantiate(mountParticles, transform.position, Quaternion.identity).Play();
    }

    public void DismountTool()
    {
        if (currentTool == null) return;

        currentTool.OnDismounted();
        Destroy(currentTool.gameObject);
        currentTool = null;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public bool IsMounted() => currentTool != null;
}