using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimation : MonoBehaviour
{

    private Animator catAnimator;
    private CharacterMovement characterMovement;
    private HoldPlayer holdPlayer;

    // Start is called before the first frame update
    void Start()
    {
        catAnimator = GetComponent<Animator>(); 
        characterMovement = GetComponent<CharacterMovement>();
        holdPlayer = GetComponent<HoldPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterMovement != null && catAnimator != null)
        {
            bool isMoving = characterMovement.IsMoving();
            catAnimator.SetBool("isWalking", isMoving);
        }

        if (holdPlayer != null && catAnimator != null)
        {
            bool isHeld = holdPlayer.IsHeld();
            catAnimator.SetBool("isHeld", isHeld);
        }
    }
}
