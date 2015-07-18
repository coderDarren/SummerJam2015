using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Powers))]

public class CharacterAnimatorController : MonoBehaviour {

    public Transform upperBody;

    CharacterController character;
    Powers powers;
    Animator anim;
   

    void Start()
    {
        character = GetComponent<CharacterController>();
        powers = GetComponent<Powers>();
        anim = GetComponent<Animator>();
        
    }

    void Update()
    {
        anim.SetFloat("Run", character.GetRunInput());
        anim.SetFloat("Walk", character.GetWalkInput());
        anim.SetBool("Grounded", character.Grounded());
        anim.SetBool("Pushing", powers.Pushing());
        anim.SetBool("Holding", powers.Holding());
        anim.SetFloat("ReducedSpeed", character.ReducedSpeed);

        if (powers.Holding() && (character.GetRunInput() > 0 || character.GetWalkInput() > 0))
        {
        }
    }
}
