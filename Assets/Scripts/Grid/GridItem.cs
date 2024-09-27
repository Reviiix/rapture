using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class GridItem : MonoBehaviour
{
    private Image display;
    private Action<GridItem> onClick;
    private bool initialised;
    private bool revealed;
    public Card Value { get; private set; }

    public void Initialise(Action<GridItem> gridManagerOnClick)
    {
        if (initialised)
        {
            Debug.LogError($"Do not initialise {nameof(GridItem)} more than once.");
            return;
        }
        AssignFields();
        SubscribeToEvents(gridManagerOnClick);
        initialised = true;
    }

    private void AssignFields()
    {
        GetComponent<Button>().onClick.AddListener(OnClick); //Secured by the require component attribute.
        display = GetComponent<Image>();
    }
    
    private void SubscribeToEvents(Action<GridItem> gridManagerOnClick)
    {
        onClick += gridManagerOnClick;
    }

    public void SetValue(Card value)
    {
        if (!initialised)
        {
            Debug.LogError($"{nameof(GridItem)} has not been initialised.");
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
        display.sprite = DeckOfCards.Instance.cardBack;
    }
}
