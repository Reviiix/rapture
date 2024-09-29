using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureFunctions.UnitySpecific;
using UnityEngine;

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
    public static Action<GridItem> OnItemClick;
    public GridItem selectionOne;
    public GridItem selectionTwo;

    public IEnumerator Initialise(Action completeCallback)
    {
        yield return new WaitUntil(() => !generatingGrid);
        yield return new WaitUntil(() => DeckOfCards.Instance != null);
        generatingGrid = true;
        ResetGrid(()=>CreateGrid(() =>
        {
            SetGridItemValues();
            grid = GameObject.FindWithTag(GridTag);
            generatingGrid = false;
            completeCallback?.Invoke();
        }));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Evaluation.OnEvaluationComplete += OnEvaluationComplete;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        Evaluation.OnEvaluationComplete -= OnEvaluationComplete;
    }

    private void OnEvaluationComplete(bool match)
    {
        if (match)
        {
            StartCoroutine(selectionOne.RemoveFromPlay());
            StartCoroutine(selectionTwo.RemoveFromPlay());
        }
        else
        {
            AudioManager.Instance.PlayFailure();
            StartCoroutine(selectionOne.ResetCard(false));
            StartCoroutine(selectionTwo.ResetCard(false));
        }
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
            StartCoroutine(item.ResetCard());
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
        return fullList;
    }
    
    
    private static Card GetUniqueValue()
    {
        return DeckOfCards.Instance.TakeRandomCard();
    }

    private void OnGridItemClick(GridItem gridItem)
    {
        //Debug.Log($"{gridItem.Value.GetRank()} of {gridItem.Value.GetSuit()} revealed({gridItem.Revealed})"); //Debug tool
        SetSelections(gridItem);
        OnItemClick?.Invoke(gridItem);
        StateManager.Instance.ProgressState();
    }

    private void SetSelections(GridItem gridItem)
    {
        if (StateManager.Instance.IsPickOne())
        {
            selectionOne = gridItem;
        }
        if (StateManager.Instance.IsPickTwo())
        {
            selectionTwo = gridItem;
        }
    }
}

