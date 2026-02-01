using System;
using System.Collections;
using UnityEngine;

public class DrawCompleted : MonoBehaviour
{
    public bool outlineDetected = false;
    public Transform dots, lines;
    private bool isCompleting = false; // Flag baru untuk mengontrol proses completion
    public GameObject fakeDot;

    public static Action OnDrawComplete;

    public static Action OnFirstStepComplete;

    private void Start()
    {
        dots = GameObject.Find("Dots").transform;
        lines = GameObject.Find("Lines").transform;
        fakeDot.SetActive(true);
    }

    private void OnEnable()
    {

    }

    private void Update()
    {
        if (isCompleting) return; // Jika sudah dalam proses completion, hentikan pengecekan

        bool completed = true;
        for (int i = 0; i < dots.childCount; i++)
        {
            if (dots.GetChild(i).GetComponent<CircleCollider2D>().enabled)
            {
                completed = false;

                break;
            }
        }

        // Gunakan isCompleting untuk mencegah multiple execution dalam satu frame
        if (completed && !outlineDetected && !isCompleting)
        {

            isCompleting = true; // Set flag bahwa proses completion sedang berjalan
                                 // Destroy lines
            Debug.Log("Drawing Completed!");
            OnDrawComplete?.Invoke();
            StartCoroutine(ResetDrawing());

        }
    }


    IEnumerator ResetDrawing()
    {
        yield return new WaitForSeconds(0.3f);
        fakeDot.SetActive(false);
        // 2. Bersihkan semua garis sekaligus (bukan satu per satu dengan coroutine terpisah)
        for (int i = lines.childCount - 1; i >= 0; i--)
        {
            Destroy(lines.GetChild(i).gameObject);
        }

        yield return new WaitForSeconds(0.5f);

        // 3. Panggil fungsi utama di CraftingSystem untuk ganti ke Topeng Jadi
        OnFirstStepComplete?.Invoke();

        // Nonaktifkan mini-game ini setelah selesai
        gameObject.SetActive(false);

        isCompleting = false;
    }

    private void OnDisable()
    {
        CancelInvoke("SetSFXPlayedFlag");
    }

    public void DeleteAllLines()
    {
        foreach (Transform line in lines)
        {
            Destroy(line.gameObject);
        }
    }
}