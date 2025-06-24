using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ColorAssigner : MonoBehaviour
{
    public static ColorAssigner Instance { get; private set; }

    private List<Color> colorPalette = new List<Color>
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta,
    };

    private List<Color> shuffledPalette;
    private int currentColorIndex = 0;
    private Dictionary<GameObject, Color> assignedColors = new Dictionary<GameObject, Color>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePalette();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePalette()
    {
        shuffledPalette = colorPalette.OrderBy(x => Random.value).ToList();
        currentColorIndex = 0;
        assignedColors.Clear();
    }

    public void ResetPalette()
    {
        InitializePalette();
    }

    public Color AssignColorToObject(GameObject obj)
    {
        if (obj == null) return Color.white;

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null) return Color.white;

        if (currentColorIndex >= shuffledPalette.Count)
        {
            currentColorIndex = 0;
        }

        Color assignedColor = shuffledPalette[currentColorIndex];
        renderer.material.color = assignedColor;
        assignedColors[obj] = assignedColor;
        currentColorIndex++;

        return assignedColor;
    }

    public void AssignColors()
    {
        currentColorIndex = 0;
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject player in playerObjects)
        {
            AssignColorToObject(player);
        }

        foreach (GameObject npc in npcObjects)
        {
            AssignColorToObject(npc);
        }
    }

    /// <summary>
    /// キャラクターの色取得
    /// </summary>
    /// <param name="obj">キャラクター</param>
    /// <returns>適応されている色</returns>
    public Color GetAssignedColor(GameObject obj)
    {
        if (assignedColors.ContainsKey(obj))
        {
            return assignedColors[obj];
        }
        else
        {
            return Color.white;
        }
    }
}