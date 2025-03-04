using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    [SerializeField] Transform pointsParents;




    List<Transform> positions = new List<Transform>();
    private static GameManager instance;
    public static GameManager Instance => instance;

    void Awake()
    {
        if (instance == null) instance = this;
        foreach (Transform item in pointsParents)
        {
            positions.Add(item);

        }
    }



    public Vector3 GeRandomLocation()
    {
        return positions[UnityEngine.Random.Range(0, positions.Count - 1)].position;
    }

}
