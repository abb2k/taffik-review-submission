using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuffCradSpawner : MonoBehaviour
{
    public event UnityAction onCardSelected;

    public GameObject cardPrefab;
    public List<Card> possibleCards;

    public List<Transform> positions;

    public void CreateCards()
    {
        int amount = positions.Count;

        for (int i = 0; i < amount; i++)
        {
            GameObject spawnedCard = Instantiate(cardPrefab, positions[i]);
            buffCard card = spawnedCard.GetComponent<buffCard>();
            card.setCard(possibleCards[Random.Range(0, possibleCards.Count)]);
            card.onSelected += onCardSelect;
        }
    }

    public void onCardSelect(GameValues toAdd, float multiplierAdded)
    {
        LevelManager levelManager = GameManager.get().getLevelManager();

        levelManager.AddValues(toAdd);
        levelManager.addScoreMultiplier(multiplierAdded);

        onCardSelected.Invoke();

        clearCards();
    }

    public void clearCards()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i].childCount == 0) return;

            Destroy(positions[i].GetChild(0).gameObject);
        }
    }
}
