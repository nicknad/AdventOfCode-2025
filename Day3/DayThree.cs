using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;

namespace AdventOfCode;

internal static class DayThree
{
    public static void Solve()
    {
        // 200 rows of 12 digits numbers might not fit long/int64
        BigInteger result = 0;
        foreach (string line in File.ReadLines("Day3\\input.txt"))
        {
            ReadOnlySpan<char> input_span = line.AsSpan();
            int next_usable_idx = 0;
            for (int i =  0; i < 12; i++)
            {
                int curr_voltage = input_span[next_usable_idx] - '0';
                int rest_len = 12 - i;
                if (curr_voltage == 9)
                {
                    next_usable_idx++;
                    result += Convert.ToInt64(Math.Pow(10, rest_len - 1)) * curr_voltage;

                    continue;
                }

                // while last used index is a valid index
                int search_idx = next_usable_idx;
                int search_boundary = input_span.Length - rest_len;
                while (search_idx < search_boundary)
                {
                    search_idx++;

                    if (curr_voltage < input_span[search_idx] - '0')
                    {
                        next_usable_idx = search_idx;
                        curr_voltage = input_span[search_idx] - '0';
                        if (curr_voltage == 9)
                        {
                            break;
                        }
                    }
                }

                next_usable_idx++;
                result += Convert.ToInt64(Math.Pow(10, rest_len - 1)) * curr_voltage;
            }
        }

        Console.WriteLine(result);
    }
}
