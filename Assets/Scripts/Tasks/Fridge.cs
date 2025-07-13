using UnityEngine;

public class Fridge : MonoBehaviour
{
    [SerializeField] private Transform storagePoint;
    private float _percent;
    [SerializeField]
    private WineProgressManager wineProgressManager;

    public void ReceiveWine(WineBottle bottle)
    {
        if (bottle == null) return;

        bottle.SetCollected(false);
        bottle.AttachTo(storagePoint, Vector3.zero);
        if(wineProgressManager.CurrentProgress < 1f)
        {
            _percent = 0.0f;
        }
        else if (wineProgressManager.CurrentProgress < 2.0f && wineProgressManager.CurrentProgress > 1.0f)
        {
            _percent = 33.3f;
        }
        else if (wineProgressManager.CurrentProgress < 3f && wineProgressManager.CurrentProgress > 2.0f)
        {
            _percent = 66.6f;
        }
        else
        {
            _percent = 100.0f;
        }

        TaskManager.Instance.EndTask(1, _percent);

        Rigidbody2D rb = bottle.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        Collider2D col = bottle.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log("La nevera ha recibido la botella de vino");
    }
}
