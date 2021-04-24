using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MouseFollowingBehaviour : MonoBehaviour
{
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera != null) {
            var newPositon = _camera.ScreenToWorldPoint(Input.mousePosition);
            newPositon.z = 0;
            transform.position = newPositon;
        }
    }
}
