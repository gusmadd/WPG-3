using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int correctSlotID;              // ID slot yang benar
    public int currentSlotID = -1;         // ID slot sekarang (-1 = belum di slot)
    public bool IsPlacedCorrect => currentSlotID == correctSlotID;

    public event Action<bool> onPlacedChanged;

    RectTransform rt;
    Canvas canvas;
    CanvasGroup cg;
    Vector2 startPos;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        startPos = rt.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        cg.blocksRaycasts = false; // biar Slot bisa menerima drop
    }

    public void OnDrag(PointerEventData e)
    {
        rt.anchoredPosition += e.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        cg.blocksRaycasts = true;
        onPlacedChanged?.Invoke(IsPlacedCorrect);
    }

    public void SnapTo(Vector2 anchoredPos, int slotID)
    {
        rt.anchoredPosition = anchoredPos;
        currentSlotID = slotID;
        onPlacedChanged?.Invoke(IsPlacedCorrect);
    }

    public void ResetToStart()
    {
        currentSlotID = -1;
        rt.anchoredPosition = startPos;
        onPlacedChanged?.Invoke(false);
    }
}