using UnityEngine;

public class ToolTaskManager : MonoBehaviour
{
    public static ToolTaskManager Instance;

    public int currentMopCount = 0;
    public int currentBroomCount = 0;

    private float _mopPercent;
    private float _broomPercent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {

       CheckBroom();
       CheckMop();

    }
    private void CheckBroom()
    {
        if (currentBroomCount > 0)
        {
            _broomPercent = (currentBroomCount / 8f) * 100;
            TaskManager.Instance.EndTask(2, _broomPercent);
        }
    }
    private void CheckMop()
    {
        if (currentMopCount > 0)
        {
            _mopPercent = (currentMopCount / 4f) * 100;
            TaskManager.Instance.EndTask(3, _mopPercent);
        }
    }
}
