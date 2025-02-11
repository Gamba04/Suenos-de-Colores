using System.Collections.Generic;
using UnityEngine;

public static class WebcamProcessing
{
    private const float brightnessBoost = 1.4f;

    public static Color ScanColor(WebCamTexture webcam, Vector2 position, float radius)
    {
        // Process values
        position *= webcam.height;
        position += new Vector2(webcam.width, webcam.height) / 2;

        radius *= webcam.height;

        // Get color
        List<Color> pixels = GetValidPixels(webcam, position, radius);
        Color color = GetAverage(pixels);

        // Process color
        color *= brightnessBoost;
        color.a = 1;

        return color;
    }

    private static List<Color> GetValidPixels(WebCamTexture webcam, Vector2 center, float radius)
    {
        List<Color> colors = new List<Color>();

        for (int x = 0; x < webcam.width; x++)
        {
            for (int y = 0; y < webcam.height; y++)
            {
                Vector2 position = new Vector2(x, y);

                if (Vector2.Distance(center, position) <= radius)
                {
                    colors.Add(webcam.GetPixel(x, y));
                }
            }
        }

        return colors;
    }

    private static Color GetAverage(List<Color> pixels)
    {
        Color color = default;

        pixels.ForEach(pixel => color += pixel);
        color /= pixels.Count;

        return color;
    }
}