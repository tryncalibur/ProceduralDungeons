using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    public GameObject[] BuildingBlocks; 
    public int rows = 5;
    public int cols = 5;
    private int footprint = 30;

    // Start is called before the first frame update
    void Start()
    {
        ProceduralDungeon PD = new ProceduralDungeon(rows, cols);
        var dungeon = PD.Dungeon;
        Debug.Log(PD.PrintPD());

        for (int i = 0; i < cols; ++i)
        {
            for (int j = 0; j < rows; ++j)
            {
                int r = rows - j - 1;
                int c = i;
                // Big Room
                if (dungeon[r, c].Visit == 1 && dungeon[r, c].Status == 'B')
                {
                    // Add Floor
                    Vector3 pos = new Vector3(i * footprint, 0, j * footprint);
                    Instantiate(BuildingBlocks[0], pos, Quaternion.identity);

                    // Add Walls
                    if (!dungeon[r, c].Up) Instantiate(BuildingBlocks[1], pos, Quaternion.identity);
                    else if (dungeon[r, c].Up && dungeon[r - 1, c].Status != 'B') Instantiate(BuildingBlocks[2], pos, Quaternion.identity);

                    if (!dungeon[r, c].Left) Instantiate(BuildingBlocks[1], pos, Quaternion.AngleAxis(90, Vector3.up));
                    else if (dungeon[r, c].Left && dungeon[r, c - 1].Status != 'B') Instantiate(BuildingBlocks[2], pos, Quaternion.AngleAxis(90, Vector3.up));

                    Vector3 pos2 = new Vector3(i * footprint + 30, 0, j * footprint);
                    if (!dungeon[r, c].Right) Instantiate(BuildingBlocks[1], pos2, Quaternion.AngleAxis(90, Vector3.up));
                    else if (dungeon[r, c].Right && dungeon[r, c + 1].Status != 'B') Instantiate(BuildingBlocks[2], pos2, Quaternion.AngleAxis(90, Vector3.up));

                    Vector3 pos1 = new Vector3(i * footprint, 0, j * footprint - 30);
                    if (!dungeon[r, c].Down) Instantiate(BuildingBlocks[1], pos1, Quaternion.identity);
                    else if (dungeon[r, c].Down && dungeon[r + 1, c].Status != 'B') Instantiate(BuildingBlocks[2], pos1, Quaternion.identity);

                }
                // Small Room
                else if (dungeon[r, c].Visit == 1)
                {
                    // Add Floor
                    Vector3 pos = new Vector3(i * footprint + 5, 0, j * footprint - 5);
                    Instantiate(BuildingBlocks[3], pos, Quaternion.identity);

                    // Add Walls
                    Vector3 pos1 = new Vector3(i * footprint + 5, 0, j * footprint - 4);
                    if (dungeon[r, c].Up) Instantiate(BuildingBlocks[5], pos1, Quaternion.identity);
                    else Instantiate(BuildingBlocks[4], pos1, Quaternion.identity);

                    Vector3 pos2 = new Vector3(i * footprint + 26, 0, j * footprint - 5); 
                    if (dungeon[r, c].Right) Instantiate(BuildingBlocks[5], pos2, Quaternion.AngleAxis(90, Vector3.up));
                    else Instantiate(BuildingBlocks[4], pos2, Quaternion.AngleAxis(90, Vector3.up));

                    Vector3 pos3 = new Vector3(i * footprint + 4, 0, j * footprint - 25);
                    if (dungeon[r, c].Left) Instantiate(BuildingBlocks[5], pos3, Quaternion.AngleAxis(-90, Vector3.up));
                    else Instantiate(BuildingBlocks[4], pos3, Quaternion.AngleAxis(-90, Vector3.up));

                    Vector3 pos4 = new Vector3(i * footprint + 25, 0, j * footprint - 26);
                    if (dungeon[r, c].Down) Instantiate(BuildingBlocks[5], pos4, Quaternion.AngleAxis(180, Vector3.up));
                    else Instantiate(BuildingBlocks[4], pos4, Quaternion.AngleAxis(180, Vector3.up));

                    // Add Addons to small Dungeon
                    switch (dungeon[r, c].Status)
                    {
                        case 'O':
                            int RNDSmall = UnityEngine.Random.Range(0, 10);
                            Vector3 Small = new Vector3(i * footprint + 10, 0, j * footprint - 10);
                            if (RNDSmall < 5) break;
                            else if (RNDSmall < 5) Instantiate(BuildingBlocks[8], Small, Quaternion.identity);
                            else if (RNDSmall < 6) Instantiate(BuildingBlocks[9], Small, Quaternion.identity);
                            else if (RNDSmall < 7) Instantiate(BuildingBlocks[10], Small, Quaternion.identity);
                            else if (RNDSmall < 8) Instantiate(BuildingBlocks[11], Small, Quaternion.identity);
                            break;
                        case 'D':
                            break;
                        case 'S':
                            Vector3 S = new Vector3(i * footprint + 10, 0, j * footprint - 10);
                            Instantiate(BuildingBlocks[6], S, Quaternion.identity);
                            break;
                        case 'E':
                            Vector3 E = new Vector3(i * footprint + 10, 0, j * footprint - 10);
                            Instantiate(BuildingBlocks[7], E, Quaternion.identity);
                            break;
                        default:
                            Debug.Log($"Unexpected char in Dungeon.[{r}, {c}.Status]");
                            break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

