using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Stats : NetworkBehaviour
{
    public ImprovisedPlayerScript player;
    private float rhealth;
    private float rspeed;
    private float rjumpHeight;
    private float rattackPower;

    [SerializeField] private NetworkVariable<float> health = new NetworkVariable<float>(100,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    );
    [SerializeField]
    private NetworkVariable<int> coins = new NetworkVariable<int>(100,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    );
    [SerializeField] private NetworkVariable<float> speed = new NetworkVariable<float>(5,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    );
    [SerializeField] private NetworkVariable<float> jumpHeight = new NetworkVariable<float>(12,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    );
    [SerializeField] private NetworkVariable<float> attackPower = new NetworkVariable<float>(5,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    );

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            health.OnValueChanged += OnHealthValueChanged;
            speed.OnValueChanged += OnSpeedValueChanged;
            jumpHeight.OnValueChanged += OnJumpHeightValueChanged;
            attackPower.OnValueChanged += OnAttackPowerValueChanged;

        }
    }

    private void OnHealthValueChanged(float previous, float newValue)
    {
        if (health.Value <= 0)
        {
            //player.gameOverFunctionSkillIssueInMyOpinionActually
        }
    }

    private void OnSpeedValueChanged(float previous, float newValue)
    {

    }

    private void OnJumpHeightValueChanged(float previous, float newValue)
    {

    }

    private void OnAttackPowerValueChanged(float previous, float newValue)
    {

    }

    public int GetCoins()
    {
        return coins.Value;
    }
    public void AddCoins(int coins)
    {
        this.coins.Value += coins;
    }

    public void SetCoins(int coins)
    {
        this.coins.Value = coins;
    }

    public float GetHealth()
    {
        return health.Value;
    }

    public void TakeDamage(float damage)
    {
        health.Value -= damage;
    }

    public float GetSpeed()
    {
        return speed.Value;
    }

    public void SetSpeed(float speed)
    {
        this.speed.Value = speed;
    }

    public float GetAttack()
    {
        return attackPower.Value;
    }

    public void SetAttack(float attackPower)
    {
        this.attackPower.Value = attackPower;
    }

    public float GetJumpHeight()
    {
        return jumpHeight.Value;
    }

    public void SetJumpHeight(float jumpHeight)
    {
        this.jumpHeight.Value = jumpHeight;
    }

    public void ResetHealth()
    {
        health.Value = rhealth;
    }

    public void ResetSpeed()
    {
        speed.Value = rspeed;
    }

    public void ResetJumpHeight()
    {
        jumpHeight.Value = rjumpHeight;
    }

    public void ResetAttack()
    {
        attackPower.Value = rattackPower;
    }

    public void SetHealth(float health)
    {
        this.health.Value = health;
    }

    // Start is called before the first frame update
    void Start()
    {
        rhealth = health.Value;
        rspeed = speed.Value;
        rjumpHeight = jumpHeight.Value;
        rattackPower = attackPower.Value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
