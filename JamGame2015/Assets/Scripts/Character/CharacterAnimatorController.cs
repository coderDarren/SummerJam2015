using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PickUpObject))]

public class CharacterAnimatorController : MonoBehaviour {

    public Transform upperBody;

    CharacterController character;
    PickUpObject pickup;
    Animator anim;
   

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

        if (pickup.Holding() && (character.GetRunInput() > 0 || character.GetWalkInput() > 0))
        {
        }
    }
}
