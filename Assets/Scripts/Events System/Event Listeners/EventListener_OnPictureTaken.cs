using System;
using UnityEngine;
using JetBrains.Annotations;

public class EventListener_OnPictureTaken : EventListener
{
    protected override void Awake()
    {
        base.Awake();
        
        CameraSight camSight = GetComponentInChildren<CameraSight>();

        if (camSight == null)
        {
            Debug.Log("No Camera sight children of object " + gameObject.name);
            return;
        }

        camSight.OnImportantPictureTakenNonStatic += () => { Trigger(); };
    }
}