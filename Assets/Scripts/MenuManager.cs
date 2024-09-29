using PureFunctions.UnitySpecific;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private Canvas menu;
    [SerializeField] private Button menuButton;
    [FormerlySerializedAs("playButton")] [SerializeField] private Button closeButton;

    [SerializeField] private Button restartButton;


    public void Initialise()
    {
        menuButton.onClick.AddListener(OnMenuPressed);
        closeButton.onClick.AddListener(OnMenuPressed);
        restartButton.onClick.AddListener(OnMenuPressed);
    }

    private void OnMenuPressed()
    {
        var currentState = StateManager.Instance.IsMenuState();
        StateManager.Instance.SetMenuState(!currentState, () =>
        {
            menu.enabled = !currentState;
        });
    }
}
