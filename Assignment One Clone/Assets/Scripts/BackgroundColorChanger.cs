using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    public Camera mainCamera; // The main camera
    public Color[] colors; // Array of colors to blend between
    public float duration = 5.0f; // Duration to blend between colors

    private int currentIndex = 0;
    private float t = 0.0f;

   private void Update()
    {
        if (colors.Length == 0)
            return;

        // Increment the t value based on time and duration
        t += Time.deltaTime / duration;

        // Blend between current color and the next color
        mainCamera.backgroundColor = Color.Lerp(colors[currentIndex], colors[(currentIndex + 1) % colors.Length], t);

        // If t has reached 1, move to the next color
        if (t >= 1.0f)
        {
            t = 0.0f;
            currentIndex = (currentIndex + 1) % colors.Length;
        }
    }
}
