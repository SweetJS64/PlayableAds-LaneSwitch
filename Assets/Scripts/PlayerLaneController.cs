using UnityEngine;

public class PlayerLaneController : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] private float LaneOffset = 2f;
    [SerializeField] private float LaneChangeSpeed = 6f;
    [SerializeField] private float TiltAngle = 30f;
    [SerializeField] private float TiltSpeed = 10f;

    private int _laneIndex = 1;
    private Vector3 _targetPos;
    private float _targetTilt;
    private Camera _camera;
    private bool _inputReady;

    private void Start()
    {
        _camera = Camera.main;
        _targetPos = transform.position;
        SnapToLane(_laneIndex);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        var state = GameManager.Instance.State;

        if (state != GameState.Running && state != GameState.Finishing)
        {
            _inputReady = false;
            return;
        }

        if (!_inputReady)
        {
            _inputReady = true;
            return;
        }

        if (InputHelper.WasTap(out var clickX))
        {
            var playerX = _camera.WorldToScreenPoint(transform.position).x;
            var isTapLeft = clickX < playerX;

            _laneIndex += isTapLeft ? -1 : 1;
            _laneIndex = Mathf.Clamp(_laneIndex, 0, 2);

            SetTargetLane(_laneIndex);
            _targetTilt = isTapLeft ? -TiltAngle : TiltAngle;
        }

        if (Mathf.Abs(transform.position.x - _targetPos.x) < 0.5f)
            _targetTilt = 0f;

        transform.position = Vector3.Lerp(transform.position, _targetPos, LaneChangeSpeed * Time.deltaTime);
        var newY = Mathf.LerpAngle(transform.eulerAngles.y, _targetTilt, TiltSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, newY, 0f);
    }

    private void SetTargetLane(int lane)
    {
        var targetX = (lane - 1) * LaneOffset;
        _targetPos = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void SnapToLane(int lane)
    {
        SetTargetLane(lane);
        transform.position = _targetPos;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance == null) return;
        
        if (other.CompareTag(Tags.Obstacle))
        {
            GameManager.Instance.LoseToObstacle();
            return;
        }

        if (other.CompareTag(Tags.Finish))
        {
            GameManager.Instance.StartBossFight();
            return;
        }
        
        if (other.CompareTag(Tags.BuffPower))
        {
            GameManager.Instance.AddPower(10);
            var pooledObject = other.GetComponent<PooledObject>();
            pooledObject.ReturnToPool();
            return;
        }
        
        if (other.CompareTag(Tags.DebuffPower))
        {
            GameManager.Instance.AddPower(-10);
            var pooledObject = other.GetComponent<PooledObject>();
            pooledObject.ReturnToPool();
            return;
        }
    }
}