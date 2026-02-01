using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class pembantu agar data bahan dan visualnya berpasangan
[System.Serializable]
public class ListBahan
{
    public string ingredientName; // Contoh: "PAPER", "SCISSOR"
    public Sprite visualObjek;     // Gambar untuk fase ejaan
}

[CreateAssetMenu(fileName = "NewMaskData", menuName = "GameTopeng/MaskData")]
public class MaskData : ScriptableObject
{
    [Header("Informasi Level")]
    public string maskLevelName;

    [Header("Fase 1: Ejaan (Spelling)")]
    // List ini digunakan saat menyusun huruf di awal permainan
    public List<ListBahan> bahanList;

    [Header("Fase 2: Crafting")]
    // Urutan item yang harus di-drop ke CraftingContainer
    // Contoh Level 1: "Kertas", "Gunting"
    // Contoh Level 3: "Karton", "Gunting", "Cat"
    public string[] craftingRequiredItems;

    [Header("Visual Final")]
    // Gambar topeng yang sudah jadi untuk level ini
    public Sprite finalMaskSprite;
}