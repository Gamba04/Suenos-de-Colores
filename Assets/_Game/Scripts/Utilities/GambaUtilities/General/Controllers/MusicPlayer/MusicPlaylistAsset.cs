using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = title, menuName = "Gamba/" + title)]
public class MusicPlaylistAsset : ScriptableObject
{
    private const string title = "Playlist";

    [SerializeField]
    private List<AudioClip> songs;

    public AudioClip GetSong(ref int position, bool loop)
    {
        if (songs.Count == 0) return null;

        if (loop)
        {
            position %= songs.Count;
        }
        else if (position >= songs.Count) return null;

        AudioClip song = songs[position];

        if (song == null) throw new MissingReferenceException($"Song number {position} is missing in {name}.");

        return songs[position];
    }
}