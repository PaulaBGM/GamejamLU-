using UnityEngine;

public class TendalsControllers : MonoBehaviour
{
    [SerializeField]
    private Clothesline _firstClothesline;
    [SerializeField]
    private Clothesline _secondClothesline;
    [SerializeField]
    private Clothesline _thirdClothesline;

    private void Update()
    {
        float summatory = _firstClothesline.hangingClothes.Count +
                          _secondClothesline.hangingClothes.Count +
                          _thirdClothesline.hangingClothes.Count;
        if (summatory > 0.0f)
        {
            TaskManager.Instance.EndTask(7, (summatory / 6) * 100);
        }
    }
}
