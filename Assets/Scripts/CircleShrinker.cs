using UnityEngine;

/// <summary>
/// Hace que el círculo se contraiga suavemente.
/// </summary>
public class CircleShrinker : MonoBehaviour
{
    private Vector3 targetScale;
    private float duration;
    private Vector3 startScale;
    private float timer = 0f;

    public void Initialize(Vector3 target, float time)
    {
        targetScale = target;
        duration = time;
        startScale = transform.localScale;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / duration);
        transform.localScale = Vector3.Lerp(startScale, targetScale, t);

        if (t >= 1f)
        {
            // Opcional: cuando termina
            Destroy(this); // solo destruye el script, no el círculo
        }
    }
}