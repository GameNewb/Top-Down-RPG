using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    // Transform of the GameObject you want to shake
    public Transform transform;

    // Desired duration of the shake effect
    [SerializeField] private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    public float shakeMagnitude = 0.7f;

    // A measure of how quickly the shake effect should evaporate
    [SerializeField] private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    public Vector3 initialPosition;

    void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    
    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    public void TriggerShake(float shakeSecs)
    {
        shakeDuration = shakeSecs;
    }
    
    /* TODO - Fix Coroutine
    public IEnumerator ShakeCamera(float shakeSecs)
    {
        gameObject.GetComponent<CinemachineBrain>().enabled = false;
        shakeDuration = shakeSecs;

        while (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            Debug.Log("Shake: " + shakeDuration);
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }

        yield return new WaitForSeconds(2f);

        shakeDuration = 0f;
        transform.localPosition = initialPosition;
        gameObject.GetComponent<CinemachineBrain>().enabled = true;
    }*/
}
