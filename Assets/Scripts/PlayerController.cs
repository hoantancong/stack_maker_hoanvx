using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform playerBodyTrans;
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask brickHoleLayer;
    [SerializeField] private LayerMask winGateLayer;
    [SerializeField] private LayerMask winChestLayer;
    [SerializeField] private Transform brickContainer;
    //prefab
    [SerializeField] private GameObject brickPrefab;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 startTouchPosition;
    private bool isDragging = false;
    //check to skip already checked brick
    private BrickHole oldBrick = null;
    private bool justTurn = false;
    private int brickCount = 0;
    //animation
    [SerializeField] private Animator playerAnimator;
  
    private void Awake()
    {
 
    }
    public void OnInit()
    {
        moveDirection = Vector3.zero;
        playerAnimator.ResetTrigger("win");
        playerAnimator.Play("Idle");

    }
    //get swipe action to change direction of player
    private void handleInput()
    {

        //if player is already move... skip
        if (moveDirection != Vector3.zero) return;
        //
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // Lưu điểm bắt đầu chạm hoặc nhấn
            startTouchPosition = Input.GetMouseButtonDown(0) ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if (isDragging)
            {
                Vector2 endTouchPosition = Input.GetMouseButtonUp(0) ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
                Vector2 direction = endTouchPosition - startTouchPosition;

                // Normalize
                direction.Normalize();

                // determine direction
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    moveDirection = direction.x > 0 ? Vector3.right : Vector3.left;
                }
                else
                {
                    moveDirection = direction.y > 0 ? Vector3.up : Vector3.down; // 
                }
                justTurn = true;
                isDragging = false; 
            }
        }
    }
    private void CheckWall(Vector3 direction)
    {
        float distanceToCheck = 0.5f;
        RaycastHit hit;
        Debug.DrawRay(transform.position, direction, Color.green, 10.0f, false);
        if (Physics.Raycast(transform.position, direction, out hit, distanceToCheck, wallLayer))
        {
            // Wall detected in the direction of movement
            moveDirection = Vector3.zero;
       
        }

    }
    private void MoveCharacter()
    {

        Vector3 directionVector = Vector3.zero;

        if (moveDirection == Vector3.up)
        {
            directionVector = Vector3.forward;
            //rotate 
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveDirection == Vector3.down)
        {
            directionVector = Vector3.back;
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else if (moveDirection == Vector3.left)
        {
            directionVector = Vector3.left;
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (moveDirection == Vector3.right)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
            directionVector = Vector3.right; 
        }

        if (directionVector != Vector3.zero)
        {
            // move character
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + directionVector, moveSpeed * Time.deltaTime);

            // check wall
            CheckWall(directionVector);
            //check out of brick
            CheckOutOfBrick(directionVector);

        }


    }
    //check brick and unbrick

    private void CheckBrick()
    {
        float distanceToCheck = 2f;
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.cyan, 10f, false);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceToCheck, brickLayer))
        {


            if (hit.collider != null)
            {
                Brick checkBrick = hit.collider.gameObject.GetComponent<Brick>();
                if (checkBrick.RemoveBrick()==true)
                {
                    //add new to player brick
                    AddBrick();
                }
                oldBrick = null;
            }



        }


    }
    private void CheckOutOfBrick(Vector3 direction)
    {
        if (brickCount <= 0)
        {
            Debug.DrawRay(transform.position + transform.forward*.5f, Vector3.down, Color.yellow,10f,false);
            if (Physics.Raycast(transform.position + transform.forward*.5f, Vector3.down, out RaycastHit unbrickForward, 2f, brickHoleLayer))
            {
                BrickHole brickHoleForward = unbrickForward.collider.GetComponent<BrickHole>();
                if (!brickHoleForward.HasBrick())
                {
                    moveDirection = Vector3.zero;
                    Debug.Log(transform.position);
                }

            }
        }
    }

    private void CheckBrickHole()
    {
        bool hasUnbrickForward = false;
        //check forward if there is unbrick ahead? when player just stop and turn
        if (justTurn)
        {
            justTurn = false;
            Debug.DrawRay(transform.position + transform.forward, Vector3.down, Color.red, 5f, false);
            if (Physics.Raycast(transform.position + transform.forward*0.5f, Vector3.down, out RaycastHit unbrickForward, 2f, brickHoleLayer))
            {
                BrickHole brickHoleForward = unbrickForward.collider.GetComponent<BrickHole>();
                hasUnbrickForward = brickHoleForward.HasBrick();
            }
        }


        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit unbrick,2f, brickHoleLayer))
        {
            BrickHole brickHole = unbrick.collider.GetComponent<BrickHole>();
            if (hasUnbrickForward)
            {
                Debug.Log("Collected");
                //next has brick => collect brick that player stand in
                brickHole.RemoveBick();
                AddBrick();
                oldBrick = brickHole;
            }
            else
            {
                //just normal go

                //just check only one per brick
                if (oldBrick?.GetComponent<BrickHole>() == brickHole) return;
                //
                oldBrick = brickHole;
                if (brickHole.HasBrick())
                {
                    brickHole.RemoveBick();
                    AddBrick();
                }
                else
                {

                    RemoveBrick();
                    brickHole.AddBrick();
                }
            }
           
        }
        
    }
    private void CheckFinish()
    {
        //check pass the gate and active confetti
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit confettiHit, 2f, winGateLayer))
        {
            WinPos winPos = confettiHit.collider.GetComponentInParent<WinPos>();
            winPos.ActiveConfetti();
            Debug.Log("Confetti");
            confettiHit.collider.GetComponent<BoxCollider>().enabled = false;
        }
        if (Physics.Raycast(transform.position,transform.forward, out RaycastHit chestHit, 0.5f, winChestLayer))
        {
            //win level
            WinPos winPos = chestHit.collider.GetComponentInParent<WinPos>();
            winPos.OpenChest();
            moveDirection = Vector3.zero;
            chestHit.collider.GetComponent<BoxCollider>().enabled = false;
            //
            //
            StartCoroutine(VictorySequence());
           
        }
    }

    private IEnumerator VictorySequence()
    {
        //paused state
        GameManager.Instance.GameState = GameState.Pause;
        //1. Remove remain brick effect
        int count = brickCount;
        Debug.Log("Count"+count);
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.05f);
            RemoveBrick();
        }
        //2. play animated effect
        yield return new WaitForSeconds(0.2f);
        playerAnimator.SetTrigger("win");
        //3. finish
        yield return new WaitForSeconds(2.2f);
        GameManager.Instance.WinLevel(count);

    }

    private void AddBrick()
    {
        GameObject newBrick = Instantiate(brickPrefab);
        brickCount++;
        newBrick.transform.SetParent(brickContainer);
        newBrick.transform.position = new Vector3(brickContainer.position.x, brickContainer.position.y + 0.3f * brickContainer.childCount, brickContainer.transform.position.z);
        //lift body up
        playerBodyTrans.position += Vector3.up * 0.3f;
    }
    private void RemoveBrick()
    {
        //remove one brick
        Destroy(brickContainer.GetChild(brickContainer.childCount - 1).gameObject);
        brickCount--;
        playerBodyTrans.position -= Vector3.up * 0.3f;

    }
    private void ClearBrick()
    {
        //remove all
    }
    // Update is called once per frame


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState != GameState.GamePlay) return;
        handleInput();
        MoveCharacter();
        CheckBrick();
        CheckBrickHole();
        CheckFinish();
        /*if (brickContainer.childCount > 0)
        {
            CheckBrickHole();
        }*/
    }
/*    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState != GameState.GamePlay) return;
        handleInput();
        MoveCharacter();
        CheckBrick();
        CheckBrickHole();
        CheckFinish();
    }*/
}
