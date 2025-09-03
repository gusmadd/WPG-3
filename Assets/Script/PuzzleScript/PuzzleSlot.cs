using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class PuzzleSlot : MonoBehaviour, IDropHandler
{
    public int slotID;

    public void OnDrop(PointerEventData eventData)
    {
        var piece = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<PuzzlePiece>() : null;
        if (piece == null) return;

        RectTransform slotRT = GetComponent<RectTransform>();
        piece.SnapTo(slotRT.anchoredPosition, slotID);
    }
}