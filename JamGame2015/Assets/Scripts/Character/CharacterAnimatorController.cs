using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PickUpObject))]

public class CharacterAnimatorController : MonoBehaviour {

    CharacterController character;
    PickUpObject pickup;
    Animator anim;

    float airborneTimer = 0;
    float airborneAllowance = 1;
    bool falling = false;

    void Start()
    {
        character = GetComponent<CharacterController>();
        pickup = GetComponent<PickUpObject>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("Run", character.GetRunInput());
        anim.SetFloat("Walk", character.GetWalkInput());
        anim.SetBool("Grounded", character.Grounded());
        //anim.SetBool("Pushing", pickup.Pushing());
        anim.SetBool("Holding", pickup.Holding());
        anim.SetFloat("ReducedSpeed", character.ReducedSpeed);
        anim.SetBool("Falling", falling);

        CheckFalling();
    }

    void CheckFalling()
    {
        if (!character.Grounded())
        {
            if (airborneTimer < airborneAllowance)
                airborneTimer += Time.deltaTime;

            if (airborneTimer >= airborneAllowance)
                falling = true;
        }
        else
        {
            airborneTimer = 0;
            falling = false;
        }
    }
}
