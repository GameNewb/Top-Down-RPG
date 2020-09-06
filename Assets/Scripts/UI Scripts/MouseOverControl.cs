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
        Vector3 newTransform = new Vector3(gameObject.transform.position.x - 130f, gameObject.transform.position.y - 10f, gameObject.transform.position.z);
        menuCursor.transform.position = newTransform;
    }

}
