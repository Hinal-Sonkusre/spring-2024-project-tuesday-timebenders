using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour
{

    private Vector2 Target;
    public float speed;
    public GameObject effect;
    public float areaofEffect;
    public LayerMask whatIsDestructible;
    public int damage;
    public int health;

    void Start()
    {
        Target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, Target) < 0.1f)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collider2D[] ObjectsToDamage = Physics2D.OverlapCircleAll(transform.position, areaofEffect, whatIsDestructible);
            for (int i = 0; i < ObjectsToDamage.Length; i++)
            {
                ObjectsToDamage[i].GetComponent<DestructiblePlatform>().health -= damage;
            }
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaofEffect);
    }
}
