using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void OnCLickNotify(GameObject go);

public class SelectMapTrigger : EventTrigger
{
    public event OnCLickNotify Clicked;

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnClicked(gameObject);
    }

    protected virtual void OnClicked(GameObject go)
    {
        Clicked?.Invoke(go);
    }
}
