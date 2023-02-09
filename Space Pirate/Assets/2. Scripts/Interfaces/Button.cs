using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Material offMaterial;
    [SerializeField] Material onMaterial;

    MeshRenderer mesh;

    [Header("Flag Recipients")]
    [SerializeField] GameObject pressLink;
    [SerializeField] GameObject holdLink;

    [Header("Other")]
    [SerializeField] float animationSpeed;
    
    [SerializeField] float offHeight;
    [SerializeField] float onHeight;

    [SerializeField] Transform model;

    public bool selected;
    public bool held;
    public bool continuing;

    void Start()
    {
        held = false;
        selected = false;

        mesh = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected || held) {
            mesh.material = onMaterial;
        } else {
            mesh.material = offMaterial;
        }

        Debug.Log(onHeight + " " + model.localPosition + " " + offHeight);
        if (held || continuing) {
            if (model.localPosition.y > onHeight) {
                model.Translate(0, -animationSpeed * Time.deltaTime, 0);
            } else {
                continuing = false;
            }
        } else {
            if (model.localPosition.y < offHeight) {
                model.Translate(0, animationSpeed * Time.deltaTime, 0);
            }
        }

        if (selected) {
            selected = false;
        }
    }

    public void Press()
    {
        StartCoroutine(AutoHold());
    }

    IEnumerator AutoHold() {
        held = true;
        
        yield return new WaitForSeconds(0.5f);

        held = false;
    }
}
