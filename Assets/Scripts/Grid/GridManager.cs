using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureFunctions.UnitySpecific;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Transform gridArea;
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] [Range(2, 7)] public int amountOfRows;
    [SerializeField] [Range(2, 10)] public int amountOfCardsPerRow;
    private readonly List<GridItem> cards = new ();
    private GameObject grid;
    public Action<GridItem> OnCardClick;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DeckOfCards.Instance != null);
        yield return new WaitUntil(() => !active);
        SetCardSlotValues();
        grid = GameObject.FindWithTag("Grid");
    }

    public bool active = false;
    #if UNITY_EDITOR
    private void OnValidate()
    {
        //return;
        if (Application.isPlaying)
        {
            return;
        }
        if (active) return;
        active = true;
        ResetGrid(()=>CreateGrid(() =>
        {
            active = false;
        }));
    }
    #endif

    private void CreateGrid(Action completeCallback = null)
    {
        grid = Instantiate(gridPrefab, gridArea);
        for (var i = 0; i < amountOfRows; i++)
        {
            var row = Instantiate(rowPrefab, grid.transform);
            for (var j = 0; j < amountOfCardsPerRow; j++)
            {
                if (Application.isPlaying)
                {
                    var card = Instantiate(cardPrefab, row.transform).GetComponent<GridItem>();
                    cards.Add(card);
                    card.Initialise(OnGridItemClick);
                }
                else
                {
                    Instantiate(cardPrefab, row.transform);
                }
            }
        }
        if (!MathUtilities.IsEvenNumber(cards.Count))
        {
            Debug.LogWarning($"Having an Odd amount of {nameof(cards)} means not every card has a match!");
        }
        completeCallback?.Invoke();
    }

    private void ResetGrid(Action completeCallback = null)
    {
        cards.Clear();
        grid = GameObject.FindWithTag("Grid");
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

    [ContextMenu(nameof(SetCardSlotValues))]
    public void SetCardSlotValues()
    {
        var cardValues = CreateCardList(cards.Count);
        var index = 0;
        foreach (var card in cards)
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

    private void OnGridItemClick(GridItem gridItem)
    {
        OnCardClick?.Invoke(gridItem);
    }

    private void ResetCards()
    {
        foreach (var card in cards)
        {
            card.ResetCard();
        }
    }
}

