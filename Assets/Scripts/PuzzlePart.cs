using System;
using DG.Tweening;
using UnityEngine;

public class PuzzlePart : MonoBehaviour
{
    public bool canBePlaced { get; private set; }
    [SerializeField] private Renderer renderer;
    [SerializeField] private Color placeableMaterialColor;
    [SerializeField] private Color standardColor;
    private EmptyTile emptyTile;
    [SerializeField] private Vector3 initialPos;
    
    
    private MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", placeableMaterialColor);
        initialPos = transform.localPosition;
    }

    public void GetPlaced(bool forceReturn = false)
    {
        if (forceReturn)
        {
            canBePlaced = false;
            if (emptyTile)
            {
                emptyTile.tileIsTaken = false;
                emptyTile = null;
            }
            
            transform.DOLocalMove(initialPos, 0.1f);
            return;
        }
        
        if (emptyTile)
        {
            transform.DOMove(emptyTile.transform.position, 0.2f).OnComplete((() => emptyTile.tileIsTaken = true));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EmptyTile>(out var emptyTile))
        {
            if (emptyTile.isPlaceable && !emptyTile.tileIsTaken)
            {
                this.emptyTile = emptyTile;
                propertyBlock.SetColor("_Color", placeableMaterialColor);
                renderer.SetPropertyBlock(propertyBlock);
                canBePlaced = true;
            }
            else
            {
                this.emptyTile = null;
                propertyBlock.SetColor("_Color", standardColor);
                renderer.SetPropertyBlock(propertyBlock);
                canBePlaced = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EmptyTile>(out var emptyTile))
        {
            this.emptyTile = null;
            canBePlaced = false;
            propertyBlock.SetColor("_Color", standardColor);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}