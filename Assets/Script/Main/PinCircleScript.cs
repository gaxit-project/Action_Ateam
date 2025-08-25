using UnityEngine;
using UnityEngine.UI;

public class PinCircleScript : MonoBehaviour
{
    [SerializeField] private GameObject pinObject;
    [SerializeField] private GameObject[] _pinPrefab = new GameObject[7];
    private PinBase pinBase;

    // 体(0)か頭（１）のマテリアル色を取るか
    private int targetMaterialIndex = 0;

    private void Reset()
    {
        // UIの名前から対応するピン名を取得
        string targetName = gameObject.name.Replace("_img", "");

        // シーン内から名前完全一致で検索
        GameObject found = GameObject.Find(targetName);
        if (found != null)
        {
            pinObject = found;

            // 名前に "Gold" が含まれていたら index = 1 に設定
            if (pinObject.name.Contains("Gold"))
            {
                targetMaterialIndex = 1;
            }
            // 名前に "Black" が含まれていたら index = 1 に設定
            else if (pinObject.name.Contains("Black"))
            {
                targetMaterialIndex = 1;
            }
            else
            {
                targetMaterialIndex = 0;
            }
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
            Renderer rend = pinObject.GetComponent<Renderer>();
            if (rend != null && targetMaterialIndex < rend.materials.Length)
            {
                Color pinColor = rend.materials[targetMaterialIndex].color;
                GetComponent<Image>().color = pinColor;
            }
        }
    }
}
