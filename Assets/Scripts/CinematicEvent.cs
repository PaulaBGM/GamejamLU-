using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicEvent : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _cinematicPanels;

    private int _currentIndex = 0;

    private void Awake()
    {
        foreach (GameObject panel in _cinematicPanels)
        {
            panel.SetActive(false);
        }
    }

    private void Start()
    {
        if (_cinematicPanels.Count > 0)
        {
            _cinematicPanels[0].SetActive(true);
            _currentIndex = 1; // El índice apunta al siguiente que se debe mostrar
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GoNextPanel();
        }
    }

    private void GoNextPanel()
    {
        if (_currentIndex < _cinematicPanels.Count)
        {
            _cinematicPanels[_currentIndex].SetActive(true);
            _currentIndex++;
        }
        else
        {
            // Ya hemos mostrado todos los paneles, ir a la siguiente escena
            SceneManager.LoadScene(2);
        }
    }
}