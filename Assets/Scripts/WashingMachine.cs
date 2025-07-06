using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachine : MonoBehaviour
{
    [SerializeField] private float washDuration = 20f;
    [SerializeField] private AudioClip alarmSound;

    private AudioSource audioSource;
    private bool playerInRange = false;
    private bool isWashing = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playerInRange && !isWashing && Input.GetKeyDown(KeyCode.E))
        {
            TryReceiveItems();
        }
    }

    private void TryReceiveItems()
    {
        PlayerPickUp player = Object.FindFirstObjectByType<PlayerPickUp>();

        if (player != null && player.HasItems())
        {
            List<PickupItem> items = player.DropAllItemsTo(transform);

            // Destruye los objetos para que "desaparezcan" en la lavadora
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }

            StartCoroutine(WashItems());
        }
    }

    private IEnumerator WashItems()
    {
        isWashing = true;
        Debug.Log("Lavado Iniciado");

        yield return new WaitForSeconds(washDuration);

        Debug.Log("Lavado completado");
        isWashing = false;

        if (alarmSound && audioSource)
        {
            audioSource.PlayOneShot(alarmSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
