using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class Interactable : MonoBehaviour
{
    public Renderer RendererObject { get; private set; }

    private void Awake()
    {
        RendererObject = GetComponent<Renderer>();
    }
}