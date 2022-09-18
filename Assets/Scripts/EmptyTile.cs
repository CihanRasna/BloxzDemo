using System;
using UnityEngine;

public class EmptyTile : MonoBehaviour
{
    public event Action<EmptyTile> TileIsTakenAction;
    public event Action<EmptyTile> TakenActionRevoked;
    public bool isPlaceable { get; private set; }

    public EmptyTile hasPair;
    
    private bool _tileIsTaken;
    public bool tileIsTaken
    {
        get => _tileIsTaken;
        set
        {
            if (value)
            {
                TileIsTakenAction.Invoke(this);
            }
            else
            {
                TakenActionRevoked.Invoke(this);
            }
            
            _tileIsTaken = value;
        }
    }

    [SerializeField] private Renderer renderer;
    [SerializeField] private BoxCollider collider;
    

    public void IsTilePlaceable(bool value, Material placeableMaterial)
    {
        isPlaceable = value;
        if (isPlaceable)
        {
            renderer.material = placeableMaterial;
            //collider.enabled = true;
        }
    }
}
