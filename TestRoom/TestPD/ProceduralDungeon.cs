using System;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;

public class ProceduralDungeon
{

    // Data about each room Mapwise
    public class room
    {
        Random rnd = new Random();
        public bool Up
        { get; set; }
        public bool Down
        { get; set; }
        public bool Left
        { get; set; }
        public bool Right
        { get; set; }

        public char Status
        { get; set; }

        public int Visit
        { get; set; }

        public room()
        {
            Up = rnd.Next(3) == 1;
            Down = rnd.Next(3) == 1;
            Left = rnd.Next(3) == 1;
            Right = rnd.Next(3) == 1;
            Status = 'O';
            Visit = 0;
        }
    }



    public room[,] Dungeon
    {get;}
    private int NumRow;
    private int NumCol;
    private int StartRow;
    private int StartCol;
    private int EndRow;
    private int EndCol;

    public ProceduralDungeon(int r = 5, int c = 5)
    {
        NumRow = r;
        NumCol = c;

        // Generate all rooms
        Dungeon = new room[r, c];
        for (int i = 0; i < r; ++i)
        {
            for (int j = 0; j < c; ++j)
            {
                Dungeon[i, j] = new room();
            }
        }

        // Clean Edges
        for (int i = 0; i < r; ++i)
        {
            Dungeon[i, 0].Left = false;
            Dungeon[i, c - 1].Right = false;
        }
        for (int i = 0; i < c; ++i)
        {
            Dungeon[0, i].Up = false;
            Dungeon[r-1, i].Down = false;
        }

        // Set Start/End
        Random rnd = new Random();
        StartRow = rnd.Next(r);
        StartCol = rnd.Next(c);
        Dungeon[StartRow, StartCol].Status = 'S';
        while (true)
        {
            EndRow = rnd.Next(r);
            EndCol = rnd.Next(c);
            if (StartRow != EndRow || StartCol != EndCol)
            {
                Dungeon[EndRow, EndCol].Status = 'E';
                break;
            }
        }

        // Connect rooms based on entrances
        for (int i = 0; i < r; ++i)
        {
            for (int j = 0; j < c; ++j)
            {
                if (Dungeon[i, j].Up) Dungeon[i - 1, j].Down = true;
                if (Dungeon[i, j].Down) Dungeon[i + 1, j].Up = true;
                if (Dungeon[i, j].Left) Dungeon[i, j - 1].Right = true;
                if (Dungeon[i, j].Right) Dungeon[i, j + 1].Left = true;
            }
        }

        // Determine number of independent blocks
        int SearchNum = 1;
        Search(StartRow, StartCol, SearchNum);
        for (int i = 0; i < r; ++i)
        {
            for (int j = 0; j < c; ++j)
            {
                if (Dungeon[i, j].Visit == 0) Search(i, j, ++SearchNum);
            }
        }
    }

    public void PrintPD()
    {
        for (int i = 0; i < NumRow; ++i)
        {
            for (int j = 0; j < NumCol; ++j)
            {
                if (Dungeon[i, j].Up) Console.Write(" | ");
                else Console.Write("   ");
            }
            Console.Write("\n");

            for (int j = 0; j < NumCol; ++j)
            {
                if ((i == StartRow && j == StartCol) || (i == EndRow && j == EndCol))
                {
                    if (Dungeon[i, j].Left) Console.Write($"-{Dungeon[i, j].Status}");
                    else Console.Write($" {Dungeon[i, j].Status}");
                }
                else
                {
                    if (Dungeon[i, j].Left) Console.Write($"-{Dungeon[i, j].Visit}");
                    else Console.Write($" {Dungeon[i, j].Visit}");
                }
                if (Dungeon[i, j].Right) Console.Write("-");
                else Console.Write(" ");
            }
            Console.Write("\n");

            for (int j = 0; j < NumCol; ++j)
            {
                if (Dungeon[i, j].Down) Console.Write(" | ");
                else Console.Write("   ");
            }
            Console.Write("\n");
        }
    }
    
    // Find connected blocks of dungeon
    private bool Search(int r, int c, int Mark)
    {
        room CurrentRoom = Dungeon[r, c];
        bool ReturnVal = false;

        // Room is already visited
        if (CurrentRoom.Visit != 0) return false;
       
        // End Room found
        if (CurrentRoom.Status == 'E') ReturnVal = true;

        // Search Other Rooms
        CurrentRoom.Visit = Mark;
        if (CurrentRoom.Up)    ReturnVal |= Search(r - 1, c, Mark);
        if (CurrentRoom.Down)  ReturnVal |= Search(r + 1, c, Mark);
        if (CurrentRoom.Left)  ReturnVal |= Search(r, c - 1, Mark);
        if (CurrentRoom.Right) ReturnVal |= Search(r, c + 1, Mark);

        return ReturnVal;
    }

    // Clear Visit marks
    private void ClearSearch()
    {
        for (int i = 0; i < NumRow; ++i)
        {
            for (int j = 0; j < NumCol; ++j)
            {
                Dungeon[i, j].Visit = 0;
            }
        }
    }
}
