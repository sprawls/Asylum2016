using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineController : MonoBehaviour
{
    private Renderer _renderer;
    private float _defaultWidth;
    private Color _color = new Color(1f, 0.764f, 0f);

    [UsedImplicitly]
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _defaultWidth = _renderer.material.GetFloat("_Outline");
        _renderer.material.SetColor("_OutlineColor", _color);
    }

    public void Show()
    {
        _renderer.material.SetFloat("_Outline", _defaultWidth);
    }

    public void Hide()
    {
        _renderer.material.SetFloat("_Outline", 0);
    }
}