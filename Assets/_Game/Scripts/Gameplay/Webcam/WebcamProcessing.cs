using System.Collections.Generic;
using UnityEngine;

public static class WebcamProcessing
{
    public static Color GetTargetColor(WebCamTexture texture, float radius)
    {
        List<Color> colors = GetValidColors(texture, radius * texture.height);

        return GetAverage(colors);
    }

    private static List<Color> GetValidColors(WebCamTexture texture, float radius)
    {
        List<Color> colors = new List<Color>();
        Vector2 center = new Vector2(texture.width, texture.height) / 2;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Vector2 position = new Vector2(x, y);

                if (Vector2.Distance(center, position) <= radius)
                {
                    colors.Add(texture.GetPixel(x, y));
                }
            }
        }

        return default;
    }

    private static Color GetAverage(List<Color> colors)
    {
        Color color = default;

        colors.ForEach(c => color += c);
        color /= colors.Count;

        return color;
    }
}