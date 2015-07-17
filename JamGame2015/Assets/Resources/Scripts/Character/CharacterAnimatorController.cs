using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Powers))]

public class CharacterAnimatorController : MonoBehaviour {

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
        anim.SetFloat("Push", powers.GetPushInput());
    }
}
