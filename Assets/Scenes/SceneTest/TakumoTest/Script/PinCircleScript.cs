using UnityEngine;
using UnityEngine.UI;

public class PinCircleScript : MonoBehaviour
{
    /// <summary>
    /// このscriptは対象Pinに対応するimgへ直接アタッチします。
    /// そうすることによりPinが倒れるとimgが消えます。
    /// </summary>
    [SerializeField] private GameObject pinObject; // 対象のピン（Inspector で割り当て）

    private PinBase pinBase;

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
