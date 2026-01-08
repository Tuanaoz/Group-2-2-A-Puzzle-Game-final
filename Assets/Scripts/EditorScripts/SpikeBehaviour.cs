using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{

    private bool isOn = false;
    private float timer = 0f;
    public float timerThreshold = 3f;

    public void Toggle() {
        isOn = !isOn;
    }

    public bool IsOn() {
        return isOn;
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer >= timerThreshold) {
            Toggle();
            timer = 0f;
        }
    }
}
