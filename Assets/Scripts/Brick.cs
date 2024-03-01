using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject brick;
    private bool hasBrick = true;
    void Start()
    {
        
    }
    public bool RemoveBrick()
    {
        if (!hasBrick) return false;
        hasBrick = false;
        Destroy(brick);
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
