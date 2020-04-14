using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private MeshRenderer mesh;

    [Header("Basic Variables")]
    public float maxHealth;
    // [HideInInspector]
    // The real health value
    public float health;
    // [HideInInspector]
    // The health value that's displayed on the health bar
    public float displayHealth;
    // Alive state
    public bool Alive { get => health > 0; }

    public GameObject deathSound;
    
    [Header("Damage Animation")]
    // The duration of the damage animation
    public float damageAnimDuration;
    // The curve of the damage animation
    public AnimationCurve damageAnimCurve;

    [Header("Death Animation")]
    
    
    // Executes upon being damaged
    public UnityEvent onDamage;
    // Executes upon death
    public UnityEvent onDeath;

    void Awake()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        health = maxHealth;
    }

    public void Damage(float damage)
    {
        health -= damage;
        onDamage.Invoke();
        StopCoroutine(DamageAnim(0));
        StartCoroutine(DamageAnim(damage));
    }

    public void Kill()
    {
        GameObject death = Instantiate(deathSound, transform.position, Quaternion.identity);
        death.GetComponent<AudioSource>().Play();
        onDeath.Invoke();
        Destroy(gameObject);
        Destroy(death, 3);
    }

    // TODO: do this without a coroutine
    IEnumerator DamageAnim(float damage)
    {
        Color defaultCol = mesh.material.color;
        float startTime = Time.time;
        while (Time.time - startTime < damageAnimDuration)
        {
            float prog = (Time.time - startTime) / damageAnimDuration;
            mesh.material.color = Color.Lerp(Color.red, defaultCol, damageAnimCurve.Evaluate(prog));
            yield return new WaitForEndOfFrame();
        }
    }

    void Update()
    {
        if (!Alive) Kill();
        displayHealth = Mathf.Lerp(displayHealth, health, .2f);
    }
}
