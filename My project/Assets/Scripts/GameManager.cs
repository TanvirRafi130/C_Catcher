using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    [SerializeField] Transform pointsParents;



    [SerializeField] GameObject arena;
    // List<Transform> positions = new List<Transform>();
    private static GameManager instance;
    public static GameManager Instance => instance;

    void Awake()
    {
        if (instance == null) instance = this;
        // foreach (Transform item in pointsParents)
        // {
        //     positions.Add(item);

        // }
    }



    public Vector3 GeRandomLocation()
    {
        // return positions[UnityEngine.Random.Range(0, positions.Count - 1)].position;
        // Generate a random point within the arena plane's bounds
        if (arena == null)
        {
            arena = GameObject.Find("Arena"); // Ensure your plane is named "Arena"
        }
        if (arena == null)
        {
            Debug.LogError("Arena GameObject not found!");
            return Vector3.zero;
        }
        var plane = arena.GetComponent<Renderer>();
        if (plane == null)
        {
            Debug.LogError("Arena does not have a Renderer component!");
            return Vector3.zero;
        }
        Bounds bounds = plane.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.center.y; // Keep y at plane height
        return new Vector3(x, y, z);
    }

}
