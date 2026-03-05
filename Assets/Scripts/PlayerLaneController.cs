using UnityEngine;

public class PlayerLaneController : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] private float LaneOffset = 2f;
    [SerializeField] private float LaneChangeSpeed = 6f;
    
    private int _laneIndex = 1;
    private Vector3 _targetPos;

    private void Start()
    {
        _targetPos = transform.position;
        SnapToLane(_laneIndex);
    }

    private void Update()
    {
        if (WasTap())
        {
            var clickX = Input.mousePosition.x;
            var playerX = Camera.main.WorldToScreenPoint(transform.position).x;
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
}