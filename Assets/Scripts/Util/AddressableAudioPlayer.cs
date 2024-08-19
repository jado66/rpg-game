using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class AddressableAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> cachedClips = new Dictionary<string, AudioClip>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayAddressableSound(string address)
    {
        if (cachedClips.TryGetValue(address, out AudioClip cachedClip))
        {
            audioSource.PlayOneShot(cachedClip);
        }
        else
        {
            Addressables.LoadAssetAsync<AudioClip>(address).Completed += OnAudioLoaded;
        }
    }

    private void OnAudioLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip clip = handle.Result;
            cachedClips[handle.DebugName] = clip;
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError($"Failed to load audio clip: {handle.DebugName}");
        }
    }

    public void PreloadSound(string address)
    {
        if (!cachedClips.ContainsKey(address))
        {
            Addressables.LoadAssetAsync<AudioClip>(address).Completed += OnPreloadCompleted;
        }
    }

    private void OnPreloadCompleted(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cachedClips[handle.DebugName] = handle.Result;
            Debug.Log($"Preloaded audio clip: {handle.DebugName}");
        }
        else
        {
            Debug.LogError($"Failed to preload audio clip: {handle.DebugName}");
        }
    }

    public void ClearCache()
    {
        foreach (var clip in cachedClips.Values)
        {
            Addressables.Release(clip);
        }
        cachedClips.Clear();
    }

    private void OnDestroy()
    {
        ClearCache();
    }
}