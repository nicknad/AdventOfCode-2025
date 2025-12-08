using System;
using System.Collections.Generic;
using System.Numerics;

namespace AdventOfCode;

internal static class DayFive
{
    record struct IngredientRange(long Min, long Max) : IEquatable<IngredientRange>;
    public static void Solve()
    {
        BigInteger result = 0;
        #region PART ONE
        //List<IngredientRange> db = new List<IngredientRange>();
        //bool is_ingridient_id = false;
        //foreach (string line in System.IO.File.ReadLines("Day5\\input.txt"))
        //{
        //    if (string.IsNullOrWhiteSpace(line))  {
        //        is_ingridient_id = true;
        //        continue;
        //    }

        //    if (is_ingridient_id) {
        //        long ingridient_id = long.Parse(line);
        //        if (db.Any(range => ingridient_id >= range.Min && ingridient_id <= range.Max))
        //        {
        //            result++;
        //        }
        //    } // is range
        //    else {
        //        var rng = line.AsSpan();
        //        var pt = rng.IndexOf('-');
        //        db.Add(new(long.Parse(rng.Slice(0, pt)), long.Parse(rng.Slice(pt + 1))));
        //    }
        //}
        #endregion

        List<IngredientRange> db = new(200);
        foreach (string line in System.IO.File.ReadLines("Day5\\input.txt")) {
            if (string.IsNullOrWhiteSpace(line)) { break; }
            var rng = line.AsSpan();
            var pt = rng.IndexOf('-');

            db.Add(new IngredientRange(long.Parse(rng[..pt]), long.Parse(rng[(pt + 1)..])));
        }

        db.Sort((a, b) => {
            if (a.Min < b.Min) { return -1; }
            if (a.Min > b.Min) { return 1; }
            if (a.Max > b.Max) { return 1; }
            if (a.Max < b.Max) { return -1; }

            return 0;
        });

        List<IngredientRange> merged = new(200);
        IngredientRange candidate = db[0];
        for (int i = 1; i < db.Count; i++) {
            var cur = db[i];
            if (candidate.Max < cur.Min)  {
                merged.Add(candidate);
                candidate = cur;
                if (i == db.Count - 1)
                {
                    merged.Add(candidate);
                }
                
                continue;
            }

            candidate = new(candidate.Min, Math.Max(candidate.Max, cur.Max));
            if (i == db.Count - 1) {
                merged.Add(candidate);
            }
        }

        foreach (var entry in merged) {
            result += entry.Max - entry.Min + 1;
        }

        Console.WriteLine(result);
    }
}
