﻿using System;
using PureFunctions.UnitySpecific;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckOfCards : Singleton<DeckOfCards>
    {
        [SerializeField] private bool allowDuplicateCards;
        [SerializeField] private Card[] cards;
        public Sprite cardBack;

        private void Awake()
        {
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i].Initialise(i);
            }
        }

        public Card TakeRandomCard()
        {
            while (true)
            {
                var cardIndex = Random.Range(0, cards.Length);
                
                if (allowDuplicateCards) return cards[cardIndex];
                
                if (cards[cardIndex].InPlay) continue;

                cards[cardIndex].MarkActive();
                
                return cards[cardIndex];
            }
        }

        public void ReturnCardToDeck(Card card)
        {
            ReturnCardToDeck(card.DeckIndex);
        }
        
        public void ReturnCardToDeck(int card)
        {
            if (!cards[card].InPlay && !allowDuplicateCards) throw new Exception($"Cheat attempt or critical error. Attempting to return duplicate card to deck and deck type does not support this. (card index: {card})");
            cards[card].MarkInActive();
        }
    }
