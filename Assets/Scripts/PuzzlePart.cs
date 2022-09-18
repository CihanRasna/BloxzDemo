using System;
using UnityEngine;

public class PuzzlePart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EmptyTile>(out var emptyTile))
        {
            if (emptyTile.isPlaceable)
            {
                Debug.Log("CAN PLACE");
            }
        }
    }
}