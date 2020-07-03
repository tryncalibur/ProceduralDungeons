﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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

    public class Path
    {
        public Tuple<int, int> Coord
        { get; set; }
        public int Direction
        { get; set; }

        public Path(int r, int c, int D)
        {
            Coord = new Tuple<int, int>(r, c);
            Direction = D;
        }
        public Path(Tuple<int, int> t, int D)
        {
            Coord = t;
            Direction = D;
        }
    }



    public room[,] Dungeon
    { get; }
    private int NumRow;
    private int NumCol;
    private int StartRow;
    private int StartCol;
    private int EndRow;
    private int EndCol;

    private List<List<Tuple<int, int>>> BlockList = new List<List<Tuple<int, int>>>();
    private List<Tuple<int, int>> Temp = new List<Tuple<int, int>>();

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
        while (!Search(StartRow, StartCol, SearchNum))
        {
            Temp.Clear();
             
            // Mark Blocks
            for (int i = 0; i < r; ++i)
            {
                for (int j = 0; j < c; ++j)
                {
                    if (Dungeon[i, j].Visit == 0)
                    {
                        Search(i, j, ++SearchNum);
                        BlockList.Add(Temp);
                        Temp = new List<Tuple<int, int>>();
                    }
                }
            }


            // Connect Blocks (Priorty to start)
            foreach (List<Tuple<int, int>> Block in BlockList)
            {
                var ConnectOrigin = new List<Path>();
                var ConnectDiff = new List<Path>();
                
                foreach(Tuple<int, int> Room in Block)
                {
                    int Row = Room.Item1;
                    int Col = Room.Item2;

                    if ((Row != 0) && !(Dungeon[Row, Col].Up) && (Dungeon[Row, Col].Visit != Dungeon[Row-1, Col].Visit))
                    {
                        if (Dungeon[Row - 1, Col].Visit == 1) ConnectOrigin.Add(new Path(Room, 1));
                        else ConnectDiff.Add(new Path(Room, 1));
                    }

                    if ((Row != r-1) && !(Dungeon[Row, Col].Down) && (Dungeon[Row, Col].Visit != Dungeon[Row+1, Col].Visit))
                    {
                        if (Dungeon[Row + 1, Col].Visit == 1) ConnectOrigin.Add(new Path(Room, 2));
                        else ConnectDiff.Add(new Path(Room, 2));
                    }

                    if ((Col != 0) && !(Dungeon[Row, Col].Left) && (Dungeon[Row, Col].Visit != Dungeon[Row, Col-1].Visit))
                    {
                        if (Dungeon[Row, Col - 1].Visit == 1) ConnectOrigin.Add(new Path(Room, 3));
                        else ConnectDiff.Add(new Path(Room, 3));
                    }

                    if ((Col != c-1) && !(Dungeon[Row, Col].Down) && (Dungeon[Row, Col].Visit != Dungeon[Row, Col+1].Visit))
                    {
                        if (Dungeon[Row, Col + 1].Visit == 1) ConnectOrigin.Add(new Path(Room, 4));
                        else ConnectDiff.Add(new Path(Room, 4));
                    }
                }

                // Determine Type of correction
                if (ConnectDiff.Count != 0 || ConnectOrigin.Count != 0)
                {
                    Path MakeConnect = new Path(-1, -1, -1);
                    if (ConnectDiff.Count != 0) MakeConnect = ConnectDiff[rnd.Next(ConnectDiff.Count)];
                    if (ConnectOrigin.Count != 0) MakeConnect = ConnectOrigin[rnd.Next(ConnectOrigin.Count)];

                    // Connect two blocks
                    switch (MakeConnect.Direction)
                    {
                        case 1:
                            Dungeon[MakeConnect.Coord.Item1, MakeConnect.Coord.Item2].Up = true;
                            Dungeon[MakeConnect.Coord.Item1 - 1, MakeConnect.Coord.Item2].Down = true;
                            break;
                        case 2:
                            Dungeon[MakeConnect.Coord.Item1, MakeConnect.Coord.Item2].Down = true;
                            Dungeon[MakeConnect.Coord.Item1 + 1, MakeConnect.Coord.Item2].Up = true;
                            break;
                        case 3:
                            Dungeon[MakeConnect.Coord.Item1, MakeConnect.Coord.Item2].Left = true;
                            Dungeon[MakeConnect.Coord.Item1, MakeConnect.Coord.Item2 - 1].Right = true;
                            break;
                        case 4:
                            Dungeon[MakeConnect.Coord.Item1, MakeConnect.Coord.Item2].Right = true;
                            Dungeon[MakeConnect.Coord.Item1, MakeConnect.Coord.Item2 + 1].Left = true;
                            break;
                        default:
                            Console.WriteLine("Undefine Direction");
                            break;
                    }
                }
            }
            

            // Clear Blocks
            ClearSearch();
            BlockList.Clear();
            SearchNum = 1;
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

        Temp.Add(new Tuple<int, int>(r, c));

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
