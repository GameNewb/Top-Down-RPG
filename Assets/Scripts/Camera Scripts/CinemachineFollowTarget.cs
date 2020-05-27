using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineFollowTarget : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        var cinemachineCamera = GetComponent<CinemachineVirtualCamera>();

        // If Object is not initialize, find the Player game object
        if (targetObject == null)
        {
            targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        cinemachineCamera.Follow = targetObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
