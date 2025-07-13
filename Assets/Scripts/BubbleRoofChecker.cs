using System.Collections.Generic;
using UnityEngine;

public class BubbleRoofChecker : MonoBehaviour
{
    [SerializeField]
    private GameObject _bubbleRoof;
    [SerializeField]
    private GameObject _bubbleSecondFloor;
    [SerializeField]
    private GameObject _bubbleFirstFloor;
    [SerializeField]
    private GameObject _bubbleCellar;

    private Dictionary<string, GameObject> _tagToBubble;

    private void Awake()
    {
        // Inicializamos el diccionario de forma segura
        _tagToBubble = new Dictionary<string, GameObject>
        {
            { "Roof",         _bubbleRoof        },
            { "SecondFloor",  _bubbleSecondFloor },
            { "FirstFloor",   _bubbleFirstFloor  },
            { "Cellar",       _bubbleCellar      }
        };
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_tagToBubble.TryGetValue(other.tag, out GameObject targetBubble))
        {
            ToggleAllBubblesExcept(targetBubble);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_tagToBubble.ContainsKey(other.tag))
        {
            ToggleAllBubblesExcept(null);
        }
    }

    private void ToggleAllBubblesExcept(GameObject bubbleToKeepActive)
    {
        foreach (GameObject bubble in _tagToBubble.Values)
        {
            if (bubble == null)
            {
                Debug.LogWarning("[BubbleRoofChecker] Falta asignar alguna burbuja en el Inspector.");
                continue;
            }

            bubble.SetActive(bubble == bubbleToKeepActive);
        }
    }
}
