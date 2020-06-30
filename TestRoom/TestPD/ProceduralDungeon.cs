using System;
using System.CodeDom.Compiler;

public class ProceduralDungeon {

    // Data about each room
    private class room
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
            Up = rnd.Next(2) == 1;
            Down = rnd.Next(2) == 1;
            Left = rnd.Next(2) == 1;
            Right = rnd.Next(2) == 1;
            Status = 0;
        }
    }

    private room[,] dungeon;
    public void Generate(int r, int c)
    {
        // Generate all rooms
        dungeon = new room[r, c];
        for (int i = 0; i < r; ++i)
        {
            for (int j = 0; j < c; ++j)
            {
                dungeon[i, j] = new room();
            }
        }

        // Clean Edges
        for (int i = 0; i < r; ++i)
        {
            dungeon[i, 0].Left = false;
            dungeon[i, c - 1].Right = false;
        }
        for (int i = 0; i < c; ++i)
        {
            dungeon[0, i].Up = false;
            dungeon[r-1, i].Down = false;
        }
    }
}
