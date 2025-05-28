using UnityEngine;

public class PinManager : MonoBehaviour
{
    [SerializeField] private PinBase[] WhitePins;
    [SerializeField] private PinBase[] RedPins;
    [SerializeField] private PinBase[] BlackPins;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Debug.Log($"現在の倒したピンの数: {GetKnockedDownPinCount()}");
        }
    }

    public int GetKnockedDownPinCount()
    {
        int count = 0;
        foreach(PinBase pin in WhitePins)//白色のピンは加点
        {
            if (pin.IsKnockedDownPin())
            {
                count++;
            }
        }

        foreach (PinBase pin in RedPins)//赤色のピンは多めに加点
        {
            if (pin.IsKnockedDownPin())
            {
                count += 3;
            }
        }


        foreach (PinBase pin in BlackPins)//黒色のピンは減点
        {
            if (pin.IsKnockedDownPin())
            {
                count -= 3;
            }
        }
        return count;
    }
}
