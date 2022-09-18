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
    [SerializeField] private Material placeableMaterial;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private TableMatrix tableMatrix;
    [SerializeField] private DraggableItem draggableItem;
    [SerializeField] private PuzzlePart puzzlePartPrefab;

    [SerializeField, HideInInspector] private List<EmptyTile> _placeableTiles;

    private Vector3 _offset;

    public void GetValuesFromLevel(EmptyTile tilePrefab, Material placeableMaterial, CinemachineVirtualCamera vCam,
        TableMatrix tableMatrix, DraggableItem draggableItem, PuzzlePart puzzlePartPrefab)
    {
        this.tilePrefab = tilePrefab;
        this.placeableMaterial = placeableMaterial;
        this.vCam = vCam;
        this.tableMatrix = tableMatrix;
        this.draggableItem = draggableItem;
        this.puzzlePartPrefab = puzzlePartPrefab;

        StartCoroutine(StartTileCo());
    }

    private IEnumerator StartTileCo()
    {
        RemoveTile();

        if (tableMatrix == null)
        {
            tableMatrix = GameManager.Instance.ReturnRandomLetter();
        }

        verticalCount = tableMatrix.CustomCellDrawing.GetUpperBound(0) + 1;
        horizontalCount = tableMatrix.CustomCellDrawing.GetUpperBound(1) + 1;

        if (horizontalCount > vCam.m_Lens.OrthographicSize)
        {
            var size = vCam.m_Lens.OrthographicSize;
            var desiredSize = horizontalCount - size;
            vCam.m_Lens.OrthographicSize += desiredSize;
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
                go.transform.DOPunchScale(Vector3.one * 1.1f, 0.25f, 1);
                go.IsTilePlaceable(tableMatrix.CustomCellDrawing[i, j], placeableMaterial);
                if (go.isPlaceable)
                {
                    _placeableTiles.Add(go);
                }

                tile.Add(go);
            }
        }
        
        GeneratePuzzlePieces();
    }

    [Button]
    private void GeneratePuzzlePieces()
    {
        var level = LevelManager.Instance.currentLevel as Level;
        var pos = new List<Vector3>(level.draggablePositions);
        level.placeableTiles = new List<EmptyTile>(_placeableTiles);
        
        while (_placeableTiles.Count > 0)
        {
            var tileCount = 0;
            var rndMax = Random.Range(2, Mathf.Min(5, _placeableTiles.Count)); // TODO : BUG?
            var draggableParent = Instantiate(draggableItem, level.transform, true);
            var first = _placeableTiles[0];
            var orderByDist = _placeableTiles.OrderBy(w =>
                Vector3.Distance(first.transform.position, w.transform.position)).ToList();

            foreach (var emptyTile in orderByDist)
            {
                emptyTile.TileIsTakenAction += level.TileIsTaken;
                emptyTile.TakenActionRevoked += level.TileRevoked;
                
                float dist = Vector3.Distance(first.transform.position, emptyTile.transform.position);

                if (dist <= 1f && tileCount < rndMax)
                {
                    var go = Instantiate(puzzlePartPrefab, emptyTile.transform.localPosition, Quaternion.identity,
                        draggableParent.transform);
                    go.transform.localPosition -= draggableParent.transform.position;
                    _placeableTiles.Remove(emptyTile);
                    tileCount += 1;
                    first = emptyTile;
                }
            }
            
            draggableParent.CenterOnChildren();
            draggableParent.SetPositionOfItem(pos[0]);
            pos.RemoveAt(0);
            
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