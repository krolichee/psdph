using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Utils
{
    public partial class SplitTextToRatio
    {
        public static class Splitter
        {
            public static List<string[]> splitIn(int parts, string[] words)
            {
                var result = new List<string[]>();
                if (parts<=1)
                    return new List<string[]> { new string[] { string.Join(" ", words) } };
                for (int nextPartStart = 1; nextPartStart <= words.Length-parts+1; nextPartStart++)
                {
                    var lines = new List<string>();
                    lines.Add(string.Join(" ", words.Take(nextPartStart)));
                    var leastArranges = splitIn(parts-1, words.Skip(nextPartStart).ToArray());
                    foreach (var item in leastArranges)
                        result.Add(lines.Concat(item).ToArray());
                }
                return result;
            }
            static List<string[]> getArranges(string str)
            {
                var words = str.Split(' ');
                List<string[]> arranges = new List<string[]>();
                for (int i = words.Length; i >= 1; i--)
                {
                    arranges.AddRange(splitIn(i,words));
                }
                return arranges;
            }
            static double getHeightWithBalanced(string[] lines)
            {
                double result = 1;
                double w = lines.Select(s => s.Length).Max();
                foreach (var line in lines)
                    result += w / line.Length;
                return result;
            }
            static double getArrangeRatio(string[] lines)
            {
                double w = lines.Select(s => s.Length).Max();
                double h = getHeightWithBalanced(lines);
                return w / h;
            }
            static string[] getBestArrange(List<string[]> arranges, double ratio)
            {
                var bestRatio = arranges.Min(l => Math.Abs(getArrangeRatio(l) - ratio));
                string[] result = arranges.Find(l => Math.Abs(getArrangeRatio(l) - ratio) == bestRatio);
                return result;
            }
            static bool arrangeFilter(string[] lines)
            {
                var widths = lines.Select(s => s.Length);
                var min = widths.Min();
                var max = widths.Max();
                return max/2<min;

            }
            static List<string[]> cull(List<string[]> arranges)
            {
                var result = arranges.Where(arrangeFilter).ToList();

                return result.Count != 0 ? result : arranges;
            }
            public static string Split(string str, double ratio)
            {
                if (str?.Length == 0 || str == null)
                    return "";
                var arranges = cull( getArranges(str));
                var bestArrange = getBestArrange(arranges,ratio);
                return string.Join("\n",bestArrange);
            }
        }
    }
}
