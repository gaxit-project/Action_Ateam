using UnityEngine;

public class PinManager : SingletonMonoBehaviour<PinManager>
{
    [SerializeField] private PinBase[] pins;

    public int GetKnockedDownPinCount()
    {
        int count = 0;
        foreach(PinBase pin in pins)
        {
            if (pin.IsKnockedDownPin())
            {
                count++;
            }
        }
        return count;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Debug.Log($"Œ»İ‚Ì“|‚µ‚½ƒsƒ“‚Ì”: {GetKnockedDownPinCount()}");
        }
    }
}
