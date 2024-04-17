using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameBillboarding : MonoBehaviour
{
    public Transform target;
    public TextMeshPro usernameText;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetName(string name)
    {
        usernameText = GetComponent<TextMeshPro>();
        usernameText.SetText(name);
    }

    void LateUpdate()
    {
        if (target == null && Camera.main != null)
        {
            target = Camera.main.transform;
        }

        if (target != null)
        {
            //transform.LookAt(target);

            transform.rotation = Quaternion.LookRotation(transform.position - target.position);

        }
    }
}
