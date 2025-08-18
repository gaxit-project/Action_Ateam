using UnityEngine;
using UnityEngine.UI;

public class PinCircleScript : MonoBehaviour
{
    [SerializeField] private GameObject pinObject;
    private PinBase pinBase;

    private void Reset()
    {
        // UIの名前から対応するピン名を取得
        // 例: "pin1_img" → "pin1"
        string targetName = gameObject.name.Replace("_img", "");

        // シーン内から名前完全一致で検索
        GameObject found = GameObject.Find(targetName);
        if (found != null)
        {
            pinObject = found;
        }
    }

    private void Awake()
    {
        Reset();
    }

    void Start()
    {
        if (pinObject != null)
        {
            pinBase = pinObject.GetComponent<PinBase>();
        }
    }

    void Update()
    {
        if (pinBase != null && pinBase.GiveIsFallDown())
        {
            GetComponent<Image>().enabled = false;
        }
    }
}
