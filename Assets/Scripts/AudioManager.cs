using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] soundEffects;

    // Dictionary untuk menyimpan mapping nama ke AudioClip
    private Dictionary<string, AudioClip> sfxDictionary;

    private bool isBGMActive = true;
    private bool isSFXActive = true;

    private void Awake()
    {
        // Singleton pattern implementation
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Setup audio sources jika belum ada
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        // Inisialisasi dictionary
        InitializeSFXDictionary();
    }

    private void InitializeSFXDictionary()
    {
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in soundEffects)
        {
            if (clip != null)
            {
                sfxDictionary[clip.name] = clip;
            }
        }
    }

    private void Start()
    {
        PlayBGM();
    }

    private void PlayBGM()
    {
        if (backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.Play();
        }
    }

    // Method untuk memainkan SFX dengan index
    public void PlaySFX(int sfxIndex)
    {
        if (isSFXActive && sfxIndex >= 0 && sfxIndex < soundEffects.Length)
        {
            sfxSource.PlayOneShot(soundEffects[sfxIndex]);
        }
        else
        {
            Debug.LogWarning($"SFX dengan index {sfxIndex} tidak ditemukan!");
        }
    }

    // Method untuk memainkan SFX dengan nama
    public void PlaySFX(string sfxName)
    {
        if (isSFXActive && sfxDictionary.ContainsKey(sfxName))
        {
            sfxSource.PlayOneShot(sfxDictionary[sfxName]);
        }
        else
        {
            Debug.LogWarning($"SFX dengan nama {sfxName} tidak ditemukan!");
        }
    }

    // Toggle BGM on/off
    public void ToggleBGM()
    {
        isBGMActive = !isBGMActive;
        bgmSource.mute = !isBGMActive;
    }

    // Toggle SFX on/off
    public void ToggleSFX()
    {
        isSFXActive = !isSFXActive;
        sfxSource.mute = !isSFXActive;
    }

    public bool IsBGMActive()
    {
        return isBGMActive;
    }

    public bool IsSFXActive()
    {
        return isSFXActive;
    }

    // Method untuk mendapatkan nama semua SFX yang tersedia
    public string[] GetAvailableSFXNames()
    {
        string[] names = new string[sfxDictionary.Count];
        sfxDictionary.Keys.CopyTo(names, 0);
        return names;
    }
}