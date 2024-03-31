using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour
{
    public GameObject effect;
    public float areaofEffect;
    public LayerMask whatIsDestructible;
    private int playerCount = 0; // Counter for the number of players colliding with the wall
    private int dashingPlayerCount = 0; // Counter for the number of dashing players colliding with the wall
    private Vector2 lastDashingPlayerPosition; // Position of the last dashing player who collided

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            playerCount++; // Increment the counter when a player enters
            PlayerControl playerControl = other.GetComponent<PlayerControl>();
            if (playerControl != null && playerControl.isDashing)
            {
                dashingPlayerCount++; // Increment the counter when a dashing player enters
                lastDashingPlayerPosition = other.transform.position; // Update the position of the last dashing player
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            playerCount--; // Decrement the counter when a player exits
            PlayerControl playerControl = other.GetComponent<PlayerControl>();
            if (playerControl != null && playerControl.isDashing)
            {
                dashingPlayerCount--; // Decrement the counter when a dashing player exits
            }
        }
    }

    void Update()
    {
        if (playerCount >= 2 && dashingPlayerCount >= 2) // Check if at least two players are colliding and dashing
        {
            Instantiate(effect, lastDashingPlayerPosition, Quaternion.identity); // Instantiate the effect at the last dashing player's position

            Collider2D[] ObjectsToDamage = Physics2D.OverlapCircleAll(lastDashingPlayerPosition, areaofEffect, whatIsDestructible);
            for (int i = 0; i < ObjectsToDamage.Length; i++)
            {
                Destroy(ObjectsToDamage[i].gameObject);
            }

            Destroy(gameObject); // Destroy the wall
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaofEffect);
    }
}
