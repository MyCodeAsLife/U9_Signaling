using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    private AudioSource _audioSource;

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
        if (other.GetComponent<Robber>())
        {
            _audioSource?.Play();
            StartCoroutine(IncreasingVolume());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Robber>())
            StartCoroutine(DecreasingVolume());
    }

    private IEnumerator IncreasingVolume()
    {
        _volume *= Time.deltaTime;

        if (_volume >= _maxVolume)
        {

            yield break;
        }

        yield return null;
    }

    private IEnumerator DecreasingVolume()
    {
        _volume /= Time.deltaTime;
        _audioSource.volume = _volume;

        if (_volume <= _minVolume)
        {
            _audioSource?.Stop();
            yield break;
        }

        yield return null;
    }
}
