using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class ImprovisedPlayerScript : NetworkBehaviour
{
    //Name
    public NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>(
        "", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public UsernameBillboarding usernameText;
    //Audio
    public AudioSource source;
    public PlayerAudio playerAudio;

    //Camera Movement
    public float yaw;
    public float pitch;
    public float mouseSensitivity;
    public Camera playercamera;

    //Player Movement
    public float extraGravity;
    public float dashDistance;

    private Vector3 direction;
    private bool isMoving;
    private Animator animator;
    private Coroutine dashCorou;
    private Coroutine slamCorou;
    private Coroutine atkCorou;

    //Etcetera
    public ViewModel viewModel;
    public Projectile dodgeballPrefab;

    private Stats playerStats;
    private ShopUI shop;
    private PlayerCanvas playerCanvas;
    private Rigidbody body;
    private bool isGrounded;
    private bool isDashing;
    private bool swingCooldown;

    public Vector3 spawnpoint { get; set; }

    //Calculations
    private Vector3 spawnPos;
    private Vector3 cameraForward;
    private bool isTyping = false;

    //Attack
    public Attack attack;
    public WeaponController weaponController;

    public override void OnNetworkSpawn()
    {
        //Basically this runs on other players, not you
        if (!IsOwner)
        {
            playercamera.enabled = false;
            playercamera.gameObject.GetComponent<AudioListener>().enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }


    public void SetName()
    {
        usernameText.SetName(playerName.Value.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<Stats>();
        body = GetComponent<Rigidbody>();
        swingCooldown = true;

        if (IsOwner)
        {
            shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopUI>();
            shop.SetPlayer(this);
            playerCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PlayerCanvas>();
            playerCanvas.SetHealthBar(playerStats.GetHealth());
            playerCanvas.UpdateGoldCounter(playerStats.GetCoins());

            Cursor.lockState = CursorLockMode.Locked;

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if(playerCanvas != null) Chat();

        if (isTyping) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }

        if (playerStats.GetHealth() <= 0)
        {
            Respawn();
            return;
        }

        CameraControl();
        Move();
        PlayerAttackActivation();
        Jump();
    }

    void CameraControl()
    {
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");

        // Clamp pitch thing idk
        pitch = Mathf.Clamp(pitch, -80, 80);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playercamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    void Move()
    {
        direction = new Vector3(0, 0, 0);
        isMoving = false;

        if (Input.GetKey("w"))
        {
            direction += transform.forward;
        }
        if (Input.GetKey("a"))
        {
            direction -= transform.right;
        }
        if (Input.GetKey("s"))
        {
            direction -= transform.forward;
        }
        if (Input.GetKey("d"))
        {
            direction += transform.right;
        }

        if (direction.x != 0 || direction.y != 0 || direction.z != 0) isMoving = true;
        animator.SetBool("isRunning", isMoving);

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && isGrounded)
        {
            if (dashCorou != null) StopCoroutine(dashCorou);

            isDashing = true;
            dashCorou = StartCoroutine(Dashing(direction));
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isGrounded)
        {
            if (slamCorou != null) StopCoroutine(slamCorou);

            isDashing = true;
            slamCorou = StartCoroutine(Slam());
        }

            if (!isDashing) transform.position += (direction.normalized * playerStats.GetSpeed() * Time.deltaTime);
    }

    IEnumerator Dashing(Vector3 direction)
    {
        body.AddForce(direction.normalized * dashDistance * 200);
        yield return new WaitForSeconds(0.3f);
        body.velocity *= 0.2f;
        body.angularVelocity *= 0.2f;

        isDashing = false;
        yield return new WaitForSeconds(0.4f);

    }

    IEnumerator Slam()
    {
        body.AddForce(new Vector3(0,-1000,0));
        
        while (!isGrounded)
        {
            yield return null;
            body.AddForce(new Vector3(0, -50, 0));
        }

        body.velocity *= 0f;
        body.angularVelocity *= 0f;

        isDashing = false;
        yield return new WaitForSeconds(1f);

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerAudio.PlayAudio("jump");
            isGrounded = false;
            body.velocity += new Vector3(0, playerStats.GetJumpHeight(), 0);
        }
    }

    void PlayerAttackActivation()
    {
        if (Input.GetMouseButtonDown(0) && swingCooldown)
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerAudio.PlayAudio("attack");
            if (atkCorou != null) { StopCoroutine(atkCorou); }
            atkCorou = StartCoroutine(AttackCoroutine());
        }
        else if (Input.GetMouseButtonDown(1)) //Right click to fire
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerAudio.PlayAudio("projectile");

            cameraForward = playercamera.transform.forward;
            spawnPos = cameraForward * 3 + transform.position;
            spawnPos.y += 0.5f;

            ProjectileServerRpc(spawnPos, cameraForward);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ProjectileServerRpc(Vector3 spawnPos, Vector3 cameraForward)
    {
        Projectile projectile = Instantiate(dodgeballPrefab, spawnPos, Quaternion.identity);
        projectile.GetComponent<NetworkObject>().Spawn();
        projectile.Fire(cameraForward);
    }

    IEnumerator AttackCoroutine()
    {
        swingCooldown = false;
        viewModel.PlayAttackAnim();
        yield return new WaitForSeconds(0.1f);
        attack.Activate(playerStats.GetAttack() + weaponController.weapon.attackPower);
        yield return new WaitForSeconds(0.3f);
        attack.Deactivate();
        yield return new WaitForSeconds(0.45f);
        swingCooldown = true;
        
    }

    void Chat()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isTyping = playerCanvas.ToggleChat(playerName.Value.ToString());
        }
    }

    void FixedUpdate()
    {
        body.AddForce(Vector3.down * extraGravity);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyAI>())
        {
            playerAudio.PlayAudio("damage");
            if (IsOwner)
            {
                EnemyAI zombie = collision.gameObject.GetComponent<EnemyAI>();
                Vector3 direction = transform.position - zombie.transform.position;
                body.AddForce(direction.normalized * 10, ForceMode.VelocityChange);

                playerStats.TakeDamage(zombie.enemyStats.GetAttack());
                playerCanvas.SetHealthBar(playerStats.GetHealth());
            }
        }
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    public bool GetGroundedState()
    {
        return isGrounded;
    }
    public bool GetIsMovingState()
    {
        return isMoving;
    }

    public void Heal(float heal)
    {
        if (!IsOwner) return;
        if (playerStats.GetHealth() <= 100-heal)
        {
            playerStats.TakeDamage(heal * -1);
        }
        else
        {
            playerStats.SetHealth(100);
        }
        playerCanvas.SetHealthBar(playerStats.GetHealth());
    }

    public void AddGold(int coin)
    {
        if (!IsOwner) return;
        playerStats.AddCoins(coin);
        playerCanvas.UpdateGoldCounter(playerStats.GetCoins());
    }

    public void ShowShop()
    {
        shop.ShowShop();
    }

    public void HideShop()
    {
        shop.HideShop();
    }

    public int GetCoins()
    {
        return playerStats.GetCoins();
    }

    public void SetCoins(int gold)
    {
        playerStats.SetCoins(gold);
        playerCanvas.UpdateGoldCounter(playerStats.GetCoins());
    }

    public void Respawn()
    {
        transform.position = spawnpoint;
        playerStats.ResetHealth();

        int coinCount = playerStats.GetCoins();

        playerStats.SetCoins(coinCount / 2);
        playerCanvas.SetHealthBar(playerStats.GetHealth());
        playerCanvas.UpdateGoldCounter(playerStats.GetCoins());
    }
}
