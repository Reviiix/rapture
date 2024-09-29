using pure_unity_methods;
using StateManagement;
using UnityEngine;
using UnityEngine.UI;

public class Menu : Singleton<Menu>
{
    [SerializeField] private Canvas display;
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
            display.enabled = !currentState;
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

    private static void RestartPressed()
    {
        StateManager.Instance.RestartGame();
    }
}
