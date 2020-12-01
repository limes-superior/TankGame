using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Player : NetworkedBehaviour
    {
        public Transform cannonTransform;
        public GameObject bulletPrefab;

        private Rigidbody rb;
        private float thrust;
        private float rotation;

        private readonly float maxSpeed = 30f;
        private readonly float rotationSpeed = 250f;

        public override void NetworkStart()
        {
            base.NetworkStart();
            rb = GetComponent<Rigidbody>();

            if (IsLocalPlayer)
            {
                SetupControls();
            }
        }

        private void FixedUpdate()
        {
            if (!IsLocalPlayer)
            {
                return;
            }

            rb.AddRelativeForce(Vector3.forward * (thrust * (Time.fixedDeltaTime * 500f)), ForceMode.Force);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, -rotation * rotationSpeed * Time.fixedDeltaTime,0));

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
        }
        [ServerRPC(RequireOwnership = false)]
        public void ResetPlayerServer()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.up * 2;
            Debug.Log("Player " + NetworkingManager.Singleton.LocalClientId);
        }
        public void ResetPlayer()
        {
            /*
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.position = Vector3.up*2;
            Debug.Log("Player "+NetworkingManager.Singleton.LocalClientId);*/
            InvokeServerRpc(ResetPlayerServer);
            
        }

        [ServerRPC]
        public void CmdFire()
        {
            var actualRotation = transform.rotation;
            Debug.Log(bulletPrefab.name);
            var bullet = Instantiate(bulletPrefab, cannonTransform.position, actualRotation);
            var bulletScript = bullet.GetComponent<Bullet>();
            var bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = rb.velocity;
            bulletRb.AddRelativeForce(Vector3.forward * bulletScript.speed);

            bullet.GetComponent<NetworkedObject>().Spawn();
            Destroy(bullet, 4f);
        }

        private void SetupControls()
        {
            var leftRight = new InputAction("LeftRight");
            leftRight.AddCompositeBinding("Axis")
                .With("Positive", "<Keyboard>/rightArrow")
                .With("Negative", "<Keyboard>/leftArrow");
            leftRight.Enable();

            var topBottom = new InputAction("LeftRight");
            topBottom.AddCompositeBinding("Axis")
                .With("Positive", "<Keyboard>/upArrow")
                .With("Negative", "<Keyboard>/downArrow");
            topBottom.Enable();

            var fireButton = new InputAction("Fire");
            fireButton.AddBinding("<Keyboard>/space");
            fireButton.Enable();

            leftRight.started += ctx => rotation = -ctx.ReadValue<float>();
            leftRight.canceled += ctx => rotation = 0f;
            topBottom.started += ctx => thrust = ctx.ReadValue<float>();
            topBottom.canceled += ctx => thrust = 0f;
            fireButton.performed += ctx => InvokeServerRpc(CmdFire);
        }
    }
}
