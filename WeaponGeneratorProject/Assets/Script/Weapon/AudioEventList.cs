using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;


[System.Serializable]
public class AudioEvent
{
    public string eventName = "";
    public EventReference eventPath;
}


[CreateAssetMenu(fileName = "New AudioEventList", menuName = "Game/Data/AudioEventList")]
public class AudioEventList : ScriptableObject
{
    [SerializeField] private List<AudioEvent> audioEvents = new List<AudioEvent>();
    private EventInstance instance;

    public void PlayAudioEventOneShot(string eventName)
    {
        foreach (var audioEvent in audioEvents)
        {
            if (audioEvent.eventName == eventName)
            {
                Debug.Log($"<color=#FFB12B>Play Audio {audioEvent.eventName}</color>");
                RuntimeManager.PlayOneShot(audioEvent.eventPath);
                return;
            }
        }
        Debug.LogError($"AudioEvent : {eventName} not found");
    }

    public void PlayAudioEvent(string eventName)
    {
        foreach (var audioEvent in audioEvents)
        {
            if (audioEvent.eventName == eventName)
            {
                Debug.Log($"<color=#FFB12B>Play Audio {audioEvent.eventName}</color>");
                instance = RuntimeManager.CreateInstance(audioEvent.eventPath);
                instance.start();
                instance.release();
                return;
            }
        }
        Debug.LogError($"AudioEvent : {eventName} not found");
    }

}
