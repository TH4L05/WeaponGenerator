using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public Transform roomTransform;
    public Transform miniRoomTransform;
    public Transform player;
    public Transform miniPlayer;

    public UnityEvent OnUpArrowPressed;
    public UnityEvent OnDownArrowPressed;
    public UnityEvent OnLeftArrowPressed;
    public UnityEvent OnRightArrowPressed;

    public GameObject levelRoom;
    public GameObject miniRoom;

    public Material wallMaterial;
    public Material doorMaterial;
    public Material puzzleMaterial;


    private void Awake()
    {
        miniRoom = Instantiate(levelRoom);
        //miniRoom.transform.position = new Vector3(0f, 0.5f, 0f);
        miniRoom.transform.parent = miniRoomTransform;
        miniRoom.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
        miniRoom.transform.localPosition = new Vector3(0f, 0f, 0f);
        SetMaterial(miniRoom);
    }

    private void SetMaterial(GameObject obj)
    {
        var tag = obj.tag;
        var mr = obj.GetComponent<MeshRenderer>();

        if (mr != null)
        {
            switch (tag)
            {
                case "Wall":
                    mr.material = wallMaterial;
                    break;

                case "Door":
                    mr.material = doorMaterial;
                    break;

                case "Puzzle":
                    mr.material = puzzleMaterial;
                    break;

                case "Ignore":
                    Destroy(obj);
                    break;

                default:
                    break;
            }
        }
        
        foreach (Transform child in obj.transform)
        {
            SetMaterial(child.gameObject);
        }
    }


    private void Update()
    {
        //Debug.Log(roomTransform.localEulerAngles);
        miniRoomTransform.rotation = roomTransform.rotation;
        miniPlayer.rotation = player.rotation;
    }

    public void UpArrowIsPressed()
    {
        OnUpArrowPressed?.Invoke();
    }

    public void DownArrowIsPressed()
    {
        OnDownArrowPressed?.Invoke();
    }

    public void LeftArrowIsPressed()
    {
        OnLeftArrowPressed?.Invoke();
    }

    public void RightArrowIsPressed()
    {
        OnRightArrowPressed?.Invoke();
    }

}
