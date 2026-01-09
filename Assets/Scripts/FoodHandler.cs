using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHandler : MonoBehaviour
{
    public GameObject particleEffect, foodObject;
    void OnEnable()
    {
        Destroy(foodObject);
        Destroy(Instantiate(particleEffect,transform.position, transform.rotation), 1.5f);
    }
}
