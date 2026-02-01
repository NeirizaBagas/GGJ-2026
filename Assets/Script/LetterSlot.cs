using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterSlot : MonoBehaviour, IDropHandler
{
    public char hurufTarget;
    private TextMeshProUGUI slotText;

    public static Action OnLetterPlaced;

    private void Start()
    {
        slotText = GetComponentInChildren<TextMeshProUGUI>();
        if (slotText != null)
        {
            slotText.text = hurufTarget.ToString();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 1. Pastikan objek yang dilepas punya script DragnDrop
        DragnDrop draggedLetter = eventData.pointerDrag.GetComponent<DragnDrop>();

        if (draggedLetter != null)
        {
            // 2. Cek apakah slot masih kosong?
            if (transform.childCount <= 1)
            {
                // 3. Cek apakah hurufnya sesuai dengan target slot ini?
                if (draggedLetter.hurufSaya == hurufTarget)
                {
                    // BENAR: Masukkan ke slot
                    draggedLetter.transform.SetParent(this.transform);

                    // Reset posisi lokal agar pas di tengah (0,0,0)
                    RectTransform rect = draggedLetter.GetComponent<RectTransform>();
                    rect.localPosition = Vector2.zero;

                    Debug.Log("Huruf " + hurufTarget + " benar!");

                    // Panggil Event untuk lapor ke GameManager
                    OnLetterPlaced?.Invoke();
                }
                else
                {
                    // SALAH: Jangan lakukan apa-apa. 
                    // DragnDrop.cs akan mendeteksi parent-nya tidak berubah, 
                    // lalu memulangkannya ke posisi awal di OnEndDrag.
                    Debug.Log("Huruf salah! Seharusnya: " + hurufTarget);
                }
            }
            else
            {
                Debug.Log("Slot sudah terisi!");
            }
        }
    }
}
