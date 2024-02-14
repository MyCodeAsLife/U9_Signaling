using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    const string Robber = "Robber";

    private AudioSource _audioSource;
    private Coroutine _increaseSignaling;
    private float _volume;
    private float _maxVolume;
    private float _minVolume;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _maxVolume = 1.0f;
        _minVolume = 0.001f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Robber)
        {
            _audioSource?.Play();
            _increaseSignaling = StartCoroutine(IncreasingVolume());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Robber)
        {
            StopCoroutine(_increaseSignaling);
            StartCoroutine(DecreasingVolume());
        }
    }

    private IEnumerator IncreasingVolume()
    {
        while (true)
        {
            _volume += (0.1f * Time.deltaTime);
            _audioSource.volume = _volume;

            if (_volume >= _maxVolume)
                yield break;

            yield return null;
        }
    }

    private IEnumerator DecreasingVolume()
    {
        while (true)
        {
            _volume -= (0.1f * Time.deltaTime);
            _audioSource.volume = _volume;

            if (_volume <= _minVolume)
            {
                _audioSource?.Stop();
                yield break;
            }

            yield return null;
        }
    }
}
