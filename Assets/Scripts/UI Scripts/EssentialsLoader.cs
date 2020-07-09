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
            if (sceneLoader != null)
            {
                Vector3 sceneLoaderPosition = sceneLoader.transform.position;
                Quaternion rotation = sceneLoader.transform.rotation;

                Instantiate(player, sceneLoaderPosition, rotation);
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
    }

}
