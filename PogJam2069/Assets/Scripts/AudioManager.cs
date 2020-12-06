using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip musicClip1;
    public AudioClip axeChop1;

    public AudioClip btnHover;
    public AudioClip btnClick;

    [HideInInspector]
    public static AudioManager Amanager;
    public static GameObject musicObj;
    public static AudioSource musicSrc;
    public static GameObject sfxObj;
    public static AudioSource sfxSrc;


    void Awake() {
        if (Amanager != null && Amanager != this) {
            Destroy(this.gameObject);
        }
        else {
            Amanager = this;
            musicObj = this.gameObject.transform.GetChild(0).gameObject;
            musicSrc = musicObj.GetComponent<AudioSource>();
            sfxObj = this.gameObject.transform.GetChild(1).gameObject;
            sfxSrc = sfxObj.GetComponent<AudioSource>();
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        playMusic1();
    }

    public void playMusic1() {
        musicSrc.clip = musicClip1;
        musicSrc.Play();
    }

    public void playHoverButton() {
        sfxSrc.volume = 0.9f;
        sfxSrc.clip = btnHover;
        sfxSrc.Play(); 
    }

    public void playClickButton() {
        sfxSrc.volume = 0.7f;
        sfxSrc.clip = btnClick;
        sfxSrc.Play();
    }
}
