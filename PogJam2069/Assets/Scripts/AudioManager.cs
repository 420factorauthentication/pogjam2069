using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip musicClip1;

    public AudioClip btnHover;
    public AudioClip btnClick;

    public AudioClip axeChop1;
    public AudioClip treeFall1;
    public AudioClip givingTreeChop1;
    public AudioClip givingTreeFall1;

    [HideInInspector]
    public static AudioManager Amanager;
    public static GameObject musicObj;
    public static AudioSource musicSrc;
    public static GameObject sfxObj1;
    public static AudioSource sfxSrc1;
    public static GameObject sfxObj2;
    public static AudioSource sfxSrc2;


    void Awake() {
        if (Amanager != null && Amanager != this) {
            Destroy(this.gameObject);
        }
        else {
            Amanager = this;
            musicObj = this.gameObject.transform.GetChild(0).gameObject;
            musicSrc = musicObj.GetComponent<AudioSource>();
            sfxObj1 = this.gameObject.transform.GetChild(1).gameObject;
            sfxSrc1 = sfxObj1.GetComponent<AudioSource>();
            sfxObj2 = this.gameObject.transform.GetChild(2).gameObject;
            sfxSrc2 = sfxObj2.GetComponent<AudioSource>();
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        playMusic1();
    }

    // Music
    public void playMusic1() {
        musicSrc.clip = musicClip1;
        musicSrc.Play();
    }

    // UI
    public void playHoverButton() {
        sfxSrc1.volume = 0.9f;
        sfxSrc1.clip = btnHover;
        sfxSrc1.Play(); 
    }

    public void playClickButton() {
        sfxSrc2.volume = 0.7f;
        sfxSrc2.clip = btnClick;
        sfxSrc2.Play();
    }

    // Gameplay
    public void axeChop() {
        sfxSrc1.volume = 0.5f;
        sfxSrc1.clip = axeChop1;
        sfxSrc1.Play();
    }

    public void treeFall() {
        sfxSrc2.volume = 0.7f;
        sfxSrc2.clip = treeFall1;
        sfxSrc2.Play();
    }

    public void givingTreeChop() {
        sfxSrc1.volume = 0.5f;
        sfxSrc1.clip = givingTreeChop1;
        sfxSrc1.Play();
    }

    public void givingTreeFall() {
        sfxSrc2.volume = 0.7f;
        sfxSrc2.clip = givingTreeFall1;
        sfxSrc2.Play();
    }
}
