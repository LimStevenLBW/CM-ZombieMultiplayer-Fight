using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewModel : MonoBehaviour
{
    public Animator weaponAnimator;
    
    
    public void PlayAttackAnim()
    {
        weaponAnimator.SetTrigger("attack");
    }
    
    public void SetMovingIdle()
    {
        weaponAnimator.SetBool("isRunning", true);
    }

    public void SetIdle()
    {
        weaponAnimator.SetBool("isRunning", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
