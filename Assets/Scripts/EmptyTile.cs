using System;
using UnityEngine;

public class EmptyTile : MonoBehaviour
{
    public bool isPlaceable { get; private set; }

    [SerializeField] private Renderer _renderer;

    public void IsTilePlaceable(bool value, Material placeableMaterial)
    {
        isPlaceable = value;
        if (isPlaceable)
        {
            _renderer.material = placeableMaterial;
        }
    }
}
