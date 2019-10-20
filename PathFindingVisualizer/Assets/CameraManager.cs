using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float m_MovementSpeed;
    [SerializeField]
    private float m_RotationSpeed;

    [SerializeField]
    private GameObject m_GraphGameObject;

    [SerializeField]
    private Camera m_MainCamera;

    private General m_MainScript;
    [SerializeField]
    private GameObject m_InitialCameraRotationObject;

    private void Start()
    {
        m_MainScript = m_GraphGameObject.GetComponent<General>();
        positionMainCamera();
    }

    private void positionMainCamera()
    {
        
        int cameraX = m_MainScript.K_Columns / 2;
        int cameraZ = m_MainScript.K_Rows/ 2;
        int cameraY = (int)((m_MainScript.K_Rows + m_MainScript.K_Columns) * 0.5f);
        m_MainCamera.transform.position = new Vector3(2.5f*cameraX, cameraY, cameraZ);
        m_MainCamera.transform.rotation = m_InitialCameraRotationObject.transform.rotation;

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W))//up
        {
            transform.position += transform.up * (Time.deltaTime * m_MovementSpeed);
        }
        if (Input.GetKey(KeyCode.S))//down
        {
            transform.position -= transform.up * (Time.deltaTime * m_MovementSpeed);
        }
        if (Input.GetKey(KeyCode.D))//right
        {
            transform.position += transform.right * (Time.deltaTime * m_MovementSpeed);
        }
        if (Input.GetKey(KeyCode.A))//left
        {
            transform.position -= transform.right * (Time.deltaTime * m_MovementSpeed);
        }
        if (Input.GetKey(KeyCode.Q))//rotate down
        {
            transform.RotateAround(transform.position, transform.right, Time.deltaTime * m_RotationSpeed);
        }
        if (Input.GetKey(KeyCode.E))//rotate up
        {
            transform.RotateAround(transform.position, transform.right, Time.deltaTime * (-m_RotationSpeed));
        }

    }

    void OnGUI()
    {
        transform.position += transform.forward* (Input.mouseScrollDelta.y *(Time.deltaTime * m_MovementSpeed*2));
    }
}
