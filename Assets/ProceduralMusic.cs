using UnityEngine;
using System; 

public class ProceduralMusic : MonoBehaviour
{
	// un-optimized version
	public double Gain = 0.25;
	private double sampling_frequency = 44100;
	public int t = 0;

	int musicTrack = 2;
	int numTracks = 3;
	float timePerTrack = 90;

	public static bool MuteMusic = false;

	float trackTime = 0;
	public float gainFade = 1;

	void Awake()
	{
		musicTrack = (3 + Application.loadedLevel) % numTracks;
		AudioSettings.outputSampleRate = (int)sampling_frequency;
	}

	void Update()
	{
		trackTime += Time.deltaTime / Time.timeScale;
		if (trackTime > timePerTrack)
		{
			gainFade -= Time.deltaTime / Time.timeScale;
		}
		else if (trackTime < 1 && gainFade < 1)
			gainFade += Time.deltaTime / Time.timeScale;
		else
			gainFade = 1f;

		if (gainFade <= 0)
		{
			trackTime = 0;
			musicTrack = (musicTrack + 1) % numTracks;
		}		
	}



	void OnAudioFilterRead(float[] data, int channels)
	{
		if (MuteMusic)
			return;

		// update increment in case frequency has changed
		for (var i = 0; i < data.Length; i = i + channels)
		{
			t = (t + 1);
			float sample = SampleFunction(t);
			
			// add sample (moded to char size) to existing audio (sfx etc)
			data[i] = data[i] + (float)(gainFade * Gain * (sample % 256) / 256f);
			
			// stereo copy
			if (channels == 2) data[i + 1] = data[i];
		}
	}

	int SampleFunction(int t)
	{
		switch(musicTrack)
		{
			case 0: return SampleFunction1(t);
			case 1: return SampleFunction2(t);
			case 2:
			default:
				return SampleFunction3(t);
		}
	}

	int SampleFunction1(int t)
	{
		t = (int)(t * 8000f / sampling_frequency);
		return (t * 5 & t >> 7) | (t * 3 & t >> 10);
	}

	int SampleFunction2(int t)
	{
		t = (int)(t * 8000f / sampling_frequency);
		return (int)(t / 1e7 * t * t + t) % 127 | t >> 4 | t >> 5 | t % 127 + (t >> 16) | t;
	}

	int SampleFunction3(int t)
	{
		return ((t*("36364689"[t>>13&7]&15))/12&128)+(((((t>>12)^(t>>12)-2)%11*t)/4|t>>13)&127);
	}
}