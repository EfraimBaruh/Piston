using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{

    public static PlacementManager Instance { get; private set; }


    [SerializeField] private List<Transform> Objects;

    private Dictionary<Transform, List<PlaceHolder>> ObjectPlacements = new Dictionary<Transform, List<PlaceHolder>>();

    private GrabbableObject _grabbedObject;
    private PlaceHolder _placeHolder;
    private KeyValuePair<Transform, List<PlaceHolder>> _currentObjectPair;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        GatherPlacements();
    }

    private void GatherPlacements()
    {
        foreach(Transform objectModel in Objects)
        {
            PlaceHolder[] placeHolders = objectModel.GetComponentsInChildren<PlaceHolder>();
            ObjectPlacements.Add(objectModel, placeHolders.ToList());
        }
    }

    public void OnPartGrabbed(Transform targetObject, int partID, GrabbableObject grabbable)
    {
        _grabbedObject = grabbable;

        _currentObjectPair = GetTargetPlacement(targetObject);

        SetTargetOrientation(grabbable.TargetOrientation);

        _placeHolder = GetTargetPlaceHolder(_currentObjectPair.Value, partID);

        _placeHolder.DoHighlight(true);
    }

    public void OnPartRelased()
    {
        _grabbedObject = null;

        _placeHolder.DoHighlight(false);

        _placeHolder = null;
    }

    public void OnPartPlaced()
    {
        _grabbedObject.OnGrabbablePlaced();

        _grabbedObject = null;

        _placeHolder.DoHighlight(false);

        UpdatePlacementBlockages();

        ControlForCompleteness();

        _placeHolder = null;
    }

    private KeyValuePair<Transform, List<PlaceHolder>> GetTargetPlacement(Transform targetObj)
    {
        foreach(KeyValuePair<Transform, List<PlaceHolder>> pair in ObjectPlacements)
        {
            if(pair.Key == targetObj)
            {
                return pair;
            }
        }

        throw new Exception("target object not found");
    }

    private PlaceHolder GetTargetPlaceHolder(List<PlaceHolder> placeHolders, int partID)
    {
        foreach(PlaceHolder placeHolder in placeHolders)
        {
            if (placeHolder.holder.ID == partID)
                return placeHolder;
        }

        throw new Exception("Place holder with ID:" + partID + " does not exist");
    }

    private void UpdatePlacementBlockages()
    {
        foreach(PlaceHolder p in _currentObjectPair.Value)
        {
            if (p.holder.blockedBy == _placeHolder.holder.ID)
                p.holder.blockedBy = -1;
        }
    }

    private void ControlForCompleteness()
    {
        foreach (PlaceHolder p in _currentObjectPair.Value)
        {
            if (!p.holder.isFilled)
                return;
        }
        Debug.Log("complete");
        PopUpManager.Instance.PopUP(1);
        _currentObjectPair.Key.GetComponent<Animator>().SetTrigger("success");

    }

    private void SetTargetOrientation(TargetOrientation orientation)
    {
        _currentObjectPair.Key.GetComponent<Animator>().SetTrigger(orientation == TargetOrientation.lyingOnSide ? "side" : "idle");
    }
}
