using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Variables used for transitioning the Player to the next scene
    [SerializeField] string sceneToLoad;
    [SerializeField] string sceneTransitionFrom;

    [SerializeField] SceneEntrance sceneEntrance;

    [SerializeField] float loadTimer = 1f;

    private PlayerController playerInstance;
    private GameMenu gameMenu;

    // Start is called before the first frame update
    void Start()
    {
        //sceneEntrance.sceneTransitionedFrom = sceneTransitionFrom;
        playerInstance = PlayerController.instance;
        gameMenu = GameMenu.instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides with transition box, move to next scene
        if (other.tag == "Player")
        {
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        // Fade the screen from black to white
        // Disable player controls when loading
        UIFade.instance.FadeToBlack();
        PlayerController.instance.sceneTransitionFrom = sceneTransitionFrom;
        PlayerController.instance.isLoading = true;
        gameMenu.DisableControl(true);
        yield return new WaitForSeconds(loadTimer);
        SceneManager.LoadScene(sceneToLoad);
        UIFade.instance.FadeFromBlack();
        PlayerController.instance.isLoading = false;
        gameMenu.DisableControl(false);
    }
}
