using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPrefabScript : MonoBehaviour
{
    private Vector3 m_Position;

    [SerializeField]
    private Material m_DefaultMaterial;

    public Vector3 Position { get => m_Position; set => m_Position = value; }
    public Material DefaultMaterial { get => m_DefaultMaterial; set => m_DefaultMaterial = value; }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
