using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_GraphObject;
    private General m_GameScript;
    // Start is called before the first frame update
    void Start()
    {
        m_GameScript = m_GraphObject.GetComponent<General>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBelmanFordButtonPressed()
    {
        m_GameScript.RunBelmanFord();
    }

    public void OnDfsPressed()
    {
        m_GameScript.RunDFS();
    }

    public void OnAstarButtonPressed()
    {
        m_GameScript.RunAstar();
    }

    public void OnDijkstraButtonPressed()
    {
        m_GameScript.RunDijkstra();
    }

    public void OnClearButtonPressed()
    {
        m_GameScript.ClearBoard();
    }
}
