using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that details where the player lands when the Scene loads
public class SceneEntrance : MonoBehaviour
{
    private PlayerController playerInstance;


    [Header("Scene Where Player Is Expected To Come From")]
    public string sceneTransitionedFrom;

    // Start is called before the first frame update
    void Start()
    {
        playerInstance = PlayerController.instance;

        // Set the position of player after transitioning into a new scene
        if (playerInstance != null)
        {
            string playerTransitionName = playerInstance.sceneTransitionFrom;

            if (sceneTransitionedFrom == playerTransitionName)
            {
                playerInstance.transform.position = transform.position;
            }
        }
        
    }
    
}
