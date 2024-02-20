using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    private Coroutine _changeVolume;
    private AudioSource _audioSource;

    private float _maxVolume;
    private float _minVolume;
    private float _rateOfChange;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0f;
        _maxVolume = 1f;
        _minVolume = 0f;
        _rateOfChange = 0.35f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Robber>(out Robber robber))
            TurnOnAlarm();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Robber>(out Robber robber))
            TurnOffAlarm();
    }

    private IEnumerator ChangeVolume(float volume)
    {
        while (_audioSource.volume != volume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, volume, _rateOfChange * Time.deltaTime);
            yield return null;
        }

        CheckStop();
        _changeVolume = null;
    }

    private void TurnOnAlarm()
    {
        if (_changeVolume == null)
            _audioSource?.Play();
        else
            StopCoroutine(_changeVolume);

        _changeVolume = StartCoroutine(ChangeVolume(_maxVolume));
    }

    private void TurnOffAlarm()
    {
        if (_changeVolume != null)
            StopCoroutine(_changeVolume);

        _changeVolume = StartCoroutine(ChangeVolume(_minVolume));
    }

    private void CheckStop()
    {
        if (_audioSource.volume <= _minVolume)
            _audioSource?.Stop();
    }
}
