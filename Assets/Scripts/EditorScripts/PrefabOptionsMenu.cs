using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabOptionsMenu : MonoBehaviour
{

    private GameObject targetPrefab;

    // Start is called before the first frame update
    void Start()
    {
        targetPrefab = null;
        gameObject.SetActive(false);
    }

    public void OpenMenu(GameObject prefab) {
        targetPrefab = prefab;
        transform.position = Input.mousePosition;
        gameObject.SetActive(true);
        if (targetPrefab.tag == "Switch") {
            // Show connect to door option
            transform.GetChild(2).gameObject.SetActive(true);
        } else {
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void CloseMenu() {
        targetPrefab = null;
        gameObject.SetActive(false);
    }

    public void DeletePrefab() {
        Destroy(targetPrefab);
        CloseMenu();
    }

    public void RotatePrefab()
    {
        if (targetPrefab == null)
            return;
        EnemyMovement em = targetPrefab.GetComponent<EnemyMovement>();

        if (em != null)
        {
            if (Mathf.Abs(em.ChangeDirection.z) > Mathf.Abs(em.ChangeDirection.x))
            {
                em.ChangeDirection = Vector3.right;
            }
            else
            {
                em.ChangeDirection = Vector3.forward;
            }
            targetPrefab.transform.rotation = Quaternion.LookRotation(em.ChangeDirection);
        }
        else
        {
            targetPrefab.transform.Rotate(0, 90f, 0);
        }
    }

    public void connectSwitchToDoor(Door door)
    {
        if (targetPrefab == null)
            return;
        Switch sw = targetPrefab.GetComponent<Switch>();
        if (sw != null)
        {
            sw.connectedToDoor = door;
        }
        CloseMenu();
    }

}
