using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class LetterArea : MonoBehaviour
{
    private int verticalCount;
    private int horizontalCount;
    
    [SerializeField] private EmptyTile tilePrefab;
    [SerializeField] private List<EmptyTile> tile;
    [SerializeField] private Material material;
    [SerializeField] private Material placeableMaterial;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private TableMatrix tableMatrix;
    

    private Vector3 _offset;

    [Button]
    private void Start()
    {
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
                Debug.Log(tableMatrix.CustomCellDrawing[i, j]);

                var go = Instantiate(tilePrefab, transform);
                var pos = new Vector3(j, 0.5f, -i);
                go.transform.localPosition = pos  + _offset;
                go.transform.DOPunchScale(Vector3.one * 1.1f, 0.05f, 1);
                go.placeablePosition = tableMatrix.CustomCellDrawing[i, j];
                if (go.placeablePosition)
                {
                    go.GetComponent<Renderer>().material = placeableMaterial;
                }
                tile.Add(go);
            }
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