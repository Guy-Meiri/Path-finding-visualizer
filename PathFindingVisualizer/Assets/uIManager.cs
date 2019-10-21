using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_GraphObject;
    private General m_GameScript;
    //private int m_BoardSize = 0;
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

    public void OnRowsChanged(float i_newRows)
    {
        int rows = (int)i_newRows;
        if (rows != m_GameScript.K_Rows)
        {
            //Debug.Log("rows changed to: " + rows);
            m_GameScript.ReInitalizeGame(rows, m_GameScript.K_Columns);
        }
    }

    public void OnColumnsChanged(float i_NewColumns)
    {
        int columns = (int)i_NewColumns;
        if (columns != m_GameScript.K_Columns)
        {
            //Debug.Log("columns changed to: " + columns);
            m_GameScript.ReInitalizeGame(m_GameScript.K_Rows, columns);
        }
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
    }
}
