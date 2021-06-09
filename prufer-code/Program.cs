using System;
using System.Collections.Generic;
using System.IO;

namespace prufer_code
{
    class Program
    {
        static void Main(string[] args)
        {
            (int, int) sizeG = (0, 0);
            List<(int, int)> edgesList = ReadFileG(ref sizeG);
            List<int> code = CodePrefer(edgesList, sizeG);
            Console.WriteLine("Код Прюфера");
            foreach (var elem in code)
            {
                Console.Write(String.Format("{0,3}", elem));
            }
        }
        
        static List<(int, int)> ReadFileG(ref (int, int) sizeG)
        {
            string path = "test.txt";
            List<(int, int)> edgesList = new List<(int, int)>();
            StreamReader file = new StreamReader(path);
            string[] size = file.ReadLine()?.Split(' ');
            if (size != null)
            {
                sizeG = (Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));
                for (int i = 0; i < sizeG.Item2; ++i)
                {
                    size = file.ReadLine()?.Split(' ');
                    if (size != null) edgesList.Add((Convert.ToInt32(size[0]), Convert.ToInt32(size[1])));
                }
            }
            return edgesList;
        }

        static int[,] AdjMatrix((int, int) sizeG, List<(int, int)> edgesList)
        {
            int[,] matrixA = new int[sizeG.Item1, sizeG.Item1];
            for (int i = 0; i < sizeG.Item1; i++)
                for (int j = 0; j < sizeG.Item1; j++)
                    matrixA[i, j] = 0;
            for (int k = 0; k < sizeG.Item2; k++)
            {
                matrixA[((edgesList[k].Item1) - 1), ((edgesList[k].Item2) - 1)] = 1;
                matrixA[((edgesList[k].Item2) - 1), ((edgesList[k].Item1) - 1)] = 1;
            }
            return matrixA;
        }
        
        static List<int> CodePrefer(List<(int, int)> edgesList, (int, int) sizeG)
        {
            List<int> code = new List<int>();
            for (int i = 0; i < sizeG.Item1 - 2; i++)
            {
                List<int> searchLeaflet = SearchInDepth(sizeG, edgesList);
                int min = MinInList(searchLeaflet);
                for (int j = 0; j < edgesList.Count; j++)
                {
                    if (min == edgesList[j].Item1)
                    {
                        code.Add(edgesList[j].Item2);
                        edgesList.RemoveAt(j);
                        sizeG = (sizeG.Item1, sizeG.Item2 - 1);
                        break;
                    }
                    if (min == edgesList[j].Item2)
                    {
                        code.Add(edgesList[j].Item1);
                        edgesList.RemoveAt(j);
                        sizeG = (sizeG.Item1, sizeG.Item2 - 1);
                        break;
                    }
                }
            }
            return code;
        }
        
        static List<int> SearchInDepth((int, int) sizeG, List<(int, int)> edgesList)
        {
            int start = MaxInDuoList(edgesList);
            int[,] matrix = AdjMatrix(sizeG, edgesList);
            List<int> result = new List<int>();
            List<int> turn = new List<int>();
            List<int> visited = new List<int>();
            int varCheck = 0;
            turn.Add(start);
            visited.Add(start);
            while (turn.Count != 0)
                for (int i = 0; i < sizeG.Item1; i++)
                    if (i + 1 == start)
                        for (int j = 0; j < sizeG.Item1; j++)
                        {
                            if (matrix[i, j] == 1 && SearchInList(visited, (j + 1)))
                            {
                                varCheck = 0;
                                start = j + 1;
                                turn.Add(start);
                                visited.Add(start);
                                break;
                            }
                            if (j == sizeG.Item1 - 1)
                            {
                                if (varCheck == 0)
                                    result.Add(turn[turn.Count - 1]);
                                varCheck++;
                                turn.Remove(turn[turn.Count - 1]);
                                if (turn.Count == 0)
                                    break;
                                start = turn[turn.Count - 1];
                                break;
                            }
                        }
            return result;
        }
        
        static bool SearchInList(List<int> list, int n)
        {
            foreach (var item in list)
            {
                if (item == n)
                {
                    return false;
                }
            }
            return true;
        }
        
        static int MaxInDuoList(List<(int, int)> duoList)
        {
            int max = duoList[0].Item1;
            foreach (var item in duoList)
            {
                if (max < item.Item1)
                    max = item.Item1;
                if (max < item.Item2)
                    max = item.Item2;
            }
            return max;
        }

        static int MinInList(List<int> list)
        {
            int min = list[0];
            for (int i = 0; i < list.Count; i++)
                if (min > list[i])
                    min = list[i];
            return min;
        }
    }
}