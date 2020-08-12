using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG.Core {
    public class FollowCamera : MonoBehaviour
    {
        CinemachineVirtualCamera followCamera;
        float deadZoneWidth = 0f;
        float softZoneWidth = 0f;
        float zoomSpeed = 3f;
        float yAxis;
        void Start() {
            followCamera = GetComponent<CinemachineVirtualCamera>();
            deadZoneWidth = followCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth;
            softZoneWidth = followCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneWidth;
        }
        void Update() {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                followCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                followCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }

            if (Input.GetMouseButton(1)) {  // 右鍵 
                Vector3 rotateValue;
                yAxis = Input.GetAxis("Mouse X");
                rotateValue = new Vector3(0, yAxis * -10, 0);
                followCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0;
                followCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneWidth = 0;
                transform.eulerAngles = transform.eulerAngles - rotateValue;
            }
            if (Input.GetMouseButton(0)){  // 左鍵 
                followCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = deadZoneWidth;
                followCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_SoftZoneWidth = softZoneWidth;
            }
        }
    }
}
