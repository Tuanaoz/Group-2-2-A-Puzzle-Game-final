using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAnimation : MonoBehaviour
{

    private Animator bearAnimator;
    private EnemyMovement enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        bearAnimator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bearAnimator != null)
        {
            if (enemyMovement != null && !enemyMovement.IsMovingForward()) {
                bearAnimator.SetBool("WalkForward", true);
                bearAnimator.SetBool("WalkBackward", false);
            } else {
                bearAnimator.SetBool("WalkBackward", true);
                bearAnimator.SetBool("WalkForward", false);

            }
        }
    }
}
