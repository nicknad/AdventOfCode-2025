using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode;

internal static class DayNine
{
    internal record struct RectangleEdge(int X, int Y) {
        internal static RectangleEdge Parse(ReadOnlySpan<char> span) {
            int x = int.Parse(span[..span.IndexOf(',')]);
            span = span.Slice(span.IndexOf(',') + 1);
            int y = int.Parse(span);
            return new(x, y);
        }

        internal long Area(RectangleEdge other) => (long)(1 + Math.Abs(other.X - X)) * (1 + Math.Abs(other.Y - Y));
    }

    public static void Solve()
    {
        List<RectangleEdge> edges = [.. System.IO.File.ReadLines("Day9\\input.txt").Select(line => RectangleEdge.Parse(line.AsSpan()))];

        long max = 0;
        for (int i = 0; i < edges.Count - 1; i++) { 
            for (int j = i + 1; j < edges.Count; j++) {
                RectangleEdge edge = edges[i];
                RectangleEdge other = edges[j];
                long area = edge.Area(other);
                if (area > max) max = area;
            }
        }

        Console.WriteLine($"Max area: {max}");
    }
}
