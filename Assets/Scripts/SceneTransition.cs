using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    [SerializeField] string sceneTransitionName;

    [SerializeField] SceneEntrance sceneEntrance;

    // Start is called before the first frame update
    void Start()
    {
        sceneEntrance.transitionName = sceneTransitionName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(sceneToLoad);

            // Keep track of the transition scene name
            PlayerController.instance.sceneTransitionName = sceneTransitionName;
        }
    }
}
