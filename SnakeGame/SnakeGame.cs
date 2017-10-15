using System;
using System.Collections.Generic;
using System.Threading;

class SnakeGame
{
    class Point
    {
        public int row;
        public int col;

        public Point(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                Point other = (Point)obj;
                return this.row == other.row &&
                       this.col == other.col;
            }

            return false;
        }
    }

    static Queue<Point> snake;
    static Point snakeHead;
    static int rowVelocity = 0;
    static int colVelocity = 1;

    static bool isGameOver = false;

    static Point apple;
    static Random randomGenerator = new Random();

    static void Main()
    {
        SetupTerminal();
        InitializeSnake();
        SpawnApple();

        while (!isGameOver)
        {
            ReadInput();

            ClearApple();
            ClearSnake();
            
            // UpdateApple();
            UpdateSnake();

            DrawApple();
            DrawSnake();

            Thread.Sleep(100);
        }

        WriteAt(Console.WindowHeight / 2,
            Console.WindowWidth / 2 - 4,
            ConsoleColor.Red,
            "Game Over");

        Console.ReadLine();
    }

    static void ReadInput()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                rowVelocity = -1;
                colVelocity = 0;
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                rowVelocity = 0;
                colVelocity = 1;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                rowVelocity = 1;
                colVelocity = 0;
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                rowVelocity = 0;
                colVelocity = -1;
            }
        }
    }
    
    static void SpawnApple()
    {
        int newRow = randomGenerator.Next(Console.WindowHeight);
        int newCol = randomGenerator.Next(Console.WindowWidth);
        apple = new Point(newRow, newCol);
    }

    static void ClearApple()
    {
        WriteAt(apple.row, apple.col, ConsoleColor.Red, " ");
    }

    static void DrawApple()
    {
        WriteAt(apple.row, apple.col, ConsoleColor.Red, "@");
    }

    static bool IsAppleEaten(Point p)
    {
        return apple.row == p.row &&
               apple.col == p.col;
    }

    static void InitializeSnake()
    {
        snake = new Queue<Point>();

        snake.Enqueue(new Point(0, 0));
        snake.Enqueue(new Point(0, 1));
        snake.Enqueue(new Point(0, 2));
        snake.Enqueue(new Point(0, 3));

        snakeHead = new Point(0, 4);
        snake.Enqueue(snakeHead);
    }

    static void ClearSnake()
    {
        foreach (Point p in snake)
        {
            WriteAt(p.row, p.col, ConsoleColor.Black, " ");
        }
    }

    static void UpdateSnake()
    {
        snakeHead = new Point(snakeHead.row + rowVelocity,
                              snakeHead.col + colVelocity);

        if (!IsInBounds(snakeHead) || 
            IsInsideSnake(snakeHead))
        {
            isGameOver = true;
            return;
        }
        
        if (IsAppleEaten(snakeHead))
        {
            SpawnApple();
        }
        else
        {
            snake.Dequeue();
        }

        snake.Enqueue(snakeHead);
    }

    static void DrawSnake()
    {
        foreach (Point p in snake)
        {
            WriteAt(p.row, p.col, ConsoleColor.Green, "*");
        }
    }

    static bool IsInsideSnake(Point p)
    {
        return snake.Contains(p);
    }

    static bool IsInBounds(Point p)
    {
        return p.row >= 0 && p.row < Console.WindowHeight &&
               p.col >= 0 && p.col < Console.WindowWidth;
    }

    static void SetupTerminal()
    {
        Console.WindowWidth = 70;
        Console.WindowHeight = 21;
        Console.BufferWidth = Console.WindowWidth;
        Console.BufferHeight = Console.WindowHeight;
        Console.Title = "Snake Challenge";

        Console.CursorVisible = false;
    }

    static void WriteAt(int row, int col, ConsoleColor color, string text)
    {
        Console.SetCursorPosition(col, row);
        Console.ForegroundColor = color;
        Console.Write(text);
    }
}