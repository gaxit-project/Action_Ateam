using UnityEngine;

public class PinManager : SingletonMonoBehaviour<PinManager>
{
    [SerializeField] private PinBase[] pins;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Debug.Log($"Œ»İ‚Ì“|‚µ‚½ƒsƒ“‚Ì”: {GetKnockedDownPinCount()}");
        }
    }

    public int GetKnockedDownPinCount()//Œ»İ“|‚µ‚Ä‚¢‚éƒsƒ“‚Ì–{”
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
}
