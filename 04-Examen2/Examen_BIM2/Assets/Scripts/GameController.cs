using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;

    public List<Sprite> gamepuzzles = new();

    public List<Button> btns = new();

    public bool firstGuess, secondGuess;

    private int countGuesses, countCorrectGuesses, gameGuesses, firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/animals");    
    }
    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamepuzzles);
        gameGuesses = gamepuzzles.Count / 2;
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles()
    {
        int index = 0;
        for (int i = 0; i < btns.Count; i++)
        {
            if (index == btns.Count / 2)
            {
                index = 0;
            }
            gamepuzzles.Add(puzzles[index]);
            index ++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }
    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamepuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamepuzzles[firstGuessIndex];
        } else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamepuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamepuzzles[secondGuessIndex];
            countGuesses++;
            StartCoroutine(CheckIfThePuzzleMatch());
        }
    }

    IEnumerator CheckIfThePuzzleMatch()
    {
        yield return new WaitForSeconds(.5f);
        if (firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex)
        {
            yield return new WaitForSeconds(.2f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0,0,0,0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);
            CheckIfTheGamneIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(.2f);
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }
        yield return new WaitForSeconds(.2f);
        firstGuess = secondGuess = false;
    }

    void CheckIfTheGamneIsFinished()
    {
        countCorrectGuesses++;
        if (countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Fin del juego");
            Debug.Log("Tomó " + countGuesses + " intentos para ganar");
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randIndex = Random.Range(i, list.Count);
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
