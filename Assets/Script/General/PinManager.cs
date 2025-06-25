using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    [SerializeField] private List<PinBase> WhitePins = new List<PinBase>();
    [SerializeField] private List<PinBase> GoldPins = new List<PinBase>();
    [SerializeField] private List<PinBase> BlackPins = new List<PinBase>();

    [Header("Pinの配置(Prefab)")]
    [SerializeField] private GameObject[] _pinPrefab = new GameObject[3];

    private GameManager gameManager;
    private Vector3 Position = new Vector3(0, 0, 0);

    public void InsertPin(int stageNum)
    {
        int rnd = Random.Range(0, 3);
        switch (stageNum)
        {
            case 0:
                Position = new Vector3(42, -2, 0);
                break;
            case 1:
                Position = new Vector3(33530, -2, 863);
                break;
            case 2:
                Position = new Vector3(58, 4, 25494);
                break;

        }
        GameObject parent = Instantiate(_pinPrefab[rnd], Position, Quaternion.identity);
        
        PinBase[] childPins = parent.GetComponentsInChildren<PinBase>();
        foreach (PinBase pin in childPins)
        {
            string name = pin.gameObject.name;

            if (name.StartsWith("WhitePin"))
            {
                WhitePins.Add(pin);
            }
            else if (name.StartsWith("GoldPin"))
            {
                GoldPins.Add(pin);
            }
            else if (name.StartsWith("BlackPin"))
            {
                BlackPins.Add(pin);
            }
            else
            {
                Debug.LogWarning($"未分類のピンオブジェクト名: {name}");
            }
        }
    }

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

        foreach (PinBase pin in GoldPins)//金色のピンは多めに加点
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
        return WhitePins.Concat(GoldPins).Concat(BlackPins).ToArray();
    }
}
