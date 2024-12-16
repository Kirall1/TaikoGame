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
    public bool songStarted = false;
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
        ReadFromFile();
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

        // Очищаем состояние дорожек (если нужно)

        Debug.Log("SongManager завершил текущую работу.");
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

        lanes[0].Initialize(false, false);
        lanes[0].SetTimeStamps(array);
        lanes[1].Initialize(false, true);
        lanes[1].SetTimeStamps(array);
        lanes[2].Initialize(true, false);
        lanes[2].SetTimeStamps(array);
        lanes[3].Initialize(true, true);
        lanes[3].SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }
    public void StartSong()
    {
        audioSource.Play();
        songStarted = true;
        ScoreManager.ResetScore();
    }
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    void Update()
    {

    }
}