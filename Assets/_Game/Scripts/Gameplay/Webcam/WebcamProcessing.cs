using System.Collections.Generic;
using UnityEngine;

public static class WebcamProcessing
{
    public static Color GetPictureColor(Texture2D picture, float radius)
    {
        List<Color> colors = GetValidColors(picture, radius * picture.height);

        return GetAverage(colors);
    }

    private static List<Color> GetValidColors(Texture2D picture, float radius)
    {
        List<Color> colors = new List<Color>();
        Vector2 center = new Vector2(picture.width, picture.height) / 2;

        for (int x = 0; x < picture.width; x++)
        {
            for (int y = 0; y < picture.height; y++)
            {
                Vector2 position = new Vector2(x, y);

                if (Vector2.Distance(center, position) <= radius)
                {
                    colors.Add(picture.GetPixel(x, y));
                }
            }
        }

        return colors;
    }

    private static Color GetAverage(List<Color> colors)
    {
        Color color = default;

        colors.ForEach(c => color += c);
        color /= colors.Count;

        return color;
    }
}