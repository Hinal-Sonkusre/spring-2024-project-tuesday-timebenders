using UnityEngine;

public class TimeFreezePowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Make sure your player GameObject has the "Player" tag.
        {
            collision.gameObject.GetComponent<PlayerControl>().EnableTimeFreeze();
            Destroy(gameObject); // Destroy or deactivate the power-up GameObject.
        }
    }
}