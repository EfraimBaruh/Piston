using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(HighlightGlow))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Renderer))]
public class PlaceHolder : MonoBehaviour
{
    public Holder holder;

    private HighlightGlow highlightGlow;
    private BoxCollider _collider;
    private Renderer _renderer;
    
    private void Start()
    {
        highlightGlow = GetComponent<HighlightGlow>();
        _collider = GetComponent<BoxCollider>();
        _renderer = GetComponent<Renderer>();
    }

    public void DoHighlight(bool highlight)
    {
        if(highlight && holder.blockedBy >= 0)
        {
            PopUpManager.Instance.PopUP(0);
            return;
        }


        highlightGlow.DoHighLight(highlight);
        _collider.enabled = highlight;
    }

    private void OnTriggerEnter(Collider other)
    {
        holder.isFilled = true;

        PlacementManager.Instance.OnPartPlaced();

        _renderer.enabled = false;

        other.transform.parent = transform;

        other.transform.DOLocalMove(holder.offsetPos, 0.2f);
        other.transform.DOLocalRotate(holder.offsetRot, 0.2f);
    }
}

[System.Serializable]
public class Holder
{
    public int ID;
    public bool isFilled;
    public int blockedBy = -1;

    public Vector3 offsetPos;
    public Vector3 offsetRot;
}
