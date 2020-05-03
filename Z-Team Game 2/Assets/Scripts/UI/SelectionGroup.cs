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

    public int defaultElement;
    [SerializeField] Button[] elements;
    [SerializeField] public OnSelectionChanged onSelectionChanged;

    int currentIndex;

    // Start is called before the first frame update
    void Awake()
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
        SelectInternal(currentIndex);
    }

    /// <summary>
    /// Internal method for selecting indices
    /// </summary>
    /// <param name="index">Index of the element to select</param>
    /// <returns>The previous element selected</returns>
    private int SelectInternal(int index)
    {
        int prev = currentIndex;
        currentIndex = index;

        Selected = elements[currentIndex];
        elements[prev].interactable = true;
        Selected.interactable = false;
        return prev;
    }

    /// <summary>
    /// Change the selected element
    /// </summary>
    /// <param name="index">Index of the element to select</param>
    public void Select(int index)
    {
        int prev = SelectInternal(index);
        onSelectionChanged.Invoke(elements[prev], Selected);
    }

    /// <summary>
    /// Change the selected element without invoking the selection event
    /// </summary>
    /// <param name="index">Index of the element to select</param>
    public void SelectNoInvoke(int index)
    {
        SelectInternal(index);
    }
}
