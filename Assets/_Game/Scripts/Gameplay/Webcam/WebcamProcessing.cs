using UnityEngine;

public static class WebcamProcessing
{
    private const float brightnessBoost = 1.1f;

    public static Color ScanColor(WebCamTexture webcam, Vector2 position, float size)
    {
        // Process values
        position *= webcam.height;
        position += new Vector2(webcam.width, webcam.height) / 2;

        size *= webcam.height;

        // Get color
        Color[] pixels = GetPixels(webcam, position, size);
        Color color = GetAverage(pixels);

        // Process color
        color *= brightnessBoost;
        color.a = 1;

        return color;
    }

    private static Color[] GetPixels(WebCamTexture webcam, Vector2 center, float size)
    {
        Vector2 block = Vector2.one * size;
        Vector2 start = center - block / 2;

        return webcam.GetPixels((int)start.x, (int)start.y, (int)block.x, (int)block.y);
    }

    private static Color GetAverage(Color[] pixels)
    {
        Color color = default;
        float total = default;

        foreach (Color pixel in pixels)
        {
            float weight = Mathf.Max(pixel.r, pixel.g, pixel.b);

            color += pixel * weight;
            total += weight;
        }

        if (total == 0) return default;

        color /= total;

        return color;
    }
}