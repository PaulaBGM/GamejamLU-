using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerMechanic : MonoBehaviour
{
    private bool _timerRunning;
    private bool _isOnFirstHalf;
    private bool _isOnHalfTime;
    private bool _isOnLastMinute;
    private float _timer;
    [SerializeField]
    private TextMeshProUGUI _timerText;
    private void Awake()
    {
        _timer = 0.0f;
        _isOnFirstHalf = true;
        _isOnHalfTime = false;
        _isOnLastMinute = false;
        StartTimer();
    }
    private void Update()
    {
        if (_timerRunning && !OptionsMenu.Instance.IsOpen)
        {
            _timer += Time.deltaTime;
            _timerText.text = FormatTime(_timer);
        }
        CheckTimerState();
        CheckTimerColor();
    }
    private void CheckTimerState()
    {
        if (_timer < 150f)
        {
            _isOnFirstHalf = true;
            _isOnHalfTime = false;
            _isOnLastMinute = false;
        }
        else if (_timer < 240f)
        {
            _isOnFirstHalf = false;
            _isOnHalfTime = true;
            _isOnLastMinute = false;
        }
        else if (_timer < 300f)
        {
            _isOnFirstHalf = false;
            _isOnHalfTime = false;
            _isOnLastMinute = true;
        }
        else
        {
            ShowResults();
        }
    }
    private void CheckTimerColor()
    {
        if (_isOnFirstHalf)
        {
            _timerText.color = Color.green;
        }
        else if (_isOnHalfTime)
        {
            _timerText.color = Color.yellow;
        }
        else if (_isOnLastMinute)
        {
            _timerText.color = Color.red;
        }
    }
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void ResetTimer()
    {
        _timer = 0.0f;
        _timerText.text = FormatTime(_timer);
    }
    public void StartTimer()
    {
        _timerRunning = true;
    }
    public void StopTimer()
    {
        _timerRunning = false;
    }
    private void ShowResults()
    {
        SceneManager.LoadScene(4);
    }
}