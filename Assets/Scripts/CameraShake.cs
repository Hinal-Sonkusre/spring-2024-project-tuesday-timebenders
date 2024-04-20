using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;  // or UnityEngine.Rendering.HighDefinition depending on your pipeline

public class CameraShake : MonoBehaviour
{
    public Volume globalVolume;  // Drag your Global Volume here through the Inspector
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        // Try to retrieve the chromatic aberration component from the global volume
        if (globalVolume.profile.TryGet(out chromaticAberration))
        {
            chromaticAberration.active = false;
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        Time.timeScale = 0f;  // Pause the game
        float elapsed = 0.0f;

        if (chromaticAberration != null)
            chromaticAberration.active = true;  // Enable chromatic aberration

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.unscaledDeltaTime; // Use unscaledDeltaTime to continue counting time while the game is paused

            // Optionally adjust the intensity of the chromatic aberration during the shake
            chromaticAberration.intensity.value = Mathf.Lerp(0.75f, 0.0f, elapsed / duration);

            yield return null; // Wait until the next frame before continuing the loop
        }

        if (chromaticAberration != null)
            chromaticAberration.active = false;  // Disable chromatic aberration

        Time.timeScale = 1f;  // Unpause the game
        transform.position = originalPosition; // Reset the camera position
    }
}
