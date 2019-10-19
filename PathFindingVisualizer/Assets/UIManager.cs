using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private General m_GameScript;

    // Start is called before the first frame update
    void Start()
    {
        m_GameScript = GetComponent<General>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAStarButtonPressed()
    {
        Debug.Log("a star pressed");
    }
}
