using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   //SceneManager.LoadSceneAsync()

public class MenuButton : MonoBehaviour
{
   public Animator anim;
    public void StartGame() {


        StartCoroutine(StartGgame());

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

    IEnumerator StartGgame()


    {
        anim.SetTrigger("asdf");
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("SampleScene");
        yield return null;

    }
}
