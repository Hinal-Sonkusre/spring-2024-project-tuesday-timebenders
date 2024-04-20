using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;

        // Pause the game
        Time.timeScale = 0f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.unscaledDeltaTime; // Use unscaledDeltaTime to continue counting time while the game is paused

            yield return null; // Wait until the next frame before continuing the loop
        }

        // Unpause the game
        Time.timeScale = 1f;
        transform.position = originalPosition; // Reset the camera position
    }
}
