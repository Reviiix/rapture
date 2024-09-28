using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class GridItem : MonoBehaviour
{
    private bool Initialised { get; set; }
    public bool Revealed { get; private set; }
    private Image display;
    private Action<GridItem> onClick;
    public Card Value { get; private set; }

    public void Initialise(Action<GridItem> gridManagerOnClick)
    {
        if (Initialised)
        {
            Debug.LogError($"Do not initialise {nameof(GridItem)} more than once.");
            return;
        }
        AssignFields();
        SubscribeToEvents(gridManagerOnClick);
        Initialised = true;
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
    
    private void OnDisable()
    {
        if (onClick != null)
        {
            onClick -= GridManager.OnCardClick;
        }
    }

    public void SetValue(Card value)
    {
        if (!Initialised)
        {
            Debug.LogError($"{nameof(GridItem)} has not been initialised.");
        }
        Value = value;
        //display.sprite = value.GetCardSprite(); //Debug tool
    }

    private void OnClick()
    {
        Revealed = !Revealed;
        if (Revealed)
        {
            Reveal();
            AudioManager.Instance.PlaySuccess();
        }
        else
        {
            Hide();
            AudioManager.Instance.PlayFailure();
        }
        onClick(this);
    }

    private void Reveal()
    {
        var v = Value.GetCardSprite();
        display.sprite = v;
    }

    private void Hide()
    {
        display.sprite = DeckOfCards.Instance.cardBack;
    }

    public void ResetCard()
    {
        Revealed = false;
        display.sprite = DeckOfCards.Instance.cardBack;
    }
}
