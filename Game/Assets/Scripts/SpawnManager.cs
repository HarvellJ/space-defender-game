using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject spawnLeft;
    public GameObject spawnRight;
    public GameObject spawnUpperLeft;
    public GameObject spawnUpperRight;

    // Distant background spawns
    public GameObject distantSpawn1;
    public GameObject distantSpawn2;
    public GameObject distantSpawn3;
    public GameObject distantSpawn4;
    public GameObject distantSpawn5;
    public GameObject distantSpawn6;
    public GameObject distantSpawn7;
    public GameObject distantSpawn8;

    GameObject previousSpawn;
    GameObject previousDistantSpawn;

    // Start is called before the first frame update
    void Start()
    {
        previousSpawn = spawnLeft;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetSpawnPoint()
    {
        // ensure enemies are evenly distributed between spawns
        if (previousSpawn == spawnLeft)
        {
            previousSpawn = spawnUpperLeft;
            return spawnUpperLeft;
        }
        else if (previousSpawn == spawnUpperLeft)
        {
            previousSpawn = spawnUpperRight;
            return spawnUpperRight;
        }
        else if (previousSpawn == spawnUpperRight)
        {
            previousSpawn = spawnRight;
            return spawnRight;
        }
        else
        {
            previousSpawn = spawnLeft;
            return spawnLeft;
        }
    }

    public GameObject GetDistantSpawnPoint()
    {
        if (previousSpawn == distantSpawn1)
        {
            previousSpawn = distantSpawn2;
            return distantSpawn2;
        }
        else if (previousSpawn == distantSpawn2)
        {
            previousSpawn = distantSpawn3;
            return distantSpawn3;
        }
        else if (previousSpawn == distantSpawn3)
        {
            previousSpawn = distantSpawn4;
            return distantSpawn4;
        }
        else if (previousSpawn == distantSpawn4)
        {
            previousSpawn = distantSpawn5;
            return distantSpawn5;
        }
        else if (previousSpawn == distantSpawn5)
        {
            previousSpawn = distantSpawn6;
            return distantSpawn6;
        }
        else if (previousSpawn == distantSpawn6)
        {
            previousSpawn = distantSpawn7;
            return distantSpawn7;
        }
        else if (previousSpawn == distantSpawn7)
        {
            previousSpawn = distantSpawn8;
            return distantSpawn8;
        }
        else
        {
            previousSpawn = distantSpawn1;
            return distantSpawn1;
        }
    }
}
