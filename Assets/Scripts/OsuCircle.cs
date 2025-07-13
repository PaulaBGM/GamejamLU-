using UnityEngine;

public class OsuCircle : MonoBehaviour
{
    [Header("Tamaños")]
    public Vector3 normalScale = Vector3.one;
    public Vector3 beatScale = Vector3.one * 1.2f;
    public float beatDuration = 0.2f;

    [Header("Contraer")]
    public Vector3 shrinkScale = Vector3.one * 0.5f;
    public float shrinkDuration = 0.3f;

    private bool isBeating = false;
    private float beatTimer = 0f;

    private bool isShrinking = false;
    private float shrinkTimer = 0f;

    void Start()
    {
        transform.localScale = normalScale;
    }

    void Update()
    {
        if (!isShrinking)
        {
            if (isBeating)
            {
                beatTimer += Time.deltaTime;
                float t = beatTimer / beatDuration;

                transform.localScale = Vector3.Lerp(beatScale, normalScale, t);

                if (t >= 1f)
                {
                    isBeating = false;
                    beatTimer = 0f;
                    transform.localScale = normalScale;
                }
            }
        }
        else
        {
            shrinkTimer += Time.deltaTime;
            float t = shrinkTimer / shrinkDuration;

            transform.localScale = Vector3.Lerp(normalScale, shrinkScale, t);

            if (t >= 1f)
            {
                isShrinking = false;
                shrinkTimer = 0f;
                transform.localScale = shrinkScale;
            }
        }
    }

    public void PulseBeat()
    {
        if (!isShrinking)
        {
            isBeating = true;
            beatTimer = 0f;
            transform.localScale = beatScale;
        }
    }

    public void TriggerShrink()
    {
        isShrinking = true;
        shrinkTimer = 0f;
    }
}