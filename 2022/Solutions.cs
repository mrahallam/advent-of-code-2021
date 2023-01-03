namespace AOC2022;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

internal class Solutions
{
    public static void Day16Solution(ref List<string> Data)
    {
        Dictionary<string, int> flowRates = new();
        Dictionary<string, List<string>> graph = new();
        HashSet<string> visited = new();
        Queue<string> queue = new();
        int limit = 30;
        int timer = 0;
        string start = "AA";
        string current = start;
        for (int i = 0; i < Data.Count; i++)
        {
            string[] split = Data[i].Split(new Char[] { ' ', '=', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            //Console.WriteLine(string.Join("; ", split));
            flowRates.Add(split[1], int.Parse(split[5]));

            graph.Add(split[1], new List<string> { split[10] });
            for (int j = 11; j < split.Length; j++)
            {
                graph[split[1]].Add(split[j]);
            }
        }
        foreach (var item in flowRates)
        {
            Console.WriteLine(item.Key + " ==> " + item.Value);
        }
        foreach (var item in graph)
        {
            Console.WriteLine(item.Key + " ==> " + string.Join("; ", item.Value));
        }

        void bfs(Dictionary<string, List<string>> graph, string start)
        {
            visited.Add(start);
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                Console.WriteLine(vertex + " ");

                foreach (var neighbour in graph[vertex])
                {
                    Console.WriteLine("Neighbour is " + neighbour);
                    Console.WriteLine(string.Join("; ", visited));
                    if (!visited.Contains(neighbour))
                    {
                        Console.WriteLine("Adding neighbour " + neighbour);
                        visited.Add(neighbour);
                        queue.Enqueue(neighbour);
                        Console.WriteLine(string.Join(", ", queue));
                    }
                }
            }
        }
        bfs(graph, start);
    }
    public static void Day15Solution(ref List<string> Data)
    {
        Point sensor = new();
        Point beacon = new();
        int testRow = 2000000;
        long upperBound = 4000000;
        HashSet<Point> noBeacon = new();
        HashSet<Point> outOfRange = new();
        Dictionary<Point, int> sensors = new();
        // Populate heights
        for (int i = 0; i < Data.Count; i++)
        {
            var parts = Data[i].Split(":");
            var senLoc = parts[0].Split("Sensor at ")[1].Split(", ");
            var beaLoc = parts[1].Split(" closest beacon is at ")[1].Split(",").ToList();
            sensor.X = int.Parse(senLoc[0].Split("x=")[1]);
            sensor.Y = int.Parse(senLoc[1].Split("y=")[1]);
            beacon.X = int.Parse(beaLoc[0].Split("x=")[1]);
            beacon.Y = int.Parse(beaLoc[1].Split("y=")[1]);

            int manhattenDistance = Math.Abs(beacon.X - sensor.X) + Math.Abs(beacon.Y - sensor.Y);
            //Console.WriteLine(sensor.ToString() + ", " + beacon.ToString()
            //    + "manhatten = " + (manhattenDistance)
            //    + "; in range? " + (sensor.Y + manhattenDistance > testRow && sensor.Y - manhattenDistance < testRow));
            if (sensor.Y + manhattenDistance > testRow && sensor.Y - manhattenDistance < testRow)
            {

                for (int j = sensor.X - manhattenDistance; j < sensor.X + manhattenDistance; j++)
                {
                    if (Math.Abs(j - sensor.X) + Math.Abs(testRow - sensor.Y) <= manhattenDistance)
                    {
                        noBeacon.Add(new Point(j, testRow));
                    }
                }
            }
            var startPoint = new Point(sensor.X + manhattenDistance + 1, sensor.Y);
            for (int k = 0; k <= manhattenDistance + 1; k++)
            {
                if (startPoint.X > 0
                    && startPoint.Y > 0
                    && startPoint.X <= upperBound
                    && startPoint.Y <= upperBound)
                {
                    outOfRange.Add(startPoint);
                    startPoint.Offset(-1, -1);
                };
            }
            startPoint = new Point(sensor.X + manhattenDistance + 1, sensor.Y);
            for (int k = 0; k <= manhattenDistance + 1; k++)
            {
                if (startPoint.X > 0
                    && startPoint.Y > 0
                    && startPoint.X <= upperBound
                    && startPoint.Y <= upperBound)
                {
                    outOfRange.Add(startPoint);
                    startPoint.Offset(-1, 1);
                }
            }
            startPoint = new Point(sensor.X - manhattenDistance - 1, sensor.Y);
            for (int k = 0; k <= manhattenDistance; k++)
            {
                if (startPoint.X > 0
                    && startPoint.Y > 0
                    && startPoint.X <= upperBound
                    && startPoint.Y <= upperBound)
                {
                    outOfRange.Add(startPoint);
                    startPoint.Offset(1, 1);
                }
            }
            startPoint = new Point(sensor.X - manhattenDistance - 1, sensor.Y);
            for (int k = 0; k <= manhattenDistance + 1; k++)
            {
                if (startPoint.X > 0
                    && startPoint.Y > 0
                    && startPoint.X <= upperBound
                    && startPoint.Y <= upperBound)
                {
                    outOfRange.Add(startPoint);
                    startPoint.Offset(1, -1);
                }
            }
            sensors.Add(sensor, manhattenDistance);
        }
        Console.WriteLine("Part a solution: " + (noBeacon.Count - 1));
        Console.WriteLine("===============================");
        bool checkInRange(Point p1)
        {
            foreach (var p2 in sensors)
            {
                if (Math.Abs(p1.X - p2.Key.X) + Math.Abs(p1.Y - p2.Key.Y) > p2.Value
                    && !sensors.ContainsKey(p1))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        foreach (var p1 in outOfRange)
        {
            if (checkInRange(p1))
            {
                Console.WriteLine("Part b solution: " + ((4000000 * Convert.ToInt64(p1.X)) + Convert.ToInt64(p1.Y)));
                Console.WriteLine("===============================");
            }
        }
    }
    public static void Day14Solution(char Part, ref List<string> Data)
    {
        Point entryPoint = new(500, 0);
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        Dictionary<Point, bool> pointsDict = new();
        //List<Point> pointsArray = new List<Point>();
        Point[] directions = { new Point(0, 1), new Point(-1, 1), new Point(1, 1) }; // down, down-left, down-right
                                                                                     // Populate heights
        for (int i = 0; i < Data.Count; i++)
        {
            string[] split = Data[i].Split(new Char[] { ',', '-', '>', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < split.Count() - 2; j += 2)
            {
                var p1 = new Point(int.Parse(split[j]), int.Parse(split[j + 1]));
                var p2 = new Point(int.Parse(split[j + 2]), int.Parse(split[j + 3]));

                fillBetween(ref pointsDict, p1, p2);
                minX = minX < p1.X ? minX : p1.X;
                maxX = maxX > p1.X ? maxX : p1.X;
                maxY = maxY > p1.Y ? maxY : p1.Y;
            }
        }
        int floor = maxY + 1;

        void fillBetween(ref Dictionary<Point, bool> pointsDict, Point p1, Point p2)
        {
            if (p1.X - p2.X == 0)
            {
                // if p1 bigger, swap
                if (p1.Y > p2.Y)
                {
                    swapPoint(ref p1, ref p2);
                }
                for (int k = p1.Y; k <= p2.Y; k++)
                {
                    if (!pointsDict.ContainsKey(new Point(p1.X, k)))
                    {
                        pointsDict.Add(new Point(p1.X, k), true);
                    }
                }
            }
            else if (p1.Y - p2.Y == 0)
            {
                // if p1 bigger, swap
                if (p1.X > p2.X)
                {
                    swapPoint(ref p1, ref p2);
                }
                for (int k = p1.X; k <= p2.X; k++)
                {
                    if (!pointsDict.ContainsKey(new Point(k, p1.Y)))
                    {
                        pointsDict.Add(new Point(k, p1.Y), true);
                    }
                }
            }
        }
        void swapPoint(ref Point p1, ref Point p2)
        {
            Point swapPoint = p1;
            p1 = p2;
            p2 = swapPoint;
        }
        void drawGrid(Dictionary<Point, bool> pointsDict)
        {
            for (int i = 0; i < maxY + 3; i++)
            {
                string line = "";
                for (int j = minX; j <= maxX; j++)
                {
                    if (pointsDict.ContainsKey(new Point(j, i)))
                    {
                        line += "#";
                    }
                    else
                    {
                        line += " ";
                    }
                }
                Console.WriteLine(line);
            }
        }
        // drop the sand
        bool dropSand(ref Dictionary<Point, bool> pointsDict, Point entryPoint)
        {
            var sand = entryPoint;
            while (true)
            {
                if (Part == 'b' && sand.Y == floor)
                {
                    pointsDict.Add(sand, true);
                    return true;
                }
                else if (pointsDict.ContainsKey(new Point(sand.X, sand.Y))
                    || (Part == 'a' && sand.Y + 1 > maxY)
                    || (Part == 'b' && sand.Y + 1 > floor))
                {
                    return false;
                }
                else if (!pointsDict.ContainsKey(new Point(sand.X, sand.Y + directions[0].Y)))
                {
                    sand.Offset(directions[0]);
                }
                else if (!pointsDict.ContainsKey(new Point(sand.X + directions[1].X, sand.Y + directions[1].Y)))
                {
                    sand.Offset(directions[1]);
                }
                else if (!pointsDict.ContainsKey(new Point(sand.X + directions[2].X, sand.Y + directions[2].Y)))
                {
                    sand.Offset(directions[2]);
                }
                else
                {
                    pointsDict.Add(sand, true);
                    //Update these for drawing the full width
                    minX = minX < sand.X ? minX : sand.X;
                    maxX = maxX > sand.X ? maxX : sand.X;
                    return true;
                }
            }
        }
        //before 
        drawGrid(pointsDict);
        //drop the sand
        int counter = 0;
        while (dropSand(ref pointsDict, entryPoint))
        {
            counter++;
        }
        //after
        //drawGrid(pointsDict);
        Console.WriteLine("Part " + Part + " solution: " + counter);
        Console.WriteLine("===============================");
    }

    // ripped off this!
    // https://github.com/oddrationale/AdventOfCode2022CSharp/blob/main/Day13.ipynb
    public static void Day13Solution()
    {
        var input = File.ReadAllText("..//..//..//Data//InputData13.txt")
            .Split("\n\n")
            .Select(pair => pair.Split("\n"))
            .Select(pair => (Left: JsonDocument.Parse(pair[0]).RootElement, Right: JsonDocument.Parse(pair[1]).RootElement));

        int comparePackets(JsonElement left, JsonElement right)
        {
            if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
            {
                return left.GetInt32() - right.GetInt32();
            }
            else if (left.ValueKind == JsonValueKind.Number)
            {
                return comparePackets(JsonDocument.Parse($"[{left.GetInt32()}]").RootElement, right);
            }
            else if (right.ValueKind == JsonValueKind.Number)
            {
                return comparePackets(left, JsonDocument.Parse($"[{right.GetInt32()}]").RootElement);
            }
            else
            {
                foreach (var (nextLeft, nextRight) in Enumerable.Zip(left.EnumerateArray(), right.EnumerateArray()))
                {
                    var current = comparePackets(nextLeft, nextRight);
                    if (current == 0)
                    {
                        continue;
                    }
                    else
                    {
                        return current;
                    }
                }

                return left.GetArrayLength() - right.GetArrayLength();
            }
        }

        var Part1 = input
            .Select((pair, i) => (Order: comparePackets(pair.Left, pair.Right), Index: i + 1))
            .Where(t => t.Order < 0)
            .Select(t => t.Index)
            .Sum();
        Console.WriteLine("Part a solution: " + Part1);
        Console.WriteLine("===============================");

        var packets = input
    .SelectMany(pair => new JsonElement[] { pair.Left, pair.Right })
    .Append(JsonDocument.Parse("[[2]]").RootElement)
    .Append(JsonDocument.Parse("[[6]]").RootElement)
    .OrderBy(packet => packet, Comparer<JsonElement>.Create((l, r) => comparePackets(l, r)));

        var divider1 = packets
            .Select((packet, index) => (Packet: packet, Index: index + 1))
            .First(item => comparePackets(item.Packet, JsonDocument.Parse("[[2]]").RootElement) == 0).Index;

        var divider2 = packets
            .Select((packet, index) => (Packet: packet, Index: index + 1))
            .First(item => comparePackets(item.Packet, JsonDocument.Parse("[[6]]").RootElement) == 0).Index;

        var Part2 = divider1 * divider2;

        Console.WriteLine("Part b solution: " + Part2);
        Console.WriteLine("===============================");

    }
    public static void Day12Solution(char Part, ref List<string> Data)
    {
        //Input data into a Grid
        int maxHeight = Data.Count;
        int maxWidth = Data[0].Length;
        int[,] heights = new int[maxHeight, maxWidth];
        Dictionary<char, Point> Directions = new Dictionary<char, Point> {
            {'u',new Point(0,-1) },{'d',new Point(0,1) },{'l', new Point(-1,0) },{'r', new Point(1,0)} };
        Dictionary<Point, List<Tuple<Point, int>>> Nodes = new Dictionary<Point, List<Tuple<Point, int>>>();
        Point Start = new Point(0, 0);
        Point End = new Point(0, 0);
        // Populate heights
        for (int i = 0; i < maxHeight; i++)
        {
            for (int j = 0; j < maxWidth; j++)
            {
                if (Data[i][j] == 'S')
                {
                    Start.X = j;
                    Start.Y = i;
                    heights[i, j] = Convert.ToInt32('a') - 96;
                }
                else if (Data[i][j] == 'E')
                {
                    End.X = j;
                    End.Y = i;
                    heights[i, j] = Convert.ToInt32('z') - 96;
                }
                else
                {
                    heights[i, j] = Convert.ToInt32(Data[i][j]) - 96;
                }
            }
        }
        // populate nodes graph
        for (int i = 0; i < maxHeight; i++)
        {
            for (int j = 0; j < maxWidth; j++)
            {
                Point CurrentNode = new Point(j, i);
                foreach (Point d in Directions.Values)
                {
                    Point NextNode = new Point(d.X + j, d.Y + i);
                    if ((NextNode.X < maxWidth && NextNode.X >= 0 && NextNode.Y < maxHeight && NextNode.Y >= 0)
                        && ((Part == 'a' && heights[NextNode.Y, NextNode.X] - heights[i, j] <= 1)
                            || (Part == 'b' && heights[i, j] - heights[NextNode.Y, NextNode.X] <= 1)))
                    {
                        if (Nodes.ContainsKey(CurrentNode))
                        {
                            Nodes[CurrentNode].Add(new Tuple<Point, int>(NextNode, 1));
                        }
                        else
                        {
                            Nodes.Add(CurrentNode, new List<Tuple<Point, int>> { new Tuple<Point, int>(NextNode, 1) });
                        }
                    }
                }
            }
        }
        Dictionary<Point, bool> Visited = new Dictionary<Point, bool>();
        Dictionary<Point, int> Distance = new Dictionary<Point, int>();
        Point Current = Start;
        if (Part == 'b') Current = End;
        int ThisDistance = 0;
        bool FoundEnd = false;
        while (!FoundEnd)
        {
            Visited.Add(Current, true);
            if (Nodes.ContainsKey(Current))
            {
                foreach (var NextPoint in Nodes[Current])
                {
                    if ((Part == 'a' && NextPoint.Item1 == End) | (Part == 'b' && heights[NextPoint.Item1.Y, NextPoint.Item1.X] == 1))
                    {
                        Distance.Add(NextPoint.Item1, ThisDistance + NextPoint.Item2);
                        FoundEnd = true;
                        Current = NextPoint.Item1;
                        break;
                    }
                    if (!Visited.ContainsKey(NextPoint.Item1))
                        if (Distance.ContainsKey(NextPoint.Item1))
                        {
                            if ((ThisDistance + 1) < Distance[NextPoint.Item1])
                                Distance[NextPoint.Item1] = ThisDistance + NextPoint.Item2;
                        }
                        else
                        {
                            Distance.Add(NextPoint.Item1, ThisDistance + NextPoint.Item2);
                        }
                }
            }
            if (!FoundEnd)
            {
                int smallest = int.MaxValue;
                Point smallestPoint = new Point(-1, -1);
                foreach (Point dis in Distance.Keys)
                {
                    if (Distance[dis] < smallest && !Visited.ContainsKey(dis))
                    {
                        smallest = Distance[dis];
                        smallestPoint = dis;
                    }
                }
                Current = smallestPoint;
                ThisDistance = Distance[Current];
            }
        }
        Console.WriteLine("Part " + Part.ToString() + " solution: " + (Distance[Current]));
        Console.WriteLine("===============================");
    }
    public static void Dijkstra(ref List<string> Data)
    {
        int y = Data.Count;
        int x = Data[0].Split(",").Length;
        int[,] graph = new int[y, x];
        for (int i = 0; i < Data.Count; i++)
        {
            string[] row = Data[i].Split(",");
            for (int j = 0; j < x; j++)
            {
                graph[j, i] = Convert.ToInt32(row.ElementAt(j));
            }
        }
        foreach (var item in SupportRoutines.dijkstra(graph, 0, x))
        {
            Console.WriteLine(item.Key + " ==> " + item.Value);
        }
    }
    public static void Day09aSolution(ref List<string> Data)
    {
        Point head = new Point(0, 0);
        Point tail = head;
        Dictionary<string, Point> Directions = new Dictionary<string, Point>();
        Dictionary<string, Point> Diagonals = new Dictionary<string, Point>();
        Dictionary<Point, int> visitPointCount = new Dictionary<Point, int>();
        visitPointCount.Add(tail, 1);
        //populate directions
        Directions["L"] = new Point(-1, 0);
        Directions["R"] = new Point(1, 0);
        Directions["D"] = new Point(0, 1);
        Directions["U"] = new Point(0, -1);
        //populate diagonal directions
        Diagonals["LU"] = new Point(-1, -1);
        Diagonals["RU"] = new Point(1, -1);
        Diagonals["LD"] = new Point(-1, 1);
        Diagonals["RD"] = new Point(1, 1);

        bool tooFar(Point head, Point tail)
        {
            if (Math.Abs(head.X - tail.X) > 1
                    || Math.Abs(head.Y - tail.Y) > 1)
            {
                //Console.WriteLine("too far!" + head.ToString() + tail.ToString());
                return true;
            }
            else
            {
                //Console.WriteLine("not too far!" + head.ToString() + tail.ToString());
                return false;
            }
        }
        //Console.WriteLine("Head = " + head.ToString() + "; Tail = " + tail.ToString());

        for (int i = 0; i < Data.Count; i++)
        {
            string dir = Data[i].Split(" ")[0];
            int dist = int.Parse(Data[i].Split(" ")[1]);
            for (int j = 0; j < dist; j++)
            {
                head.X += Directions[dir].X;
                head.Y += Directions[dir].Y;
                //Console.WriteLine("Head = " + head.ToString());
                if (tooFar(head, tail))
                {
                    bool updatedTail = false;
                    foreach (var d in Directions)
                    {
                        Point newTail = new Point(tail.X + d.Value.X, tail.Y + d.Value.Y);
                        if (!tooFar(head, newTail)

                            && (
                                head.X - newTail.X == 0
                                || head.Y - newTail.Y == 0))
                        {
                            tail.X += d.Value.X;
                            tail.Y += d.Value.Y;
                            //Console.WriteLine(tail.ToString());
                            //Console.WriteLine("Setting updated to true and breaking from loop");
                            updatedTail = true;
                            break;
                        }
                    }
                    if (!updatedTail)
                    {
                        //Console.WriteLine("entering diagonals");
                        foreach (var d in Diagonals)
                        {
                            Point newTail = new Point(tail.X + d.Value.X, tail.Y + d.Value.Y);
                            if (!tooFar(head, newTail))
                            {
                                tail.X += d.Value.X;
                                tail.Y += d.Value.Y;
                                //Console.WriteLine(tail.ToString());
                                updatedTail = true;
                                break;
                            }
                        }
                    }
                    if (updatedTail)
                    {
                        if (visitPointCount.ContainsKey(tail))
                        {
                            visitPointCount[tail] += 1;
                        }
                        else
                        {
                            visitPointCount.Add(tail, 1);
                        }
                    }
                }
                //Console.WriteLine("Tail = " + tail.ToString());
            }
        }
        Console.WriteLine("Part a solution: " + visitPointCount.Count);
        Console.WriteLine("===============================");
    }
    public static void Day09bSolution(ref List<string> Data)
    {
        Point head = new Point(0, 0);
        Point[] tail = new Point[10];
        for (int i = 0; i < tail.Length; i++)
        {
            // initialise all tail to origin
            tail[i] = head;
        }
        Dictionary<string, Point> Directions = new Dictionary<string, Point>();
        Dictionary<string, Point> Diagonals = new Dictionary<string, Point>();
        Dictionary<Point, int> visitPointCount = new Dictionary<Point, int>();
        visitPointCount.Add(tail[tail.Length - 1], 1);
        //populate directions
        Directions["L"] = new Point(-1, 0);
        Directions["R"] = new Point(1, 0);
        Directions["D"] = new Point(0, 1);
        Directions["U"] = new Point(0, -1);
        //populate diagonal directions
        Diagonals["LU"] = new Point(-1, -1);
        Diagonals["RU"] = new Point(1, -1);
        Diagonals["LD"] = new Point(-1, 1);
        Diagonals["RD"] = new Point(1, 1);

        // test to see if the points are too far apart
        bool tooFar(Point p1, Point p2)
        {
            if (Math.Abs(p1.X - p2.X) > 1
                    || Math.Abs(p1.Y - p2.Y) > 1)
            {
                //Console.WriteLine("too far!" + head.ToString() + tail.ToString());
                return true;
            }
            else
            {
                //Console.WriteLine("not too far!" + head.ToString() + tail.ToString());
                return false;
            }
        }
        //Console.WriteLine("Head = " + head.ToString() + "; Tail = " + tail.ToString());

        for (int i = 0; i < Data.Count; i++)
        {
            //Console.WriteLine(Data[i]);
            string dir = Data[i].Split(" ")[0]; // direction
            int dist = int.Parse(Data[i].Split(" ")[1]); //distance travelled in that direction
            for (int j = 0; j < dist; j++)
            {
                // no need for head, just a tail+1
                tail[0].X += Directions[dir].X;
                tail[0].Y += Directions[dir].Y;
                // array of updated state of each tail item
                bool[] updatedTail = new bool[10];
                // loop through each tail item
                for (int k = 1; k < tail.Length; k++)
                {
                    if (tooFar(tail[k - 1], tail[k]))
                    {
                        // if up/down/left/right possible, do that
                        foreach (var d in Directions)
                        {
                            Point newTail = new Point(tail[k].X + d.Value.X, tail[k].Y + d.Value.Y);
                            if (!tooFar(tail[k - 1], newTail)

                                && (
                                    tail[k - 1].X - newTail.X == 0
                                    || tail[k - 1].Y - newTail.Y == 0))
                            {
                                tail[k].X += d.Value.X;
                                tail[k].Y += d.Value.Y;
                                updatedTail[k] = true;
                                break;
                            }
                        }
                        // else, do diagonal direction
                        if (!updatedTail[k])
                        {
                            //Console.WriteLine("entering diagonals");
                            foreach (var d in Diagonals)
                            {
                                Point newTail = new Point(tail[k].X + d.Value.X, tail[k].Y + d.Value.Y);
                                if (!tooFar(tail[k - 1], newTail))
                                {
                                    tail[k].X += d.Value.X;
                                    tail[k].Y += d.Value.Y;
                                    //Console.WriteLine(tail.ToString());
                                    updatedTail[k] = true;
                                    break;
                                }
                            }
                        }
                        // if that tail item has been updated, and it's the last item in the tail, update the visited grid
                        if (updatedTail[k] && k == tail.Length - 1)
                        {
                            Console.WriteLine(tail[k].ToString());
                            if (visitPointCount.ContainsKey(tail[k]))
                            {
                                visitPointCount[tail[k]] += 1;
                            }
                            else
                            {
                                visitPointCount.Add(tail[k], 1);
                            }
                        }
                    }

                }
            }
        }
        Console.WriteLine("Part b solution: " + visitPointCount.Count);
        Console.WriteLine("===============================");
    }
    public static void Day09aSolutionBounded(ref List<string> Data)
    {
        int maxWidth = 6;
        int maxHeight = 5;
        Point head = new Point(0, maxHeight - 1);
        Point tail = head;
        Dictionary<string, Tuple<string, Point>> Directions = new Dictionary<string, Tuple<string, Point>>();
        Dictionary<string, Tuple<string, Point>> Diagonals = new Dictionary<string, Tuple<string, Point>>();
        //populate directions
        Directions["L"] = new Tuple<string, Point>("R", new Point(-1, 0));
        Directions["R"] = new Tuple<string, Point>("L", new Point(1, 0));
        Directions["D"] = new Tuple<string, Point>("U", new Point(0, 1));
        Directions["U"] = new Tuple<string, Point>("D", new Point(0, -1));
        //populate diagonal directions
        Diagonals["LU"] = new Tuple<string, Point>("R", new Point(-1, -1));
        Diagonals["RU"] = new Tuple<string, Point>("L", new Point(1, -1));
        Diagonals["LD"] = new Tuple<string, Point>("U", new Point(-1, 1));
        Diagonals["RD"] = new Tuple<string, Point>("D", new Point(1, 1));

        // test if head has gone too far
        bool tooFar(Point head, Point tail)
        {
            if (Math.Abs(head.X - tail.X) > 1
                    || Math.Abs(head.Y - tail.Y) > 1)
            {
                //Console.WriteLine("too far!" + head.ToString() + tail.ToString());
                return true;
            }
            else
            {
                //Console.WriteLine("not too far!" + head.ToString() + tail.ToString());
                return false;
            }
        }
        // create grid
        int[,] grid = new int[maxHeight, maxWidth];
        // create visited grid
        bool[,] visitedGrid = new bool[maxHeight, maxWidth];

        // position head
        grid[head.Y, head.X] = 1;
        //SupportRoutines.printGridInt(grid);
        for (int i = 0; i < Data.Count; i++)
        {
            if (tail != head)
            {
                grid[tail.Y, tail.X] = 2;
            }
            string dir = Data[i].Split(" ")[0];
            int dist = int.Parse(Data[i].Split(" ")[1]);
            //Console.WriteLine(dir + " " + dist);
            for (int j = 0; j < dist; j++)
            {
                // move head
                grid[head.Y, head.X] = 0;
                var nextPos = head;
                nextPos.X += Directions[dir].Item2.X;
                nextPos.Y += Directions[dir].Item2.Y;
                if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.X == maxWidth || nextPos.Y == maxHeight)
                {
                    dir = Directions[dir].Item1;
                    head.X += Directions[dir].Item2.X;
                    head.Y += Directions[dir].Item2.Y;
                }
                else
                {
                    head.X += Directions[dir].Item2.X;
                    head.Y += Directions[dir].Item2.Y;
                }
                //Console.WriteLine(head.ToString());
                grid[head.Y, head.X] = 1;
                SupportRoutines.printGridInt(grid);
                // move tail
                if (tail != head)
                {
                    grid[tail.Y, tail.X] = 0;
                }

                if (tooFar(head, tail))
                {
                    bool updatedTail = false;
                    foreach (var d in Directions)
                    {
                        Point newTail = new Point(tail.X + d.Value.Item2.X, tail.Y + d.Value.Item2.Y);
                        if (!tooFar(head, newTail)
                            && newTail.X >= 0
                            && newTail.Y >= 0
                            && (
                                head.X - newTail.X == 0
                                || head.Y - newTail.Y == 0))
                        {
                            tail.X += d.Value.Item2.X;
                            tail.Y += d.Value.Item2.Y;
                            //Console.WriteLine(tail.ToString());
                            //Console.WriteLine("Setting updated to true and breaking from loop");
                            updatedTail = true;
                            break;
                        }
                    }
                    if (!updatedTail)
                    {
                        //Console.WriteLine("entering diagonals");
                        foreach (var d in Diagonals)
                        {
                            Point newTail = new Point(tail.X + d.Value.Item2.X, tail.Y + d.Value.Item2.Y);
                            if (!tooFar(head, newTail) && newTail.X >= 0 && newTail.Y >= 0)
                            {
                                tail.X += d.Value.Item2.X;
                                tail.Y += d.Value.Item2.Y;
                                //Console.WriteLine(tail.ToString());
                                updatedTail = true;
                                break;
                            }
                        }
                    }
                    if (updatedTail && !visitedGrid[tail.Y, tail.X])
                    {
                        visitedGrid[tail.Y, tail.X] = true;
                    }
                }
                if (tail != head)
                {
                    grid[tail.Y, tail.X] = 2;
                }
                SupportRoutines.printGridInt(grid);
            }
        }
        SupportRoutines.printGridBool(visitedGrid);
        Console.WriteLine("Part a solution: " + (SupportRoutines.countVisible(visitedGrid) + 1));
        Console.WriteLine("===============================");
    }
    public static void Day01aSolution(ref List<string> Data)
    {
        int largest = 0;
        int subTotal = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            //end of elf
            if (Data[i] == "")
            {
                //set largest to subTotal, if subTotal is larger
                if (subTotal > largest)
                {
                    largest = subTotal;
                }
                subTotal = 0;

            }
            else
            {
                subTotal += Convert.ToInt32(Data[i]);
                //is new sub-total larger than largest, and end of file reached
                if (subTotal > largest && i == Data.Count - 1)
                {
                    largest = subTotal;
                    subTotal = 0;
                }
            }
        }
        Console.WriteLine("Part a solution: " + largest);
        Console.WriteLine("===============================");
    }
    public static void Day01bSolution(ref List<string> Data)
    {
        int subTotal = 0;
        int topThreeSum = 0;
        List<int> totals = new List<int>();
        for (int i = 0; i < Data.Count; i++)
        {
            //end of elf
            if (Data[i] == "")
            {
                //add new subtotal to list of totals
                totals.Add(subTotal);
                subTotal = 0;

            }
            else
            {
                subTotal += Convert.ToInt32(Data[i]);
                //did we find the end of the file
                if (i == Data.Count - 1)
                {
                    totals.Add(subTotal);
                }
            }
        }
        //No linq
        //Using .Sort()
        //totals.Sort();
        //Using quickSort implementation
        SupportRoutines.quickSort(totals, 0, totals.Count - 1);
        int n = 3; //get top n
        for (int i = totals.Count - 1; i > totals.Count - (n + 1); i--)
        {
            topThreeSum += totals[i];
        }
        //using Linq
        //var topThree = totals.OrderByDescending(i => i).Take(3).ToList();
        //foreach (int total in topThree)
        //{
        //    topThreeSum += total;
        //}
        Console.WriteLine("Part b solution: " + topThreeSum);
        Console.WriteLine("===============================");
    }
    public static void Day02aSolution(ref List<string> Data)
    {
        //define the mapping of hand to actual hand, points and win condition
        Dictionary<string, Tuple<string, int, string>> map = new Dictionary<string, Tuple<string, int, string>>();
        map["X"] = new Tuple<string, int, string>("A", 1, "C");//rock
        map["Y"] = new Tuple<string, int, string>("B", 2, "A");//paper
        map["Z"] = new Tuple<string, int, string>("C", 3, "B");//scissors

        int score = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            string[] round = Data[i].Split(" ");
            if (round[0] == map[round[1]].Item1) //draw, 3
            {
                score += (3 + map[round[1]].Item2);
            }
            else if (round[0] == map[round[1]].Item3) //win, 6
            {
                score += (6 + map[round[1]].Item2);
            }
            else //lose, 0
            {
                score += (0 + map[round[1]].Item2);
            }
        }
        Console.WriteLine("Part a solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day02bSolution(ref List<string> Data)
    {
        //dictionary of hand, win/loss/points
        Dictionary<string, Tuple<string, string, int>> map = new Dictionary<string, Tuple<string, string, int>>();
        map["A"] = new Tuple<string, string, int>("C", "B", 1);//rock
        map["B"] = new Tuple<string, string, int>("A", "C", 2);//paper
        map["C"] = new Tuple<string, string, int>("B", "A", 3);//scissors
        int score = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            string[] round = Data[i].Split(" ");
            //player wins
            if (round[1] == "Z")
            {
                //win score + points for hand that matches elf hand loss hand
                score += (6 + map[map[round[0]].Item2].Item3);
            }
            //player loses
            else if (round[1] == "X") //lose,3
            {
                //lose score + points for hand that matches elf hand win hand
                score += (0 + map[map[round[0]].Item1].Item3);
            }
            //player draws
            else
            {
                //draw score + points for hand that matches elf hand
                score += (3 + map[round[0]].Item3);
            }
        }
        Console.WriteLine("Part b solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day03aSolution(ref List<string> Data)
    {
        int score = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            int halfway = Data[i].Length / 2;
            // Creating array of string length 
            List<char> compartment1 = new List<char>();
            List<char> compartment2 = new List<char>();

            // Copy character by character into List of chars 
            for (int j = 0; j < halfway; j++)
            {
                compartment1.Add(Data[i][j]);
            }
            for (int j = halfway; j < Data[i].Length; j++)
            {
                compartment2.Add(Data[i][j]);
            }
            var commonValues = compartment1.Intersect(compartment2);
            foreach (char value in commonValues)
            {
                if (char.IsLower(value))
                {
                    score += ((int)value - 96);
                }
                else
                {
                    score += ((int)value - 38);
                }
            }

        }
        Console.WriteLine("Part a solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day03bSolution(ref List<string> Data)
    {
        int score = 0;
        //3 lists of chars to store the three elf rucksacks
        List<char> rucksack1 = new List<char>();
        List<char> rucksack2 = new List<char>();
        List<char> rucksack3 = new List<char>();

        //iterate through the list
        for (int i = 0; i < Data.Count; i++)
        {
            //use modulus to get elf 1,2 and 3, then reset
            if (i % 3 == 0)
            {
                rucksack1.Clear();
                for (int j = 0; j < Data[i].Length; j++)
                {
                    //elf 1 rucksack contents
                    rucksack1.Add(Data[i][j]);
                }

            }
            if (i % 3 == 1)
            {
                rucksack2.Clear();
                for (int j = 0; j < Data[i].Length; j++)
                {
                    //elf 2 rucksack contents
                    rucksack2.Add(Data[i][j]);
                }
            }
            if (i % 3 == 2)
            {
                rucksack3.Clear();
                for (int j = 0; j < Data[i].Length; j++)
                {
                    //elf 3 rucksack contents
                    rucksack3.Add(Data[i][j]);
                }
                //get items common to all three lists using set intersections
                List<char> commonValues = rucksack1.Intersect(rucksack2).Intersect(rucksack3).ToList();
                if (char.IsLower(commonValues[0]))
                {
                    score += ((int)commonValues[0] - 96);
                }
                else
                {
                    score += ((int)commonValues[0] - 38);
                }
            }

        }
        Console.WriteLine("Part b solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day04aSolution(ref List<string> Data)
    {
        int score = 0;
        //iterate through the list
        for (int i = 0; i < Data.Count; i++)
        {
            int lowerBound1 = Convert.ToInt32(Data[i].Split(",")[0].Split('-')[0]);
            int upperBound1 = Convert.ToInt32(Data[i].Split(",")[0].Split('-')[1]);
            int lowerBound2 = Convert.ToInt32(Data[i].Split(",")[1].Split('-')[0]);
            int upperBound2 = Convert.ToInt32(Data[i].Split(",")[1].Split('-')[1]);
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            for (int j = lowerBound1; j <= upperBound1; j++)
            {
                left.Add(j);
            }
            for (int j = lowerBound2; j <= upperBound2; j++)
            {
                right.Add(j);
            }
            var intersection = left.Intersect(right).ToList();
            if (left.Count == intersection.Count || right.Count == intersection.Count)
            {
                //Console.WriteLine(Data[i] + "overlap found!");
                score++;
            }
        }
        Console.WriteLine("Part a solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day04bSolution(ref List<string> Data)
    {
        int score = 0;
        //iterate through the list
        for (int i = 0; i < Data.Count; i++)
        {
            int lowerBound1 = Convert.ToInt32(Data[i].Split(",")[0].Split('-')[0]);
            int upperBound1 = Convert.ToInt32(Data[i].Split(",")[0].Split('-')[1]);
            int lowerBound2 = Convert.ToInt32(Data[i].Split(",")[1].Split('-')[0]);
            int upperBound2 = Convert.ToInt32(Data[i].Split(",")[1].Split('-')[1]);
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            for (int j = lowerBound1; j <= upperBound1; j++)
            {
                left.Add(j);
            }
            for (int j = lowerBound2; j <= upperBound2; j++)
            {
                right.Add(j);
            }
            var intersection = left.Intersect(right).ToList();
            if (intersection.Count > 0)
            {
                //Console.WriteLine(Data[i] + "overlap found!");
                score++;
            }
        }
        Console.WriteLine("Part b solution: " + score);
        Console.WriteLine("===============================");
    }
    public static void Day05aSolution(ref List<string> Data)
    {
        int lineNumber = 0;
        SortedDictionary<int, List<string>> stacks = new SortedDictionary<int, List<string>>();
        for (int i = 0; i < Data.Count; i++)
        {
            lineNumber++;
            if (char.IsNumber(Data[i][1]))
            {
                break;
            }
            else
            {
                for (int j = 0; j < Data[i].Length; j++)
                {
                    int index = ((j + 3) / 4);
                    if (char.IsLetter(Data[i][j]))
                    {
                        if (stacks.ContainsKey(index))
                        {
                            stacks[index].Add(Data[i][j].ToString());
                        }
                        else
                        {
                            stacks[index] = new List<string> { Data[i][j].ToString() };
                        }
                    }
                }
            }
        }
        //foreach (var stack in stacks)
        //{
        //    Console.WriteLine(stack.Key + " ==> " + string.Join(" , ", stack.Value));
        //}
        for (int i = lineNumber; i < Data.Count; i++)
        {
            if (Data[i].Length > 1 && char.IsLetter(Data[i][1]))
            {
                int howMany = 0;
                int moveFrom = 0;
                int moveTo = 0;
                List<int> steps = new List<int>();
                string[] numbers = Regex.Split(Data[i], @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int j = int.Parse(value);
                        steps.Add(j);
                    }
                }
                if (steps.Count == 3)
                {
                    howMany = steps[0];
                    moveFrom = steps[1];
                    moveTo = steps[2];
                }
                for (int j = 0; j < howMany; j++)
                {
                    string crate = stacks[moveFrom][0];
                    stacks[moveFrom].RemoveAt(0);
                    if (stacks.ContainsKey(moveTo))
                    {
                        stacks[moveTo].Insert(0, crate);
                    }
                    else
                    {
                        stacks[moveTo] = new List<string> { crate };
                    }
                }
            }
            //foreach (var stack in stacks)
            //{
            //    Console.WriteLine(stack.Key + " ==> " + string.Join(" , ", stack.Value));
            //}
        }
        string answer = "";
        foreach (var stack in stacks)
        {
            if (stack.Value.Count > 0)
            {
                answer += stack.Value[0];
            }
        }
        // move 3 from 1 to 3
        Console.WriteLine("Part a solution: " + answer);
        Console.WriteLine("===============================");
    }
    public static void Day05bSolution(ref List<string> Data)
    {
        int lineNumber = 0;
        SortedDictionary<int, List<string>> stacks = new SortedDictionary<int, List<string>>();
        for (int i = 0; i < Data.Count; i++)
        {
            lineNumber++;
            if (char.IsNumber(Data[i][1]))
            {
                break;
            }
            else
            {
                for (int j = 0; j < Data[i].Length; j++)
                {
                    int index = ((j + 3) / 4);
                    if (char.IsLetter(Data[i][j]))
                    {
                        if (stacks.ContainsKey(index))
                        {
                            stacks[index].Add(Data[i][j].ToString());
                        }
                        else
                        {
                            stacks[index] = new List<string> { Data[i][j].ToString() };
                        }
                    }
                }
            }
        }
        //foreach (var stack in stacks)
        //{
        //    Console.WriteLine(stack.Key + " ==> " + string.Join(" , ", stack.Value));
        //}
        for (int i = lineNumber; i < Data.Count; i++)
        {
            if (Data[i].Length > 1 && char.IsLetter(Data[i][1]))
            {
                int howMany = 0;
                int moveFrom = 0;
                int moveTo = 0;
                List<int> steps = new List<int>();
                string[] numbers = Regex.Split(Data[i], @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        int j = int.Parse(value);
                        steps.Add(j);
                    }
                }
                if (steps.Count == 3)
                {
                    howMany = steps[0];
                    moveFrom = steps[1];
                    moveTo = steps[2];
                }
                List<string> crates = stacks[moveFrom].GetRange(0, howMany);
                crates.Reverse();
                stacks[moveFrom].RemoveRange(0, howMany);
                if (stacks.ContainsKey(moveTo))
                {
                    foreach (string crate in crates)
                    {
                        stacks[moveTo].Insert(0, crate);
                    }
                }
                else
                {
                    stacks[moveTo] = crates;
                }
            }
            //foreach (var stack in stacks)
            //{
            //    Console.WriteLine(stack.Key + " ==> " + string.Join(" , ", stack.Value));
            //}
        }
        string answer = "";
        foreach (var stack in stacks)
        {
            if (stack.Value.Count > 0)
            {
                answer += stack.Value[0];
            }
        }
        Console.WriteLine("Part b solution: " + answer);
        Console.WriteLine("===============================");
    }
    public static void Day06aSolution(ref List<string> Data)
    {
        int packetLength = 4;
        for (int i = 0; i < Data.Count; i++)
        {
            int index = 0;
            for (int j = packetLength - 1; j < Data[i].Length; j++)
            {
                List<char> quad = new List<char>();
                for (int k = 0; k < packetLength; k++)
                {
                    quad.Add(Data[i][j - k]);
                }
                var CharSet = new HashSet<char>(quad);
                //Console.WriteLine(string.Join(", ", quad) + " ==> " + string.Join(", ", CharSet));
                if (CharSet.Count == packetLength)
                {
                    index = j + 1;
                    break;
                }
            }
            Console.WriteLine("Part a solution: " + index);
            Console.WriteLine("===============================");
        }
    }
    public static void Day06bSolution(ref List<string> Data)
    {
        int packetLength = 14;
        for (int i = 0; i < Data.Count; i++)
        {
            int index = 0;
            for (int j = packetLength - 1; j < Data[i].Length; j++)
            {
                List<char> quad = new List<char>();
                for (int k = 0; k < packetLength; k++)
                {
                    quad.Add(Data[i][j - k]);
                }
                var CharSet = new HashSet<char>(quad);
                //Console.WriteLine(string.Join(", ", quad) + " ==> " + string.Join(", ", CharSet));
                if (CharSet.Count == packetLength)
                {
                    index = j + 1;
                    break;
                }
            }
            Console.WriteLine("Part b solution: " + index);
            Console.WriteLine("===============================");
        }
    }
    public static void Day07Solution(ref List<string> Data)
    {
        int result = 0;
        int diskSpace = 70000000;
        int unusedSpaceReq = 30000000;
        // build the path as a list of string items
        List<string> path = new List<string>();
        // each path root has a total value, stored in a dict
        Dictionary<string, int> folders = new Dictionary<string, int>();
        // loop through all lines
        for (int i = 0; i < Data.Count; i++)
        {
            // remove last path entry 
            if (Data[i].Contains("$ cd .."))
            {
                path.RemoveAt(path.Count - 1);
            }
            // add new directory to path
            else if (Data[i].Contains("$ cd"))
            {
                path.Add(Data[i].Split(" ")[2]);
            }
            // if there is a number (file of size)..
            else if (char.IsNumber(Data[i][0]))
            {
                // get the number
                int fileSize = Convert.ToInt32(Data[i].Split(" ")[0]);
                // loop through current path items, starting with the root
                for (int j = 0; j < path.Count; j++)
                {
                    // build a list of path items to that index
                    List<string> pathSlice = new List<string>();
                    for (int k = 0; k < j + 1; k++)
                    {
                        pathSlice.Add(path[k]);
                    }
                    //join that list as a string and set it as the dict key
                    string pathKey = string.Join("/", pathSlice);
                    //if the key exists, add value to the total for that path
                    if (folders.ContainsKey(pathKey))
                    {
                        folders[pathKey] += fileSize;
                    }
                    //if the key doesn't, create it and set the inital value to value
                    else
                    {
                        folders.Add(pathKey, fileSize);
                    }
                }
            }
        }
        // Part a, loop through the dict and add any directory of size < 10000 to the result
        foreach (var item in folders)
        {
            if (item.Value < 100000)
            {
                result += item.Value;
            }
            //Console.WriteLine(item.Key + " ==> " + item.Value);
        }
        Console.WriteLine("Part a solution: " + result);
        Console.WriteLine("===============================");

        // Part b, if not enough remaining space..
        result = 0;
        int remainingSpace = diskSpace - folders["/"];
        //Console.WriteLine("Remaining space is " + remainingSpace);
        if (remainingSpace < unusedSpaceReq)
        {
            //loop through the dict, ordered on value (ASC)
            foreach (var item in folders.OrderBy(key => key.Value))
            {
                //Console.WriteLine(item.Key + " ==> " + item.Value);
                // if removing that directory gives a satisfactory result, return its size
                if (remainingSpace + item.Value >= unusedSpaceReq)
                {
                    result = item.Value;
                    break;
                }
            }
        }
        Console.WriteLine("Part b solution: " + result);
        Console.WriteLine("===============================");
    }

    // Nick's fancy class based recursive solution
    public static void Day07SolutionNSW(ref List<string> Data)
    {
        SupportRoutines.fso root = new SupportRoutines.fso("/", 0, false, null);
        SupportRoutines.fso current = root;
        bool lsmode = false;
        foreach (string s in Data)
        {
            string[] statement = s.Split(' ');
            if (statement[0] == "$")
            {
                lsmode = false;
                switch (statement[1])
                {
                    case "cd":
                        if (statement[2] == "/")
                            current = root;
                        else
                            current = current.CD(statement[2]);
                        break;
                    case "ls":
                        lsmode = true;
                        break;
                }
            }
            else if (lsmode)
                if (statement[0] == "dir")
                    current.AddFileOrDirectory(statement[1], 0, false);
                else
                    current.AddFileOrDirectory(statement[1], Convert.ToInt32(statement[0]), true);
        }
        Console.WriteLine("NSW Part a solution: " + (root.SizeLessThan(100000)));
        Console.WriteLine("===============================");
        int Smallestsize = 70000000;
        root.DirectoryToDelete((30000000 - (70000000 - root.Size())), ref Smallestsize);
        Console.WriteLine("NSW Part b solution: " + (Smallestsize));
        Console.WriteLine("===============================");
    }
    public static void Day08aSolution(ref List<string> Data)
    {
        int y = Data.Count;
        int x = Data[0].Length;
        int[,] trees = new int[y, x]; //the forest
        bool[,] visible = new bool[y, x]; // bool grid, true is visible from 1/4 directions
        bool[,] visibleX = new bool[y, x]; // bool grid, true is visible from 1/4 directions
        bool[,] visibleY = new bool[y, x]; // bool grid, true is visible from 1/4 directions
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                trees[i, j] = int.Parse(Data[i][j].ToString());
                if (j == 0 || j == x - 1 || i == 0 || i == y - 1)
                {
                    visible[i, j] = true;
                    visibleX[i, j] = true;
                    visibleY[i, j] = true;
                }
                else
                {
                    visible[i, j] = false;
                    visibleX[i, j] = false;
                    visibleY[i, j] = false;
                }
            }
        }
        // update the bool grid, true if already true, or visible from this direction
        void goFromLeft(ref bool[,] visibleX)
        {
            for (int i = 0; i < y; i++)
            {
                int currentMaxHeight = trees[i, 0];
                for (int j = 1; j < x; j++)
                {
                    if (trees[i, j] > currentMaxHeight)
                    {
                        visibleX[i, j] = true;
                    }
                    if (currentMaxHeight < trees[i, j])
                    {
                        currentMaxHeight = trees[i, j];
                    }
                }
            }
        }
        void goFromRight(ref bool[,] visibleX)
        {
            for (int i = 0; i < y; i++)
            {
                int currentMaxHeight = trees[i, y - 1];
                for (int j = x - 2; j >= 0; j--)
                {
                    if (j > 0 && visible[i, j])
                    {
                        //stop when we get to those marked visible from the left
                        break;
                    }
                    if (trees[i, j] > currentMaxHeight)
                    {
                        visibleX[i, j] = true;
                    }
                    if (currentMaxHeight < trees[i, j])
                    {
                        currentMaxHeight = trees[i, j];
                    }
                }
            }
        }
        void goFromBottom(ref bool[,] visibleY)
        {
            for (int i = 0; i < x; i++)
            {
                int currentMaxHeight = trees[x - 1, i];
                for (int j = y - 2; j >= 0; j--)
                {
                    if (trees[j, i] > currentMaxHeight)
                    {
                        visibleY[j, i] = true;
                    }
                    if (currentMaxHeight < trees[j, i])
                    {
                        currentMaxHeight = trees[j, i];
                    }
                }
            }
        }
        void goFromTop(ref bool[,] visibleY)
        {
            for (int i = 0; i < x; i++)
            {
                int currentMaxHeight = trees[0, i];
                for (int j = 0; j < y - 1; j++)
                {
                    if (j > 0 && visibleY[j, i])
                    {
                        //stop when we get to those marked visible from the bottom
                        break;
                    }
                    if (trees[j, i] > currentMaxHeight)
                    {
                        visibleY[j, i] = true;
                    }
                    if (currentMaxHeight < trees[j, i])
                    {
                        currentMaxHeight = trees[j, i];
                    }
                }
            }
        }
        // check each direction
        goFromLeft(ref visibleX);
        goFromRight(ref visibleX);
        goFromBottom(ref visibleY);
        goFromTop(ref visibleY);

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (visibleX[i, j] || visibleY[i, j])
                {
                    visible[i, j] = true;
                }
            }
        }
        Console.WriteLine("Part a solution: " + SupportRoutines.countVisible(visible));
        Console.WriteLine("===============================");

    }
    public static void Day08bSolution(ref List<string> Data)
    {
        Dictionary<string, Point> Directions = new Dictionary<string, Point> { // This is slower
            {"Left", new Point(-1, 0)},
            {"Right", new Point(1,0)},
            {"Down", new Point(0,-1)},
            {"Up", new Point(0, 1)}
        };
        int ForestHeight = Data.Count;
        int ForestWidth = Data[0].Length;
        int[,] trees = new int[ForestHeight, ForestWidth];
        // populate the forest
        for (int i = 0; i < ForestHeight; i++)
        {
            for (int j = 0; j < ForestWidth; j++)
            {
                trees[i, j] = int.Parse(Data[i][j].ToString());
            }
        }
        // print a grid of ints
        void printGridInt(int[,] grid)
        {
            for (int i = 0; i < ForestHeight; i++)
            {
                string line = "";
                for (int j = 0; j < ForestWidth; j++)
                {
                    line += grid[i, j];
                }
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
        // get the score for each position in the forest
        int getMaxScore(int[,] grid)
        {
            int maxScore = 0;
            for (int i = 0; i < ForestHeight; i++)
            {
                for (int j = 0; j < ForestWidth; j++)
                {
                    int currentScore = goLeft(i, j) * goRight(i, j) * goUp(i, j) * goDown(i, j);
                    if (maxScore < currentScore)
                    {
                        maxScore = currentScore;
                    }
                }
            }
            return maxScore;
        }
        int goLeft(int y, int x)
        {
            int visibleScore = 0;
            int height = trees[y, x];
            // using dictionary of direction modifiers, this is way slower
            x--;
            while (x >= 0)
            {
                if (trees[y, x] < height)
                {
                    visibleScore += 1;
                }
                else
                {

                    visibleScore += 1;
                    break;
                }
                x += Directions["Left"].X;
            }
            //for (int j = x - 1; j >= 0; j--) // this is faster than above
            //{
            //    if (trees[y, j] < height)
            //    {
            //        visibleScore += 1;
            //    }
            //    if (trees[y, j] >= height)
            //    {
            //        visibleScore += 1;
            //        break;
            //    }
            //}
            return visibleScore;
        }
        int goRight(int y, int x)
        {
            int visibleScore = 0;
            int height = trees[y, x];
            for (int j = x + 1; j < ForestWidth; j++)
            {

                if (trees[y, j] < height)
                {
                    visibleScore += 1;

                }
                if (trees[y, j] >= height)
                {
                    visibleScore += 1;
                    break;
                }
            }
            return visibleScore;
        }
        int goUp(int y, int x)
        {
            int visibleScore = 0;
            int height = trees[y, x];
            for (int j = y - 1; j >= 0; j--)
            {
                if (trees[j, x] < height)
                {
                    visibleScore += 1;

                }
                if (trees[j, x] >= height)
                {
                    visibleScore += 1;
                    break;
                }
            }
            return visibleScore;
        }
        int goDown(int y, int x)
        {
            int visibleScore = 0;
            int height = trees[y, x];
            for (int j = y + 1; j < ForestHeight; j++)
            {
                if (trees[j, x] < height)
                {
                    visibleScore += 1;

                }
                if (trees[j, x] >= height)
                {
                    visibleScore += 1;
                    break;
                }
            }
            return visibleScore;
        }
        Console.WriteLine("Part b solution: " + getMaxScore(trees));
        Console.WriteLine("===============================");
    }
}