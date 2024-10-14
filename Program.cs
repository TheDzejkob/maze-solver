using System;
using System.Collections.Generic;

class Program
{
    static int width, height;
    static int[,] maze;
    static Random rand = new Random();

    static void Main(string[] args)
    {
        Console.WriteLine("šířka:");
        width = int.Parse(Console.ReadLine()) * 2 + 1;  // srovná stěny pokud lichý počet 
        Console.WriteLine("Výška:");
        height = int.Parse(Console.ReadLine()) * 2 + 1; // -||-

        maze = new int[width, height];

        GenerateMaze();
        DisplayMaze();

        Console.WriteLine("\n Řešení");
        SolveMaze();
    }

    static void GenerateMaze()
    {
        Stack<(int, int)> stack = new Stack<(int, int)>();
        int startX = 1, startY = 1;
        stack.Push((startX, startY));
        maze[startX, startY] = 1;

        while (stack.Count > 0)
        {
            var (x, y) = stack.Peek();
            List<(int, int)> neighbors = new List<(int, int)>();


            if (x > 1 && maze[x - 2, y] == 0) neighbors.Add((x - 2, y)); 
            if (x < width - 2 && maze[x + 2, y] == 0) neighbors.Add((x + 2, y)); 
            if (y > 1 && maze[x, y - 2] == 0) neighbors.Add((x, y - 2)); 
            if (y < height - 2 && maze[x, y + 2] == 0) neighbors.Add((x, y + 2)); 

            if (neighbors.Count > 0)
            {
                var (nx, ny) = neighbors[rand.Next(neighbors.Count)];
                maze[nx, ny] = 1;
                maze[(x + nx) / 2, (y + ny) / 2] = 1;
                stack.Push((nx, ny));
            }
            else
            {
                stack.Pop();
            }
        }
    }

    static void SolveMaze()
    {
        Queue<(int, int)> queue = new Queue<(int, int)>();
        bool[,] visited = new bool[width, height];
        (int, int)[,] parent = new (int, int)[width, height];
        int startX = 1, startY = 1;
        int endX = width - 2, endY = height - 2;

        queue.Enqueue((startX, startY));
        visited[startX, startY] = true;
        parent[startX, startY] = (-1, -1);

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            if (x == endX && y == endY)
            {
                while (x != -1 && y != -1)
                {
                    maze[x, y] = 2; 
                    (x, y) = parent[x, y];
                }
                DisplayMaze();
                return;
            }

            
            foreach (var (nx, ny) in new (int, int)[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) })
            {
                if (nx >= 0 && ny >= 0 && nx < width && ny < height && !visited[nx, ny] && maze[nx, ny] == 1)
                {
                    queue.Enqueue((nx, ny));
                    visited[nx, ny] = true;
                    parent[nx, ny] = (x, y);
                }
            }
        }

        Console.WriteLine("Nenalezeno řešení!");
    }

    static void DisplayMaze()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (maze[x, y] == 0)
                    Console.Write("#"); // Stěna
                else if (maze[x, y] == 1)
                    Console.Write(" "); // Cesta
                else if (maze[x, y] == 2)
                    Console.Write("."); // Správná cesta
            }
            Console.WriteLine();
        }
    }
}
