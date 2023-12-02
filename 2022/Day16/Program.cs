using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using static Day16.Program;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day16
{
    internal class Program
    {
        internal static List<Valve> valves = new List<Valve>();
        internal static int TotalMinutes = 26;
        internal static Hashtable ShortestPaths = new Hashtable();
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

            int maxMoves = TotalMinutes;
            int moves = 0;
            int totalFlow = 0;
            int currentFlowRate = 0;
            Valve currentValve = valves.First(v => v.Name == "AA");
            List<Valve> examinedValves = new List<Valve>();
            List<Valve> openValves = new List<Valve>();
            Valve valveToOpen = null;
            List<List<Move>> moveSets = new List<List<Move>>();
            List<Move> movesSoFar = new List<Move>();

            // calculate shortest paths between valves with a value
            List<Valve> valvesWithValue = valves.Where(v => v.FlowRate > 0).ToList();
            foreach(Valve targetValve in valvesWithValue)
            {
                // find shortest path from every other valve with value
                foreach(Valve sourceValve in valvesWithValue.Append(currentValve))
                {
                    if (sourceValve != targetValve)
                    {
                        List<string> path = GetShortestPath(sourceValve, targetValve, valves.Count);
                        string key = string.Concat(sourceValve.Name, "-", targetValve.Name);
                        ShortestPaths[key] = path;
                    }
                }
            }

            CompleteOpenKey = string.Join("", valves.Select(v => v.Name).Order().ToArray());

            //moveSets = EvaluateMoves(currentValve, new List<Move>(), 30);

            int bestMoveSetTotalFlow = 0;
            List<Move> bestMoveSet = EvaluateMoves(currentValve, currentValve, new List<Move>(), TotalMinutes);

            int moveCount = 1;
            foreach (Move move in bestMoveSet)
            {
                Console.WriteLine("== Minute " + moveCount + " ==");
                Console.WriteLine("current pressure: " + currentFlowRate);

                totalFlow += currentFlowRate;

                if (move.NoMove)
                {
                    Console.WriteLine("All valves open");
                    Console.WriteLine();
                    continue;
                }

                if (string.IsNullOrEmpty(move.PersonOpenValve) == false)
                {
                    Console.WriteLine("Person Open valve " + move.PersonOpenValve);
                    currentFlowRate += valves.First(v => v.Name == move.PersonOpenValve).FlowRate;
                }
                else
                {
                    // move to valve
                    Console.WriteLine("Person Move to " + move.PersonMoveToValve);
                }

                if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
                {
                    Console.WriteLine("Elephant Open valve " + move.ElephantOpenValve);
                    currentFlowRate += valves.First(v => v.Name == move.ElephantOpenValve).FlowRate;
                }
                else
                {
                    // move to valve
                    Console.WriteLine("Elephant Move to " + move.ElephantMoveToValve);
                }

                moveCount++;

                Console.WriteLine();
            }

            Console.Write("Total Flow: " + totalFlow);
        }

        internal static Hashtable BestRemainingMoves = new Hashtable();

        internal static string CompleteOpenKey;

        internal static List<string> GetShortestPath(Valve sourceValve, Valve destinationValve, int shortestPathFromHere)
        {
            List<string> retVal = new List<string>() { "!!" };
            if(shortestPathFromHere == 0)
            {
                // stop evaluation
                return retVal;
            }

            foreach (Valve nextValve in sourceValve.ConnectionValves)
            {
                if (nextValve.Name == destinationValve.Name)
                {
                    return new List<string>() { destinationValve.Name };
                }

                // not one of the connections, so find the shortest path
                List<string> path = new List<string>();
                path.Add(nextValve.Name);
                path.AddRange(GetShortestPath(nextValve, destinationValve, shortestPathFromHere - 1));
                if(path.Last() != "!!" && path.Count < shortestPathFromHere)
                {
                    shortestPathFromHere = path.Count;
                    retVal = path;
                }
            }

            return retVal;

            // did not return so find the shortest from here
            //return paths.OrderBy(p => p.Count).First();
        }

        internal static List<Move> EvaluateMoves(Valve currentPersonValve, Valve currentElephantValve, List<Move> movesSoFar, int remainingMoves)
        {
            var personOpenValveNames = movesSoFar.Where(m => string.IsNullOrEmpty(m.PersonOpenValve) == false).Select(m => m.PersonOpenValve);
            var elephantOpenValveNames = movesSoFar.Where(m => string.IsNullOrEmpty(m.ElephantOpenValve) == false).Select(m => m.ElephantOpenValve);
            string openKey = string.Join("", personOpenValveNames.Concat(elephantOpenValveNames).Order().ToArray());

            string bestMoveKey = string.Format("{0}-{1}-{2}-{3}", currentPersonValve.Name, currentElephantValve.Name, remainingMoves, openKey);
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

            if(openKey == CompleteOpenKey)
            {
                // no need to move
                List<Move> newMoveSet = new List<Move>(movesSoFar);
                newMoveSet.Add(new Move() { NoMove = true });
            }

            List<List<Move>> possibleMoveSets = new List<List<Move>>();

            List<Move> possibleMoves = EvaluateMovesHelper2(currentPersonValve, currentElephantValve, movesSoFar);


            //List<List<string>> possibleMoveSets = new List<List<string>>();

            if (remainingMoves == TotalMinutes)
            {
                Console.WriteLine("Evaluating " + possibleMoves.Count + " moves");

                TotalMinutes = 10;
                remainingMoves = 10;
                // find the best 10 for the first 10
                foreach (Move move in possibleMoves)
                {
                    List<Move> newMoveSet = new List<Move>(movesSoFar);
                    newMoveSet.Add(move);
                    if (string.IsNullOrEmpty(move.PersonOpenValve) == false)
                    {
                        if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
                        {
                            // opening 2 valves
                            possibleMoveSets.Add(EvaluateMoves(currentPersonValve, currentElephantValve, newMoveSet, remainingMoves - 1));
                        }
                        else if (move.PersonOpenValve != move.ElephantMoveToValve || possibleMoves.Count == 1)
                        {
                            // person open, elephant move
                            possibleMoveSets.Add(EvaluateMoves(currentPersonValve, valves.First(v => v.Name == move.ElephantMoveToValve), newMoveSet, remainingMoves - 1));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
                        {
                            if (move.PersonMoveToValve != move.ElephantOpenValve || possibleMoves.Count == 1)
                            {
                                // person move, elephant open
                                possibleMoveSets.Add(EvaluateMoves(valves.First(v => v.Name == move.PersonMoveToValve), currentElephantValve, newMoveSet, remainingMoves - 1));
                            }
                        }
                        else if (move.PersonMoveToValve != move.ElephantMoveToValve || possibleMoves.Count == 1)
                        {
                            // moving to 2 valves but skip if they are the same valve
                            possibleMoveSets.Add(EvaluateMoves(valves.First(v => v.Name == move.PersonMoveToValve), valves.First(v => v.Name == move.ElephantMoveToValve), newMoveSet, remainingMoves - 1));
                        }
                    }
                }

                List<BestMove> bestMoves = new List<BestMove>();
                foreach (List<Move> moveSet in possibleMoveSets)
                {
                    int currentFlowRate = 0;
                    int totalFlow = 0;
                    foreach (Move move in moveSet)
                    {
                        totalFlow += currentFlowRate;
                        if (string.IsNullOrEmpty(move.PersonOpenValve) == false)
                        {
                            currentFlowRate += valves.First(v => v.Name == move.PersonOpenValve).FlowRate;
                        }
                        if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
                        {
                            currentFlowRate += valves.First(v => v.Name == move.ElephantOpenValve).FlowRate;
                        }
                    }

                    bestMoves.Add(new BestMove() { Moves = moveSet, TotalFlow = totalFlow });
                }

                bestMoves = bestMoves.OrderByDescending(b => b.TotalFlow).Take(25).ToList();
                possibleMoves = bestMoves.Select(b => b.Moves.First()).ToList();

                TotalMinutes = 26;
                remainingMoves = 26;
            }

            int topLevelMoveCount = 1;
            foreach (Move move in possibleMoves)
            {
                if (remainingMoves == TotalMinutes)
                {
                    Console.WriteLine("Top level move " + topLevelMoveCount++ + " " + System.DateTime.Now.ToString());
                }
                List<Move> newMoveSet = new List<Move>(movesSoFar);
                newMoveSet.Add(move);
                if (string.IsNullOrEmpty(move.PersonOpenValve) == false)
                {
                    if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
                    {
                        // opening 2 valves
                        possibleMoveSets.Add(EvaluateMoves(currentPersonValve, currentElephantValve, newMoveSet, remainingMoves - 1));
                    }
                    else if (move.PersonOpenValve != move.ElephantMoveToValve || possibleMoves.Count == 1)
                    {
                        // person open, elephant move
                        possibleMoveSets.Add(EvaluateMoves(currentPersonValve, valves.First(v => v.Name == move.ElephantMoveToValve), newMoveSet, remainingMoves - 1));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
                    {
                        if (move.PersonMoveToValve != move.ElephantOpenValve || possibleMoves.Count == 1)
                        {
                            // person move, elephant open
                            possibleMoveSets.Add(EvaluateMoves(valves.First(v => v.Name == move.PersonMoveToValve), currentElephantValve, newMoveSet, remainingMoves - 1));
                        }
                    }
                    else if (move.PersonMoveToValve != move.ElephantMoveToValve || possibleMoves.Count == 1)
                    {
                        // moving to 2 valves but skip if they are the same valve
                        possibleMoveSets.Add(EvaluateMoves(valves.First(v => v.Name == move.PersonMoveToValve), valves.First(v => v.Name == move.ElephantMoveToValve), newMoveSet, remainingMoves - 1));
                    }
                }
            }

            //foreach(Move move in possibleMoves)
            //{
            //    List<Move> newMoveSet = new List<Move>(movesSoFar);
            //    newMoveSet.Add(move);
            //    if(string.IsNullOrEmpty(move.PersonOpenValve) == false)
            //    {
            //        if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
            //        {
            //            // opening 2 valves
            //            possibleMoveSets.Add(EvaluateMoves(currentPersonValve, currentElephantValve, newMoveSet, remainingMoves - 1));
            //        }
            //        else if(move.PersonOpenValve != move.ElephantMoveToValve || possibleMoves.Count == 1)
            //        {
            //            // person open, elephant move
            //            possibleMoveSets.Add(EvaluateMoves(currentPersonValve, valves.First(v => v.Name == move.ElephantMoveToValve), newMoveSet, remainingMoves - 1));
            //        }
            //    }
            //    else
            //    {
            //        if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
            //        {
            //            if (move.PersonMoveToValve != move.ElephantOpenValve || possibleMoves.Count == 1)
            //            {
            //                // person move, elephant open
            //                possibleMoveSets.Add(EvaluateMoves(valves.First(v => v.Name == move.PersonMoveToValve), currentElephantValve, newMoveSet, remainingMoves - 1));
            //            }
            //        }
            //        else if(move.PersonMoveToValve != move.ElephantMoveToValve || possibleMoves.Count == 1)
            //        {
            //            // moving to 2 valves but skip if they are the same valve
            //            possibleMoveSets.Add(EvaluateMoves(valves.First(v => v.Name == move.PersonMoveToValve), valves.First(v => v.Name == move.ElephantMoveToValve), newMoveSet, remainingMoves - 1));
            //        }
            //    }
            //}

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
                    if (string.IsNullOrEmpty(move.PersonOpenValve) == false)
                    {
                        currentFlowRate += valves.First(v => v.Name == move.PersonOpenValve).FlowRate;
                    }
                    if (string.IsNullOrEmpty(move.ElephantOpenValve) == false)
                    {
                        currentFlowRate += valves.First(v => v.Name == move.ElephantOpenValve).FlowRate;
                    }
                }

                if (totalFlow > bestMoveSetTotalFlow)
                {
                    bestMoveSetTotalFlow = totalFlow;
                    bestMoveSet = moveSet;

                    List<Move> remainingMoveSet = new List<Move>(bestMoveSet);
                    remainingMoveSet.RemoveRange(0, TotalMinutes - remainingMoves);
                    BestRemainingMoves[bestMoveKey] = new BestMove()
                    {
                        Moves = remainingMoveSet,
                        TotalFlow = totalFlow
                    };
                }
            }

            if(bestMoveSet == null)
            {
                //throw new Exception("whaaa?");
                // nothing left to do... so add do nothings
                bestMoveSet = new List<Move>(movesSoFar);
                for(int i = 0; i < remainingMoves; i++)
                {
                    bestMoveSet.Add(new Move()
                    {
                        NoMove = true
                    });
                }
            }

            return bestMoveSet;
        }

        internal class BestMove
        {
            internal List<Move> Moves;
            internal int TotalFlow;
        }

        internal static List<Move> EvaluateMovesHelper2(Valve currentPersonValve, Valve currentElephantValve, List<Move> movesSoFar)
        {
            List<Move> possibleMoves = new List<Move>();

            var personOpenValveNames = movesSoFar.Where(m => string.IsNullOrEmpty(m.PersonOpenValve) == false).Select(m => m.PersonOpenValve);
            var elephantOpenValveNames = movesSoFar.Where(m => string.IsNullOrEmpty(m.ElephantOpenValve) == false).Select(m => m.ElephantOpenValve);
            

            string nextPersonValve = string.Empty;
            string nextElephantValve = string.Empty;
            Move lastMove = null;
            if (movesSoFar.Count > 0)
            {
                lastMove = movesSoFar.Last();
                if (lastMove.PersonMovesToOpenValve != null)
                {
                    if (lastMove.PersonMovesToOpenValve.Last() == currentPersonValve.Name)
                    {
                        // made it to the open valve
                    }
                    else
                    {
                        // in the middle of a person move to an open valve
                        int currentMoveIndex = lastMove.PersonMovesToOpenValve.IndexOf(currentPersonValve.Name);
                        nextPersonValve = lastMove.PersonMovesToOpenValve[currentMoveIndex + 1];
                    }
                }

                if (lastMove.ElephantMovesToOpenValve != null)
                {
                    if (lastMove.ElephantMovesToOpenValve.Last() == currentElephantValve.Name)
                    {
                        // made it to the open valve
                    }
                    else
                    {
                        // in the middle of a elephant move to an open valve
                        int currentMoveIndex = lastMove.ElephantMovesToOpenValve.IndexOf(currentElephantValve.Name);
                        nextElephantValve = lastMove.ElephantMovesToOpenValve[currentMoveIndex + 1];
                    }
                }
            }

            List<string> openedValves = personOpenValveNames.Concat(elephantOpenValveNames).ToList();
            List<Valve> unopenedValves = valves.Where(v => openedValves.Contains(v.Name) == false).ToList();
            List<List<string>> possiblePersonMoves = new List<List<string>>();
            List<List<string>> possibleElephantMoves = new List<List<string>>();
            foreach (Valve valve in unopenedValves)
            {
                if (nextPersonValve == string.Empty)
                {
                    string personKey = string.Concat(currentPersonValve.Name, "-", valve.Name);
                    List<string> personShortestPath = ShortestPaths[personKey] as List<string>;
                    if (personShortestPath != null && personShortestPath.Count < TotalMinutes - movesSoFar.Count)
                    {
                        // consider it
                        possiblePersonMoves.Add(personShortestPath);
                    }
                }

                if (nextElephantValve == string.Empty)
                {
                    string elephantKey = string.Concat(currentElephantValve.Name, "-", valve.Name);
                    List<string> elephantShortestPath = ShortestPaths[elephantKey] as List<string>;
                    if (elephantShortestPath != null && elephantShortestPath.Count < TotalMinutes - movesSoFar.Count)
                    {
                        // consider it
                        possibleElephantMoves.Add(elephantShortestPath);
                    }
                }
            }

            // assume the nearest is best
            //possiblePersonMoves = possiblePersonMoves.OrderBy(p => p.Count).Take(5).ToList();
            //possibleElephantMoves = possibleElephantMoves.OrderBy(p => p.Count).Take(5).ToList();
            possiblePersonMoves = possiblePersonMoves.OrderBy(p => p.Count).ToList();
            possibleElephantMoves = possibleElephantMoves.OrderBy(p => p.Count).ToList();

            if (nextPersonValve != string.Empty)
            {
                // continue person move
                if (nextElephantValve != string.Empty)
                {
                    // continue both moves
                    possibleMoves.Add(new Move() { PersonMoveToValve = nextPersonValve, PersonMovesToOpenValve = lastMove.PersonMovesToOpenValve, ElephantMoveToValve = nextElephantValve, ElephantMovesToOpenValve = lastMove.ElephantMovesToOpenValve });
                }
                else
                {
                    if (currentElephantValve.FlowRate > 0 &&
                            movesSoFar.Exists(m => (m.PersonOpenValve == currentElephantValve.Name || m.ElephantOpenValve == currentElephantValve.Name)) == false)
                    {
                        possibleMoves.Add(new Move() { PersonMoveToValve = nextPersonValve, PersonMovesToOpenValve = lastMove.PersonMovesToOpenValve, ElephantOpenValve = currentElephantValve.Name });
                    }

                    foreach (List<string> possibleElephantNextPath in possibleElephantMoves)
                    {
                        possibleMoves.Add(new Move() { PersonMoveToValve = nextPersonValve, PersonMovesToOpenValve = lastMove.PersonMovesToOpenValve, ElephantMoveToValve = possibleElephantNextPath.First(), ElephantMovesToOpenValve = possibleElephantNextPath });
                    }
                }
            }
            else
            {
                // not in the middle of a person move
                if (currentPersonValve.FlowRate > 0 &&
                movesSoFar.Exists(m => (m.PersonOpenValve == currentPersonValve.Name || m.ElephantOpenValve == currentPersonValve.Name)) == false)
                {
                    // can open a person valve
                    if (currentElephantValve.FlowRate > 0 && currentElephantValve != currentPersonValve &&
                        movesSoFar.Exists(m => (m.PersonOpenValve == currentElephantValve.Name || m.ElephantOpenValve == currentElephantValve.Name)) == false)
                    {
                        // open both at once
                        possibleMoves.Add(new Move() { PersonOpenValve = currentPersonValve.Name, ElephantOpenValve = currentElephantValve.Name });
                    }

                    if (nextElephantValve != string.Empty)
                    {
                        // open person valve, continue elephant move
                        possibleMoves.Add(new Move() { PersonOpenValve = currentPersonValve.Name, ElephantMoveToValve = nextElephantValve, ElephantMovesToOpenValve = lastMove.ElephantMovesToOpenValve });
                    }
                    else
                    {
                        // open person valve, try various elephant moves
                        foreach (List<string> possibleElephantNextPath in possibleElephantMoves)
                        {
                            possibleMoves.Add(new Move() { PersonOpenValve = currentPersonValve.Name, ElephantMoveToValve = possibleElephantNextPath.First(), ElephantMovesToOpenValve = possibleElephantNextPath });
                        }
                    }
                }
                else
                {
                    // not opening a person valve, find a new person path
                    foreach (List<string> possiblePersonNextPath in possiblePersonMoves)
                    {
                        if (currentElephantValve.FlowRate > 0 &&
                            movesSoFar.Exists(m => (m.PersonOpenValve == currentElephantValve.Name || m.ElephantOpenValve == currentElephantValve.Name)) == false)
                        {
                            // can open the elephant valve
                            possibleMoves.Add(new Move() { PersonMoveToValve = possiblePersonNextPath.First(), PersonMovesToOpenValve = possiblePersonNextPath, ElephantOpenValve = currentElephantValve.Name });
                        }

                        // no open elephant valve
                        if (nextElephantValve != string.Empty)
                        {
                            // pick a person path, continue elephant move
                            possibleMoves.Add(new Move() { PersonMoveToValve = possiblePersonNextPath.First(), PersonMovesToOpenValve = possiblePersonNextPath, ElephantMoveToValve = nextElephantValve, ElephantMovesToOpenValve = lastMove.ElephantMovesToOpenValve });
                        }
                        else
                        {
                            // pick one path for each but probably shouldn't happen this way?
                            foreach (List<string> possibleElephantNextPath in possibleElephantMoves)
                            {
                                possibleMoves.Add(new Move() { PersonMoveToValve = possiblePersonNextPath.First(), PersonMovesToOpenValve = possiblePersonNextPath, ElephantMoveToValve = possibleElephantNextPath.First(), ElephantMovesToOpenValve = possibleElephantNextPath });
                            }
                        }
                    }
                }
            }

            return possibleMoves;
        }

        internal static List<Move> EvaluateMovesHelper(Valve currentPersonValve, Valve currentElephantValve, List<Move> movesSoFar)
        {
            List<Move> possibleMoves = new List<Move>();

            if (currentPersonValve.FlowRate > 0 && 
                movesSoFar.Exists(m => (m.PersonOpenValve == currentPersonValve.Name || m.ElephantOpenValve == currentPersonValve.Name)) == false)
            {
                if(currentElephantValve.FlowRate > 0 && currentElephantValve != currentPersonValve &&
                    movesSoFar.Exists(m => (m.PersonOpenValve == currentElephantValve.Name || m.ElephantOpenValve == currentElephantValve.Name)) == false)
                {
                    possibleMoves.Add(new Move() { PersonOpenValve = currentPersonValve.Name, ElephantOpenValve = currentElephantValve.Name });
                }

                // possible moves are sets of moves to any unopened valve
                foreach (Valve valve in currentElephantValve.ConnectionValves)
                {
                    possibleMoves.Add(new Move() { PersonOpenValve = currentPersonValve.Name, ElephantMoveToValve = valve.Name });
                }
            }

            foreach (Valve valve in currentPersonValve.ConnectionValves)
            {
                if(currentElephantValve.FlowRate > 0 &&
                    movesSoFar.Exists(m => (m.PersonOpenValve == currentElephantValve.Name || m.ElephantOpenValve == currentElephantValve.Name)) == false)
                {
                    possibleMoves.Add(new Move() { PersonMoveToValve = valve.Name, ElephantOpenValve = currentElephantValve.Name });
                }

                foreach (Valve elephantValve in currentElephantValve.ConnectionValves)
                {
                    possibleMoves.Add(new Move() { PersonMoveToValve = valve.Name, ElephantMoveToValve = elephantValve.Name });
                }
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
            internal string PersonOpenValve;
            internal string PersonMoveToValve;
            internal List<string> PersonMovesToOpenValve;
            internal string ElephantOpenValve;
            internal string ElephantMoveToValve;
            internal List<string> ElephantMovesToOpenValve;
            internal bool NoMove = false;
        }

        internal class PossibleMove
        {
            internal string PersonOpenValve;
            internal List<string> PersonMoveToValve;
            internal string ElephantOpenValve;
            internal string ElephantMoveToValve;
            internal bool NoMove = false;
        }
    }
}