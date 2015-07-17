using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class CharacterAnimatorController : MonoBehaviour {

    CharacterController character;
    Animator anim;

    void Start()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("Run", character.GetRunInput());
        anim.SetFloat("Walk", character.GetWalkInput());
    }
}
