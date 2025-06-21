using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;

public class ColorAssigner : MonoBehaviour
{ 
    public GameObject[] objects;

    private GameManager gameManager;

    private List<Color> colorPalette = new List<Color>
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta,
    };

    void Start()
    {

        if ((gameManager.NumHumanPlayers + gameManager.NumBots) > 4)
        {
            Debug.LogWarning("最大オブジェクト数は4つです。");
            return;
        }

        List<Color> availableColors = colorPalette.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                Renderer renderer = objects[i].GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material newMaterial = new Material(renderer.material);
                    newMaterial.color = availableColors[i];
                    renderer.material = newMaterial;
                }
            }
        }
    }
}
