using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewMaskData", menuName = "GameTopeng/MaskData")]
public class MaskData : ScriptableObject
{
    public string maskLevelName;

    [Header("Ejaan Bahan (Urutan)")]
    public string[] ingredientNames; // Contoh: "PAPER", "SCISSOR", "GLUE"

    [Header("Visual Objek")]
    public Sprite[] visualObjek; // Contoh: gambar kertas, gunting, lem
}