using System.Collections;
using System.Text.RegularExpressions;
using static Day16.Program;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day16
{
    internal class Program
    {
        internal static List<Valve> valves = new List<Valve>();
        static void Main(string[] args)
        {
            Console.WriteLine("Day 16!");

            foreach (string line in File.ReadLines("input.txt"))
            {
                Match match = Regex.Match(line, @"Valve (\w\w) has flow rate=(\d+); tunnels? leads? to valves? ([\w, ]+)");
                List<string> connectingValveNames = Regex.Matches(match.Groups[3].Value, @"(\w\w)").Select(m => m.Groups[1].Value).ToList();
                valves.Add(new Valve()
                {
                    Name = match.Groups[1].Value,
                    FlowRate = int.Parse(match.Groups[2].Value),
                    ConnectingValveNames = connectingValveNames,
                    IsOpen = false
                });
            }

            int maxMoves = 30;
            int moves = 0;
            int totalFlow = 0;
            int currentFlowRate = 0;
            Valve currentValve = valves.First(v => v.Name == "AA");
            List<Valve> examinedValves = new List<Valve>();
            List<Valve> openValves = new List<Valve>();
            Valve valveToOpen = null;
            List<List<Move>> moveSets = new List<List<Move>>();
            List<Move> movesSoFar = new List<Move>();

            //moveSets = EvaluateMoves(currentValve, new List<Move>(), 30);

            int bestMoveSetTotalFlow = 0;
            List<Move> bestMoveSet = EvaluateMoves(currentValve, new List<Move>(), 30);

            int moveCount = 1;
            foreach (Move move in bestMoveSet)
            {
                Console.WriteLine("== Minute " + moveCount + " ==");
                Console.WriteLine("current pressure: " + currentFlowRate);

                totalFlow += currentFlowRate;
                if (string.IsNullOrEmpty(move.OpenValve) == false)
                {
                    Console.WriteLine("Open valve " + move.OpenValve);
                    currentFlowRate += valves.First(v => v.Name == move.OpenValve).FlowRate;
                }
                else
                {
                    // move to valve
                    Console.WriteLine("Move to " + move.MoveToValve);
                }

                moveCount++;

                Console.WriteLine();
            }

            Console.Write("Total Flow: " + totalFlow);
        }

        internal static Hashtable BestRemainingMoves = new Hashtable();

        internal static List<Move> EvaluateMoves(Valve currentValve, List<Move> movesSoFar, int remainingMoves)
        {
            string openKey = string.Join("",movesSoFar.Where(m => string.IsNullOrEmpty(m.OpenValve) == false).Select(m => m.OpenValve).Order().ToArray());
            string bestMoveKey = string.Format("{0}-{1}-{2}", currentValve.Name, remainingMoves, openKey);
            BestMove bestRemainingMove = BestRemainingMoves[bestMoveKey] as BestMove;
            if(bestRemainingMove != null)
            {
                List<Move> newMoveSet = new List<Move>(movesSoFar);
                newMoveSet.AddRange(bestRemainingMove.Moves);
                return newMoveSet;
            }

            if (remainingMoves == 0)
            {
                return movesSoFar;
            }

            List<List<Move>> possibleMoveSets = new List<List<Move>>();

            List<Move> possibleMoves = EvaluateMovesHelper(currentValve, movesSoFar);
            foreach(Move move in possibleMoves)
            {
                List<Move> newMoveSet = new List<Move>(movesSoFar);
                newMoveSet.Add(move);
                if(string.IsNullOrEmpty(move.OpenValve) == false)
                {
                    possibleMoveSets.Add(EvaluateMoves(currentValve, newMoveSet, remainingMoves-1));
                }
                else
                {
                    possibleMoveSets.Add(EvaluateMoves(valves.First(v => v.Name == move.MoveToValve), newMoveSet, remainingMoves-1));
                }
            }

            //only return the best one
            int bestMoveSetTotalFlow = -1;
            List<Move> bestMoveSet = null;

            foreach (List<Move> moveSet in possibleMoveSets)
            {
                int currentFlowRate = 0;
                int totalFlow = 0;
                foreach (Move move in moveSet)
                {
                    totalFlow += currentFlowRate;
                    if (string.IsNullOrEmpty(move.OpenValve) == false)
                    {
                        currentFlowRate += valves.First(v => v.Name == move.OpenValve).FlowRate;
                    }
                    else
                    {
                        // move to valve
                    }
                }

                if (totalFlow > bestMoveSetTotalFlow)
                {
                    bestMoveSetTotalFlow = totalFlow;
                    bestMoveSet = moveSet;

                    List<Move> remainingMoveSet = new List<Move>(bestMoveSet);
                    remainingMoveSet.RemoveRange(0, 30 - remainingMoves);
                    BestRemainingMoves[bestMoveKey] = new BestMove()
                    {
                        Moves = remainingMoveSet,
                        TotalFlow = totalFlow
                    };
                }
            }

            if(bestMoveSet == null)
            {
                 throw new Exception("whaaa?");
            }

            return bestMoveSet;
        }

        internal class BestMove
        {
            internal List<Move> Moves;
            internal int TotalFlow;
        }

        internal static List<Move> EvaluateMovesHelper(Valve currentValve, List<Move> movesSoFar)
        {
            List<Move> possibleMoves = new List<Move>();

            if (currentValve.FlowRate > 0 && movesSoFar.Exists(m => m.OpenValve == currentValve.Name) == false)
            {
                possibleMoves.Add(new Move() { OpenValve = currentValve.Name });
            }

            foreach (Valve valve in currentValve.ConnectionValves)
            {
                possibleMoves.Add(new Move() { MoveToValve = valve.Name });
            }

            return possibleMoves;
        }

        internal class Valve
        {
            internal string Name;
            internal int FlowRate;
            internal List<string> ConnectingValveNames = new List<string>();
            internal List<Valve> connectionValvesField = null;
            internal List<Valve> ConnectionValves
            {
                get
                {
                    if (connectionValvesField == null)
                    {
                        connectionValvesField = new List<Valve>();
                        foreach(string name in ConnectingValveNames)
                        {
                            connectionValvesField.Add(valves.First(v => v.Name == name));
                        }
                    }
                    return connectionValvesField;
                }
            }
            internal bool IsOpen = false;
        }

        internal class Move
        {
            internal string OpenValve;
            internal string MoveToValve;
        }
    }
}