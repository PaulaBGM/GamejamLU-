using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    
    [Header("Tasks")]
    [SerializeField]
    private Task WineTask;
    [SerializeField]
    private Task SweepTask;
    [SerializeField]
    private Task ScrubTask;
    [SerializeField]
    private Task DishesTask;
    [SerializeField]
    private Task ClothesTask;
    [SerializeField]
    private Task WashingMachineTask;
    [SerializeField]
    private Task HangClothesTask;
    
    [Header("Task Progress Images")]
    [SerializeField]
    private Image WineTaskProgress;
    [SerializeField]
    private Image SweepTaskProgress;
    [SerializeField]
    private Image ScrubTaskProgress;
    [SerializeField]
    private Image DishesTaskProgress;
    [SerializeField]
    private Image ClothesTaskProgress;
    [SerializeField]
    private Image WashingMachineTaskProgress;
    [SerializeField]
    private Image HangClothesTaskProgress;

    [Header("Result Images")]
    [SerializeField]
    private Sprite NiceResult;
    [SerializeField]
    private Sprite MediumResult;
    [SerializeField]
    private Sprite BadResult;
    [SerializeField]
    private Image CharacterHeader;
    [SerializeField]
    private Sprite CharacterHeaderBadSprite;
    [SerializeField]
    private Sprite CharacterHeaderMediumSprite;
    [SerializeField]
    private Sprite CharacterHeaderNiceSprite;

    [SerializeField]
    private GameObject _taskListPanel;
    [SerializeField]
    private GameObject _taskListButton;

    public float FinalPercent;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }
    }
    private void Start()
    {
        //Initialize all tasks as not completed
        WineTask.Completed = false;
        SweepTask.Completed = false;
        ScrubTask.Completed = false;
        DishesTask.Completed = false;
        ClothesTask.Completed = false;
        WashingMachineTask.Completed = false;
        HangClothesTask.Completed = false;
        //Set all task progress to 0
        WineTask.CompletedPercent = 0.0f;
        SweepTask.CompletedPercent = 0.0f;
        ScrubTask.CompletedPercent = 0.0f;
        DishesTask.CompletedPercent = 0.0f;
        ClothesTask.CompletedPercent = 0.0f;
        WashingMachineTask.CompletedPercent = 0.0f;
        HangClothesTask.CompletedPercent = 0.0f;
        //Set all task progress images to default (empty)
        WineTaskProgress.gameObject.SetActive(false);
        SweepTaskProgress.gameObject.SetActive(false);
        ScrubTaskProgress.gameObject.SetActive(false);
        DishesTaskProgress.gameObject.SetActive(false);
        ClothesTaskProgress.gameObject.SetActive(false);
        WashingMachineTaskProgress.gameObject.SetActive(false);
        HangClothesTaskProgress.gameObject.SetActive(false);

        CalculateFinalPercent();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            ToggleTaskList();
        }
    }
    public void EndTask(int id, float percent)
    {
        switch(id)
        {
            case 1:
                EndWineTask(percent);
                break;
            case 2:
                EndSweepTask(percent);
                break;
            case 3:
                EndScrubTask(percent);
                break;
            case 4:
                EndDishesTask(percent);
                break;
            case 5:
                EndClothesTask(percent);
                break;
            case 6:
                EndWashingMachineTask(percent);
                break;
            case 7:
                EndHangClothesTask(percent);
                break;
        }
    }
    private void EndWineTask(float percent)
    {
        WineTask.CompletedPercent = percent;
        WineTaskProgress.gameObject.SetActive(true);
        if (percent > 66.7f)
        {
            WineTask.Completed = true;
            WineTaskProgress.sprite = NiceResult;
        }
        else if (percent > 33.4f)
        {
            WineTask.Completed = true;
            WineTaskProgress.sprite = MediumResult;
        }
        else
        {
            WineTask.Completed = true;
            WineTaskProgress.sprite = BadResult;
        }
        CalculateFinalPercent();
    }
    private void EndSweepTask(float percent)
    {
        SweepTask.CompletedPercent = percent;
        SweepTaskProgress.gameObject.SetActive(true);
        if (percent > 66.7f)
        {
            SweepTask.Completed = true;
            SweepTaskProgress.sprite = NiceResult;
        }
        else if (percent > 33.4f)
        {
            SweepTask.Completed = true;
            SweepTaskProgress.sprite = MediumResult;
        }
        else
        {
            SweepTask.Completed = true;
            SweepTaskProgress.sprite = BadResult;
        }
        CalculateFinalPercent();
    }
    private void EndScrubTask(float percent)
    {
        ScrubTask.CompletedPercent = percent;
        ScrubTaskProgress.gameObject.SetActive(true);
        if (percent > 66.7f)
        {
            ScrubTask.Completed = true;
            ScrubTaskProgress.sprite = NiceResult;
        }
        else if (percent > 33.4f)
        {
            ScrubTask.Completed = true;
            ScrubTaskProgress.sprite = MediumResult;
        }
        else
        {
            ScrubTask.Completed = true;
            ScrubTaskProgress.sprite = BadResult;
        }
        CalculateFinalPercent();
    }
    private void EndDishesTask(float percent)
    {
        DishesTask.CompletedPercent = percent;
        DishesTaskProgress.gameObject.SetActive(true);
        if (percent > 66.7f)
        {
            DishesTask.Completed = true;
            DishesTaskProgress.sprite = NiceResult;
        }
        else if (percent > 33.4f)
        {
            DishesTask.Completed = true;
            DishesTaskProgress.sprite = MediumResult;
        }
        else
        {
            DishesTask.Completed = true;
            DishesTaskProgress.sprite = BadResult;
        }
        CalculateFinalPercent();
    }
    private void EndClothesTask(float percent)
    {
        ClothesTask.CompletedPercent = percent;
        ClothesTaskProgress.gameObject.SetActive(true);
        if (percent > 66.7f)
        {
            ClothesTask.Completed = true;
            ClothesTaskProgress.sprite = NiceResult;
        }
        else if (percent > 33.4f)
        {
            ClothesTask.Completed = true;
            ClothesTaskProgress.sprite = MediumResult;
        }
        else
        {
            ClothesTask.Completed = true;
            ClothesTaskProgress.sprite = BadResult;
        }
        CalculateFinalPercent();
    }
    private void EndWashingMachineTask(float percent)
    {
        WashingMachineTask.CompletedPercent = percent;
        WashingMachineTaskProgress.gameObject.SetActive(true);
        if (percent > 66.7f)
        {
            WashingMachineTask.Completed = true;
            WashingMachineTaskProgress.sprite = NiceResult;
        }
        else if (percent > 33.4f)
        {
            WashingMachineTask.Completed = true;
            WashingMachineTaskProgress.sprite = MediumResult;
        }
        else
        {
            WashingMachineTask.Completed = true;
            WashingMachineTaskProgress.sprite = BadResult;
        }
        CalculateFinalPercent();
    }
    private void EndHangClothesTask(float percent)
    {
        HangClothesTask.CompletedPercent = percent;
        HangClothesTaskProgress.gameObject.SetActive(true);
        if (percent > 66.7f)
        {
            HangClothesTask.Completed = true;
            HangClothesTaskProgress.sprite = NiceResult;
        }
        else if (percent > 33.4f)
        {
            HangClothesTask.Completed = true;
            HangClothesTaskProgress.sprite = MediumResult;
        }
        else
        {
            HangClothesTask.Completed = true;
            HangClothesTaskProgress.sprite = BadResult;
        }
        CalculateFinalPercent();
    }
    public void ToggleTaskList()
    {
        _taskListPanel.SetActive(!_taskListPanel.activeSelf);
        _taskListButton.SetActive(!_taskListPanel.activeSelf);
    }
    public void CalculateFinalPercent()
    {
        FinalPercent = (
            WineTask.CompletedPercent +
            SweepTask.CompletedPercent +
            ScrubTask.CompletedPercent +
            DishesTask.CompletedPercent +
            ClothesTask.CompletedPercent +
            WashingMachineTask.CompletedPercent +
            HangClothesTask.CompletedPercent) /
            7.0f;
        if (FinalPercent > 66.7f) 
        {
            CharacterHeader.sprite = CharacterHeaderNiceSprite;
        }
        else if (FinalPercent > 33.4f)
        {
            CharacterHeader.sprite = CharacterHeaderMediumSprite;
        }
        else
        {
            CharacterHeader.sprite = CharacterHeaderBadSprite;
        }
    }
}
