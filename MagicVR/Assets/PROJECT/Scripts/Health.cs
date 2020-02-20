using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private MeshRenderer mesh;

    [Header("Basic Variables")]
    public float maxHealth;
    public float health;
    public float displayHealth;
    public AnimationCurve healthLerpCurve;
    public bool alive;
    
    [Header("Damage Animation")]
    [SerializeField]
    private int damageAnimState;
    [SerializeField]
    private float damageAnimDuration;
    [SerializeField]
    private AnimationCurve healthAnimCurve;
    
    public Material deadCharMat;
    Material defaultMat;
    
    public UnityEvent onDamage;
    public UnityEvent onDeath;

    void Awake()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        health = maxHealth;
        alive = true;
        defaultMat = mesh.material;
    }

    void Update()
    {
        if (health <= 0)
        {
            alive = false;
            Kill();
        }
        displayHealth = Mathf.Lerp(displayHealth, health, .2f);
    }

    public void Damage(float damage)
    {
        health -= damage;
        mesh.material = defaultMat;
        onDamage.Invoke();
        StopCoroutine(DamageAnim(0));
        StartCoroutine(DamageAnim(damage));
    }

    // TODO: do this without a coroutine
    IEnumerator DamageAnim(float damage)
    {
        Material damageMat = new Material(defaultMat.shader);
        damageMat.color = Color.red;
        mesh.material = damageMat;

        float startTime = Time.time;
        while (Time.time - startTime < damageAnimDuration)
        {
            float prog = (Time.time - startTime) / damageAnimDuration;
            damageMat.color = Color.Lerp(Color.red, defaultMat.color, healthAnimCurve.Evaluate(prog));
            yield return new WaitForEndOfFrame();
        }

        mesh.material = defaultMat;
    }

    public void Kill()
    {
        mesh.material = deadCharMat;
        onDeath.Invoke();
    }

    //This onTriggerEnter function allows the grenades to damage anything that has this script
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
