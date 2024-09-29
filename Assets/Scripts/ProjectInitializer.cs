using Cards;
using Grid;
using pure_unity_methods;
using Score;
using StateManagement;

/// <summary>
/// Using a project initializer can help reduce race conditions by allowing for more granular control of the load sequence.
/// </summary>
public class ProjectInitializer : Singleton<ProjectInitializer>
{
    private void Start()
    {
        StateManager.Instance.Initialise();
        MenuManager.Instance.Initialise();
        DeckOfCards.Instance.Initialise();
        Score.Score.Instance.Initialise();
        StartCoroutine(Grid.Grid.Instance.Initialise(() =>
        {
            //dependent on GridManager
            Audio.Instance.Initialise();
            Evaluator.Instance.Initialise();
        }));
    }
}
