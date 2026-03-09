using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "Game/Movement Config")]
public class MovementConfigSO : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed { get; private set; } = 7f;
    [field: SerializeField] public float DespawnZ { get; private set; } = -5f;
    [field: SerializeField] public float LaneOffset { get; private set; } = 2f;
}