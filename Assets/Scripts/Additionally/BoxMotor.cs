using UnityEngine;
using System.Collections;

namespace Motors.BoxMotors
{
    public class BoxMotor : MonoBehaviour
    {
        public Vector3[] path;
        public float movingTime;
        private float currentTime;

        private bool _isMoving = false;

        private int currentPathIndex;
        private Vector3 currentPosition;

        private GameObject _player;
        private Vector3 playerVector;
        private PlayerController playerController;

        public bool isMoving
        {
            get
            {
                return _isMoving;
            }

            set
            {
                _isMoving = value;
                playerController.blockedByAnotherObject = value;
            }
        }

        public bool StartMoving()
        {
            if (path.Length == 0)
                return false;

            currentPathIndex = 0;
            currentPosition  = path[currentPathIndex];
            currentTime      = movingTime;

            isMoving = true;

            playerVector = (_player.transform.position - transform.position);

            return true;
        }

        void Start()
        {
            _player = GlobalOptions.Player;
            playerController = _player.GetComponent<PlayerController>();
        }

        void SetPosition(Vector3 position)
        {
            transform.position = position;
            _player.transform.position = new Vector3(transform.position.x + playerVector.x, transform.position.y + playerVector.y, transform.position.z + playerVector.z);
        }

        void Update()
        {
            if (!isMoving)
                return;

            if (currentTime < 0)
            {
                SetPosition(currentPosition);

                currentPathIndex++;

                if (currentPathIndex >= path.Length)
                {
                    isMoving = false;
                }
                else
                {
                    currentPosition = path[currentPathIndex];
                    currentTime = movingTime;
                }

                return;
            }

            SetPosition(Vector3.Lerp(transform.position, currentPosition, Time.deltaTime / currentTime));
            currentTime -= Time.deltaTime;
        }
    }
}
