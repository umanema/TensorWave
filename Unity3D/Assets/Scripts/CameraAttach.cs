using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraAttach : MonoBehaviour
{
    public GameObject target;
    Camera camera;
    Vector3 prevPos = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.LookAt(target.transform);
        if (prevPos.x != target.transform.position.x || prevPos.z != target.transform.position.z)
        {
            DOTween.Clear();
            camera.transform.DOLocalMoveX(target.transform.position.x, 4f);
            camera.transform.DOLocalMoveZ(target.transform.position.z - 7, 1f);
            prevPos = target.transform.localPosition;
        }
    }
}
