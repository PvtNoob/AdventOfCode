namespace Day07 {
    internal class Hand : IComparable {
        private readonly (char card, int value)[] CardValues;
        private readonly int[] Cards;
        private readonly HandType Type;

        public long Bid { get; private set; }

        public Hand((char card, int value)[] cardValues, string cards, int bid) {
            CardValues = cardValues;
            Cards = cards.Select(ch => CardValues.First(cardValue => cardValue.card == ch).value).ToArray();
            Bid = bid;
            Type = GetHandType();
        }

        public HandType GetHandType() {
            List<int> amounts = [];
            for (int cardValue = 0; cardValue <= 12; cardValue++) {
                amounts.Add(Cards.Count(handcard => handcard == cardValue));
            }
            amounts = amounts.OrderByDescending(x => x).ToList();
            int biggestAmount = amounts[0];
            int secondBiggestAmount = amounts[1];
            int amountOfJoker = Cards.Count(handcard => handcard == -1);

            if (biggestAmount + amountOfJoker == 5) {
                return HandType.FiveOfAKind;
            } else if (biggestAmount + amountOfJoker == 4) {
                return HandType.FourOfAKind;
            } else if (biggestAmount + amountOfJoker == 3) {
                if (secondBiggestAmount == 2) {
                    return HandType.FullHouse;
                } else {
                    return HandType.ThreeOfAKind;
                }
            } else if (biggestAmount + secondBiggestAmount + amountOfJoker == 4) {
                return HandType.TwoPair;
            } else if (biggestAmount + amountOfJoker == 2) {
                return HandType.OnePair;
            } else {
                return HandType.HighCard;
            }
        }

        public int CompareTo(object? obj) {
            Hand b = (Hand)obj;
            if (Type > b.Type) {
                return 1;
            } else if (Type < b.Type) {
                return -1;
            } else {
                for (int i = 0; i < Cards.Length; i++) {
                    if (Cards[i] > b.Cards[i]) {
                        return 1;
                    } else if (Cards[i] < b.Cards[i]) {
                        return -1;
                    }
                }
                return 0;
            }
        }
    }
}