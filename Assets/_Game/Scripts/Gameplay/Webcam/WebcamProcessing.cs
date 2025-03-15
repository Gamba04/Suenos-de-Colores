using System;
using System.Threading.Tasks;
using UnityEngine;

public static class WebcamProcessing
{
    private const float brightnessBoost = 1.2f;

    private const uint asyncThreshold = 1000;

    public static async Task<Color> ScanColor(Texture2D texture, Vector2 position, float size)
    {
        // Process values
        position *= texture.height;
        position += new Vector2(texture.width, texture.height) / 2;

        size *= texture.height;

        // Get color
        Color[] pixels = GetPixels(texture, position, size);

        bool runAsync = pixels.Length >= asyncThreshold;
        Func<Color> getAverage = () => GetAverage(pixels);

        Color color = runAsync ? await Task.Run(getAverage) : getAverage();

        // Process color
        color *= brightnessBoost;
        color.a = 1;

        return color;
    }

    private static Color[] GetPixels(Texture2D texture, Vector2 center, float size)
    {
        Vector2 block = Vector2.one * size;
        Vector2 start = center - block / 2;

        return texture.GetPixels((int)start.x, (int)start.y, (int)block.x, (int)block.y);
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