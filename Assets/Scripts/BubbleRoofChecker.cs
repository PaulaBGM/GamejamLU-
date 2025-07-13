using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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

    private bool isOn = true;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_tagToBubble.TryGetValue(collision.tag, out GameObject targetBubble))
        {
            isOn = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_tagToBubble.TryGetValue(other.tag, out GameObject targetBubble) && isOn)
        {
            StartCoroutine(DeactivateWithTime(targetBubble));
        }
    }
    private IEnumerator DeactivateWithTime(GameObject targetBubble)
    {
        ToggleAllBubblesExcept(targetBubble);
        yield return new WaitForSeconds(6.0f);
        isOn = false;
        ToggleAllBubblesExcept(null);
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
