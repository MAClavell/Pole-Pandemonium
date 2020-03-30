using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickEnemyLimb : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private StickEnemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = transform.parent.GetComponentInChildren<StickEnemy>();
    }

    /// <summary>
    /// Calculate the offset for dragging sticky enemies off the pole
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        enemyScript.OnBeginDrag(transform, eventData.position);
    }

    /// <summary>
    /// Drag the sticky enemy off of the pole
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        enemyScript.OnDrag(eventData.position);
    }

    /// <summary>
    /// Player stops dragging the enemy
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        enemyScript.OnEndDrag();
    }
}
