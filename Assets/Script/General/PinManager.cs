using System.Linq;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    [SerializeField] private PinBase[] WhitePins;
    [SerializeField] private PinBase[] RedPins;
    [SerializeField] private PinBase[] BlackPins;

    public int GetKnockedDownPinCount(string PlayerID)
    {
        int count = 0;
        foreach(PinBase pin in WhitePins)//白色のピンは加点
        {
            if (pin.IsKnockedDownPin(PlayerID))
            {
                count++;
            }
        }

        foreach (PinBase pin in RedPins)//赤色のピンは多めに加点
        {
            if (pin.IsKnockedDownPin(PlayerID))
            {
                count += 3;
            }
        }


        foreach (PinBase pin in BlackPins)//黒色のピンは減点
        {
            if (pin.IsKnockedDownPin(PlayerID))
            {
                count -= 3;
            }
        }
        return count;
    }

    public PinBase[] GetAllPins()
    {
        return WhitePins.Concat(RedPins).Concat(BlackPins).ToArray();
    }
}
