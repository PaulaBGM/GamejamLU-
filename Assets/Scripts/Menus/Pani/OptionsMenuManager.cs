using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuManager : UIManager
{
    [Header("Options Menu Specific")]
    [SerializeField] private GameObject _optionsMenuPrefab;
    private GameObject _optionsMenuInstance;

    [SerializeField] private List<GameObject> _buttonsToDisableWhenOpen;

    private bool _isOptionsOpen = false;

    protected override void Awake()
    {
        base.Awake();
        // Si quieres que empiece cerrado o abierto, controla aquí:
        if (_optionsMenuInstance != null)
            _optionsMenuInstance.SetActive(false);
    }

    /// <summary>
    /// Muestra u oculta el menú de opciones.
    /// </summary>
    public void ToggleOptionsMenu()
    {
        if (_optionsMenuInstance == null)
        {
            _optionsMenuInstance = Instantiate(_optionsMenuPrefab, _mainCanvas.transform);
        }

        _isOptionsOpen = !_optionsMenuInstance.activeSelf;
        _optionsMenuInstance.SetActive(_isOptionsOpen);

        // Desactivar/activar botones que se deben deshabilitar al abrir opciones
        foreach (var btn in _buttonsToDisableWhenOpen)
        {
            if (btn != null)
                btn.SetActive(!_isOptionsOpen);
        }
    }
}
