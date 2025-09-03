using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class PuzzleManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject puzzlePanel;   // drag panel canvas puzzle
    [SerializeField] private PuzzlePiece[] pieces;     // drag semua piece di sini
    [SerializeField] public TextMeshProUGUI puzzleCompleteText;
    private bool puzzleSolved = false;
    public static PuzzleManager Instance { get; private set; }
    public System.Action OnPuzzleSolved;
    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // subscribe supaya tiap perubahan placement langsung dicek
        foreach (var p in pieces)
        {
            if (p != null) p.onPlacedChanged += OnPiecePlacedChanged;
        }

        if (puzzleCompleteText != null)
            puzzleCompleteText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        foreach (var p in pieces)
        {
            if (p != null) p.onPlacedChanged -= OnPiecePlacedChanged;
        }
    }

    private void OnPiecePlacedChanged(bool _)
    {
        foreach (var p in pieces)
        {
            if (p == null || !p.IsPlacedCorrect) return;
        }

        puzzleSolved = true;
        Debug.Log("Puzzle solved!");

        if (puzzleCompleteText != null)
            puzzleCompleteText.gameObject.SetActive(true);

    }

    private void Update()
    {
        if (puzzleSolved && Input.GetKeyDown(KeyCode.Return)) //puzzle selesai, tekan enter
        {
            if (puzzlePanel) puzzlePanel.SetActive(false);
            if (puzzleCompleteText != null)
                puzzleCompleteText.gameObject.SetActive(false);
                Debug.Log("Puzzle panel ditutup.");
            OnPuzzleSolved?.Invoke();
        }
    }
}
