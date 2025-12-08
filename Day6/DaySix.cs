using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace AdventOfCode;

internal static class DaySix
{
    public static void Solve()
    {
        List<MathProblem> math_problems = new(1000);
        string[] input = System.IO.File.ReadAllLines("Day6\\input.txt");
        ReadOnlySpan<char> ops_line = input.Last().AsSpan();
        int line_len = ops_line.Length;
        int index = 0;
        while (index < line_len)
        {
            int ops_idx = index;
            char cur_ops = ops_line[ops_idx];
            index++;
            while (index < line_len && ops_line[index] == ' ')
            {
                index++;
            }

            int input_len = (index == line_len) ? (line_len - ops_idx) : (index - 1) - ops_idx;
            math_problems.Add(new MathProblem(cur_ops, input_len));
        }

        for (int line_idx = 0; line_idx < input.Length - 1; line_idx++)
        {
            int cur_idx = 0;
            ReadOnlySpan<char> span = input[line_idx].AsSpan();
            foreach (var problem in math_problems)
            {
                problem.InputMatrix.Add(span.Slice(cur_idx, problem.SpanWidth).ToString());
                cur_idx += problem.SpanWidth + 1;
            }
        }

        long result = 0;
        foreach (var problem in math_problems)
        {
            result += problem.Calculate();
        }

        Console.WriteLine($"Day 6: {result}");
    }

    internal class MathProblem()
    {
        public MathProblem(char operation, int spanWidth) : this()
        {
            this.SpanWidth = spanWidth;
            this.Operation = operation switch
            {
                '+' or '*' => operation,
                _ => throw new ArgumentException("Invalid Operation"),
            };
        }

        public List<string> InputMatrix = new(4);
        public char Operation;
        public int SpanWidth;
        public List<long> TransformMatrix()
        {
            List<long> result = new(SpanWidth);
            int row_count = InputMatrix.Count;
            for (int col = 0; col < SpanWidth; col++) {
                long col_value = 0;
                int power_of_10 = 0;
                for (int row = row_count - 1; row >= 0; row--) {
                    char val = InputMatrix[row][col];
                    if (char.IsDigit(val)) {
                        col_value += long.Parse(val.ToString()) * (long)Math.Pow(10, power_of_10);
                        power_of_10++;
                    }
                }
                result.Add(col_value);
            }

            return result;
        }

        public long Calculate() {
            List<long> input = TransformMatrix();
            return Operation switch
            {
                '*' => input.Aggregate((a, b) => a * b),
                '+' => input.Sum(),
                _ => throw new UnreachableException()
            };
        }
    }
}
