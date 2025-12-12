using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode;

internal static class DayEight
{
    internal readonly record struct JunctionBox(int X, int Y, int Z)
    {
        public static JunctionBox Parse(ReadOnlySpan<char> span) {
            int x = int.Parse(span[..span.IndexOf(',')]);
            span = span.Slice(span.IndexOf(',') + 1);
            int y = int.Parse(span[..span.IndexOf(',')]);
            span = span[(span.IndexOf(',') + 1)..];
            int z = int.Parse(span);

            return new(x, y, z);
        }
    }
    internal record Circuit(List<JunctionBox> Boxes);
    internal readonly record struct Connection(JunctionBox Source, JunctionBox Target, long Distance)
    {
        public Connection(JunctionBox Source, JunctionBox Target) : this(Source, Target, Source.distance_to(Target)) { }
    }

    extension(JunctionBox a)
    {
        internal long distance_to(JunctionBox b) {
            long dx = a.X - b.X;
            long dy = a.Y - b.Y;
            long dz = a.Z - b.Z;
            return dx * dx + dy * dy + dz * dz;
        }
    }

    internal class CircuitSet
    {
        private readonly HashSet<JunctionBox> _look_up;

        public CircuitSet(Connection connection) {
            JunctionBox[] positions = [connection.Source, connection.Target];
            _look_up = [.. positions];
        }
        public CircuitSet(JunctionBox jb) {
            _look_up = [jb];
        }

        public bool Find(Connection connection) {
            return _look_up.Contains(connection.Source) || _look_up.Contains(connection.Target);
        }

        public void Add(Connection connection) {
            if (!_look_up.Contains(connection.Source)) {
                _look_up.Add(connection.Source);
            }

            if (!_look_up.Contains(connection.Target)) {
                _look_up.Add(connection.Target);
            }
        }

        public void Merge(CircuitSet other) {             
            foreach (var box in other._look_up) {
                _look_up.Add(box);
            }
        }

        internal int Size => _look_up.Count;
    }

    public static void Solve() {
        List<JunctionBox> positions = [.. System.IO.File.ReadLines("Day8\\input.txt").Select(line => JunctionBox.Parse(line.AsSpan()))];
        List<Connection> connections = [];
        for (int i = 0; i < positions.Count; i++) {
            for (int j = i + 1; j < positions.Count; j++) {
                connections.Add(new (positions[i], positions[j]));
            }
        }

        List<CircuitSet> position_sets = [.. positions.Select(x => new CircuitSet(x))];
        connections.Sort((a,b) => a.Distance.CompareTo(b.Distance));
        
        int idx = 0;
        Connection last_connection  = connections[idx];
        while (true) {
            List<CircuitSet> sets = [.. position_sets.Where(x => x.Find(last_connection))];
            switch (sets.Count) {
                case > 2:
                    throw new InvalidOperationException("More than two sets found for a connection!");
                case 2:
                    sets[0].Merge(sets[1]);
                    sets[0].Add(last_connection);
                    position_sets.Remove(sets[1]);
                    break;
                case 1:
                    break;
                case 0:
                    throw new InvalidOperationException("No sets found for a connection!");
            }

            if (position_sets.Count == 1) {
                break;
            }
            idx++;
            last_connection = connections[idx];
        }

      
        Console.WriteLine($"The result is: {(long)last_connection.Source.X * (long)last_connection.Target.X}");
    }
}
