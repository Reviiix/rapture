using pure_unity_methods;
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
        var currentState = StateManager.StateManager.Instance.IsMenuState();
        StateManager.StateManager.Instance.SetMenuState(!currentState, () =>
        {
            menu.enabled = !currentState;
        });
    }

    private void PlayPressed()
    {
        if (Evaluator.Instance.IsWon())
        {
            StateManager.StateManager.Instance.RestartGame();
        }
        else
        {
            OpenMenu();
        }
    }

    private void RestartPressed()
    {
        StateManager.StateManager.Instance.RestartGame();
    }
}
