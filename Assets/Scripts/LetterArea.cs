using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class LetterArea : MonoBehaviour
{
    private int verticalCount;
    private int horizontalCount;

    [SerializeField] private EmptyTile tilePrefab;
    [SerializeField, HideInInspector] private List<EmptyTile> tile;
    [SerializeField] private Material material;
    [SerializeField] private Material placeableMaterial;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private TableMatrix tableMatrix;
    [SerializeField] private DraggableItem draggableItem;
    [SerializeField] private PuzzlePart puzzlePartPrefab;

    [SerializeField, HideInInspector] private List<EmptyTile> _placeableTiles;


    private Vector3 _offset;

    [Button]
    private void Start()
    {
        if (tableMatrix == null)
        {
            tableMatrix = GameManager.Instance.ReturnRandomLetter();
        }

        verticalCount = tableMatrix.CustomCellDrawing.GetUpperBound(0) + 1;
        horizontalCount = tableMatrix.CustomCellDrawing.GetUpperBound(1) + 1;
        StartCoroutine(StartTile());
    }

    private IEnumerator StartTile()
    {
        RemoveTile();

        if (horizontalCount > vCam.m_Lens.OrthographicSize)
        {
            vCam.m_Lens.OrthographicSize = horizontalCount;
        }

        var xOffset = -((horizontalCount - 1) * 0.5f);
        var yOffset = (verticalCount * 0.5f);
        _offset = new Vector3(xOffset, 0, yOffset);

        for (var i = 0; i < verticalCount; i++)
        {
            for (var j = 0; j < horizontalCount; j++)
            {
                for (var k = 0; k < 2; k++)
                {
                    yield return null;
                }

                var go = Instantiate(tilePrefab, transform);
                var pos = new Vector3(j, 0.5f, -i);
                go.transform.localPosition = pos + _offset;
                go.transform.DOPunchScale(Vector3.one * 1.1f, 0.05f, 1);
                go.IsTilePlaceable(tableMatrix.CustomCellDrawing[i, j], placeableMaterial);
                if (go.isPlaceable)
                {
                    _placeableTiles.Add(go);
                }
                else
                {
                    go.gameObject.SetActive(false);
                }

                tile.Add(go);
            }
        }
    }

    [Button]
    private void TestPiece()
    {
        while (_placeableTiles.Count > 0)
        {
            var tileCount = 0;
            var rndMax = Random.Range(2, Mathf.Min(5, _placeableTiles.Count));
            var draggableParent = Instantiate(draggableItem);
            var first = _placeableTiles[0];
            var orderByDist = _placeableTiles.OrderBy(w =>
                Vector3.Distance(first.transform.position, w.transform.position)).ToList();

            foreach (var emptyTile in orderByDist)
            {
                float dist = Vector3.Distance(first.transform.position, emptyTile.transform.position);
                if (dist <= 2f && tileCount < rndMax)
                {
                    var go = Instantiate(puzzlePartPrefab, emptyTile.transform.localPosition, Quaternion.identity,
                        draggableParent.transform);
                    go.transform.localPosition -= draggableParent.transform.position;
                    _placeableTiles.Remove(emptyTile);
                    tileCount += 1;
                }
            }
            draggableParent.CenterOnChildren();
        }
    }

    [Button]
    private void RemoveTile()
    {
        foreach (var go in tile)
        {
            DestroyImmediate(go);
        }

        tile.Clear();
    }
}