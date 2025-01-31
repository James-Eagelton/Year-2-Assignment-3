using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private Transform cameraTransform;
    public GameObject otherHitEffect;
    public GameObject enemyHitEffect;
    public Animator gun;
    public bool canShoot;


    private void Start()
    {
        cameraTransform = Camera.main.transform;
        canShoot = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot == true)
        {
            print("FUCJK");
            
            canShoot = false;
            gun.SetTrigger("isShoot");

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
            Invoke("Cooldown", 0.90f);
        }
    }

    public void Cooldown() 
    {
        canShoot = true;
    }

}
