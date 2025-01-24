using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private Transform cameraTransform;
    public GameObject otherHitEffect;
    public GameObject enemyHitEffect;



    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, color: Color.red);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag != "Enemy")
                {
                    GameObject hitDecal = Instantiate(otherHitEffect, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.forward, hit.normal));

                    Debug.Log("hit");
                }

                if (hit.collider.tag == "Enemy") 
                {
                    GameObject hitDecal = Instantiate(enemyHitEffect, hit.point + (hit.normal * 0.01f), Quaternion.FromToRotation(Vector3.forward, hit.normal));

                    Debug.Log("hit");
                }
            }
        }
    }
}
