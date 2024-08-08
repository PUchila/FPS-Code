using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    private Ragdoll ragdoll;
    public float health = 5f;
    public ParticleSystem explode;
    public float forcevar = 15f;
    private bool alive = true;
    public DamageIndicator damageIndicator;
    public GameObject damageNumbers;
    private float t = 0;
    private float m = 0;
    private bool damaging = false;
    private GameObject go;
    public float damage = 0;
    public Camera cam;
    public float padding = 5f;
    public EnemyAI enemyAI;
    public turretScript turretScript;
    public bool isHuman = true;

    void Start(){
      alive = true;
      if (isHuman){
      enemyAI = GetComponent<EnemyAI>();
        ragdoll = GetComponent<Ragdoll>();
      }else{
        turretScript = GetComponent<turretScript>();
      }
      explode.Stop();
    }
    public void TakeDamage (float amount)
    {
        damaging = true;
        health -= amount;
        if(alive){
          ShowFloatingText(amount);
          damageIndicator.attackIndicator = true;
        }
        if (health <= 0f)
        {
            Die();
        }


    }

    void Update(){
      t += Time.deltaTime;
      if (damaging){
        m = t;
        damaging = false;
      }
      if (go!= null){
        go.transform.LookAt(cam.transform.position);
      }

      if(t-m>=1f){
        damage = 0;
        if(go != null){
          go.GetComponent<destroyText>().targetDamage = damage;
        }
      }
    }


    void ShowFloatingText(float amount){
      damage += amount;
      Destroy(go);
      go = Instantiate(damageNumbers, new Vector3(transform.position.x, transform.position.y + padding, transform.position.z), Quaternion.identity, transform);
      go.transform.LookAt(cam.transform.position);
      go.GetComponent<TextMesh>().text = damage.ToString();
      go.GetComponent<destroyText>().targetDamage = damage;

    }

    void Die()
    {
        if (alive){
          explode.Play();
          if (isHuman){
            ragdoll.EnableRagdoll();
            ragdoll.forceUp(forcevar);
            enemyAI.dead = true;
          }else{
            Destroy(gameObject, .5f);
            turretScript.dead = true;
          }
          alive = false;
      }

    }

}
