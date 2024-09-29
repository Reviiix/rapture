using PureFunctions.UnitySpecific;
using UnityEngine;

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
        ScoreTracker.Instance.Initialise();
        StartCoroutine(GridManager.Instance.Initialise(() =>
        {
            AudioManager.Instance.Initialise(); //dependent on GridManager
            Evaluator.Instance.Initialise();
        }));
    }
}
