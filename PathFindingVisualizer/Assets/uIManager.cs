using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_GraphObject;
    private General m_GameScript;
    private int m_BoardSize = 0;
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

    public void OnVolumeChanged(float volume)
    {
        m_GameScript.DrawSpeed = -volume;
    }

    public void OnBoardSizeChanged(float i_newSize)
    {
        int newBoardSize = (int)i_newSize;
        if(newBoardSize != m_BoardSize)
        {
            m_BoardSize = newBoardSize;
            Debug.Log("size: " + m_BoardSize);
            m_GameScript.ReInitalizeGame(m_BoardSize);

        }
        
    }
}
