using UnityEngine;

[CreateAssetMenu(fileName = "BuffItem", menuName = "Items/BuffItem", order = 1)]
public class BuffItem : ScriptableObject
{
    [Tooltip("Speed‚Ì•Ï‰»—Ê")]
    public float speedModifier = 0f;

    [Tooltip("Weight‚Ì•Ï‰»—Ê")]
    public float weightModifier = 0f;

    [Tooltip("Rotation‚Ì•Ï‰»—Ê")]
    public float rotationModifier = 0f;

    [Tooltip("ƒAƒCƒeƒ€‚Ìà–¾")]
    public string description = "";
}