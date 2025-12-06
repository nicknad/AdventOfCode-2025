using System;
using System.IO;
using System.Linq;

namespace AdventOfCode;
internal static class DayTwo
{
    public static void Solve()
    {
        string input = File.ReadAllText("Day2\\input-part1.txt");
        ReadOnlySpan<char> input_span = input.AsSpan();
        long result = 0;

        bool end_of_loop_reached = false;
        while (!end_of_loop_reached)
        {
            int comma_idx = input_span.IndexOf(',');
            if (comma_idx == -1)
            {
                end_of_loop_reached = true;
            }

            ReadOnlySpan<char> range_span = end_of_loop_reached ? input_span : input_span[..comma_idx];
            input_span = input_span[(comma_idx + 1)..];
            int dash_idx = range_span.IndexOf('-');
            long range_start = long.Parse(range_span[..dash_idx]);
            long range_end = long.Parse(range_span[(dash_idx + 1)..]);
            for (long value = range_start; value <= range_end; value++)
            {
                bool is_invalid_id = false;
                string value_string = value.ToString();
                ReadOnlySpan<char> value_span = value_string.AsSpan();

                for (int span_len = 1; span_len <= value_span.Length / 2; span_len++)
                {
                    if (value_span.Length % span_len != 0)
                    {
                        continue;
                    }

                    bool all_sequences_equal = true;
                    ReadOnlySpan<char> cmp_slice = value_span[..span_len];
                    for (int span_idx = span_len; span_idx < value_span.Length; span_idx += span_len)
                    {
                        if (!cmp_slice.SequenceEqual(value_span.Slice(span_idx, span_len)))
                        {
                            all_sequences_equal = false;
                            break;
                        }
                    }

                    if (all_sequences_equal)
                    {
                        is_invalid_id = true;
                        break;
                    }
                }
                
                if (is_invalid_id)
                {
                    result += value;
                }
            }
        }

        Console.WriteLine(result);
    }
}
