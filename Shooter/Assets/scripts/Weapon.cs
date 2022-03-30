using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Ammo ammoSlot;
    void Update()
    {    

        if (Input.GetButtonDown("Fire1"))
        {          
            Shoot();
        }
    }
    private void Shoot()
    {
        if (ammoSlot.GetCurrentAmmo() > 0)
        {
            PlayMuzzlleFlash();
            ProccesReycast();
            ammoSlot.ReduceCurrentAmmo();
        }
    }

    private void PlayMuzzlleFlash()
    {
        muzzleFlash.Play();
    }
    private void ProccesReycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            target.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
      GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
      Destroy(impact, 0.1f);
    }
}
