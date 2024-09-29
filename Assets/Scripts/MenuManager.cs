using PureFunctions.UnitySpecific;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private Canvas menu;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button restartButton;

    public void Initialise()
    {
        menuButton.onClick.AddListener(OpenMenu);
        playButton.onClick.AddListener(PlayPressed);
        restartButton.onClick.AddListener(RestartPressed);
    }

    public void OpenMenu()
    {
        var currentState = StateManager.Instance.IsMenuState();
        StateManager.Instance.SetMenuState(!currentState, () =>
        {
            menu.enabled = !currentState;
        });
    }

    private void PlayPressed()
    {
        if (Evaluator.Instance.IsWon())
        {
            StateManager.Instance.RestartGame();
        }
        else
        {
            OpenMenu();
        }
    }

    private void RestartPressed()
    {
        StateManager.Instance.RestartGame();
    }
}
