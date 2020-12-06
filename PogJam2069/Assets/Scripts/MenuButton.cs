using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   //SceneManager.LoadSceneAsync()

public class MenuButton : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadSceneAsync("SampleScene");
        AudioManager.Amanager.playClickButton();
    }

    public void Credits() {
        SceneManager.LoadSceneAsync("Credits");
        AudioManager.Amanager.playClickButton();
    }

    public void QuitGame() {
        Application.Quit();
        AudioManager.Amanager.playClickButton();
    }

    public void MainMenu() {
        SceneManager.LoadSceneAsync("MainMenu");
        AudioManager.Amanager.playClickButton();
    }


    public void HoverSound() {
        AudioManager.Amanager.playHoverButton();
    }
}
