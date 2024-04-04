using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAI : NetworkBehaviour, Attackable
{
    private bool isActive;
    private Coroutine dyingCorou;

    public float forceAmount;
    public Stats enemyStats;
    public AudioClip tookDamageClip;
    public AudioSource source;
    public Material hitMat;
    public Material atlas;
    public SkinnedMeshRenderer mesh;
    public GameObject goldPrefab;

    private Animator animator;
    private ImprovisedPlayerScript target;
    private Vector3 direction;
    private Vector3 impactAngle;
    private Coroutine StunCoroutine;
    private bool isStunned  = false;   

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        animator = GetComponent<Animator>();
        isActive = true;
        StartCoroutine(UpdateTargetsRoutine());
        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<ImprovisedPlayerScript>();
    }

    void SearchForClosestPlayer(IList playerlist)
    {
        float closestDistance = 999999;

        foreach (ImprovisedPlayerScript plr in playerlist)
        {
            float distance = Vector3.Distance(plr.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = plr;
            }
        }
    }

    IEnumerator UpdateTargetsRoutine()
    {
        while (true) 
        {
            List<ImprovisedPlayerScript> playerlist = GameManager.instance.playerList;

            yield return new WaitForSeconds(1);

            if (playerlist == null) continue;

            SearchForClosestPlayer(playerlist);
            

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {                
        if (!IsServer) return;

        if (target == null) return;

        if (!isActive || isStunned) return;

        direction = new Vector3(0,0,0);

        if (enemyStats.GetHealth() <= 0)
        {
            isActive = false;
            if (dyingCorou != null) { StopCoroutine(dyingCorou); }
            dyingCorou = StartCoroutine(Degde());
        }

        if (isActive)
        {
            direction = target.transform.position - transform.position;
            transform.position += (direction.normalized * enemyStats.GetSpeed() * Time.deltaTime);
            if (direction.normalized.x > 0 || direction.normalized.z > 0 || direction.normalized.y > 0) animator.SetBool("isWalking", true); else { animator.SetBool("isWalking", false); }
        }
    }

    IEnumerator Degde()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(goldPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (target == null) return;
        //This line stores the rotation toward the player.
        Vector3 rotation = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    private void OnCollisionStay(Collision other)
    {
        
        if (other.gameObject.tag == "Player" && isActive && target)
        {
            /*
            Rigidbody body = other.gameObject.GetComponent<Rigidbody>();
            Vector3 liftOffset = new Vector3(0, 0.2f, 0);

            impactAngle = target.transform.position - transform.position;

            body.AddRelativeForce((impactAngle + liftOffset) * forceAmount);
            */
        }
        
    }

    public void Stun(float time)
    {
        if (StunCoroutine != null) { StopCoroutine(StunCoroutine); }
        StartCoroutine(StunTimer(time));
        animator.SetBool("isWalking", false);
    }

    IEnumerator StunTimer(float time)
    {
        isStunned = true;
        yield return new WaitForSeconds(time);
        isStunned = false;
    }

    public void Attacked(float playerForceAmount, Vector3 forceDirection, float attackPower)
    {
        //take damage loll
        enemyStats.TakeDamage(attackPower);
        source.PlayOneShot(tookDamageClip);

        StartCoroutine(AttackedCoroutine());

        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        Vector3 liftOffset = new Vector3(0, 0.2f, 0);
        body.AddForce((forceDirection + liftOffset) * playerForceAmount);
    }

    IEnumerator AttackedCoroutine()
    {
        mesh.material = hitMat;
        yield return new WaitForSeconds(0.2f);
        mesh.material = atlas;
    }
}
