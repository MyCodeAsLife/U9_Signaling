using System.Collections;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Signaling : MonoBehaviour
{
    [SerializeField] private Collider _sensor;
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

    private void OnEnable()
    {
        _sensor.GetComponent<Sensor>().OnIntruderEnter += TurnOnAlarm;
        _sensor.GetComponent<Sensor>().OnIntruderExit += TurnOffAlarm;
    }

    private void OnDisable()
    {
        _sensor.GetComponent<Sensor>().OnIntruderEnter -= TurnOnAlarm;
        _sensor.GetComponent<Sensor>().OnIntruderExit -= TurnOffAlarm;
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
