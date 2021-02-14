using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    #region variables
    public Rigidbody2D playerRb;
    public float speed;
    public float sprintMod;
    private float sprint;
    public float maxSprint;
    public float jumpf;
    [SerializeField] bool isGrounded;
    public bool isAirborne;
    public bool isInteracting;
    private Vector2 moveInput;
    public float fJumpPressedRemember = 0.2f;
    public float fGroundedRemember = 0.2f;

    public string jump;
    public string down;
    public string left;
    public string right;
    public string interact;
    public string special;

    public LayerMask interactableLayer;
    public float detectInterRad;
    public UIManager uiManager;
    public Interactable current;

    public float ropeSpeed;
    public bool rope;
    public float ropeLbound;
    public float ropeRbound;
    public float ropeBbound;
    public float ropeTbound;
    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canMoveDown = true;
    public bool canMoveUp = true;
    public float ropeY;
    public float ropeX;

    public bool dashSpecial;
    public float dashSpeed;
    public float timeBtwDashes;
    public float dashDuration;
    [SerializeField] float dashCountdown;

    public bool doubleJumpSpecial;
    public int maxJumps;
    [SerializeField] int jumpsRemaining;
    private bool didSubtract;

    public int questCount = 0;
    public int inventorySize;
    public bool[] isFull;
    public GameObject[] inventory;
    public GameObject GrabberObj;
    public Grabber GrabberSrc;
    #endregion

    private void Awake()
    {
        isFull = new bool[inventorySize];
        inventory = new GameObject[inventorySize];
        rope = true;
        isInteracting = false;
        ropeLbound = ropeRbound = ropeBbound = ropeTbound = 0;
        jumpsRemaining = maxJumps;
        didSubtract = false;
        this.playerRb.velocity = new Vector2(this.playerRb.velocity.x, 0);
        if (SceneManager.GetActiveScene().buildIndex > 0 && this.gameObject.name.Contains("1"))
        {
            GameObject newGrab = Instantiate(GrabberObj, this.transform.position, Quaternion.identity);
            newGrab.GetComponent<Grabber>().TriggerOnInteractMethod(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*** player movement ***/
        // collisions
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            isGrounded = true;
            didSubtract = false;
            jumpsRemaining = maxJumps;
            isAirborne = false;
        }
        if (collision.collider.CompareTag("Respawn")) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        /*** player movement ***/
        if (collision.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Environment"))) isGrounded = false;
    }


    void Update()
    {
        /*** player movement **/
        // sprinting
        canMoveLeft = rope && (ropeLbound == 0 || this.transform.position.x >= ropeLbound);
        canMoveDown = !rope && (ropeBbound == 0 || this.transform.position.y >= ropeBbound);
        canMoveRight = rope && (ropeRbound == 0 || this.transform.position.x <= ropeRbound);
        canMoveUp = !rope && (ropeTbound == 0 || this.transform.position.y <= ropeTbound);
        if (Input.GetKey(left) && canMoveLeft)
            moveInput.x = -1;

        else if (Input.GetKey(right) && canMoveRight)
            moveInput.x = 1;
        else
            moveInput.x = 0;

        if (Input.GetKey(KeyCode.LeftShift) && sprint >= 0)
        {
            moveInput *= sprintMod;
            sprint -= Time.deltaTime;
        }
        // Uh oh, you sprinted too much!
        else if (!Input.GetKey(KeyCode.LeftShift) && sprint <= 1)
        {
            moveInput = moveInput / sprintMod;
        }
        // Recharging sprint
        if (!Input.GetKey(KeyCode.LeftShift) && sprint <= maxSprint)
        {
            sprint += Time.deltaTime;
        }
        // jump!!
        fJumpPressedRemember -= Time.deltaTime;
        fGroundedRemember -= Time.deltaTime;
        if (fGroundedRemember < 0 && !didSubtract)
        {
            jumpsRemaining--;
            didSubtract = true;
        }
        if (fGroundedRemember > 0 && didSubtract)
        {
            jumpsRemaining = maxJumps;
            didSubtract = false;
        }
        if (isAirborne && !isInteracting)
        {
            moveInput /= 1.5f;
        }
        if (!isAirborne)
        {
            fGroundedRemember = 0.2f;
        }
        if (Input.GetKeyDown(jump))
        {
            fJumpPressedRemember = 0.2f;
        }
        if ((fJumpPressedRemember > 0) && ((fGroundedRemember > 0) || jumpsRemaining > 0) && rope)
        {
            fJumpPressedRemember = 0;
            jumpsRemaining--;
            fGroundedRemember = 0;
            playerRb.velocity = new Vector2(playerRb.velocity.x/2, jumpf);
        }
        if (Input.GetKey(jump) && !rope && isInteracting && canMoveUp)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, ropeSpeed);
        } 
        else if (Input.GetKey(down) && !rope && isInteracting && canMoveDown)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -ropeSpeed);
        }
        else if (!rope && isInteracting)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
        }
            
        /*** Interact ***/
        if (current == null)
        {
            uiManager.EnableInteractText("", "", "", false);
        }
        else
        {
            try
            {
                if (!isInteracting)
                {
                    uiManager.EnableInteractText(current.gameObject.name, "to interact with ", interact, true);

                }
                if (isInteracting)
                {
                    uiManager.EnableInteractText(current.gameObject.name, "to let go of ", interact, true);
                    Rope local;
                    try { local = current.GetComponent<Rope>(); } catch { local = null; }
                    /*if (local != null)
                    {
                        local.TriggerOnDisinteractMethod(this);
                    }*/
                }
            }
            catch { }
        }
        if (Input.GetKeyDown(interact))
        {
            
            if (current != null)
            {
                TestInteract ts;
                Rope rp;
                QuestItem q;
                Key k;
                CageKey ck;
                Chest ch;
                Grabber g;
                Cage c;
                // try to get any available interactable scripts
                try { k = current.GetComponent<Key>(); } catch { k = null; }
                if (k != null) k.TriggerOnInteractMethod(this);

                try { ck = current.GetComponent<CageKey>(); } catch { ck = null; }
                if (ck != null) ck.TriggerOnInteractMethod(this);

                try { ts = current.GetComponent<TestInteract>(); } catch { ts = null; }
                if (ts != null) ts.TriggerOnInteractMethod(this);

                try { rp = current.GetComponent<Rope>(); } catch { rp = null; }
                if (rp != null)
                {
                    if (!isInteracting)
                    {
                        rp.TriggerOnInteractMethod(this);
                        Debug.Log("Interacted");
                    }
                    else if (isInteracting)
                    {
                        rp.TriggerOnDisinteractMethod(this);
                        Debug.Log("Stopped interacting");
                    }
                }
                try { q = current.GetComponent<QuestItem>(); } catch { q = null; }
                if (q != null) q.TriggerOnInteractMethod(this);

                try { ch = current.GetComponent<Chest>(); } catch { ch = null; }
                if (ch != null) ch.TriggerOnInteractMethod(this);

                try { g = current.GetComponent<Grabber>(); } catch { g = null; }
                if (g != null) g.TriggerOnInteractMethod(this);

                try { c = current.GetComponent<Cage>(); } catch { c = null; }
                if (c != null) c.TriggerOnInteractMethod(this);
                // try to call any deinteract methods

            }
            questCount = 0;
            foreach (GameObject item in inventory)
            {
                QuestItem q;
                try { q = item.GetComponent<QuestItem>(); } catch { q = null; }
                if (q != null && item != null) questCount++;
            }
            if (questCount == 3)
            {
                int i = 0;
                foreach (GameObject item in inventory)
                {
                    QuestItem q;
                    try { q = item.GetComponent<QuestItem>(); } catch { q = null; }
                    if (q != null)
                    {
                        inventory[i] = null;
                        isFull[i] = false;
                    }
                    i++;
                }
                Instantiate(GrabberObj, this.transform.position, Quaternion.identity);
            }
        }
        dashCountdown -= Time.deltaTime;
        if (Input.GetKeyDown(special) && dashSpecial && dashCountdown <= -timeBtwDashes)
        {
            dashCountdown = dashDuration;
        }
        if (dashCountdown >= 0)
        {
            moveInput *= dashSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach (GameObject item in inventory)
            {
                Grabber g;
                try { g = item.GetComponent<Grabber>(); } catch { g = null; }
                if (g != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("CageKey")) hit.collider.GetComponent<CageKey>().Select(this);
                        if (hit.collider.CompareTag("Boss"))
                        {
                            Debug.Log("Change da world my final message goodbye -Boss");
                            hit.collider.GetComponent<Boss>().Die();
                        }
                    }

                }
            }
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
        
    }

    public void InteractableFind(Interactable item, bool enter)
    { 
        if (enter) current = item;
        else current = null;
        //if (!uiManager.isAlreadyActive && enter) uiManager.EnableInteractText(item.gameObject.name, "to interact with ", interact, true);
        //else uiManager.EnableInteractText(item.gameObject.name, "", interact, false);
    }
    private void FixedUpdate()
    {
        /*** player movement ***/
        if (isInteracting)
        {
            this.transform.position = rope ? new Vector2(this.transform.position.x, ropeY) : new Vector2(ropeX, this.transform.position.y);
        }
        playerRb.velocity = new Vector2(moveInput.x * speed, playerRb.velocity.y);
    }

}
