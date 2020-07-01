using System;
using System.CodeDom.Compiler;

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

        int Status
        { get; set; }
        public room()
        {
            Up = rnd.Next(3) == 1;
            Down = rnd.Next(3) == 1;
            Left = rnd.Next(3) == 1;
            Right = rnd.Next(3) == 1;
            Status = 0;
        }
    }



    public room[,] Dungeon
    {get;}
    private int NumRow;
    private int NumCol;

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
                if (Dungeon[i, j].Left) Console.Write("-0");
                else Console.Write(" 0");
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
}
