using System.Collections;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _2023;
public class Day7
{   
    private static string filename = "day7/input.txt";
    
    // class with a source range start int, destination range start int, and range length int
    public class CardHand
    {
        public string rawHand;
        public string handRank;
        public int bid;

        public void SetCardRank()
        {
            int cardRank = 0;
            List<char> distinctCards = rawHand.Distinct().ToList();
            
                // more than one
                int maxOfCard = 0;
                bool foundThreeOfAKind = false;
                bool foundPair = false;
                
                for(int i = 0; i < distinctCards.Count; i++)
                {
                    if(distinctCards[i] == 'J')
                    {
                        continue; // skip
                    }

                    int count = rawHand.Count(x => x == distinctCards[i]);
                    if(count == 5)
                    {
                        cardRank = 9;
                        Console.WriteLine("Five of a kind!");
                        break;
                    }
                    else if(count == 4)
                    {
                        cardRank = 8;
                        Console.WriteLine("Four of a kind!");
                        break;
                    }
                    else if(count == 3)
                    {
                        foundThreeOfAKind = true;
                        if(foundPair)
                        {
                            cardRank = 7;
                            Console.WriteLine("Full house!");
                            break;
                        }
                    }
                    else if(count == 2)
                    {
                        if(foundPair)
                        {
                            cardRank = 5;
                            Console.WriteLine("Two pairs!");
                            break;
                        }
                        else
                        {
                            foundPair = true;
                            if(foundThreeOfAKind)
                            {
                                cardRank = 7;
                                Console.WriteLine("Full house!");
                                break;
                            }
                        }
                    }
                }

                if(cardRank == 0)
                {
                    if(foundThreeOfAKind)
                    {
                        cardRank = 6;
                        Console.WriteLine("Three of a kind!");
                    }
                    else if(foundPair)
                    {
                        cardRank = 2;
                        Console.WriteLine("Pair!");
                    }
                }

                if(distinctCards.Contains('J'))
                {
                    int count = rawHand.Count(x => x == 'J');
                    
                    switch(cardRank)
                    {
                        case 0: // only high card
                            if(count == 5)
                            {
                                cardRank = 9; // five of a kind
                            } 
                            else if(count == 4)
                            {
                                cardRank = 9; // five of a kind
                            }
                            else if(count == 3)
                            {
                                cardRank = 8; // four of a kind
                            }
                            else if(count == 2)
                            {
                                cardRank = 6; // three of a kind
                            }
                            else {
                                cardRank = 2; // one pair
                            }
                            break;
                        case 2: // pair
                            if(count == 3)
                            {
                                cardRank = 9; // five of a kind
                            }
                            else if(count == 2)
                            {
                                cardRank = 8; // four of a kind
                            }
                            else if(count == 1)
                            {
                                cardRank = 6; // three of a kind
                            }
                            break;
                        case 5: // two pair
                            cardRank = 7; // full house
                            break;
                        case 6: // three of a kind
                            if(count == 2)
                            {
                                cardRank = 9; // five of a kind
                            }
                            else if(count == 1)
                            {
                                cardRank = 8; // four of a kind
                            }
                            break;
                        case 7: // full house
                            throw new Exception("impossible");
                        case 8: // four of a kind
                            cardRank = 9; // five of a kind
                            break;
                        case 9: // five of a kind
                            throw new Exception("impossible");
                        default:
                            throw new Exception("impossible! cardRank: " + cardRank);
                    }
            }

            //convert characters to hex value
            handRank = string.Concat(cardRank, rawHand.Replace('A','E').Replace('T','A').Replace('J','1').Replace('Q','C').Replace('K','D'));

            //Console.WriteLine("Hex hand: " + hexHand);

            //cardRank = int.Parse(hexHand, System.Globalization.NumberStyles.HexNumber);
            //cardRank = int.Parse(Convert.FromHexString(hexHand));

            Console.WriteLine("Hand rank: " + handRank);
        }

    }

    private static List<CardHand> ParseCards()
    {
        List<CardHand> cardHands = new List<CardHand>();
        System.IO.StreamReader file = new System.IO.StreamReader(filename);
        string line;
        while((line = file.ReadLine()) != null)
        {

            string[] lineSplits = line.Trim().Split(" ");
            CardHand cardHand = new CardHand();
            cardHand.rawHand = lineSplits[0];
            cardHand.bid = int.Parse(lineSplits[1]);
            Console.WriteLine("Found card hand: " + cardHand.rawHand + ", bid: " + cardHand.bid);
            cardHand.SetCardRank();
            cardHands.Add(cardHand);
        }

        file.Close();

        return cardHands;
    }

    public static void RunPart1()
    {
        List<CardHand> cards = ParseCards().OrderBy(x => x.handRank).ToList();

        long totalWinnings = 0;
        for(int i = 0; i < cards.Count; i++)
        {
            totalWinnings += cards[i].bid * (i+1);
            Console.WriteLine("Card " + (i+1) + ": " + cards[i].handRank + " (" + cards[i].rawHand + ", bid: " + cards[i].bid + ")");
        }

        Console.WriteLine("Part 1 : " + totalWinnings);
        // 253113544 is too low
        // 253205868 is right!

        // part 2:
        // 252352670 is too low
        // 253689008 is too low
        // 253907829 is right!
    }

    public static void RunPart2()
    {
        ParseCards();
    }
}