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

    private float poopTimer;
    private float lifetimeTimer = 0f;

    public event Action OnSeagullDestroyed;

    private void Start()
    {
        ResetPoopTimer();
    }

    private void Update()
    {
        // Movimiento horizontal
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        lifetimeTimer += Time.deltaTime;
        poopTimer -= Time.deltaTime;

        // Suelta una caca al azar (blanca o negra)
        if (poopTimer <= 0f)
        {
            DropPoop();
            ResetPoopTimer();
        }

        // Destruye la gaviota tras cierto tiempo
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
        GameObject poopPrefab = UnityEngine.Random.value < 0.5f ? whitePoopPrefab : blackPoopPrefab;
        Instantiate(poopPrefab, transform.position + Vector3.down, Quaternion.identity);
    }
}
