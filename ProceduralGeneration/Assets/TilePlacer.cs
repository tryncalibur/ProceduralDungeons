using System.Collections;
using System.Collections.Generic;
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
        Vector3 pos = new Vector3(0, 0, 0);
        Instantiate(BuildingBlocks[0], pos, Quaternion.identity);

        Vector3 pos2 = new Vector3(footprint, 0, footprint);
        Instantiate(BuildingBlocks[0], pos2, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
