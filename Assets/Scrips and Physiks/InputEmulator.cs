using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple static input emulator. Other scripts can query GetKey/GetKeyDown/GetKeyUp
/// for KeyCodes that are controlled by UI buttons (like MobileButton).
/// Attach InputEmulator.Updater to any GameObject in the scene (it will auto-create one if needed).
/// </summary>
public static class InputEmulator
{
    private class KeyState
    {
        public bool current = false; // state this frame
        public bool previous = false; // state previous frame
    }

    private static readonly Dictionary<KeyCode, KeyState> states = new Dictionary<KeyCode, KeyState>();

    // Called by UI elements to set the desired state immediately.
    public static void SetKeyState(KeyCode key, bool pressed)
    {
        if (!states.TryGetValue(key, out var ks))
        {
            ks = new KeyState();
            states[key] = ks;
        }
        ks.current = pressed;
    }

    // Called from the updater at the end of the frame to shift states for GetKeyDown/GetKeyUp logic
    internal static void AdvanceFrame()
    {
        foreach (var kv in states)
        {
            var ks = kv.Value;
            ks.previous = ks.current;
        }
    }

    // Query helpers
    public static bool GetKey(KeyCode key)
    {
        return states.TryGetValue(key, out var ks) && ks.current;
    }

    public static bool GetKeyDown(KeyCode key)
    {
        if (!states.TryGetValue(key, out var ks)) return false;
        return ks.current && !ks.previous;
    }

    public static bool GetKeyUp(KeyCode key)
    {
        if (!states.TryGetValue(key, out var ks)) return false;
        return !ks.current && ks.previous;
    }

    // Internal MonoBehaviour to drive frame transitions. Auto-creates on first access.
    private class Updater : MonoBehaviour
    {
        void LateUpdate()
        {
            AdvanceFrame();
        }
    }

    // Ensure there's an updater in the scene so AdvanceFrame is called each frame.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureUpdater()
    {
        var existing = Object.FindFirstObjectByType<Updater>();
        if (existing == null)
        {
            var go = new GameObject("InputEmulator_Updater") { hideFlags = HideFlags.DontSave };
            go.AddComponent<Updater>();
        }
    }
}
