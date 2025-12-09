using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode;

internal static class DaySeven
{
    public static void Solve()
    {
        IEnumerable<string> lines = System.IO.File.ReadLines("Day7\\input.txt");
        Dictionary<int, long> timelines = [];
        timelines[lines.First().IndexOf('S')] = 1;
        foreach (string line in lines.Skip(1))
        {
            ReadOnlySpan<char> span = line.AsSpan();
            Dictionary<int, long> next = [];
            foreach ((int beam_idx, long paths) in timelines)
            {
                switch (span[beam_idx])
                {
                    case '^':
                        next[beam_idx - 1] = next.GetValueOrDefault(beam_idx - 1) + paths;
                        next[beam_idx + 1] = next.GetValueOrDefault(beam_idx + 1) + paths;
                        break;
                    case '.':
                        next[beam_idx] = next.GetValueOrDefault(beam_idx) + paths;
                        break;
                    default:
                        throw new UnreachableException();
                }
            }

            timelines = next;
        }
        long totalWays = timelines.Values.Sum();
        Console.WriteLine(totalWays);
    }

    public static void SolvePartOne()
    {
        int splits = 0;
        IEnumerable<string> lines = System.IO.File.ReadLines("Day7\\input.txt");
        Queue<int> beams = new();
        beams.Enqueue(lines.First().IndexOf('S'));
        HashSet<int> current_beams = [];
        foreach (string line in lines.Skip(1))
        {
            char[] chars = line.ToCharArray();
            while (beams.Count > 0)
            {
                int beam_idx = beams.Dequeue();
                switch (chars[beam_idx])
                {
                    case '^':
                        splits++;
                        if (!current_beams.Contains(beam_idx - 1))
                        {
                            chars[beam_idx - 1] = '|';
                            current_beams.Add(beam_idx - 1);
                        }

                        if (!current_beams.Contains(beam_idx + 1))
                        {
                            chars[beam_idx + 1] = '|';
                            current_beams.Add(beam_idx + 1);
                        }
                        break;
                    case '.':
                        if (!current_beams.Contains(beam_idx))
                        {
                            chars[beam_idx] = '|';
                            current_beams.Add(beam_idx);
                        }
                        break;
                    case '|':
                        break;
                    default:
                        throw new UnreachableException();
                }
            }

            Console.WriteLine(chars);
            foreach (int idx in current_beams)
            {
                beams.Enqueue(idx);
            }
            current_beams.Clear();
        }

    }
}