using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

internal static class DayFour
{
    private record struct Coordinate
    {
        public int Row; 
        public int Col;

        public Coordinate(int row, int col)
        {
            Row = row; Col = col;
        }

        public int ToIndex(int row_len) => Col + row_len * Row;
    }

    private const int linebreak_len = 2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Coordinate[] GetAdjecants(Coordinate c, int max_row, int max_col) =>
    c switch
    {
        _ when c.Row == 0 && c.Col == 0 => [new (c.Row, c.Col + 1), new(c.Row + 1, c.Col), new(c.Row + 1, c.Col + 1)],
        _ when c.Row == 0 && c.Col == max_col => [new(c.Row, c.Col - 1), new(c.Row + 1, c.Col - 1), new(c.Row + 1, c.Col)],
        _ when c.Row == max_row && c.Col == 0 => [new(c.Row - 1, c.Col - 1), new(c.Row - 1, c.Col), new(c.Row, c.Col - 1)],
        _ when c.Row == max_row && c.Col == max_col => [new(c.Row - 1, c.Col - 1), new(c.Row - 1, c.Col), new (c.Row, c.Col - 1)],
        _ when c.Col == 0 => [new(c.Row - 1, c.Col), new(c.Row - 1, c.Col + 1), new(c.Row, c.Col + 1), new(c.Row + 1, c.Col), new(c.Row + 1, c.Col + 1)],
        _ when c.Row == 0 => [new(c.Row, c.Col - 1), new(c.Row, c.Col + 1), new(c.Row + 1, c.Col - 1), new(c.Row + 1, c.Col), new(c.Row + 1, c.Col + 1)],
        _ when c.Col == max_col => [new(c.Row - 1, c.Col - 1), new(c.Row - 1, c.Col), new(c.Row, c.Col - 1), new(c.Row + 1, c.Col - 1), new(c.Row + 1, c.Col)],
        _ when c.Row == max_row => [new(c.Row - 1, c.Col - 1), new(c.Row - 1, c.Col), new(c.Row - 1, c.Col + 1), new(c.Row, c.Col - 1), new(c.Row, c.Col + 1)],
        _ => [new(c.Row - 1, c.Col - 1), new(c.Row - 1, c.Col), new(c.Row - 1, c.Col + 1), new(c.Row, c.Col - 1), new(c.Row, c.Col + 1), new(c.Row + 1, c.Col - 1), new(c.Row + 1, c.Col), new(c.Row + 1, c.Col + 1)]
    };


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsPaperRoll(this char c) => c == '@';


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Coordinate GetCoordinateByIndex(int idx, int row_len) => new(idx / (row_len + linebreak_len), idx % (row_len + linebreak_len));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char GetCharByCoordinates(Span<char> input_span, Coordinate coordinate, int row_len)
    {
        return input_span[coordinate.Col + coordinate.Row * (row_len + linebreak_len)];
    }

    public static void Solve()
    {
        int result = 0;
        char[] input = File.ReadAllText("Day4/input.txt").ToCharArray();
        Span<char> input_span = input.AsSpan();
        int upperbound_col = input_span.IndexOf('\r') ;
        int row_len = upperbound_col + linebreak_len;
        int upperbound_row = (input_span.Length + linebreak_len) / row_len;

        while(true)
        {
            List<Coordinate> coordinates_to_change = new (); // capacity guess?
            int idx = 0;
            while (idx < input_span.Length)
            {
                Coordinate coordinate = GetCoordinateByIndex(idx, upperbound_col);

                if (GetCharByCoordinates(input_span, coordinate, upperbound_col).IsPaperRoll()) {
                    int paperroll_count = 0;
                    foreach (var adj in GetAdjecants(coordinate, upperbound_row - 1, upperbound_col - 1)) {
                        if (GetCharByCoordinates(input_span, adj, upperbound_col).IsPaperRoll()) {
                            paperroll_count++;
                        }

                        if (paperroll_count >= 4) { break; }
                    }

                    if (paperroll_count < 4) {
                        coordinates_to_change.Add(coordinate);
                        result++;
                    }
                }

                idx++;
            }

            if (coordinates_to_change.Count == 0) { break; }

            foreach (var coordinate in coordinates_to_change) {
                input_span[coordinate.ToIndex(row_len)] = '.';
            }
        }

        Console.WriteLine(result);
    }
}

