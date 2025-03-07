using UnityEditor;
using UnityEngine.Rendering;

[InitializeOnLoad]
public static class DisableDebugManager
{
    static DisableDebugManager()
    {
        DebugManager.instance.enableRuntimeUI = false;
    }
}