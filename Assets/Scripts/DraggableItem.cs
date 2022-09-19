using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private Vector3 _initialPos;
    private bool onPlace = false;
    public bool OnPlace => onPlace;
    
    [SerializeField] private List<PuzzlePart> puzzleParts;
    public void SetPositionOfItem(Vector3 pos)
    {
        _initialPos = pos;
        transform.position = pos;
        transform.DOPunchScale(Vector3.one * 1.5f, 0.5f,1);
    }
    
    public void SucceedAnimation()
    {
        foreach (var tile in puzzleParts)
        {
            var pos = tile.transform.localPosition;
            var rndTime = Random.Range(0.1f, 0.5f);
            tile.transform.DOLocalMoveY(pos.y + 0.5f, rndTime).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void TryToGetPlace(bool forceReturn = false)
    {
        if (forceReturn)
        {
            foreach (var t in puzzleParts)
            {
                onPlace = false;
                t.GetPlaced(true);
            }

            transform.DOMove(_initialPos, 0.5f).SetDelay(0.35f);
            return;
        }
        
        var canPlace = puzzleParts.TrueForAll(p => p.canBePlaced);
        onPlace = canPlace;
        if (canPlace)
        {
            StartCoroutine(PlacePuzzleParts());
        }
        else
        {
            transform.DOMove(_initialPos, 0.5f);
        }
    }

    private IEnumerator PlacePuzzleParts()
    {
        for (var i = 0; i < puzzleParts.Count; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                yield return null;
            }
            puzzleParts[i].GetPlaced();
        }
    }

    public void CenterOnChildren()
    {
        puzzleParts = new List<PuzzlePart>(GetComponentsInChildren<PuzzlePart>().ToList());
        
        var pos = Vector3.zero;
        
        foreach(var c in puzzleParts)
        {
            var childTransform = c.transform;
            pos += childTransform.position;
            childTransform.parent = null;
        }
        
        pos /= puzzleParts.Count;
        transform.position = pos;
        foreach(var c in puzzleParts)
            c.transform.parent = transform;
    }    
}
