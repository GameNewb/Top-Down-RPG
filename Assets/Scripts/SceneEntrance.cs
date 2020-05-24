using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    private PlayerController playerInstance;
    public string transitionName;

    // Start is called before the first frame update
    void Start()
    {
        playerInstance = PlayerController.instance;

        // Set the position of player after transitioning into a new scene
        if (playerInstance != null)
        {
            string playerTransitionName = playerInstance.sceneTransitionName;

            if (transitionName == playerTransitionName)
            {
                playerInstance.transform.position = transform.position;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
