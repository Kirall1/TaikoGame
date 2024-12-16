using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    List<Note> notes;
    List<Note> notesBot;
    public List<double> timeStamps;
    public bool isBot;
    public bool isRed;
    double botMissChance;
    int spawnIndex;
    int inputIndex;
    int inputIndexBot;

    // Start is called before the first frame update
    void Start()
    {
        Initialize(false, false);
    }

    public void Initialize(bool isBot, bool isRed)
    {
        notes = new List<Note>();
        notesBot = new List<Note>();

        timeStamps = new List<double>();
        botMissChance = 0.1;
        spawnIndex = 0;
        inputIndex = 0;
        inputIndexBot = 0;
        this.isBot = isBot;
        this.isRed = isRed;
    }
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                notesBot.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count && !isBot)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }

        if(inputIndexBot < timeStamps.Count && isBot)
        {
            double timeStamp = timeStamps[inputIndexBot];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Math.Abs(audioTime - timeStamp) < marginOfError)
            {
                if (UnityEngine.Random.value <= botMissChance) // 10% вероятность промаха
                {
                    print($"BotMiss");
                    BotMiss();
                    inputIndexBot++;
                }
                else
                {
                    
                    print($"BotHit");
                    if(isRed)
                    {
                        BotHitLeft();
                    }
                    else
                    {
                        BotHitRight();
                    }
                    Destroy(notesBot[inputIndexBot].gameObject);
                    inputIndexBot++;
                }
            }
        }
    }
    private void Hit()
    {
        ScoreManager.Hit();
    }
    private void Miss()
    {
        ScoreManager.Miss();
    }
    private void BotHitLeft()
    {
        ScoreManager.HitBotLeft();
    }
    private void BotHitRight()
    {
        ScoreManager.HitBotRight();
    }
    private void BotMiss()
    {
        ScoreManager.MissBot();
    }
}