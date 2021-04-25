using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MouseFollowingBehaviour : MonoBehaviour
{
    private Camera _camera;
    public bool smooth;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        var newPositon = _camera.ScreenToWorldPoint(Input.mousePosition);
        newPositon.z = 0;
        transform.position = newPositon;
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera != null) {
            if (smooth) {
                var targetPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = 0;
                transform.position += (targetPosition - transform.position) * (Time.deltaTime * 10);
            } else {
                var newPositon = _camera.ScreenToWorldPoint(Input.mousePosition);
                newPositon.z = 0;
                transform.position = newPositon;
                
            }
        }
    }
}
