using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject foodCollection;

    public CubeChunk world;
    public float foodGenerationFrequency = 1;

    private float lastFoodGenerationTime = float.MinValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastFoodGenerationTime > foodGenerationFrequency)
        {
            lastFoodGenerationTime = Time.time;
            float x = Random.Range(0, world.xSize - 1);
            float z = Random.Range(0, world.zSize - 1);
            float y = 0.5f;

            while (world.IsSet((int) x, (int) y + 1, (int) z))
            {
                y += 1.0f;
            }

            y += 1.0f;
            Vector3 carrotPos = world.LocalToWorldCoordinates(new Vector3((int) x, (int) y, (int) z));
            Instantiate(foodPrefab, carrotPos, Quaternion.identity, foodCollection.transform);
            //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = carrotPos;
        }
    }
}