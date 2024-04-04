using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public ImprovisedPlayerScript player;
    
    //Audio
    private AudioSource source;
    
    public AudioClip attackclip;
    public AudioClip damageclip;
    public AudioClip deathclip;
    public AudioClip jumpclip;
    public AudioClip projectileclip;
    public AudioClip footstepClip;

    public void PlayAudio(string audioName)
    {
        if (audioName == "attack") source.PlayOneShot(attackclip);
        else if (audioName == "projectile") source.PlayOneShot(projectileclip);
        else if (audioName == "damage") source.PlayOneShot(damageclip);
        else if (audioName == "died") source.PlayOneShot(deathclip);
        else if (audioName == "jump") source.PlayOneShot(jumpclip);
        else if (audioName == "nil") source.PlayOneShot(projectileclip);

    }

    IEnumerator FootstepCoroutine()
    {
        while (true)
        {
            if (player.GetIsMovingState() && player.GetGroundedState()) source.PlayOneShot(footstepClip);
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(FootstepCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
