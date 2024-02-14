using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _route;
    [SerializeField] private float _movementSpeed;

    private Transform[] _waypoints;
    private Transform _targetPoint;
    private int _nextPointIndex;

    private void Start()
    {
        _waypoints = new Transform[_route.childCount];

        for (int i = 0; i < _waypoints.Length; i++)
            _waypoints[i] = _route.GetChild(i).GetComponent<Transform>();

        //_movementSpeed = 3f;
        _targetPoint = _waypoints[_nextPointIndex];
    }

    private void LateUpdate()
    {
        Move();
        Rotate();
    }

    private void Rotate()
    {
        Vector3 direction = (_targetPoint.position - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction;
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * _movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPoint.position) < 1.5f)
        {
            _nextPointIndex++;

            if (_nextPointIndex >= _waypoints.Length)
                _nextPointIndex = 0;

            _targetPoint = _waypoints[_nextPointIndex];
        }
    }
}
