using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{

    [Serializable]
    public struct LevelData
    {
        public ChickenType chickenType;
        public int chickenCount;

    }

    public Action<ChickenType> onCatchingTarget;

    [SerializeField] List<LevelData> levelRequirements;
    GameObject arena;
    private static GameManager instance;
    public static GameManager Instance => instance;
    [SerializeField, ReadOnly] int totalLevelTarget = 0;

    void Awake()
    {
        if (instance == null) instance = this;

    }
    private void Start()
    {
        arena = FindFirstObjectByType<Plane_For_Chickens>().gameObject;
        totalLevelTarget = 0;
        Ai[] allGameObjects = FindObjectsByType<Ai>(FindObjectsSortMode.None);
        totalLevelTarget = allGameObjects.Length;
        onCatchingTarget += OnScore;
    }

    void OnScore(ChickenType chickenType)
    {
        totalLevelTarget--;
        if (totalLevelTarget <= 0)
        {
            Debug.Log("All targets caught!");
            // Handle level completion logic here
        }
    }



    public Vector3 GeRandomLocation(Vector3 objectPos)
    {
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
        float y = bounds.center.y; // Keep y at plane height

        // Generate a random point within bounds
        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);
        Vector3 randomPoint = new Vector3(x, y, z);

        // Calculate direction from objectPos to randomPoint
        Vector3 direction = randomPoint - objectPos;

        // Ensure the direction is valid (not pointing in the same general direction)
        // We'll try a maximum number of times to prevent infinite loops
        int maxAttempts = 100;
        int attempts = 0;

        float minDistanceFromPlayer = 5f;
        while (
            Vector3.Dot(objectPos.normalized, direction.normalized) > 0f
            && attempts < maxAttempts
            || Vector3.Distance(Player.Instance.gameObject.transform.position, randomPoint) < minDistanceFromPlayer
        )
        {
            x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);
            randomPoint = new Vector3(x, y, z);
            direction = randomPoint - objectPos;
            attempts++;
        }

        return randomPoint;

    }
}
