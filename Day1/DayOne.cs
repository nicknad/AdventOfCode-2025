using System;
using System.IO;

namespace AdventOfCode;

internal static class DayOne
{
    public static void Solve() {
        var lines = File.ReadAllLines("Day1\\Input.txt");
        int curr_pos = 50;
        int result = 0;
        bool started_null = false;

        foreach (string line in lines)
        {
            int val = int.Parse(line.AsSpan(1));

            switch (line[0])
            {
                case 'L':
                    curr_pos -= val;
                    if (curr_pos <= 0)
                    {
                        int temp = Math.Abs(curr_pos / 100);
                        result += started_null ? temp : temp + 1;
                        curr_pos += (temp + 1) * 100;
                    }
                    break;

                case 'R':
                    curr_pos += val;
                    if (curr_pos >= 100)
                    {
                        int temp = curr_pos / 100;
                        result += temp;
                    }
                    break;

                default: throw new InvalidDataException();
            }

            curr_pos = curr_pos % 100;
            AssertCurrPosIsValid(curr_pos);
            started_null = curr_pos == 0;
        }

        Console.WriteLine(result);
    }

    private static void AssertCurrPosIsValid(int curr_pos)
    {
        // This check will now pass correctly because 100 is impossible
        if (curr_pos < 0 || curr_pos >= 100)
        {
            throw new Exception($"Invalid State curr_pos: {curr_pos} reached");
        }
    }
}
