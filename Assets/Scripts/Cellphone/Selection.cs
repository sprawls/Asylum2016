using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Selection : MonoBehaviour,  ISelectHandler, ISubmitHandler
{
    public static event Action OnSelectButton;
    public static event Action OnButtonClicked;

    public void OnSelect (BaseEventData eventData)
    {
        OnSelectButton.Invoke();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnButtonClicked.Invoke();
    }

}
