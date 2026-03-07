using UnityEngine;

public class PlayerLaneController : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] private float LaneOffset = 2f;
    [SerializeField] private float LaneChangeSpeed = 6f;
    
    private int _laneIndex = 1;
    private Vector3 _targetPos;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _targetPos = transform.position;
        SnapToLane(_laneIndex);
    }

    private void Update()
    {
        if (GameManager.Instance != null && 
            GameManager.Instance.State != GameState.Running &&
            GameManager.Instance.State != GameState.Finishing)
            return;
        
        if (WasTap())
        {
            var clickX = Input.mousePosition.x;
            var playerX = _camera.WorldToScreenPoint(transform.position).x;
            var isTapLeft = clickX < playerX;

            _laneIndex += isTapLeft ? -1 : 1;
            _laneIndex = Mathf.Clamp(_laneIndex, 0, 2);

            SetTargetLane(_laneIndex);
        }
        transform.position = Vector3.Lerp(transform.position, _targetPos, LaneChangeSpeed * Time.deltaTime);
    }

    private bool WasTap()
    {
        if (Input.GetMouseButtonDown(0))
            return true;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            return true;

        return false;
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
            GameManager.Instance.Lose();
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