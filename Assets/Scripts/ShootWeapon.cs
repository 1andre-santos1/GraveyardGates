using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    public bool DebugGunRange;
    public float PistolReloadTime;
    public float PistolFireTime;
    public float PistolDamage;
    public float PistolRange;
    public int PistolTotalAmmo;
    public int PistolActualAmmo;
    public AudioClip PistolShot;

    public float ShotgunReloadTime;
    public float ShotgunFireTime;
    public float ShotgunDamage;
    public float ShotgunRange;
    public int ShotgunTotalAmmo;
    public int ShotgunActualAmmo;

    public AudioClip ShotgunShot;

    public AudioClip ReloadSound;
    public GameObject GunFire;
    public GameObject GunOrigin;
    public GameObject Blood;

    public int totalAmmo = 5;

    public int actualAmmo;


    private bool isShooting = false;
    private bool isReloading = false;

    public int currentWeapon = 0;
    private int numberOfWeapons = 2;
    private List<int> weapons = new List<int>(){0};

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        PistolActualAmmo = PistolTotalAmmo;
        ShotgunActualAmmo = ShotgunTotalAmmo;
    }

    private void Update()
    {
        if (!GameObject.FindObjectOfType<GameManager>().IsGameActive)
            return;
        if (Input.GetKeyUp(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            currentWeapon++;
            if (currentWeapon >= weapons.Count)
                currentWeapon = 0;
            gameObject.GetComponent<Animator>().SetInteger("currentWeapon",weapons[currentWeapon]);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            currentWeapon--;
            if (currentWeapon < 0)
                currentWeapon = weapons.Count - 1;
            gameObject.GetComponent<Animator>().SetInteger("currentWeapon", weapons[currentWeapon]);
        }
        if (Input.GetAxisRaw("Fire1") != 1 || isShooting || isReloading)
            return;

        isShooting = true;

        if (currentWeapon == 0)
            GetComponent<AudioSource>().clip = PistolShot;
        else if(currentWeapon == 1)
            GetComponent<AudioSource>().clip = ShotgunShot;
        GetComponent<AudioSource>().Play();

        //Spawn the GunFire particle on the GunOrigin position
        SpawnParticle(GunFire,GunOrigin.transform.position);

        ManageRaycastBulletCollision();

        if (currentWeapon == 0)
        {
            PistolActualAmmo--;

            if (PistolActualAmmo <= 0)
            {
                Reload();
                return;
            }
        }
        else if (currentWeapon == 1)
        {
            ShotgunActualAmmo--;

            if (ShotgunActualAmmo <= 0)
            {
                Reload();
                return;
            }
        }
            

        Debug.Log("Is Cooldown Fire");

        StartCoroutine("CooldownPistolFireTime");
    }

    private void Reload()
    {
        isReloading = true;
        GetComponent<AudioSource>().clip = ReloadSound;
        GetComponent<AudioSource>().Play();
        Debug.Log("Is Reloading");
        if(currentWeapon == 0)
            StartCoroutine("CooldownPistolReloadTime");
        else if(currentWeapon == 1)
            StartCoroutine("CooldownShotgunReloadTime");
    }

    private IEnumerator CooldownPistolFireTime()
    {
        yield return new WaitForSeconds(PistolFireTime);
        Debug.Log("Can Shoot");
        isShooting = false;
    }

    private IEnumerator CooldownPistolReloadTime()
    {
        yield return new WaitForSeconds(PistolReloadTime);
        Debug.Log("Can Shoot");
        PistolActualAmmo = PistolTotalAmmo;
        isShooting = false;
        isReloading = false;
    }

    private IEnumerator CooldownShotgunFireTime()
    {
        yield return new WaitForSeconds(ShotgunFireTime);
        Debug.Log("Can Shoot");
        isShooting = false;
    }

    private IEnumerator CooldownShotgunReloadTime()
    {
        yield return new WaitForSeconds(ShotgunReloadTime);
        Debug.Log("Can Shoot");
        ShotgunActualAmmo = ShotgunTotalAmmo;
        isShooting = false;
        isReloading = false;
    }

    private void ManageRaycastBulletCollision()
    {
        var hitInfo = (currentWeapon == 0) ? Physics2D.Raycast(transform.position, transform.right, PistolRange) : Physics2D.Raycast(transform.position, transform.right, ShotgunRange);
       
        if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "Enemy")
        {
            if (DebugGunRange)
                Debug.DrawLine(gameObject.transform.GetChild(0).transform.position, hitInfo.point, Color.green);

            var enemyHit = hitInfo.transform.GetComponent<Enemy>();
            if(currentWeapon == 0)
                enemyHit.setHealth(enemyHit.getHealth() - (int)PistolDamage);
            else if(currentWeapon == 1)
                enemyHit.setHealth(enemyHit.getHealth() - (int)ShotgunDamage);

            //Spawn the Blood particle on the enemy that was hit position
            SpawnParticle(Blood, enemyHit.transform.position);
        }
        else
        {
            if (DebugGunRange)
                Debug.DrawLine(gameObject.transform.GetChild(0).transform.position, gameObject.transform.GetChild(0).transform.position + transform.right * Screen.width, Color.red);
        }
    }

    /// <summary>
    /// Spawn a particle given the GameObject particle
    /// and the position to spawn
    /// </summary>
    private void SpawnParticle(GameObject particle,Vector3 positionToSpawn)
    {
        var particleInstance = Instantiate(particle);
        particleInstance.transform.position = positionToSpawn;

        particleInstance.transform.rotation = new Quaternion(0,gameObject.transform.rotation.y,0,0);

        var particleDuration = particleInstance.GetComponent<ParticleSystem>().main.duration;

        StartCoroutine(WaitAndDeleteParticle(particleInstance, particleDuration));
    }

    private static IEnumerator WaitAndDeleteParticle(GameObject particle, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(particle);
    }

    public void BuyWeapon(int index)
    {
        if (gameObject.GetComponent<Player>().GetMoney() >= 400)
        {
            weapons.Add(index);
            gameObject.GetComponent<Player>().SetMoney(gameObject.GetComponent<Player>().GetMoney() - 400);
            GameObject.FindObjectOfType<UIManager>().UpdateNumberOfCoins();
        }
    }

    public void UpgradePistol_ReloadTime(int buyIndex)
    {
        switch (buyIndex)
        {
            case 1:
                if (gameObject.GetComponent<Player>().GetMoney() >= 100)
                {
                    PistolReloadTime -= (PistolReloadTime * 0.1f);
                    gameObject.GetComponent<Player>().SetMoney(gameObject.GetComponent<Player>().GetMoney() - 100);
                    GameObject.FindObjectOfType<UIManager>().UpdateNumberOfCoins();
                }
                    
                break;
        }
    }
    public void UpgradePistol_FireTime(int buyIndex)
    {
        switch (buyIndex)
        {
            case 1:
                if (gameObject.GetComponent<Player>().GetMoney() >= 100)
                {
                    PistolFireTime -= (PistolFireTime * 0.1f);
                    gameObject.GetComponent<Player>().SetMoney(gameObject.GetComponent<Player>().GetMoney() - 100);
                    GameObject.FindObjectOfType<UIManager>().UpdateNumberOfCoins();
                }
                    
                break;
        }
    }
    public void UpgradePistol_TotalAmmo(int buyIndex)
    {
        switch (buyIndex)
        {
            case 1:
                if (gameObject.GetComponent<Player>().GetMoney() >= 120)
                {
                    PistolTotalAmmo += 10;
                    gameObject.GetComponent<Player>().SetMoney(gameObject.GetComponent<Player>().GetMoney() - 120);
                    GameObject.FindObjectOfType<UIManager>().UpdateNumberOfCoins();
                }
                break;
        }
    }
}
