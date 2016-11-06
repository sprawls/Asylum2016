using UnityEngine;
using JetBrains.Annotations;

[RequireComponent(typeof(CameraSight))]
public class EventListener_OnPictureTaken : EventListener
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<CameraSight>().OnImportantPictureTakenNonStatic += () => { Trigger(); };
    }
}