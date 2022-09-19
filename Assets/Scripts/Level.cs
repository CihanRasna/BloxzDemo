using System;
using System.Collections.Generic;
using Cinemachine;
using Managers;
using UnityEngine;

public class Level : BaseLevel
{
    public List<Vector3> draggablePositions;
    [SerializeField] private EmptyTile tilePrefab;
    [SerializeField] private Material placeableMaterial;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private TableMatrix tableMatrix;
    [SerializeField] private DraggableItem draggableItemPrefab;
    [SerializeField] private PuzzlePart puzzlePartPrefab;
    [SerializeField] private LetterArea letterArea;
    public List<EmptyTile> placeableTiles;

    [HideInInspector] public List<DraggableItem> draggableItems;
    

    private void Start()
    {
        const float xOffset = 2.5f;
        const float zOffset = 5f;
        for (var i = 0; i < 4; i++)
        {
            for (var j = -2; j < 2; j++)
            {
                var pos = new Vector3(xOffset + j * 5f, 0.5f, 10f - zOffset * i);
                draggablePositions.Add(pos);
            }
        }
    }

    protected override void LevelDidLoad()
    {
        base.LevelDidLoad();
        if (tableMatrix == null)
        {
            tableMatrix = GameManager.Instance.ReturnRandomLetter();
        }
        letterArea.GetValuesFromLevel(tilePrefab, placeableMaterial, vCam, tableMatrix, draggableItemPrefab,
            puzzlePartPrefab);
    }

    public void LevelStarted()
    {
        StartLevel();
    }

    public void TileIsTaken(EmptyTile tile)
    {
        placeableTiles.Remove(tile);
        if (placeableTiles.Count == 0 && state != State.Succeeded)
        {
            letterArea.SucceedAnimation();
            foreach (var draggableItem in draggableItems)
                draggableItem.SucceedAnimation();
            Success(1);
        }
    }
    
    public void TileRevoked(EmptyTile tile)
    {
        if (!placeableTiles.Contains(tile))
        {
            placeableTiles.Add(tile);
        }
    }
}