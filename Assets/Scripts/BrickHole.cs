using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BRICK_HOLE_STATE
{
    brick= 1,
    unbrick=0
}
public class BrickHole : MonoBehaviour
{
    // Start is called before the first frame update
    private BRICK_HOLE_STATE brickState = BRICK_HOLE_STATE.unbrick;
    [SerializeField ]private GameObject brick;

    public void AddBrick()
    {
        brickState = BRICK_HOLE_STATE.brick;
        brick.SetActive(true);
    }
    public void RemoveBick()
    {
        brick.SetActive(false);
        brickState = BRICK_HOLE_STATE.unbrick;

    }
    public bool HasBrick()
    {
        return brickState == BRICK_HOLE_STATE.brick;
    }
    // Update is called once per frame
}
