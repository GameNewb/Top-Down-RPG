using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string mainMenuScene;
    public string loadGameScene;

    // Start is called before the first frame update
    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBGM(4);
        }

        if (PlayerController.instance != null)
        {
            PlayerController.instance.gameObject.SetActive(false);
        }

        if (GameMenu.instance != null)
        {
            GameMenu.instance.gameObject.SetActive(false);
        }

        if (BattleManager.instance != null)
        {
            BattleManager.instance.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitToMain()
    {
        // Clean gameplay
        this.DestroyInstances();

        if (AudioManager.instance != null)
        {
            Destroy(AudioManager.instance.gameObject);
        }

        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLastSave()
    {
        // Clean gameplay
        this.DestroyInstances();

        SceneManager.LoadScene(loadGameScene);
    }

    private void DestroyInstances()
    {
        if (GameManager.instance != null)
        {
            Destroy(GameManager.instance.gameObject);
        }

        if (PlayerController.instance != null)
        {
            Destroy(PlayerController.instance.gameObject);
        }

        if (GameMenu.instance != null)
        {
            Destroy(GameMenu.instance.gameObject);
        }

        if (BattleManager.instance != null)
        {
            Destroy(BattleManager.instance.gameObject);
        }
    }
}
