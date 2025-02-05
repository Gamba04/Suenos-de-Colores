using System.Collections.Generic;
using UnityEngine;

public static class WebcamProcessing
{
    private const float brightnessBoost = 1.4f;

    public static Color GetColor(WebCamTexture webcam, float radius)
    {
        List<Color> pixels = GetValidPixels(webcam, radius * webcam.height);
        Color color = GetAverage(pixels);

        return color * brightnessBoost;
    }

    private static List<Color> GetValidPixels(WebCamTexture webcam, float radius)
    {
        List<Color> colors = new List<Color>();
        Vector2 center = new Vector2(webcam.width, webcam.height) / 2;

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