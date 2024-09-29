using UnityEngine;

/// <summary>
/// Using a project initializer can help reduce race conditions by allowing for more granular control of your load sequence.
/// </summary>
public class ProjectInitializer : MonoBehaviour
{
    private void Start()
    {
        MenuManager.Instance.Initialise();
        StateManager.Instance.Initialise();
        DeckOfCards.Instance.Initialise();
        StartCoroutine(GridManager.Instance.Initialise(() =>
        {
            AudioManager.Instance.Initialise();
        }));
    }
}
