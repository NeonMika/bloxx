using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyCreator : MonoBehaviour
{
    public GameObject bunnyPrefab;
    public GameObject bunnyCollection;

    public CubeChunk world;

    public int min_x = 0;
    public int max_x = 0;
    public int min_z = 0;
    public int max_z = 0;
    public float bunnyCreationFrequency = 0.2f;

    private float timeSinceGameStart = float.MinValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeSinceGameStart > bunnyCreationFrequency)
        {
            timeSinceGameStart = Time.time;
            Instantiate(bunnyPrefab,
                world.LocalToWorldCoordinates(new Vector3(Random.Range(min_x, max_x), world.ySize + 5,
                    Random.Range(min_z, max_z))), Quaternion.Euler(0, Random.Range(0, 360), 0),
                bunnyCollection.transform);
        }
    }
}