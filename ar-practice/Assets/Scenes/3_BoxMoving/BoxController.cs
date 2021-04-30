using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{

    [SerializeField] const float CameraDistance = 20.5f;
    public float positionY = 0.4f;
    public GameObject[] prefab;
    private Camera mainCamera;
    
    private GameObject HoldingObject;
    private Vector3 InputPosition;
    void Start()
    {
        mainCamera = Camera.main;
        Reset();
    }



    // Update is called once per frame
    void Update()
    {

#if !UNITY_EDITOR
    if(Input.touchCount == 0 ) return;
#endif
        InputPosition = TouchHelper.TouchPosition;

        if (TouchHelper.Touch2)
        {
            Reset();
            return;
        }
        if (HoldingObject)
        {
            if (TouchHelper.IsUp)
            {
                OnPut(InputPosition);
                HoldingObject = null;
                return;
            }
            Move(InputPosition);
            return;
        }
        if (!TouchHelper.IsDown)
        {
            return;
        }
        if (Physics.Raycast(mainCamera.ScreenPointToRay(InputPosition), out var hit, mainCamera.farClipPlane))
        {
            if (hit.transform.gameObject.tag.Equals("Player"))
            {
                HoldingObject = hit.transform.gameObject;
                OnHold();
            }
        }


    }

    private void OnPut(Vector3 pos)
    {
        HoldingObject.GetComponent<Rigidbody>().useGravity = true;
        HoldingObject.transform.SetParent(null);
    }

    private void Move(Vector3 pos)
    {
        pos.z = mainCamera.nearClipPlane * CameraDistance;
        HoldingObject.transform.position = Vector3.Lerp(HoldingObject.transform.position, mainCamera.ScreenToWorldPoint(pos), Time.deltaTime * 7f);
    }

    private void OnHold()
    {
        HoldingObject.GetComponent<Rigidbody>().useGravity = false;

        HoldingObject.transform.SetParent(mainCamera.transform);
        HoldingObject.transform.rotation = Quaternion.identity;
        HoldingObject.transform.position =
               mainCamera.ViewportToWorldPoint(new Vector3(0.5f, positionY, mainCamera.nearClipPlane * CameraDistance));
    }

    private void Reset()
    {
        var pos = mainCamera.ViewportToWorldPoint(new Vector3(
            0.5f, positionY, mainCamera.nearClipPlane * CameraDistance));
        var obj = Instantiate(prefab[0], pos, Quaternion.identity, mainCamera.transform);
        var rigidbody = obj.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

    }
}
