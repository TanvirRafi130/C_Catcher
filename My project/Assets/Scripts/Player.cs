using UnityEngine;

public class Player : MonoBehaviour
{

    private static Player instance;
    public static Player Instance => instance;

    void Awake()
    {
        if(instance==null) instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
