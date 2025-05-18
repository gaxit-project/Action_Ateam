using UnityEngine;
using UnityEngine.Events;

public class ThrowingArea : MonoBehaviour
{
    public UnityEvent onThrowingAreaEntered = new UnityEvent();
    
    private bool isEntered = false;
    private ThrowingAreaManager throwingAreaManager;

    public void Initialize(ThrowingAreaManager manager)
    {
        throwingAreaManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEntered)
        {
            isEntered = true;
            onThrowingAreaEntered.Invoke();
            throwingAreaManager.OnThrowingAreaEntered();
        }
    }

    public bool IsEntered => isEntered;
}
