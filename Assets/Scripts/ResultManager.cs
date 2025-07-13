using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [Header("Result GameObjects")]
    [SerializeField]
    private GameObject _goodResult;
    [SerializeField]
    private GameObject _mediumResult;
    [SerializeField]
    private GameObject _badResult;

    [Header("Audio")]
    [SerializeField]
    private AudioClip _goodResultAudio;
    [SerializeField]
    private AudioClip _mediumResultAudio;
    [SerializeField]
    private AudioClip _badResultAudio;

    private AudioSource _audioSource;

    private void Awake()
    {
        _goodResult.SetActive(false);
        _mediumResult.SetActive(false);
        _badResult.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        // Assuming FinalPercent is set somewhere in your game logic
        float finalPercent = TaskManager.Instance.FinalPercent;
        if (finalPercent >= 66.6f)
        {
            _goodResult.SetActive(true);
            _audioSource.PlayOneShot(_goodResultAudio);
        }
        else if (finalPercent >= 33.3f)
        {
            _mediumResult.SetActive(true);
            _audioSource.PlayOneShot(_mediumResultAudio);
        }
        else
        {
            _badResult.SetActive(true);
            _audioSource.PlayOneShot(_badResultAudio);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
