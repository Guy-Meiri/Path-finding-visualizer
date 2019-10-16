using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class NeighborsPositionCalculator
    {
        private readonly int k_Rows;
        private readonly int k_Columns;

        public NeighborsPositionCalculator(int i_Rows, int i_Cols)
        {
            k_Rows = i_Rows;
            k_Columns = i_Cols;
        }

        public IList<Vector3> GetSuroundingCells(int i_Column, int i_Depth, int i_Row)
        {
            List<Vector3> suroundingCells = new List<Vector3>();

            //if the cell is not on one of the edges
            if (((i_Row > 0) && (i_Row < k_Rows - 1)) && ((i_Column > 0) && (i_Column < k_Columns - 1)))
            {
                AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
                AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
            }


            //CORNERS!
            //UpperLeft corner
            else if (i_Row == 0 && i_Column == 0)
            {
                suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));

                suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
            }
            //UpperRight corner
            else if (i_Row == 0 && i_Column == k_Columns - 1)
            {
                suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));
                suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
            }

            //ButtomLeft corner
            else if (i_Row == k_Rows - 1 && i_Column == 0)
            {
                suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));
                suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));
            }

            //ButtomRight corner
            else if (i_Row == k_Rows - 1 && i_Column == k_Columns - 1)
            {
                suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));
                suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));
            }


            //Edge but not a corner
            else if (i_Row == 0)
            {

                suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
                AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
            }
            else if (i_Row == k_Rows - 1)
            {
                suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));
                AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
            }
            else if (i_Column == 0)
            {
                suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));
                AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
            }
            else if (i_Column == k_Columns - 1)
            {
                suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));
                AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
            }

            return suroundingCells;
        }

        private static void addLeftColumn(int x, int z, List<Vector3> suroundingCells)
        {
            suroundingCells.Add(new Vector3(x - 1, 0, z - 1));

            suroundingCells.Add(new Vector3(x - 1, 0, z));
            suroundingCells.Add(new Vector3(x - 1, 0, z + 1));
        }

        private static void addRightColumn(int x, int z, List<Vector3> suroundingCells)
        {
            suroundingCells.Add(new Vector3(x + 1, 0, z - 1));

            suroundingCells.Add(new Vector3(x + 1, 0, z));
            suroundingCells.Add(new Vector3(x + 1, 0, z + 1));
        }

        private static void AddRowBelowPositions(int x, int z, List<Vector3> suroundingCells)
        {
            suroundingCells.Add(new Vector3(x - 1, 0, z + 1));
            suroundingCells.Add(new Vector3(x, 0, z + 1));
            suroundingCells.Add(new Vector3(x + 1, 0, z + 1));
        }

        private static void AddLeftAndRightPositions(int x, int z, List<Vector3> suroundingCells)
        {
            suroundingCells.Add(new Vector3(x - 1, 0, z));
            suroundingCells.Add(new Vector3(x + 1, 0, z));
        }

        private static void AddAboveAndBelowPositions(int x, int z, List<Vector3> suroundingCells)
        {
            suroundingCells.Add(new Vector3(x, 0, z + 1));
            suroundingCells.Add(new Vector3(x, 0, z - 1));
        }


        private static void AddRowAbovePositions(int x, int z, List<Vector3> suroundingCells)
        {
            suroundingCells.Add(new Vector3(x - 1, 0, z - 1));
            suroundingCells.Add(new Vector3(x, 0, z - 1));
            suroundingCells.Add(new Vector3(x + 1, 0, z - 1));
        }


        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------

        public IList<Vector3> getSuroundingCellsIncludingDiagonals(int i_Column, int i_Depth, int i_Row)
        {
            List<Vector3> suroundingCells = new List<Vector3>();

            //if the cell is not on one of the edges
            if (((i_Row > 0) && (i_Row < k_Rows - 1)) && ((i_Column > 0) && (i_Column < k_Columns - 1)))
            {
                AddRowAbovePositions(i_Column, i_Row, suroundingCells);
                AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
                AddRowBelowPositions(i_Column, i_Row, suroundingCells);
            }


            //CORNERS!
            //UpperLeft corner
            else if (i_Row == 0 && i_Column == 0)
            {
                suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));

                suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
                suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row + 1));

            }
            //UpperRight corner
            else if (i_Row == 0 && i_Column == k_Columns - 1)
            {
                suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));

                suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row + 1));
                suroundingCells.Add(new Vector3(i_Column, 0, i_Row + 1));
            }

            //ButtomLeft corner
            else if (i_Row == k_Rows - 1 && i_Column == 0)
            {
                suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));
                suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row - 1));

                suroundingCells.Add(new Vector3(i_Column + 1, 0, i_Row));
            }

            //ButtomRight corner
            else if (i_Row == k_Rows - 1 && i_Column == k_Columns - 1)
            {
                suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row - 1));
                suroundingCells.Add(new Vector3(i_Column, 0, i_Row - 1));

                suroundingCells.Add(new Vector3(i_Column - 1, 0, i_Row));
            }


            //Edge but not a corner
            else if (i_Row == 0)
            {
                AddRowBelowPositions(i_Column, i_Row, suroundingCells);
                AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
            }
            else if (i_Row == k_Rows - 1)
            {
                AddRowAbovePositions(i_Column, i_Row, suroundingCells);
                AddLeftAndRightPositions(i_Column, i_Row, suroundingCells);
            }
            else if (i_Column == 0)
            {
                addRightColumn(i_Column, i_Row, suroundingCells);
                AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
            }
            else if (i_Column == k_Columns - 1)
            {
                addLeftColumn(i_Column, i_Row, suroundingCells);
                AddAboveAndBelowPositions(i_Column, i_Row, suroundingCells);
            }

            return suroundingCells;
        }
    }




}
