using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    public float y;
    public Vector3 rotateValue;
    public float zoomSpeed = 10;

    void Update()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0) {
            GetComponentInChildren<Camera>().fieldOfView -= mouseScroll * zoomSpeed;
        }
        transform.position = target.position;
        if (Input.GetMouseButton(1)){  // 右鍵  
            y = Input.GetAxis("Mouse X");
            rotateValue = new Vector3(0, y * -10, 0);
            transform.eulerAngles = transform.eulerAngles - rotateValue;
        }
    }
}
