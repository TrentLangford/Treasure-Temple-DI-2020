using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    /*
        The player script is responsible for all of the controls and behaviors of a character such as moving, jumping,
        and using a grabber. It is the largest class in the game.
     */

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

    public bool active = true;
    #endregion

    // handles some setup
    private void Awake()
    {
        // resets the inventory
        isFull = new bool[inventorySize];
        inventory = new GameObject[inventorySize];
        // allows the characters to move horizontally (a bit of a misleading name)
        rope = true;
        // whether or not the character is currently interacting with something, in this instance, a rope
        isInteracting = false;
        // resets all of the rope bounds
        ropeLbound = ropeRbound = ropeBbound = ropeTbound = 0;
        // resets the jump counter used for the double jump
        jumpsRemaining = maxJumps;
        didSubtract = false;

        // resets the velocity in case of any weirdness going on.
        this.playerRb.velocity = new Vector2(this.playerRb.velocity.x, 0);

        // gives player 1 a grabber in levels 2 & 3
        if (SceneManager.GetActiveScene().buildIndex > 0 && this.gameObject.name.Contains("1"))
        {
            GameObject newGrab = Instantiate(GrabberObj, this.transform.position, Quaternion.identity);
            newGrab.GetComponent<Grabber>().TriggerOnInteractMethod(this);
        }

        active = true;
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
        // respawns the character if they touch a collider tagged with the respawn tag
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
        
        // detects if the player can move in a given direction. 
        // Used to make sure the character does not keep moving even if they are at the end of a rope.
        canMoveLeft = rope && (ropeLbound == 0 || this.transform.position.x >= ropeLbound);
        canMoveDown = !rope && (ropeBbound == 0 || this.transform.position.y >= ropeBbound);
        canMoveRight = rope && (ropeRbound == 0 || this.transform.position.x <= ropeRbound);
        canMoveUp = !rope && (ropeTbound == 0 || this.transform.position.y <= ropeTbound);

        // if the player presses any of the input keys, we modify the move input variable to match
        // (given that they can move in that direction)
        if (Input.GetKey(left) && canMoveLeft)
            moveInput.x = -1;
        else if (Input.GetKey(right) && canMoveRight)
            moveInput.x = 1;
        else
            moveInput.x = 0;

        // unused sprint code, did not fit the gameplay well
        /*if (Input.GetKey(KeyCode.LeftShift) && sprint >= 0)
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
        }*/
        // Jumping code

        // fJumpPressedRemember is a timer which stores how long it has been since the player pressed their jump key.
        fJumpPressedRemember -= Time.deltaTime;
        // fGroundedRemember is a timer which stores how long it has been since the player has touched the ground
        fGroundedRemember -= Time.deltaTime;
        // we decrement these timers by the time since the last frame.

        // if the player has been not grounded in the last 0.2 seconds, and we did not subtract from the remaining jumps,
        // it is safe to assume that the character has jumped, so we subtract from the jump counter. this way you cannot "store" jumps.
        if (fGroundedRemember < 0 && !didSubtract)
        {
            jumpsRemaining--;
            didSubtract = true;
        }
        // if the opposite is true, it is safe to assume that we are grounded, and we can reset the jumpsRemaining and didSubract values to their
        // respective defaults.
        if (fGroundedRemember > 0 && didSubtract)
        {
            jumpsRemaining = maxJumps;
            didSubtract = false;
        }
        // a small balancing tweak to slow down an airbore character. This makes sure that the double jump character cannot
        // jump across gaps meant for the dash character.
        if (isAirborne && !isInteracting)
        {
            moveInput /= 1.5f;
        }
        // if we are not airborne, reset the fGroundedRemember timer
        if (!isAirborne)
        {
            fGroundedRemember = 0.2f;
        }
        // if we press the jump key, reset the fJumpPressedRemember timer
        if (Input.GetKeyDown(jump))
        {
            fJumpPressedRemember = 0.2f;
        }
        // if we:
        // - pressed the jump key within the last 0.2 seconds
        // - touched the ground within the last 0.2 seconds
        // - have available jumps remaining
        // - & not using a vertical rope
        // set our player's velocity to 50% of the current velocity on the x and our jump force variable on the y
        // and reset our timers and jump counter.
        if ((fJumpPressedRemember > 0) && ((fGroundedRemember > 0) || jumpsRemaining > 0) && rope && active)
        {
            fJumpPressedRemember = 0;
            jumpsRemaining--;
            fGroundedRemember = 0;
            playerRb.velocity = new Vector2(playerRb.velocity.x/2, jumpf);
        }
        // if we:
        // - are currently pressing the jump key
        // - cannot move horizontally
        // - are using a rope
        // - & are not at the top of rope
        // set our character's velocity to the rope speed y variable
        if (Input.GetKey(jump) && !rope && isInteracting && canMoveUp)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, ropeSpeed);
        }
        // if we:
        // - are currently pressing the down key
        // - cannot move horizontally
        // - are using a rope
        // - are not at the bottom of rope
        // - & the last condition was not true
        // set our character's velocity to the negative rope speed y variable
        else if (Input.GetKey(down) && !rope && isInteracting && canMoveDown)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -ropeSpeed);
        }
        // otherwise, if we cannot move horizontally and are using a rope, set our character's y velocity to 0
        else if (!rope && isInteracting)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
        }
            
        /*** Interact ***/
        // many parts in this section deal with UI. Due to time constraints, we had to disable the UI.

        // if we are not currently detecting any useable interactables in the area, then reset the ui's interaction text.
        if (current == null)
        {
            uiManager.EnableInteractText("", "", "", false);
        }
        // if we are detecting a useable interactable in the area:
        else
        {
            try
            {
                // if we are not using a rope, set the interaction text to:
                // name: the name of the interactable game object
                // interact text:  "to interact with"
                // interact key: our character's interact key
                // UI enabled: true
                if (!isInteracting)
                {
                    uiManager.EnableInteractText(current.gameObject.name, "to interact with ", interact, true);
                }
                // if we are using a rope, set the interaction text to:
                // name: the name of the interactable game object
                // interact text:  "to let go of"
                // interact key: our character's interact key
                // UI enabled: true
                if (isInteracting)
                {
                    uiManager.EnableInteractText(current.gameObject.name, "to let go of ", interact, true);
                    // try to get a reference to a rope and call the disinteract method (which we don't do, because we are not pressing
                    // the interact key. silly me!)
                    Rope local;
                    try { local = current.GetComponent<Rope>(); } catch { local = null; }
                    /*if (local != null)
                    {
                        local.TriggerOnDisinteractMethod(this);
                    }*/
                }
            }
            // catch blocks are required after a try block, but we don't need to handle any errors because we should only run into
            // NullReferenceExeption errors.
            catch { }
        }
        // if we are pressing the interact key:
        if (Input.GetKeyDown(interact))
        {
            // if there is a usable interactable in the area
            if (current != null)
            {
                // these are all of the different interactable types that exist in the game.
                TestInteract ts; // an interactable that was used in the early stages of the game for testing
                Rope rp;         // the rope
                QuestItem q;     // the quest items (components of the grabber)
                Key k;           // keys to the quest item chests
                CageKey ck;      // the key to the Lion's cage
                Chest ch;        // the quest item chests
                Grabber g;       // the grabber (when it's constructed it must be picked up)
                Cage c;          // the Lion's cage

                // try to get any available interactable scripts
                // if we find one, call its TriggerOnInteractMethod.
                // you could also potentially call the OnInteractMethod directly, but where's the fun in that?
                // we have to add these try/catch blocks for each different interactable, which would suck
                // if this game wasn't so small.
                try { k = current.GetComponent<Key>(); } catch { k = null; }
                if (k != null) k.TriggerOnInteractMethod(this);

                try { ck = current.GetComponent<CageKey>(); } catch { ck = null; }
                if (ck != null) ck.TriggerOnInteractMethod(this);

                try { ts = current.GetComponent<TestInteract>(); } catch { ts = null; }
                if (ts != null) ts.TriggerOnInteractMethod(this);

                try { rp = current.GetComponent<Rope>(); } catch { rp = null; }
                if (rp != null)
                {
                    // for the rope we have to discern whether we should call the Interact method or the Disinteract method.
                    // if we are not interacting with a rope currently, call the interact method.
                    if (!isInteracting)
                    {
                        rp.TriggerOnInteractMethod(this);
                        Debug.Log("Interacted");
                    }
                    // if we are interacting with a rope, we need to call the disinteract method.
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

            }
            // count how many quest items are in our inventory.
            questCount = 0;
            foreach (GameObject item in inventory)
            {
                QuestItem q;
                try { q = item.GetComponent<QuestItem>(); } catch { q = null; }
                if (q != null && item != null) questCount++;
            }
            // if we have all 3 quest items, remove all of the quest items and spawn in a grabber
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

        // Dashing
        // the dash countdown timer prevents the dash character from dashing consecutively for a certain length of time.
        // it also determines the length of the dashes
        dashCountdown -= Time.deltaTime;
        // if we are pressing the special key and our character is the dash character 
        // and we have not jumped for a length of time, reset the dash countdown timer
        if (Input.GetKeyDown(special) && dashSpecial && dashCountdown <= -timeBtwDashes)
        {
            dashCountdown = dashDuration;
        }
        // if the dash countdown timer is greater than zero, that means we should be dashing
        // so we multiply the move input by the dash speed
        // one quirk of this is that you must be moving or the dash will not affect you.
        if (dashCountdown >= 0)
        {
            moveInput *= dashSpeed;
        }

        // the Grabber
        // if we pressed the left mouse button:
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // check if we have a grabber in our inventory
            foreach (GameObject item in inventory)
            {
                Grabber g;
                try { g = item.GetComponent<Grabber>(); } catch { g = null; }
                // if we do have a grabber:
                if (g != null)
                {
                    // do a quick raycast to see if we clicked on anything
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    // if we clicked on something:
                    if (hit.collider != null)
                    {
                        // if we clicked on the Lion's cage key, call the key's select method
                        // this will move the key to our characters
                        if (hit.collider.CompareTag("CageKey")) hit.collider.GetComponent<CageKey>().Select(this);
                        // if we clicked on the boss, call his Die method
                        if (hit.collider.CompareTag("Boss"))
                        {
                            // looks like you've found one of my silly debug messages. Thank you for reading my code :D
                            Debug.Log("Change da world my final message goodbye -Boss");
                            hit.collider.GetComponent<Boss>().Die();
                        }
                    }

                }
            }
        }

        // if we press the escape key, quit the game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    // called whenever the interactable detector finds an interactable
    public void InteractableFind(Interactable item, bool enter)
    { 
        if (enter) current = item;
        else current = null;
        //if (!uiManager.isAlreadyActive && enter) uiManager.EnableInteractText(item.gameObject.name, "to interact with ", interact, true);
        //else uiManager.EnableInteractText(item.gameObject.name, "", interact, false);
    }

    // FixedUpdate is called at a fixed interval so that people with higher framerates will not move faster
    private void FixedUpdate()
    {
        /*** player movement ***/
        // apply our character movement

        // if we are using a rope, set our position to:
        // - the rope's y postion if it is horizontal
        // - the rope's x position if it is vertical
        if (isInteracting)
        {
            this.transform.position = rope ? new Vector2(this.transform.position.x, ropeY) : new Vector2(ropeX, this.transform.position.y);
        }
        // apply our moveinput vector
        if (active)
        {
            playerRb.velocity = new Vector2(moveInput.x * speed, playerRb.velocity.y);
        }
        
    }

}
