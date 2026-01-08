using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAnimation : MonoBehaviour
{

    private Animator spikeAnimator;
    private SpikeBehaviour spikeBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        spikeAnimator = GetComponent<Animator>();
        spikeBehaviour = GetComponent<SpikeBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spikeAnimator != null)
        {
            if (spikeBehaviour != null && !spikeBehaviour.IsOn()) {
                spikeAnimator.SetBool("close", true);
                spikeAnimator.SetBool("open", false);
            } else {
                spikeAnimator.SetBool("close", false);
                spikeAnimator.SetBool("open", true);
            }
        }
    }
}
