using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverControl : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private GameObject menuCursor;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        float currentScreenWidth = (float) Screen.width;
        float currentScreenHeight = (float) Screen.height;

        // Calculate the position of the cursor based on the resolution
        var xPosition = (currentScreenWidth/7) - gameObject.transform.position.x;
        var yPosition = gameObject.transform.position.y;

        Vector3 newTransform = new Vector3(xPosition, yPosition, gameObject.transform.position.z);
        menuCursor.transform.position = newTransform;
    }

}
