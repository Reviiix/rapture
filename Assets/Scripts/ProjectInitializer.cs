using Cards;
using GridSystem;
using pure_unity_methods;
using ScoreSystem;
using StateManagement;

/// <summary>
/// Using a project initializer can help reduce race conditions by allowing for more granular control of the load sequence.
/// </summary>
public class ProjectInitializer : Singleton<ProjectInitializer>
{
    private void Start()
    {
        StateManager.Instance.Initialise();
        Menu.Instance.Initialise();
        DeckOfCards.Instance.Initialise();
        Score.Instance.Initialise();
        StartCoroutine(GridManager.Instance.Initialise(() =>
        {
            //dependent on GridManager
            Audio.Instance.Initialise();
            Evaluator.Instance.Initialise();
        }));
    }
}
