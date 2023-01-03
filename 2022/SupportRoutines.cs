namespace AOC2022
{
    internal class SupportRoutines
    {
        // shortest path stufff
        static bool[,] visited;

        // Check if it is possible to go to (x, y) from the
        // current position. The function returns false if the
        // cell has value 0 or already visited
        public static bool isSafe(int[,] mat, bool[,] visited, int x,
                           int y, int prevCell)
        {
            return (x >= 0 && x < mat.GetLength(0) && y >= 0
                    && y < mat.GetLength(1) && (Math.Abs(mat[x, y] - prevCell) < 2)// mat[x, y] == 1
                    && !visited[x, y]);
        }
        public static int findShortestPath(int[,] mat, int i, int j,
                                    int x, int y, int min_dist,
                                    int dist)
        {

            if (i == x && j == y)
            {
                min_dist = Math.Min(dist, min_dist);
                return min_dist;
            }

            // set (i, j) cell as visited
            visited[i, j] = true;
            // go to the bottom cell
            if (isSafe(mat, visited, i + 1, j, mat[i, j]))
            {
                min_dist = findShortestPath(mat, i + 1, j, x, y,
                                            min_dist, dist + 1);
            }
            // go to the right cell
            if (isSafe(mat, visited, i, j + 1, mat[i, j]))
            {
                min_dist = findShortestPath(mat, i, j + 1, x, y,
                                            min_dist, dist + 1);
            }
            // go to the top cell
            if (isSafe(mat, visited, i - 1, j, mat[i, j]))
            {
                min_dist = findShortestPath(mat, i - 1, j, x, y,
                                            min_dist, dist + 1);
            }
            // go to the left cell
            if (isSafe(mat, visited, i, j - 1, mat[i, j]))
            {
                min_dist = findShortestPath(mat, i, j - 1, x, y,
                                            min_dist, dist + 1);
            }
            // backtrack: remove (i, j) from the visited matrix
            visited[i, j] = false;
            return min_dist;
        }

        // Wrapper over findShortestPath() function
        public static int findShortestPathLength(int[,] mat,
                                          int[] src, int[] dest)
        {
            if (mat.GetLength(0) == 0
                || mat[src[0], src[1]] == 0
                || mat[dest[0], dest[1]] == 0)
                return -1;

            int row = mat.GetLength(0);
            int col = mat.GetLength(1);

            // construct an `M × N` matrix to keep track of
            // visited cells
            visited = new bool[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                    visited[i, j] = false;
            }

            int dist = Int32.MaxValue;
            dist = findShortestPath(mat, src[0], src[1],
                                    dest[0], dest[1], dist, 0);

            if (dist != Int32.MaxValue)
                return dist;
            return -1;
        }
        // end shortest path stuff

        // count true in bool[,]
        public static int countVisible(bool[,] boolGrid)
        {
            int count = 0;
            for (int i = 0; i < boolGrid.GetLength(0); i++)
            {
                for (int j = 0; j < boolGrid.GetLength(1); j++)
                {
                    count += boolGrid[i, j] ? 1 : 0;
                }
            }
            return count;
        }
        // print grid of integers
        public static void printGridInt(int[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    line += grid[i, j] + ",";
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        //print grid of bools
        public static void printGridBool(bool[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    line += grid[i, j] ? 1 : 0;
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        // Nicks file system class
        public class fso
        {
            private string _name = "";
            private int _size = 0;
            private bool _file = false;
            private fso _parent;
            private IDictionary<string, fso> _members = new Dictionary<string, fso>();
            public Boolean IsFile
            {
                get { return _file; }
            }
            public fso(string name, int filesize, bool file, fso parent)
            {
                _name = name;
                _size = filesize;
                _file = file;
                if (!file)
                    _parent = parent;
            }
            public fso CD(string dirname)
            {
                if (dirname == "..")
                    return _parent;
                else
                    return _members[dirname];
            }
            public void AddFileOrDirectory(string name, int filesize, bool file)
            {
                if (!_members.ContainsKey(name))
                    _members.Add(name, new fso(name, filesize, file, this));
            }
            public int Size()
            {
                if (_file)
                    return _size;
                else
                {
                    int totalsize = 0;
                    foreach (fso member in _members.Values)
                        totalsize += member.Size();
                    return totalsize;
                }
            }
            public int SizeLessThan(int requiredsize)
            {
                int totalsize = 0;
                if (!_file)
                {
                    int thissize = this.Size();
                    if (thissize <= requiredsize)
                        totalsize += thissize;
                    foreach (fso member in _members.Values)
                        if (!member.IsFile)
                            totalsize += member.SizeLessThan(requiredsize);
                }
                return totalsize;
            }
            public void DirectoryToDelete(int SizeNeeded, ref int Smallestsize)
            {
                if (!_file)
                {
                    int thissize = this.Size();
                    if ((thissize >= SizeNeeded) & (thissize < Smallestsize))
                        Smallestsize = thissize;
                    foreach (fso member in _members.Values)
                        if (!member.IsFile)
                            member.DirectoryToDelete(SizeNeeded, ref Smallestsize);
                }
            }
        }
        // dijkstra stuff
        // find vertex with min dist from set of vertices not in shortest path tree
        public static int minDistance(int[] dist,
                        bool[] sptSet, int V)
        {
            // Initialize min value
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < V; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }

        // Dijkstra's single source shortest path algorithm for a graph represented with adjacency matrix
        public static Dictionary<int, int> dijkstra(int[,] graph, int src, int V)
        {
            // array to hold shortest distance from src to i
            int[] dist = new int[V];

            // true if i included in shortest path tree or shortest distance from src to i in finalized
            bool[] sptSet = new bool[V];

            // all distances infinite and stpSet[] as false
            for (int i = 0; i < V; i++)
            {
                dist[i] = int.MaxValue;
                sptSet[i] = false;
            }

            // distance from self always 0
            dist[src] = 0;

            // Find shortest path for all vertices
            for (int count = 0; count < V - 1; count++)
            {
                // choose min dist vertex from set not yet processed, u=0 for first iteration
                int u = minDistance(dist, sptSet, V);

                // flag the picked vertex as processed
                sptSet[u] = true;

                // update dist value of the adjacent vertices of the picked vertex.
                for (int v = 0; v < V; v++)

                    // update dist[v] only if:
                    //     is not in sptSet,
                    //     there is an edge from u to v,
                    //     total weight of path from src to v through u < current dist[v]
                    if (!sptSet[v] && graph[u, v] != 0 &&
                         dist[u] != int.MaxValue && dist[u] + graph[u, v] < dist[v])
                        dist[v] = dist[u] + graph[u, v];
            }

            // return the completed distance dictionary
            Dictionary<int, int> solution = new Dictionary<int, int>();
            for (int i = 0; i < V; i++)
            {
                solution[i] = dist[i];
            }
            return solution;
        }
        public static List<string> LoadFileIntoArray(string FileName)
        {
            List<string> InputData = new List<string>();
            string FilePath = "..//..//..//Data//" + FileName + ".txt";
            foreach (string line in System.IO.File.ReadLines(FilePath))
            {
                InputData.Add(line);
            }
            Console.WriteLine("==== data loaded from '" + FileName + ".txt' ====");
            return InputData;
        }
        // dijkstra stuff end
        public static List<string> LoadDataIntoArray(int DayNumber, bool TestData)
        {
            List<string> InputData = new List<string>();
            string FilePath = "..//..//..//Data//" + (TestData ? "TestData" : "InputData") + DayNumber.ToString() + ".txt";
            foreach (string line in System.IO.File.ReadLines(FilePath))
            {
                InputData.Add(line);
            }
            Console.WriteLine("==== Day " + DayNumber.ToString() + (TestData ? " Test" : "") + " data loaded ====");
            return InputData;
        }
        //swap two values in an array
        static void swap(List<int> arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
        //places pivot with all smaller values left and all larger values right
        static int partition(List<int> arr, int low, int high)
        {
            //pivot element
            int pivot = arr[high];
            //index of smaller element
            int i = (low - 1);
            for (int j = low; j <= high - 1; j++)
            {
                //is current element smaller than the pivot
                if (arr[j] < pivot)
                {
                    //increment index of smaller element
                    i++;
                    swap(arr, i, j);
                }
            }
            swap(arr, i + 1, high);
            return (i + 1);
        }
        //quicksort recursive function
        public static void quickSort(List<int> arr, int low, int high)
        {
            if (low < high)
            {
                //partition index, arr[p] is now in correct position
                int i = partition(arr, low, high);

                //sort elements before and after partition
                quickSort(arr, low, i - 1);
                quickSort(arr, i + 1, high);
            }
        }
    }
}
