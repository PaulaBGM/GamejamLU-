using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [Header("Exit panel")]
    [SerializeField] private GameObject _exitPanel;
    [SerializeField] private List<GameObject> _menuButtons;
    [SerializeField] private List<GameObject> _buttonsToDisable;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hoverClip;
    [SerializeField] private AudioClip _clickClip;

    [Header("Scale effect")]
    [SerializeField] private float _scaleFactor = 1.1f;
    [SerializeField] private float _lerpSpeed = 12f;

    class BtnData
    {
        public RectTransform Rect;
        public Vector3 BaseScale;
        public bool Hovering;
    }
    private readonly Dictionary<GameObject, BtnData> _btnData = new();

    private void Awake()
    {
        _exitPanel.SetActive(false);

        if (_audioSource == null && Camera.main)
            _audioSource = Camera.main.GetComponent<AudioSource>();

        foreach (GameObject go in _menuButtons)
        {
            if (go == null) continue;

            _btnData[go] = new BtnData
            {
                Rect = go.GetComponent<RectTransform>(),
                BaseScale = go.transform.localScale,
                Hovering = false
            };

            AddEvent(go, EventTriggerType.PointerEnter, ev => OnHover(go, true));
            AddEvent(go, EventTriggerType.PointerExit, ev => OnHover(go, false));
            AddEvent(go, EventTriggerType.PointerClick, ev => OnClick(go));
        }
    }

    private void Update()
    {
        foreach (var kvp in _btnData)
        {
            BtnData b = kvp.Value;
            float targetX = b.Hovering ? b.BaseScale.x * _scaleFactor
                                        : b.BaseScale.x;

            Vector3 target = new Vector3(targetX, b.BaseScale.y, b.BaseScale.z);
            b.Rect.localScale = Vector3.Lerp(b.Rect.localScale, target, Time.deltaTime * _lerpSpeed);
        }
    }

    private void OnHover(GameObject go, bool enter)
    {
        _btnData[go].Hovering = enter;

        if (enter && _hoverClip && _audioSource)
            _audioSource.PlayOneShot(_hoverClip);
    }

    private void OnClick(GameObject go)
    {
        if (_clickClip && _audioSource)
            _audioSource.PlayOneShot(_clickClip);
    }

    private static void AddEvent(GameObject go, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> cb)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger) trigger = go.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new() { eventID = type };
        entry.callback.AddListener(cb);
        trigger.triggers.Add(entry);
    }

    public void StartGame() => SceneManager.LoadScene(1);
    public void ExitGame() => Application.Quit();
    public void ToggleExitPanel()
    {
        bool show = !_exitPanel.activeSelf;
        _exitPanel.SetActive(show);
        foreach (GameObject b in _buttonsToDisable) b.SetActive(!show);
    }

    public void ExitMenu () 
    { }
}
