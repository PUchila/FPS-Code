using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BossTarget : MonoBehaviour
{
    // Start is called before the first frame update
    private Ragdoll ragdoll;
    public float health = 5f;
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
    public BossScript enemyAI;
    public bool isHuman = true;
    public Slider mSlider;
    public StopWatchBoss stopwatchboss;


    void Start(){
      alive = true;
      enemyAI = GetComponent<BossScript>();
      mSlider.maxValue = health;
      mSlider.value = health;
    }
    public void TakeDamage (float amount)
    {
        if (!enemyAI.playerInSightRange)
          return;
        damaging = true;
        health -= amount;
        mSlider.value -= amount;
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
          enemyAI.dead = true;
          alive = false;
          stopwatchboss.StopTimer();
      }

    }

}
