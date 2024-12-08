using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds;


    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void Initialize()
    {
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    public void Stop()
    {
        // Останавливаем музыку
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.time = 0f;
        }

        // Очищаем текущие данные (если нужно)

        // Останавливаем все корутины
        StopAllCoroutines();
        ScoreManager.ResetScore();

        // Очищаем состояние дорожек (если нужно)

        Debug.Log("SongManager завершил текущую работу.");
    }


    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
        {
            lane.Initialize();
            lane.SetTimeStamps(array);
        }

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong()
    {
        audioSource.Play();
    }
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    void Update()
    {

    }
}