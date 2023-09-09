using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private Vector3 _position;

        private void Awake()
        {
            if (!_player)
                _player = FindObjectOfType<Player>().transform;
        }
        private void Update()
        {
            _position = _player.position;
            _position.z = -10f;
            _position.y += 1f;

            transform.position = Vector3.Lerp(transform.position, _position, Time.deltaTime);
        }
    }
}