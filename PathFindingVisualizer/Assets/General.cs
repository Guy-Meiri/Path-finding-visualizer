using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using System;

public class General : MonoBehaviour
{

    [SerializeField]
    private Camera m_MainCamera;

    [SerializeField]
    private GameObject m_CellPrefab;

    [SerializeField]
    private int k_Rows;
    [SerializeField]
    private int k_Columns;

    [SerializeField]
    private float m_DrawSpeed;

    private bool m_IsStartNodePicked = false;
    private bool m_IsTargetNodePicked = false;
    private bool m_IspathDrawn = false;
    private bool m_IsCurrentlyDrawing = false;

    private GameObject m_StartNodeGameObject;
    private GameObject m_TargetNodeGameObject;

    private INode m_StartingNode;
    private INode m_TargetNode;

    private MyDirectedGraph m_Graph;
    private int m_DefaultWeight = 1;

    [SerializeField]
    private Material m_DefaultMaterail;

    private PathFinder m_Pathfinder;
    private readonly int k_StartNodeSelection = 0;
    private readonly int k_ClearNodeSelection = 0;
    private readonly int k_TargetNodeSelection = 1;
    private readonly int k_ObstacleNodeSelection = 2;

    private Color m_StartColor = Color.blue;
    private Color m_TargetColor = Color.cyan;
    private Color m_PathColor = Color.green;
    private Color m_ObstacleColor = Color.red;
    private NeighborsPositionCalculator m_NeighborsPositionCalculator;
    void Start()
    {
        m_Graph = new MyDirectedGraph();
        m_Pathfinder = new PathFinder();
        m_NeighborsPositionCalculator = new NeighborsPositionCalculator(k_Rows, k_Columns);
        buildGraph();
        positionMainCamera();
    }
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && !m_IsCurrentlyDrawing)
        {
            GameObject currentCellObject = hit.transform.gameObject;
            Renderer renderer = currentCellObject.GetComponent<Renderer>();
            CellPrefabScript currentScript = currentCellObject.GetComponent<CellPrefabScript>();

            if (Input.GetKey(KeyCode.X))
            {
                clearStartNodeIfNeededColliding(currentCellObject, renderer);
                clearTargetNodeIfNeededColliding(currentCellObject, renderer);

                //update Weights
                int currentNodeId = getNodeIdFromCellGameObject(currentCellObject);
                updateWeights(currentNodeId, m_DefaultWeight);
                renderer.material.color = m_DefaultMaterail.color;
            }
            else if (Input.GetMouseButtonDown(k_StartNodeSelection))
            {
                clearStartNode(currentCellObject, renderer);
                m_IsStartNodePicked = true;
                m_StartNodeGameObject = currentCellObject;
                m_StartingNode = m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_StartNodeGameObject));
                renderer.material.color = m_StartColor;

                m_IspathDrawn = false;
            }
            else if (Input.GetMouseButtonDown(k_TargetNodeSelection))
            {
                clearTargetNode(currentCellObject, renderer);
                m_IsTargetNodePicked = true;
                m_TargetNodeGameObject = currentCellObject;
                m_TargetNode = m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_TargetNodeGameObject));
                renderer.material.color = m_TargetColor;

                m_IspathDrawn = false;
            }
            else if (Input.GetMouseButton(k_ObstacleNodeSelection))
            {
                clearStartNodeIfNeededColliding(currentCellObject, renderer);
                clearTargetNodeIfNeededColliding(currentCellObject, renderer);

                //update node and neighbors
                int id = getNodeIdFromCellGameObject(currentCellObject);
                foreach (IEdge neighbor in m_Graph.GetNeighbors(m_Graph.GetNodeById(id)))
                {
                    m_Graph.UpdateWeight(neighbor, 1000);
                }
                renderer.material.color = m_ObstacleColor;
            }

        }

        calcAndDrawPathIfNeeded();
    }

    private void calcAndDrawPathIfNeeded()
    {
        if (m_IsStartNodePicked && m_IsTargetNodePicked)
        {
            if (!m_IspathDrawn)
            {
                m_IsCurrentlyDrawing = true;
                clearBoard();
                IList<INode> res = m_Pathfinder.Dijkstra(m_Graph, m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_StartNodeGameObject)), m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_TargetNodeGameObject)));
                drawPath(res);
                m_IspathDrawn = true;
            }
        }
        
    }

    private void buildGraph()
    {
        createNodes();
        createEdges();
    }

    private void createEdges()
    {
        for (int x = 0; x < k_Columns; x++)
        {
            for (int z = 0; z < k_Rows; z++)
            {
                //foreach (Vector3 neighbor in getSuroundingCells(x, 0, z))
                foreach (Vector3 neighbor in m_NeighborsPositionCalculator.GetSuroundingCells(x, 0, z))
                {
                    MyEdge edge = new MyEdge(m_Graph.GetNodeById((int)(z * k_Rows + x)), m_Graph.GetNodeById((int)(neighbor.z * k_Rows + neighbor.x)), 1);
                    m_Graph.AddEdge(edge);
                }
            }
        }
    }

    private void createNodes()
    {
        MyUnityNode currNode;
        for (int z = 0; z < k_Rows; z++)
        {
            for (int x = 0; x < k_Columns; x++)
            {
                Vector3 newCellPosition = new Vector3(x, 0, z);
                GameObject cellPrefab = Instantiate(m_CellPrefab, newCellPosition, Quaternion.identity, gameObject.transform);
                cellPrefab.GetComponent<CellPrefabScript>().Position = newCellPosition;
                currNode = new MyUnityNode(x + z * k_Rows, 11, newCellPosition, cellPrefab);
                m_Graph.AddNode(currNode);
            }
        }
    }

    private void positionMainCamera()
    {
        int cameraX = k_Columns / 2;
        int cameraZ = k_Rows / 2;
        int cameraY = k_Rows + k_Columns - 9;
        m_MainCamera.transform.position = new Vector3(cameraX, cameraY, cameraZ);

    }

    IEnumerator colorCellAfterSeconds(GameObject i_Cell, float i_TimeToWait)
    {
        yield return new WaitForSeconds(i_TimeToWait);
        i_Cell.GetComponent<Renderer>().material.color = Color.green;
    }

    private void clearStartNode(GameObject i_CellGameObject, Renderer selectionRenderer)
    {
        if (m_IsStartNodePicked && m_StartNodeGameObject != null)
        {
            updateWeights(m_StartingNode.GetNodeId(), m_DefaultWeight);
            m_StartNodeGameObject.GetComponent<Renderer>().material.color = m_DefaultMaterail.color;
            m_StartingNode = null;
            m_StartNodeGameObject = null;
            m_IsStartNodePicked = false;
        }
    }

    private void clearTargetNode(GameObject i_CellGameObject, Renderer selectionRenderer)
    {
        if (m_IsTargetNodePicked && m_TargetNodeGameObject != null)
        {
            updateWeights(m_TargetNode.GetNodeId(), m_DefaultWeight);
            m_TargetNodeGameObject.GetComponent<Renderer>().material.color = m_DefaultMaterail.color;
            m_TargetNode = null;
            m_TargetNodeGameObject = null;
            m_IsTargetNodePicked = false;
        }
    }

    private void clearStartNodeIfNeededColliding(GameObject i_CellGameObject, Renderer selectionRenderer)
    {
        if (m_IsStartNodePicked)
        {
            int nodeId = getNodeIdFromCellGameObject(i_CellGameObject);
            if (nodeId == m_StartingNode.GetNodeId())
            {
                updateWeights(nodeId, m_DefaultWeight);
                selectionRenderer.material.color = m_DefaultMaterail.color;
                m_StartingNode = null;
                m_StartNodeGameObject = null;
                m_IsStartNodePicked = false;
            }
        }
    }

    private void clearTargetNodeIfNeededColliding(GameObject i_CellGameObject, Renderer selectionRenderer)
    {
        if (m_IsTargetNodePicked)
        {
            int nodeId = getNodeIdFromCellGameObject(i_CellGameObject);
            if (nodeId == m_TargetNode.GetNodeId())
            {
                updateWeights(nodeId, m_DefaultWeight);
                selectionRenderer.material.color = m_DefaultMaterail.color;
                m_TargetNode = null;
                m_TargetNodeGameObject = null;
                m_IsTargetNodePicked = false;
            }
        }
    }

    private void updateWeights(int i_Id, int i_Weight)
    {
        foreach (IEdge neighbor in m_Graph.GetNeighbors(m_Graph.GetNodeById(i_Id)))
        {
            m_Graph.UpdateWeight(neighbor, i_Weight);
        }
    }

    private void clearCell(Renderer i_CellRenderer)
    {
        if (i_CellRenderer.material.color == Color.blue)
        {
            m_IsStartNodePicked = false;
            m_StartingNode = null;
            m_StartNodeGameObject = null;
        }
        else if (i_CellRenderer.material.color == Color.cyan)
        {
            m_IsTargetNodePicked = false;
            m_TargetNode = null;
            m_TargetNodeGameObject = null;
        }
        i_CellRenderer.material = m_DefaultMaterail;
    }

    private void clearBoard()
    {
        foreach (MyUnityNode node in m_Graph.GetAllNodes())
        {
            Material defaultMaterial = node.CellPrefab.GetComponent<CellPrefabScript>().DefaultMaterial;

            Material currCellMaterial = node.CellPrefab.GetComponent<Renderer>().material;
            //if(currCellMaterial.color != Color.red && currCellMaterial.color != Color.blue && currCellMaterial.color != Color.cyan)
            if (currCellMaterial.color != Color.red && node.GetNodeId() != m_StartingNode.GetNodeId() && node.GetNodeId() != m_TargetNode.GetNodeId())
            {
                node.CellPrefab.GetComponent<Renderer>().material = defaultMaterial;
            }

        }
    }

    private void drawPath(IList<INode> res)
    {
        int nodesDrawnCount = 0;
        for (int i = 1; i < res.Count - 1; i++)
        {
            StartCoroutine(colorCellAfterSeconds(((MyUnityNode)res[i]).CellPrefab, nodesDrawnCount * m_DrawSpeed));
            nodesDrawnCount++;
            if(i == res.Count - 2)
            {
                m_IsCurrentlyDrawing = false;
            }
        }
    }

    private int getNodeIdFromCellGameObject(GameObject i_Cell)
    {
        CellPrefabScript cellScript = i_Cell.GetComponent<CellPrefabScript>();
        return (int)(cellScript.Position.x + cellScript.Position.z * k_Columns);
    }

    //private IList<Vector3> getSuroundingCells(int i_Column, int i_Depth, int i_Row)
    //{
    //    List<Vector3> suroundingCells = new List<Vector3>();

    //    //if the cell is not on one of the edges
    //    if (((i_Row > 0) && (i_Row < k_Rows - 1)) && ((i_Column > 0) && (i_Column < k_Columns - 1)))
    //    {
    //        AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
    //        AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
    //    }


    //    //CORNERS!
    //    //UpperLeft corner
    //    else if (i_Row == 0 && i_Column == 0)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));

    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
    //    }
    //    //UpperRight corner
    //    else if (i_Row == 0 && i_Column == k_Columns - 1)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));
    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
    //    }

    //    //ButtomLeft corner
    //    else if (i_Row == k_Rows - 1 && i_Column == 0)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));
    //        suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));
    //    }

    //    //ButtomRight corner
    //    else if (i_Row == k_Rows - 1 && i_Column == k_Columns - 1)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));
    //        suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));
    //    }


    //    //Edge but not a corner
    //    else if (i_Row == 0)
    //    {

    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
    //        AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
    //        //Debug.Log("In log: " + suroundingCells.Count);
    //        //suroundingCells.Add(new Vector3(i_Row + 1, 0, i_Column - 1));
    //        //suroundingCells.Add(new Vector3(i_Row + 1, 0, i_Column));
    //        //suroundingCells.Add(new Vector3(i_Row + 1, 0, i_Column + 1));
    //    }
    //    else if (i_Row == k_Rows - 1)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));
    //        AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
    //    }
    //    else if (i_Column == 0)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));
    //        AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
    //    }
    //    else if (i_Column == k_Columns - 1)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));
    //        AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
    //    }

    //    return suroundingCells;
    //}

    //private IList<Vector3> getSuroundingCellsIncludingDiagonals(int i_Column, int i_Depth, int i_Row)
    //{
    //    List<Vector3> suroundingCells = new List<Vector3>();

    //    //if the cell is not on one of the edges
    //    if (((i_Row > 0) && (i_Row < k_Rows - 1)) && ((i_Column > 0) && (i_Column < k_Columns - 1)))
    //    {
    //        AddRowAbovePositions(i_Column, i_Row, suroundingCells);
    //        AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
    //        AddRowBelowPositions(i_Column, i_Row, suroundingCells);
    //    }


    //    //CORNERS!
    //    //UpperLeft corner
    //    else if (i_Row == 0 && i_Column == 0)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));

    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
    //        suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row + 1));

    //    }
    //    //UpperRight corner
    //    else if (i_Row == 0 && i_Column == k_Columns - 1)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));

    //        suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row + 1));
    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
    //    }

    //    //ButtomLeft corner
    //    else if (i_Row == k_Rows - 1 && i_Column == 0)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));
    //        suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row - 1));

    //        suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));
    //    }

    //    //ButtomRight corner
    //    else if (i_Row == k_Rows - 1 && i_Column == k_Columns - 1)
    //    {
    //        suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row - 1));
    //        suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));

    //        suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));
    //    }


    //    //Edge but not a corner
    //    else if (i_Row == 0)
    //    {
    //        AddRowBelowPositions(i_Column, i_Row, suroundingCells);
    //        AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
    //        //Debug.Log("In log: " + suroundingCells.Count);
    //        //suroundingCells.Add(new Vector3(i_Row + 1, 0, i_Column - 1));
    //        //suroundingCells.Add(new Vector3(i_Row + 1, 0, i_Column));
    //        //suroundingCells.Add(new Vector3(i_Row + 1, 0, i_Column + 1));
    //    }
    //    else if (i_Row == k_Rows - 1)
    //    {
    //        AddRowAbovePositions(i_Column, i_Row, suroundingCells);
    //        AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
    //    }
    //    else if (i_Column == 0)
    //    {
    //        addRightColumn(i_Column, i_Row, suroundingCells);
    //        AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
    //    }
    //    else if (i_Column == k_Columns - 1)
    //    {
    //        addLeftColumn(i_Column, i_Row, suroundingCells);
    //        AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
    //    }

    //    return suroundingCells;
    //}


    //private static void addLeftColumn(int x, int z, List<Vector3> suroundingCells)
    //{
    //    suroundingCells.Add(new Vector3(x - 1, 0, z - 1));

    //    suroundingCells.Add(new Vector3(x - 1, 0, z));
    //    suroundingCells.Add(new Vector3(x - 1, 0, z + 1));
    //}

    //private static void addRightColumn(int x, int z, List<Vector3> suroundingCells)
    //{
    //    suroundingCells.Add(new Vector3(x + 1, 0, z - 1));

    //    suroundingCells.Add(new Vector3(x + 1, 0, z));
    //    suroundingCells.Add(new Vector3(x + 1, 0, z + 1));
    //}

    //private static void AddRowBelowPositions(int x, int z, List<Vector3> suroundingCells)
    //{
    //    suroundingCells.Add(new Vector3(x - 1, 0, z + 1));
    //    suroundingCells.Add(new Vector3(x, 0, z + 1));
    //    suroundingCells.Add(new Vector3(x + 1, 0, z + 1));
    //}

    //private static void AddLeftAndRightPositions(int x, int z, List<Vector3> suroundingCells)
    //{
    //    suroundingCells.Add(new Vector3(x - 1, 0, z));
    //    suroundingCells.Add(new Vector3(x + 1, 0, z));
    //}

    //private void AddAboveAndBelowPositions(int x, int z, List<Vector3> suroundingCells)
    //{
    //    suroundingCells.Add(new Vector3(x, 0, z + 1));
    //    suroundingCells.Add(new Vector3(x, 0, z - 1));
    //}


    //private static void AddRowAbovePositions(int x, int z, List<Vector3> suroundingCells)
    //{
    //    suroundingCells.Add(new Vector3(x - 1, 0, z - 1));
    //    suroundingCells.Add(new Vector3(x, 0, z - 1));
    //    suroundingCells.Add(new Vector3(x + 1, 0, z - 1));
    //}
}
