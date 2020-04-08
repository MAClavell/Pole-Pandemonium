using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionGroup : MonoBehaviour
{
    [System.Serializable]
    public class OnSelectionChanged : UnityEvent<Button, Button>
    { }

    public Button Selected { get; private set; }

    [SerializeField] Button[] elements;
    [SerializeField] int defaultElement;
    [SerializeField] public OnSelectionChanged onSelectionChanged;

    int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (elements.Length <= 0)
            return;

        //Add add listeners for group elements
        for(int i = 0; i < elements.Length; i++)
        {
            int t = i;
            elements[t].onClick.AddListener(() => { Select(t); });
        }

        if (defaultElement < 0 || defaultElement >= elements.Length)
            defaultElement = 0;

        currentIndex = defaultElement;
        Select(currentIndex);
    }

    /// <summary>
    /// Change the selected element
    /// </summary>
    /// <param name="index">Index of the element to select</param>
    public void Select(int index)
    {
        int prev = currentIndex;
        currentIndex = index;
        
        Selected = elements[currentIndex];
        elements[prev].interactable = true;
        Selected.interactable = false;

        onSelectionChanged.Invoke(elements[prev], Selected);
    }
}
