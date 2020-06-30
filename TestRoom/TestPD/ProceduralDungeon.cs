using System;

public class ProceduralDungeon {
    private class room
    {
        Random rnd = new Random();
        bool Up
        { get; set; }
        bool Down
        { get; set; }
        bool Left
        { get; set; }
        bool Right
        { get; set; }

        int Status
        { get; set; }
        private room()
        {
            Up = rnd.Next(2) == 1;
            Down = rnd.Next(2) == 1;
            Left = rnd.Next(2) == 1;
            Right = rnd.Next(2) == 1;
            Status = 0;
        }
    }


    ProceduralDungeon(int r, int c)
    {

    }
}
