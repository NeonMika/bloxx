using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltInTypes

public class MoveEyes : MonoBehaviour
{
    public GameObject LeftEye;
    public GameObject LeftPupil;
    public GameObject RightEye;
    public GameObject RightPupil;

    public bool EnabledEyeMovement;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (EnabledEyeMovement)
        {
            Vector3 mouseOnViewport = Input.mousePosition;
            float xPercent = 1.0f * mouseOnViewport.x / Camera.main.pixelWidth - 0.5f;
            float yPercent = 1.0f * mouseOnViewport.y / Camera.main.pixelHeight - 0.5f;
            Debug.Log((xPercent, yPercent));
            LeftPupil.transform.localPosition =
                new Vector3(-xPercent * 0.6f, LeftPupil.transform.localPosition.y, -yPercent * 0.6f);
            RightPupil.transform.localPosition =
                new Vector3(-xPercent * 0.6f, RightPupil.transform.localPosition.y, -yPercent * 0.6f);
        }
    }
}