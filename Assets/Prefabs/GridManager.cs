using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] private Transform grid;
    [SerializeField] [Range(1, 10)] public int rows;
    [SerializeField] [Range(1, 10)] public int cardsPerRow;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cardPrefab;
    private List<CardSlot> cards;
    private Action<CardSlot> OnCardClick;

    private void Awake()
    {
        CreateGrid();
    }

    private void Start()
    {
        SetCardSlotValues();
    }

    private void OnValidate()
    {
        CreateGrid();
    }
    
    private void CreateGrid()
    {
        for (var i = 0; i < rows; i++)
        {
            var row = Instantiate(rowPrefab, grid);
            for (var j = 0; j < cardsPerRow; j++)
            {
                var card = Instantiate(cardPrefab, row.transform).GetComponent<CardSlot>();
                cards.Add(card);
                card.Initialise(OnGridItemClick);
            }
        }

        if (IsOdd(cards.Count))
        {
            Debug.LogWarning($"Having an Odd amount of {nameof(cards)} means not every card has a match!");
        }
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

    private void OnGridItemClick(CardSlot cardSlot)
    {
        OnCardClick?.Invoke(cardSlot);
    }

    private void ResetCards()
    {
        foreach (var card in cards)
        {
            card.ResetCard();
        }
    }
}

[RequireComponent(typeof(Button))]
public class CardSlot : MonoBehaviour
{
    [SerializeField] private Image display;
    private Action<CardSlot> onClick;
    private bool initialised;
    private bool revealed;
    public CardValue Value { get; private set; }

    public void Initialise(Action<CardSlot> gridManagerOnClick)
    {
        if (initialised)
        {
            Debug.LogError($"Do not initialise {nameof(CardSlot)} more than once.");
            return;
        }
        onClick += gridManagerOnClick;
        GetComponent<Button>().onClick.AddListener(OnClick); //Secured by the require component attribute. No need to cache it.
        initialised = true;
    }

    public void SetValue(CardValue value)
    {
        if (!initialised)
        {
            Debug.LogError($"{nameof(CardSlot)} has not been initialised.");
        }
        Value = value;
    }

    private void OnClick()
    {
        if (revealed) return;
        revealed = true;
        onClick(this);
    }

    public void ResetCard()
    {
        revealed = false;
        display.sprite = Deck.Instance.cardBack;
    }
}

public class Deck : MonoBehaviour
{
    public static Deck Instance;
    public HashSet<CardValue> cards;
    public Sprite cardBack;


    public CardValue GetUniquCard()
    {
        return cards.FirstOrDefault();
    }
}

public enum CardValue
{
    AceOfDiamonds,
    TwoOfDiamond,
}
