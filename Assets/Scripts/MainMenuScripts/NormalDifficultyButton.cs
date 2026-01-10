using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDifficultyButton : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // Disable button if not in normal difficulty
        if (!DifficultyBehaviour.hardDifficulty) {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
