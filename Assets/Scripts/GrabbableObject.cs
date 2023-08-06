using UnityEngine;
using DG.Tweening;

public enum TargetOrientation
{
    idle,
    lyingOnSide
}

[RequireComponent(typeof(Collider))]
public class GrabbableObject : MonoBehaviour
{
    public Transform targetObj;

    public int partID;

    public TargetOrientation TargetOrientation;

    private Camera _mainCamera;

    private bool attached;

    private Vector3 initialPos;

    private Collider _collider;

    private void Start()
    {
        _mainCamera = Camera.main;

        _collider = GetComponent<Collider>();

        initialPos = transform.position;
    }

    public void OnMouseEnter()
    {
        //Debug.Log(partName);
    }

    public void OnMouseDown()
    {
        PlacementManager.Instance.OnPartGrabbed(targetObj, partID, this);

    }

    public void OnMouseDrag()
    {
        if (!attached)
        {
            Vector3 mousePos = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.z));

            transform.position = new Vector3(mousePos.x, transform.position.y, mousePos.z);
        }
    }


    public void OnMouseUp()
    {

        if (!attached)
        {
            PlacementManager.Instance.OnPartRelased();
            BackToInitial();
        }
    }

    public void OnGrabbablePlaced()
    {
        attached = true;

        _collider.enabled = false;
    }

    public void OnGrabbableDetached()
    {
        attached = false;

        _collider.enabled = true;

    }

    private void BackToInitial()
    {
        transform.DOMove(initialPos, 0.5f).SetEase(Ease.OutBounce);
    }

}
