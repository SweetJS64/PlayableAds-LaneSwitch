using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private float Speed = 7f;
    [SerializeField] private float DestroyZ = -5f;

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Win ||
            GameManager.Instance.State == GameState.Lose ||
            GameManager.Instance.State == GameState.BossFight)
            return;
        
        transform.Translate(Vector3.back * Speed * Time.deltaTime, Space.World);

        if (transform.position.z <= DestroyZ)
            Destroy(gameObject);
    }
}