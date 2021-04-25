using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFactDetector : MonoBehaviour
{
    public FactReference lanternFact;
    // Start is called before the first frame update

    public float timeUpsideDown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var anglesZ = transform.rotation.eulerAngles.z;
        var upsideDown = anglesZ > 90 && anglesZ < 270;
        if (upsideDown) {
            this.timeUpsideDown += Time.deltaTime;
            if (timeUpsideDown > 3) {
                lanternFact.Discover();
            }
        } else {
            this.timeUpsideDown = 0;
        }

    }
}
