using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float m_MovementSpeed;
    [SerializeField]
    private float m_RotationSpeed;

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
