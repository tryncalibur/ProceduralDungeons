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
                    Vector3 pos = new Vector3(i * footprint, 0, j * footprint);
                    Instantiate(BuildingBlocks[0], pos, Quaternion.identity);
                }
                // Small Room
                else if (dungeon[r, c].Visit == 1)
                {
                    Vector3 pos = new Vector3(i * footprint + 5, 0, j * footprint - 5);
                    Instantiate(BuildingBlocks[3], pos, Quaternion.identity);

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
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

