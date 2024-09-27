using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] private Transform grid;[SerializeField] [Range(1, 10)] public int amountOfRows;
    [SerializeField] [Range(1, 10)] public int amountOfCardsPerRow;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cardPrefab;
    private List<GameObject> rows = new List<GameObject>();
    private List<GridItem> cards = new List<GridItem>();
    private Action<GridItem> OnCardClick;

    private void Awake()
    {
        ResetGrid(CreateGrid);
    }

    private void Start()
    {
        SetCardSlotValues();
    }

    private void OnValidate()
    {
        ResetGrid(CreateGrid);
    }
    
    private void CreateGrid()
    {
        for (var i = 0; i < amountOfRows; i++)
        {
            rows.Add(Instantiate(rowPrefab, grid));
            for (var j = 0; j < amountOfCardsPerRow; j++)
            {
                var card = Instantiate(cardPrefab, rows[i].transform).GetComponent<GridItem>();
                cards.Add(card);
                card.Initialise(OnGridItemClick);
            }
        }

        if (IsOdd(cards.Count))
        {
            Debug.LogWarning($"Having an Odd amount of {nameof(cards)} means not every card has a match!");
        }
    }

    private void ResetGrid( Action completeCallback)
    {
        cards.Clear();
        UnityEditor.EditorApplication.delayCall+=()=>
        {
            foreach (var row in rows)
            {
                DestroyImmediate(row);
            }
            rows.Clear();
            completeCallback();
        };
    }

    private static bool IsOdd(int i)
    {
        return false;
    }

    private void SetCardSlotValues()
    {
        var cssards = CreateCardList(cards.Count);
        var index = 0;
        foreach (var card in cards)
        {
            card.SetValue(cssards[index]);
            index++;
        }
    }

    /// <summary>
    /// Create two matching arrays of cards and combine them together.
    /// This ensures there are always the correct amount of matches
    /// </summary>
    private static List<CardValue> CreateCardList(int amount)
    {
        var firstHalf = new List<CardValue>();
        var half = amount / 2;
        for (var i = 0; i < half; i++)
        {
            firstHalf.Add(GetUniqueValue());
        }
        var secondHalf = new List<CardValue>(firstHalf);
        var fullList = firstHalf.Concat(secondHalf).ToList();
        //TODO: Shuffle fullList
        return fullList;
    }
    
    
    private static CardValue GetUniqueValue()
    {
        return Deck.Instance.GetUniquCard();
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

