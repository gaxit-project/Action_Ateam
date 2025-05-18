using UnityEngine;
using UnityEngine.Events;

public class ThrowingAreaManager : MonoBehaviour
{
    [SerializeField] private GameObject throwingAreaPrefab;
    [SerializeField] private int throwingAreaCount = 4;
    [SerializeField] private float spacing = 2f;
    [SerializeField] private UnityEvent onAllThrowingAreaEntered = new UnityEvent();

    private ThrowingArea[] throwingAreaControllers;

    private void Start()
    {
        GenerateThrowingAreas();
    }

    private void GenerateThrowingAreas()
    {
        throwingAreaControllers = new ThrowingArea[throwingAreaCount];
        for(int i = 0; i < throwingAreaCount; i++)
        {
            GameObject area = Instantiate(throwingAreaPrefab, transform);
            area.transform.localPosition = new Vector3(i * spacing, 0, 0);
            ThrowingArea controller = area.GetComponent<ThrowingArea>();
            controller.Initialize(this);
            throwingAreaControllers[i] = controller;
        }
    }

    public void OnThrowingAreaEntered()
    {
        foreach (var controller in throwingAreaControllers)
        {
            if (!controller.IsEntered)
            {
                return;
            }
        }
        //ここから投擲モードにするプログラム呼び出して
        onAllThrowingAreaEntered.Invoke();
        Debug.Log("全プレイヤーが投擲エリアに到着しました");
    }
}
