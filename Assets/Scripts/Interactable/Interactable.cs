using UnityEngine;
using JetBrains.Annotations;

[RequireComponent(typeof(Renderer))]
public abstract class Interactable : MonoBehaviour
{
    public Renderer RendererObject { get; private set; }
    private float _outlineValue;
    private bool _highlightActive = false;

    protected abstract void OnTrigger();

    [UsedImplicitly]
    protected virtual void Awake()
    {
        RendererObject = GetComponent<Renderer>();

        _outlineValue = RendererObject.material.GetFloat("_Outline");
        ClearTargeted();
    }

    public void Trigger()
    {
        OnTrigger();
    }

    private void SetState(bool targeted)
    {
        RendererObject.material.SetFloat("_Outline", targeted ? _outlineValue : 0f);
    }

    public void SetTargeted()
    {
        _highlightActive = true;
        SetState(true);
    }

    public void ClearTargeted()
    {
        _highlightActive = false;
        RendererObject.material.SetFloat("_Outline", 0f);
    }
}