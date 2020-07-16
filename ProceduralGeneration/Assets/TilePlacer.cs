using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    public GameObject[] BuildingBlocks;
    public int rows = 5;
    public int cols = 5;
    int footprint = 30;

    // Start is called before the first frame update
    void Start()
    {
        ProceduralDungeon PD = new ProceduralDungeon(rows, cols);
        var dungeon = PD.Dungeon;

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                Debug.Log($"{dungeon[i,j].Visit}");
                // Big Room
                if (dungeon[i, j].Visit == 1 && dungeon[i, j].Status == 'B')
                {
                    Vector3 pos = new Vector3(i * footprint, 0, j * footprint);
                    Instantiate(BuildingBlocks[0], pos, Quaternion.identity);
                }
                // Small Room
                else if (dungeon[i, j].Visit == 1)
                {
                    Vector3 pos = new Vector3(i * footprint + 5, 0, j * footprint - 5);
                    Instantiate(BuildingBlocks[3], pos, Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

