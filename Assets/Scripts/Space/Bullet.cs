using Common;
using MLAPI;
using UnityEngine;

public class Bullet : NetworkedBehaviour
{
    public readonly float speed = 400f;

    private void OnCollisionEnter(Collision other)
    {
        if (IsServer)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(LayerHelper.ENEMY))
            {
                Debug.Log("Bullet hit Enemy");
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer(LayerHelper.PLAYERS))
            {
                Debug.Log("Bullet hit PLAYERS");
                other.gameObject.GetComponent<Player.Player>().ResetPlayer();
            }
            else
            {
                Debug.Log("Bullet hit something else");
            }
            Destroy(gameObject);
        }
    }
}
