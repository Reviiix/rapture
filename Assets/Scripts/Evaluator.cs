using PureFunctions.UnitySpecific;
using UnityEngine;

public class Evaluator : Singleton<Evaluator>
{
    private Card selectionOne;
    private Card selectionTwo;

    protected override void OnEnable()
    {
        base.OnEnable();
        GridManager.OnItemClick += OnStateEnter;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        GridManager.OnItemClick -= OnStateEnter;
    }

    public bool IsMatch()
    {
        return selectionOne.DeckIndex == selectionTwo.DeckIndex;
    }

    private void OnStateEnter(GridItem item)
    {
        if (StateManager.Instance.IsPickOne())
        {
            selectionOne = item.Value;
        }
        if (StateManager.Instance.IsPickTwo())
        {
            selectionTwo = item.Value;
        }
    }
}
