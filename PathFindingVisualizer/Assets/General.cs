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

    private GameObject m_StartNodeGameObject;

    private GameObject m_TargetNodeGameObject;

    private INode m_StartingNode;
    private INode m_TargetNode;

    private MyDirectedGraph m_Graph;
    private int m_DefaultWeight = 1;

    [SerializeField]
    private Material m_DefaultMaterail;

    [SerializeField]
    private Material m_ObstacleMaterial;

    [SerializeField]
    private Material m_PathMaterial;

    private PathFinder m_Pathfinder;
    private readonly int k_StartNodeSelection = 0;
    private readonly int k_ClearNodeSelection = 0;
    private readonly int k_TargetNodeSelection = 1;
    private readonly int k_ObstacleNodeSelection = 2;

    private Color m_StartColor = Color.blue;
    private Color m_TargetColor = Color.cyan;
    //private Color m_PathColor = Color.green;
    //private Color m_ObstacleColor = Color.red;
    private NeighborsPositionCalculator m_NeighborsPositionCalculator;

    public int K_Rows { get => k_Rows; }
    public int K_Columns { get => k_Columns; }

    void Start()
    {
        m_Graph = new MyDirectedGraph();
        m_Pathfinder = new PathFinder();
        m_NeighborsPositionCalculator = new NeighborsPositionCalculator(k_Rows, k_Columns);
        buildGraph();
        //debugDrawBoard();
    }


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Cell")
            {
                GameObject currentCellObject = hit.transform.gameObject;
                Renderer renderer = currentCellObject.GetComponent<Renderer>();
                CellPrefabScript currentScript = currentCellObject.GetComponent<CellPrefabScript>();

                if (Input.GetKey(KeyCode.X))
                {
                    clearStartNodeIfIsColliding(currentCellObject, renderer);
                    clearTargetNodeIfIsColliding(currentCellObject, renderer);
                    int id = getNodeIdFromCellGameObject(currentCellObject);
                    MyUnityNode node = (MyUnityNode)m_Graph.GetNodeById(id);
                    if (node.IsObstacle)
                    {
                        clearObstacleNodeIfIsColliding(currentCellObject, renderer); // adds edges if its removing an obstacle }
                    }

                    renderer.material.color = m_DefaultMaterail.color;

                }
                else if (Input.GetMouseButtonDown(k_StartNodeSelection))
                {
                    clearStartNode(currentCellObject, renderer);
                    clearObstacleNodeIfIsColliding(currentCellObject, renderer);
                    m_IsStartNodePicked = true;
                    m_StartNodeGameObject = currentCellObject;
                    m_StartingNode = m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_StartNodeGameObject));
                    renderer.material.color = m_StartColor;

                    m_IspathDrawn = false;
                }
                else if (Input.GetMouseButtonDown(k_TargetNodeSelection))
                {
                    clearTargetNode(currentCellObject, renderer);
                    clearObstacleNodeIfIsColliding(currentCellObject, renderer);
                    m_IsTargetNodePicked = true;
                    m_TargetNodeGameObject = currentCellObject;
                    m_TargetNode = m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_TargetNodeGameObject));
                    renderer.material.color = m_TargetColor;

                    m_IspathDrawn = false;
                }
                else if (Input.GetMouseButton(k_ObstacleNodeSelection))
                {
                    clearStartNodeIfIsColliding(currentCellObject, renderer);
                    clearTargetNodeIfIsColliding(currentCellObject, renderer);

                    //update node and neighbors
                    if (renderer.material.color != m_ObstacleMaterial.color)
                    {
                        int id = getNodeIdFromCellGameObject(currentCellObject);
                        m_Graph.RemoveNodeEdgesById(id);
                        m_Graph.GetNodeById(id).IsObstacle = true;
                        renderer.material.color = m_ObstacleMaterial.color;
                    }

                    //foreach (IEdge neighbor in m_Graph.GetNeighbors(m_Graph.GetNodeById(id)))
                    //{
                    //    m_Graph.UpdateWeight(neighbor, 1000);
                    //}

                }
                //calcAndDrawPathIfNeeded(); 
            }
        }

    }


    private void clearObstacleNodeIfIsColliding(GameObject i_CellGameObject, Renderer i_Renderer)
    {
        int nodeId = getNodeIdFromCellGameObject(i_CellGameObject);
        MyUnityNode node = (MyUnityNode)m_Graph.GetNodeById(nodeId);
        if (node.IsObstacle)
        {
            IList<Vector3> suroundingCells = m_NeighborsPositionCalculator.GetSuroundingCells((int)node.Position.x, (int)node.Position.y, (int)node.Position.z);

            foreach (Vector3 cellPosition in suroundingCells)
            {
                int id = getIdFromPosition(cellPosition);
                if (!m_Graph.IsObtacle(id))
                {
                    m_Graph.AddEdgeOfNodesById(nodeId, id, m_DefaultWeight);
                    m_Graph.AddEdgeOfNodesById(id, nodeId, m_DefaultWeight);
                }

            }
            node.IsObstacle = false;
        }
    }

    //private bool isObstacle(Renderer i_Renderer)
    //{
    //    return i_Renderer.material.color == m_ObstacleMaterial.color;
    //}

    private int getIdFromPosition(Vector3 pos)
    {
        return (int)(pos.z * k_Columns + pos.x);
    }

    public void RunAstar()
    {
        if (m_IsStartNodePicked && m_IsTargetNodePicked)
        {
            if (!m_IspathDrawn)
            {
                Tuple<IList<Tuple<INode, int>>, IList<INode>> res = m_Pathfinder.AstarSearch(m_Graph, m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_StartNodeGameObject)), m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_TargetNodeGameObject)));
                clearBoard();
                if (res != null && res.Item2 != null)
                {
                    //m_IsCurrentlyDrawing = true;
                    //drawPath(res.Item2);
                    drawDistancesAndPath(res.Item1, res.Item2);
                }
                m_IspathDrawn = true;
            }
        }

    }

    public void RunDijkstra()
    {
        if (m_IsStartNodePicked && m_IsTargetNodePicked)
        {
            if (!m_IspathDrawn)
            {

                //                IList<INode> res = m_Pathfinder.Dijkstra(m_Graph, m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_StartNodeGameObject)), m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_TargetNodeGameObject)));
                //Tuple<IList<Tuple<INode, int>>, IList<INode>>  


                //===========================================do Dikstra With Distances//===========================================
                Tuple<IList<Tuple<INode, int>>, IList<INode>> res = m_Pathfinder.DijkstraWithDistances(m_Graph, m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_StartNodeGameObject)), m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_TargetNodeGameObject)));
                clearBoard();
                if (res != null && res.Item2 != null)
                {
                   //m_IsCurrentlyDrawing = true;
                    //drawPath(res.Item2);
                    drawDistancesAndPath(res.Item1, res.Item2);
                }
             
                m_IspathDrawn = true;
            }
        }

    }

    public void RunBelmanFord()
    {
        if (m_IsStartNodePicked && m_IsTargetNodePicked)
        {
            if (!m_IspathDrawn)
            {

                //                IList<INode> res = m_Pathfinder.Dijkstra(m_Graph, m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_StartNodeGameObject)), m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_TargetNodeGameObject)));
                //Tuple<IList<Tuple<INode, int>>, IList<INode>>  


                //===========================================do Dikstra With Distances//===========================================
                IList<INode> res = m_Pathfinder.BlemanFordSearch(m_Graph, m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_StartNodeGameObject)), m_Graph.GetNodeById(getNodeIdFromCellGameObject(m_TargetNodeGameObject)));
                clearBoard();
                if (res != null)
                {
                    //m_IsCurrentlyDrawing = true;
                    drawPath(res);
                    //drawDistancesAndPath(res.Item1, res.Item2);
                }

                m_IspathDrawn = true;
            }
        }
    }

    public void RunDFS()
    {
        if (m_IsStartNodePicked)
        {
            IList<Tuple<INode, PathFinder.NodeStatus>> visitedOrder = m_Pathfinder.DFS(m_Graph, m_StartingNode);
            clearBoard();
            if (visitedOrder != null)
            {
                drawDfs(visitedOrder);
            }

            m_IspathDrawn = true;
        }
       
    }

    private void drawDfs(IList<Tuple<INode, PathFinder.NodeStatus>> visitedOrder)
    {
        int nodesDrawnCount = 0;

        for (int i = 1; i < visitedOrder.Count; i++)
        {
            PathFinder.NodeStatus status = visitedOrder[i].Item2;
            if(status == PathFinder.NodeStatus.Visited)
            {
                StartCoroutine(colorCellAfterSeconds(((MyUnityNode)visitedOrder[i].Item1).CellPrefab, nodesDrawnCount * m_DrawSpeed, Color.magenta));
            }
            else if(status == PathFinder.NodeStatus.Done)
            {
                StartCoroutine(colorCellAfterSeconds(((MyUnityNode)visitedOrder[i].Item1).CellPrefab, nodesDrawnCount * m_DrawSpeed, Color.red));
            }
            nodesDrawnCount++;
        }
    }

    private void drawDistancesAndPath(IList<Tuple<INode, int>> i_Distances, IList<INode> i_Path)
    {
        if (i_Distances != null && i_Path != null)
        {
            //draw distances
            int nodesDrawnCount = 0;

            float maxDistance = getMaxDistance(i_Distances);
            for (int i = 1; i < i_Distances.Count; i++)
            {
                int distance = i_Distances[i].Item2;
                float colorLevel = distance / maxDistance;
                StartCoroutine(colorCellAfterSeconds(((MyUnityNode)i_Distances[i].Item1).CellPrefab, nodesDrawnCount * m_DrawSpeed, Color.red * colorLevel + Color.yellow * (1f - colorLevel)));
                nodesDrawnCount++;
            }

            //draw final path
            for (int i = 1; i < i_Path.Count - 1; i++)
            {
                int distance = i_Distances[i].Item2;
                StartCoroutine(colorCellAfterSeconds(((MyUnityNode)i_Path[i]).CellPrefab, nodesDrawnCount * m_DrawSpeed, m_PathMaterial.color));
                nodesDrawnCount++;

            }
        }
    }

    private float getMaxDistance(IList<Tuple<INode, int>> i_Distances)
    {
        if (i_Distances == null)
            return 0;
        else
        {
            int max = i_Distances[0].Item2;
            foreach (Tuple<INode, int> nodeAndDistance in i_Distances)
            {
                if (max < nodeAndDistance.Item2)
                    max = nodeAndDistance.Item2;
            }
            return max;
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
                    MyEdge edge = new MyEdge(m_Graph.GetNodeById((int)(z * k_Columns + x)), m_Graph.GetNodeById((int)(neighbor.z * k_Columns + neighbor.x)), 1);
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
                currNode = new MyUnityNode(x + z * k_Columns, 11, newCellPosition, cellPrefab);
                m_Graph.AddNode(currNode);
            }
        }
    }

    IEnumerator colorCellAfterSeconds(GameObject i_Cell, float i_TimeToWait, Color i_Color)
    {
        yield return new WaitForSeconds(i_TimeToWait);
        i_Cell.GetComponent<Renderer>().material.color = i_Color;
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

    private void clearStartNodeIfIsColliding(GameObject i_CellGameObject, Renderer selectionRenderer)
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

    private void clearTargetNodeIfIsColliding(GameObject i_CellGameObject, Renderer selectionRenderer)
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
            int u_Id = neighbor.U.Id;
            int v_Id = neighbor.V.Id;
            m_Graph.UpdateEdgeWeightByIds(u_Id, v_Id, i_Weight);
            m_Graph.UpdateEdgeWeightByIds(v_Id, u_Id, i_Weight);
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

            //if (currCellMaterial.color != m_ObstacleMaterial.color && (node.GetNodeId() != m_StartingNode.GetNodeId()) && (node.GetNodeId() != m_TargetNode.GetNodeId()))
            if (currCellMaterial.color != m_ObstacleMaterial.color && (!node.Equals(m_StartingNode)) && (!node.Equals(m_TargetNode)))
            {
                node.CellPrefab.GetComponent<Renderer>().material = defaultMaterial;
            }

        }
    }

    private void drawPath(IList<INode> res)
    {
        if (res != null)
        {
            int nodesDrawnCount = 0;
            for (int i = 1; i < res.Count - 1; i++)
            {
                StartCoroutine(colorCellAfterSeconds(((MyUnityNode)res[i]).CellPrefab, nodesDrawnCount * m_DrawSpeed, m_PathMaterial.color));
                nodesDrawnCount++;
                //if (i == res.Count - 2)
                //{
                //    m_IsCurrentlyDrawing = false;
                //}
            }
        }
        //else
        //{
        //    m_IsCurrentlyDrawing = false;
        //}
    }

    private int getNodeIdFromCellGameObject(GameObject i_Cell)
    {
        CellPrefabScript cellScript = i_Cell.GetComponent<CellPrefabScript>();
        return (int)(cellScript.Position.x + cellScript.Position.z * k_Columns);
    }

    // DEBUG CODE ====================================================================================================================================
    private void debugDrawBoard()
    {
        m_Graph.RemoveNodeEdgesById(2);
        m_Graph.RemoveNodeEdgesById(7);
        int i = 0;
        bool i_IsBlue = true;
        foreach (MyUnityNode node in m_Graph.GetAllNodes())
        {
            StartCoroutine(doafterWait(node, i * m_DrawSpeed, i_IsBlue));
            StartCoroutine(doafterWait2(node, (i + 1) * m_DrawSpeed, i_IsBlue));
            i = i + 2;
            i_IsBlue = !i_IsBlue;
        }
    }
    IEnumerator doafterWait(MyUnityNode node, float i_Time, bool i_IsBlue)
    {
        yield return new WaitForSeconds(i_Time);
        node.CellPrefab.GetComponent<Renderer>().material.color = Color.yellow;
        foreach (IEdge neighbor in m_Graph.GetNeighbors(node))
        {
            ((MyUnityNode)neighbor.V).CellPrefab.GetComponent<Renderer>().material.color = i_IsBlue ? Color.blue : Color.red;
        }

    }
    IEnumerator doafterWait2(MyUnityNode node, float i_Time, bool i_IsBlue)
    {
        yield return new WaitForSeconds(i_Time);
        node.CellPrefab.GetComponent<Renderer>().material.color = m_DefaultMaterail.color;
        foreach (IEdge neighbor in m_Graph.GetNeighbors(node))
        {
            ((MyUnityNode)neighbor.V).CellPrefab.GetComponent<Renderer>().material.color = m_DefaultMaterail.color;
        }

    }
    // END DEBUG CODE====================================================================================================================================


}
