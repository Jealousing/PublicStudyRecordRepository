using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanCollider : MonoBehaviour
{
    List<InteractableObject> interactables = new List<InteractableObject>();
    public MeshRenderer[] arr;
    public void ScanStart()
    {
        for (int i = 0; i < arr.Length; i++) arr[i].enabled = true;

        this.gameObject.SetActive(true);
    }

    public void MeshOff()
    {
        for (int i = 0; i < arr.Length; i++) arr[i].enabled = false;
    }

    public void Scanning(float progress)
    {
        SetScale(progress * 100);
    }

    public IEnumerator ScanEnd()
    {
        SetScale(0.2f);

        yield return new WaitForSeconds(5.0f);

        foreach (InteractableObject obj in interactables)
        {
            obj.ToggleScanEffect(false);
        }
        interactables.Clear();

        yield return null;

        this.gameObject.SetActive(false);
    }


    private void SetScale(float scale)
    {
        this.gameObject.transform.localScale = new Vector3(scale,scale,scale);
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject obj = other.GetComponent<InteractableObject>();
        if(obj !=null)
        {
            interactables.Add(obj);
            obj.ToggleScanEffect(true);
        }
    }

}
