using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureFunctions.UnitySpecific;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class GridManager : Singleton<GridManager>
{
    private bool generatingGrid;
    private const string GridTag = "Grid";
    [SerializeField] private Transform gridArea;
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] [Range(2, 7)] public int amountOfRows;
    [SerializeField] [Range(2, 10)] public int amountOfItemsPerRow;
    private readonly List<GridItem> gridItems = new ();
    private GameObject grid;
    public static Action<GridItem> OnCardClick;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => !generatingGrid);
        yield return new WaitUntil(() => DeckOfCards.Instance != null);
        generatingGrid = true;
        ResetGrid(()=>CreateGrid(() =>
        {
            SetGridItemValues();
            grid = GameObject.FindWithTag(GridTag);
            generatingGrid = false;
        }));
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        //generatingGrid = false; //debug tool
        if (Application.isPlaying)
        {
            return;
        }
        if (generatingGrid) return;
        generatingGrid = true;
        ResetGrid(()=>CreateGrid(() =>
        {
            generatingGrid = false;
        }));
    }
    #endif

    private void CreateGrid(Action completeCallback = null)
    {
        grid = Instantiate(gridPrefab, gridArea);
        for (var i = 0; i < amountOfRows; i++)
        {
            var row = Instantiate(rowPrefab, grid.transform);
            for (var j = 0; j < amountOfItemsPerRow; j++)
            {
                if (Application.isPlaying)
                {
                    var card = Instantiate(cardPrefab, row.transform).GetComponent<GridItem>();
                    gridItems.Add(card);
                    card.Initialise(OnGridItemClick);
                }
                else
                {
                    Instantiate(cardPrefab, row.transform);
                }
            }
        }
        if (!MathUtilities.IsEvenNumber(gridItems.Count))
        {
            Debug.LogWarning($"Having an Odd amount of {nameof(gridItems)} means not every card has a match!");
        }
        completeCallback?.Invoke();
    }

    private void ResetGrid(Action completeCallback = null)
    {
        ResetGridItems();
        grid = GameObject.FindWithTag(GridTag);
        if (!grid)
        {
            completeCallback?.Invoke();
            return;
        }
        if (Application.isPlaying)
        {
            Destroy(grid);
            completeCallback?.Invoke();
        }
        else
        {
            UnityEditor.EditorApplication.delayCall+=()=>
            {
                DestroyImmediate(grid);
                completeCallback?.Invoke();
            };
        }
    }
    
    private void ResetGridItems()
    {
        foreach (var item in gridItems)
        {
            item.ResetCard();
        }
        gridItems.Clear();
    }

    [ContextMenu(nameof(SetGridItemValues))]
    public void SetGridItemValues()
    {
        var cardValues = CreateCardList(gridItems.Count);
        var index = 0;
        foreach (var card in gridItems)
        {
            card.SetValue(cardValues[index]);
            index++;
        }
    }

    /// <summary>
    /// Create two matching arrays of cards and combine them together.
    /// This ensures there are always the correct amount of matches
    /// </summary>
    private static List<Card> CreateCardList(int amount)
    {
        var firstHalf = new List<Card>();
        var half = amount / 2;
        for (var i = 0; i < half; i++)
        {
            firstHalf.Add(GetUniqueValue());
        }
        var secondHalf = new List<Card>(firstHalf);
        var fullList = firstHalf.Concat(secondHalf).ToList();
        //TODO: Shuffle fullList
        return fullList;
    }
    
    
    private static Card GetUniqueValue()
    {
        return DeckOfCards.Instance.TakeRandomCard();
    }

    private static void OnGridItemClick(GridItem gridItem)
    {
        OnCardClick?.Invoke(gridItem);
        Debug.Log($"{gridItem.Value.GetRank()} of {gridItem.Value.GetSuit()} revealed({gridItem.Revealed})"); //Debug tool
    }
}

