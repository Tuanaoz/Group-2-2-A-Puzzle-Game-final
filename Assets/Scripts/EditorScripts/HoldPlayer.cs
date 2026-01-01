using UnityEngine;
using System.Collections;
public class HoldPlayer : MonoBehaviour {
    public bool canMove = true;
    void Update() {
        if (!canMove) return;
    }
        public IEnumerator HoldMovement(float duration) {
            canMove = false;
            yield return new WaitForSeconds(duration);
            canMove = true;
    }
}