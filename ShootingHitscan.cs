using System;
using System.Diagnostics;
using System.Collections;
using UnityEngine;
using TMPro;


public class ShootingHitscan : MonoBehaviour{


    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float recoil = 0.1f;
    public Animator anim;
    public GrapplingGun grapple;
    public float aimSmoothing = 10f;

    public int maxAmmo = 10;
   public int currentAmmo;
   public float reloadTime = 1f;
   public bool isReloading = false;

    private Vector3 desiredPosition;
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    public float normalZVal;
    public float aimingZVal;

    [Header("Trail")]
    public float BulletSpeed;
    public TrailRenderer BulletTrail;
    public Transform BulletSpawnPoint;


    public Camera fpscamera;
    public ParticleSystem muzzleflash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;
    public GameObject crosshair;
    public GameObject reloadText;
    public TMP_Text text;
    public AudioSource source;
    public AudioClip gunshoot;


    void Start(){
      currentAmmo = maxAmmo;
    }
    void OnEnable(){
      isReloading = false;
      //anim.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (PauseMenu.GameIsPaused) return;
        text.text = currentAmmo.ToString()+" / " + maxAmmo.ToString();
        if(isReloading){
          crosshair.SetActive(false);
          reloadText.SetActive(true);
          desiredPosition = Vector3.Lerp(transform.localPosition, normalLocalPosition, Time.deltaTime * aimSmoothing);
          transform.localPosition = desiredPosition;

           return;
        }else{
          reloadText.SetActive(false);
        }

        if(currentAmmo<=0){
          StartCoroutine(Reload());
          return;
        }
        if(currentAmmo<maxAmmo && Input.GetKey(KeyCode.R)){
          StartCoroutine(Reload());
          return;
        }
        Vector3 target = normalLocalPosition;
        if (Input.GetButton("Fire2")&&grapple.canaim){
          crosshair.SetActive(false);
          //anim.SetBool("isAiming", true);
          target = aimingLocalPosition;
          //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, aimingRotation.z);
        }else{
          crosshair.SetActive(true);
          target = normalLocalPosition;
          //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, normalRotation.z);

          //anim.SetBool("isAiming", false);
        }
        desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);
        transform.localPosition = desiredPosition;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }


    }

    void Shoot ()
    {
        muzzleflash.Play();
        source.PlayOneShot(gunshoot);
        currentAmmo--;
        RaycastHit hit;
        Ray ray = new Ray(fpscamera.transform.position, fpscamera.transform.forward);
        if (Physics.Raycast(ray,out hit, range))
        {

            Target target = hit.transform.root.transform.GetComponent<Target>();
            if (target != null){
              target.TakeDamage(damage);
            }
            BossTarget bosstarget = hit.transform.root.transform.GetComponent<BossTarget>();
            if (bosstarget != null){
              bosstarget.TakeDamage(damage);
            }
            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));



            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }else{
          TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
          StartCoroutine(SpawnTrail(trail, ray.GetPoint(100f), Vector3.zero, false));
        }
        DetermineRecoil();


    }
    void DetermineRecoil(){
      transform.localPosition -= Vector3.forward * recoil;
    }
    IEnumerator Reload()
    {
      isReloading = true;

      //animator.SetBool("Reloading", true);

      yield return new WaitForSeconds(reloadTime - .25f);
      //animator.SetBool("Reloading", false);
      yield return new WaitForSeconds(1f);


      currentAmmo = maxAmmo;
      isReloading = false;
  }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
        // moving slowly when hitting something close, and not
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        //Animator.SetBool("IsShooting", false);
        Trail.transform.position = HitPoint;

        Destroy(Trail.gameObject, Trail.time);
    }



}
