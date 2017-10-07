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

    /* Snake Data*/
    static Queue<Point> snake;
    static Point snakeHead;
    static int rowVelocity = 0;
    static int colVelocity = 1;

    /* Apple Data */
    static Point apple;
    static Random RNG = new Random();

    static bool isGameOver = false;

    static void Main()
    {
        SetupConsole();
        InitializeSnake();
        SpawnApple();

        while (!isGameOver)
        {
            ReadInput();

            ClearApple();
            DrawApple();

            ClearSnake();
            UpdateSnake();
            DrawSnake();
            
            Thread.Sleep(100);
        }

        Console.CursorSize = 40;
        WriteAt(new Point(Console.WindowHeight / 2, 
                          Console.WindowWidth / 2 - 4), 
            ConsoleColor.Red, 
            "Game Over");
        Console.ReadLine();
    }

    static void ReadInput()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                rowVelocity = 0;
                colVelocity = -1;
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
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                rowVelocity = -1;
                colVelocity = 0;
            }
        }
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

    static void SpawnApple()
    {
        int newRow = RNG.Next(Console.WindowHeight - 1);
        int newCol = RNG.Next(Console.WindowWidth);
        apple = new Point(newRow, newCol);
    }

    static void ClearApple()
    {
        WriteAt(apple, ConsoleColor.Red, " ");
    }

    static void DrawApple()
    {
        WriteAt(apple, ConsoleColor.Red, "@");
    }

    static bool IsAppleEaten()
    {
        return snakeHead.row == apple.row &&
               snakeHead.col == apple.col;
    }

    static void ClearSnake()
    {
        Point tail = snake.Peek();
        WriteAt(tail, ConsoleColor.Black, " ");
    }

    static void UpdateSnake()
    {
        snakeHead = new Point(
            snakeHead.row + rowVelocity, 
            snakeHead.col + colVelocity);

        if (!IsInBounds() ||
            HasSnakeEatenItself(snakeHead))
        {
            isGameOver = true;
            return;
        }

        snake.Enqueue(snakeHead);

        if (IsAppleEaten())
        {
            SpawnApple();
        }
        else
        {
            snake.Dequeue();
        }
    }

    static void DrawSnake()
    {
        foreach (Point p in snake)
        {
            WriteAt(p, ConsoleColor.Green, "*");
        }
    }

    static bool HasSnakeEatenItself(Point newHead)
    {
        return snake.Contains(newHead);
    }

    static bool IsInBounds()
    {
        return (snakeHead.row >= 0 && 
                snakeHead.row < Console.WindowHeight) &&
               (snakeHead.col >= 0 && 
                snakeHead.col < Console.WindowWidth);
    }

    static void SetupConsole()
    {
        Console.WindowWidth = 70;
        Console.WindowHeight = 21;
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.Title = "EU Code Week Varna Snake";

        Console.CursorVisible = false;
    }

    static void WriteAt(Point p, ConsoleColor color, string text)
    {
        Console.SetCursorPosition(p.col, p.row);
        Console.ForegroundColor = color;
        Console.Write(text);
    }
}