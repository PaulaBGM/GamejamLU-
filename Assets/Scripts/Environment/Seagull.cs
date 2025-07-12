using UnityEngine;
using System;

public class Seagull : MonoBehaviour
{
    [SerializeField] private GameObject whitePoopPrefab;
    [SerializeField] private GameObject blackPoopPrefab;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float poopIntervalMin = 2f;
    [SerializeField] private float poopIntervalMax = 5f;
    [SerializeField] private float lifetime = 10f;

    [SerializeField] private AudioSource poop;
    [SerializeField] private AudioClip poopClip;

    private float poopTimer;
    private float lifetimeTimer = 0f;
    private Animator animator;

    public event Action OnSeagullDestroyed;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator no encontrado en el hijo de la gaviota.");

        if (poop == null)
            Debug.LogWarning("AudioSource 'poop' no está asignado.");
        if (poopClip == null)
            Debug.LogWarning("AudioClip 'poopClip' no está asignado.");

        ResetPoopTimer();
    }

    private void Update()
    {
        // Movimiento horizontal
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        lifetimeTimer += Time.deltaTime;
        poopTimer -= Time.deltaTime;

        // Soltar caca si toca
        if (poopTimer <= 0f)
        {
            DropPoop();
            ResetPoopTimer();
        }

        // Destruir al pasar el tiempo de vida
        if (lifetimeTimer >= lifetime)
        {
            OnSeagullDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void ResetPoopTimer()
    {
        poopTimer = UnityEngine.Random.Range(poopIntervalMin, poopIntervalMax);

    }

    private void DropPoop()
    {
        GameObject prefab = UnityEngine.Random.value < 0.5f ? whitePoopPrefab : blackPoopPrefab;
        Vector3 dropPosition = transform.position + Vector3.down;

        Instantiate(prefab, dropPosition, Quaternion.identity);

        if (poop != null && poopClip != null)
            poop.PlayOneShot(poopClip);

        if (animator != null)
            animator.SetTrigger("poop");
    }
}
