using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    private AudioSource _audioSource;
    private Coroutine _signaling;
    private float _maxVolume;
    private float _minVolume;
    private float _rateOfChange;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0f;
        _maxVolume = 0.999f;
        _minVolume = 0.001f;
        _rateOfChange = 0.55f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Robber>())
        {
            if (_signaling != null)
                StopCoroutine(_signaling);

            //_signaling = StartCoroutine(ChangeVolume(true));
            _signaling = StartCoroutine(IncreaseVolume());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Robber>())
        {
            StopCoroutine(_signaling);

            //_signaling = StartCoroutine(ChangeVolume(false));
            _signaling = StartCoroutine(DecreaseVolume());
        }
    }

    private IEnumerator IncreaseVolume()
    {
        _audioSource?.Play();

        while (_audioSource.volume <= _maxVolume)
        {
            _audioSource.volume = _audioSource.volume + _rateOfChange * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator DecreaseVolume()
    {
        while (_audioSource.volume >= _minVolume)
        {
            _audioSource.volume = _audioSource.volume - _rateOfChange * Time.deltaTime;
            yield return null;
        }

        _audioSource?.Stop();
    }

    private IEnumerator ChangeVolume(bool isRobbing)
    {
        if (isRobbing)
        {
            _audioSource?.Play();

            while (_audioSource.volume <= _maxVolume)
            {
                _audioSource.volume = _audioSource.volume + _rateOfChange * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (_audioSource.volume >= _minVolume)
            {
                _audioSource.volume = _audioSource.volume - _rateOfChange * Time.deltaTime;
                yield return null;
            }

            _audioSource?.Stop();
        }
    }
}
