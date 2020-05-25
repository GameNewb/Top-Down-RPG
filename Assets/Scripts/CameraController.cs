using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform targetObject;

    [SerializeField] private Tilemap tileMap;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    private float halfHeight;
    private float halfWidth;

    private Vector3 bottomLeftAspectRatio;
    private Vector3 topRightAspectRatio;

    // Start is called before the first frame update
    void Start()
    {
        // Set to Player obj
        targetObject = PlayerController.instance.transform;

        // Get current ratio of Camera
        this.CalculateCameraClamp();

        // Set Camera Limits when moving camera
        bottomLeftLimit = tileMap.localBounds.min + bottomLeftAspectRatio;
        topRightLimit = tileMap.localBounds.max + topRightAspectRatio; 
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        // Move Camera based on player position
        transform.position = new Vector3(targetObject.position.x, targetObject.position.y, transform.position.z);

        // Keep Camera within tilemap bounds
        float xCameraClamp = Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x);
        float yCameraClamp = Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y);
        transform.position = new Vector3(xCameraClamp, yCameraClamp, transform.position.z);
    }

    private void CalculateCameraClamp()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
        bottomLeftAspectRatio = new Vector3(halfWidth, halfHeight, 0f);
        topRightAspectRatio = new Vector3(-halfWidth, -halfHeight, 0f);
    }
}
