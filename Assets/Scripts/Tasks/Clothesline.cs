using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Clothesline : MonoBehaviour
{
    [Header("Puntos para colgar ropa")]
    [SerializeField] private Transform[] hangPoints; // Posiciones físicas donde se cuelgan las prendas
    private readonly List<PickupItem> hangingClothes = new(); // Lista de ropa colgada
    private bool[] usedHangPoints; // Para marcar los puntos ya usados

   

    private PlayerPickUp playerInZone;

    private void Awake()
    {
        usedHangPoints = new bool[hangPoints.Length];
    }

    private void OnEnable()
    {
        // Reinicia los puntos usados si se vuelve a activar el objeto
        if (usedHangPoints.Length != hangPoints.Length)
            usedHangPoints = new bool[hangPoints.Length];
    }

    private void Update()
    {
        if (playerInZone != null)
        {
            Debug.Log("Player está en el tendal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Espacio presionado dentro del tendal");
                TryHangClothes(playerInZone);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player en el tendal");
            playerInZone = other.GetComponent<PlayerPickUp>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = null;
        }
    }

    /// <summary>
    /// Intenta colgar la primera prenda limpia que lleve el jugador.
    /// </summary>
    private void TryHangClothes(PlayerPickUp player)
    {
        int freeIndex = GetNextAvailableHangPointIndex();
        if (freeIndex == -1) return; // Todos los puntos están ocupados

        PickupItem cleanItem = player.RemoveFirstCleanItem();
        if (cleanItem != null)
        {
            HangClothes(cleanItem, freeIndex);
        }
    }

    /// <summary>
    /// Cuelga la prenda en el punto de colgado indicado.
    /// </summary>
    private void HangClothes(PickupItem item, int index)
    {
        Transform hangPoint = hangPoints[index];
        item.transform.SetParent(hangPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.StopMovement();
        item.SetCleanSprite();

        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Kinematic;

        usedHangPoints[index] = true;
        hangingClothes.Add(item);
    }

    /// <summary>
    /// Busca el próximo punto disponible para colgar ropa.
    /// </summary>
    private int GetNextAvailableHangPointIndex()
    {
        for (int i = 0; i < hangPoints.Length; i++)
        {
            if (!usedHangPoints[i])
                return i;
        }
        return -1; // Todos los puntos están ocupados
    }

    /// <summary>
    /// Devuelve la lista de prendas colgadas (puede servir para sistemas como lluvia, gaviotas, etc.).
    /// </summary>
    public List<PickupItem> GetHangingClothes()
    {
        return hangingClothes;
    }
}
