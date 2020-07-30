using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    [SerializeField] GameObject UIScreen;
    [SerializeField] GameObject player;
    [SerializeField] GameObject sceneLoader;
    [SerializeField] GameObject gameMngr;
    [SerializeField] GameObject audioManager;
    [SerializeField] GameObject battleManager;

    // Start is called before the first frame update
    void Awake()
    {
        // Instantiate fade screen
        if (UIFade.instance == null)
        {
            Instantiate(UIScreen);
        }

        // Initialize Player and its position
        if (PlayerController.instance == null)
        {
            // Set the Player at the scene entrance position
            // We're loading from a scene
            if (sceneLoader != null)
            {
                Vector3 sceneLoaderPosition = sceneLoader.transform.position;
                Quaternion rotation = sceneLoader.transform.rotation;

                Instantiate(player, sceneLoaderPosition, rotation);
            } 
            else
            {
                // If we get here, we're loading from main menu
                Vector3 playerPosition = new Vector3(PlayerPrefs.GetFloat("Player_Position_X"), PlayerPrefs.GetFloat("Player_Position_Y"), PlayerPrefs.GetFloat("Player_Position_Z"));
                Quaternion playerRotation = new Quaternion(0, 0, 0, 0);
               
                Instantiate(player, playerPosition, playerRotation);
            }
        }

        // Initialize GameManager
        if (GameManager.instance == null)
        {
            Instantiate(gameMngr);
        }

        // Initialize Audio Manager
        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }

        // Initialize Audio Manager
        if (BattleManager.instance == null)
        {
            Instantiate(battleManager);
        }
    }

}
