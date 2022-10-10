using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZipCodeTw
{
    public class Address
    {
        public static Regex TOKEN_RE = new Regex(@"
            (?:
                (?<no>\d+)
                (?<subno>之\d+)?
                (?=[巷弄號樓]|$)
                |
                (?<name>.+?)
            )
            (?:
                (?<unit>[縣市鄉鎮市區村里鄰路街段巷弄號樓])
                |
                (?=\d+(?:之\d+)?[巷弄號樓]|$)
            )
        ", RegexOptions.IgnorePatternWhitespace);

        public const int NO = 1;
        public const int SUBNO = 2;
        public const int NAME = 3;
        public const int UNIT = 4;

        public static Regex TO_REPLACE_RE = new Regex(@"
            [ 　,，台~-]
            |
            [０-９]
            |
            [一二三四五六七八九]?
            十?
            [一二三四五六七八九]
            (?=[段路街巷弄號樓])
        ", RegexOptions.IgnorePatternWhitespace);

        // the strs matched but not in here will be removed
        public static Dictionary<string, string> TO_REPLACE_MAP = new Dictionary<string, string> {
            {"-", "之"}, {"~", "之"}, {"台", "臺"},
            {"１", "1"}, {"２", "2"}, {"３", "3"}, {"４", "4"}, {"５", "5"},
            {"６", "6"}, {"７", "7"}, {"８", "8"}, {"９", "9"}, {"０", "0"},
            {"一", "1"}, {"二", "2"}, {"三", "3"}, {"四", "4"}, {"五", "5"},
            {"六", "6"}, {"七", "7"}, {"八", "8"}, {"九", "9"}
        };

        public static HashSet<string> CHINESE_NUMERALS_SET = new HashSet<string> { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

        public static string Normalize(string s)
        {
            string Replace(Match m)
            {
                var groups = m.Groups;
                if (groups == null)
                    return "";

                var group0 = groups[0];
                if (group0 == null)
                    return "";
                var foundStr = group0.Value;

                if (TO_REPLACE_MAP.ContainsKey(foundStr))
                    return TO_REPLACE_MAP[foundStr];

                // for "十一" to "九十九"
                if (CHINESE_NUMERALS_SET.Contains(foundStr.Substring(0, 1)))
                {
                    var lenFound = foundStr.Length;
                    if (lenFound == 2)
                        return "1" + TO_REPLACE_MAP[foundStr.Substring(1, 1)];
                    else if (lenFound == 3)
                        return TO_REPLACE_MAP[foundStr.Substring(0, 1)] + TO_REPLACE_MAP[foundStr.Substring(2, 1)];
                }

                return "";
            }

            return TO_REPLACE_RE.Replace(s, Replace);
        }

        public static List<(string, string, string, string)> Tokenize(string addr_str)
        {
            var tokens = TOKEN_RE.Matches(Normalize(addr_str));
            var result = new List<(string, string, string, string)>();
            foreach (Match token in tokens)
            {
                var token1 = token.Groups[NO].Value;
                var token2 = token.Groups[SUBNO].Value;
                var token3 = token.Groups[NAME].Value;
                var token4 = token.Groups[UNIT].Value;

                result.Add((token1, token2, token3, token4));
            }
            return result;
        }

        private List<(string, string, string, string)> _tokens;

        public Address() { }

        public Address(string addrStr)
        {
            Initialize(addrStr);
        }

        public void Initialize(string addrStr)
        {
            _tokens = Tokenize(addrStr);
        }

        public List<(string, string, string, string)> Tokens
        {
            get
            {
                if (_tokens == null)
                    return new List<(string, string, string, string)>();
                return _tokens;
            }
        }

        public int Length
        {
            get { return Tokens.Count; }
        }

        public string Flat()
        {
            if (_tokens == null)
                return "";
            var list = _tokens.Select(v => v.Item1 + v.Item2 + v.Item3 + v.Item4).ToList();
            return String.Join("", list);
        }

        public string Flat(int iEnd)
        {
            return Flat(0, iEnd);
        }

        public string Flat(int iStart, int iEnd)
        {
            if (_tokens == null)
                return "";

            if (iStart < 0)
                iStart += _tokens.Count;
            if (iEnd < 0)
                iEnd += _tokens.Count;

            var result = new StringBuilder("");
            for (int idx = iStart; idx < iEnd; idx++)
            {
                var v = _tokens[idx];
                result.Append(v.Item1 + v.Item2 + v.Item3 + v.Item4);
            }
            return result.ToString();
        }

        public string PickToFlat(params int[] idxs)
        {
            if (_tokens == null)
                return "";

            var result = new StringBuilder("");
            foreach (var idx in idxs)
            {
                var v = _tokens[idx];
                result.Append(v.Item1 + v.Item2 + v.Item3 + v.Item4);
            }
            return result.ToString();
        }

        public AddrTuple Parse(int idx)
        {
            try
            {
                if (idx < 0)
                    idx = this.Length + idx;

                var token = _tokens[idx];
                var token1 = token.Item1;
                var token2 = token.Item2;
                if (token2.Length > 1)
                    token2 = token2.Substring(1);
                int intToken1 = 0;
                int intToken2 = 0;

                int.TryParse(token1, out intToken1);
                int.TryParse(token2, out intToken2);

                return new AddrTuple(intToken1, intToken2);
            }
            catch (Exception)
            {
                return new AddrTuple(0, 0);
            }
        }
    }

    public class AddrTuple
    {
        int _item1;
        int _item2;

        public AddrTuple(int item1, int item2)
        {
            _item1 = item1;
            _item2 = item2;
        }

        public int this[int index]
        {
            get
            {
                if (index == 0)
                    return _item1;
                else if (index == 1)
                    return _item2;
                throw new IndexOutOfRangeException($"Index {index} > 1 or < 0");
            }
        }

        public static bool operator ==(AddrTuple c1, AddrTuple c2)
        {
            return c1[0] == c2[0] && c1[1] == c2[1];
        }

        public static bool operator !=(AddrTuple c1, AddrTuple c2)
        {
            return !(c1 == c2);
        }

        public static bool operator >=(AddrTuple c1, AddrTuple c2)
        {
            return c1[0] > c2[0] || (c1[0] == c2[0] && c1[1] >= c2[1]);
        }

        public static bool operator <=(AddrTuple c1, AddrTuple c2)
        {
            return c1[0] < c2[0] || (c1[0] == c2[0] && c1[1] <= c2[1]);
        }

        public static bool operator ==(AddrTuple c1, (int, int) c2)
        {
            return c1[0] == c2.Item1 && c1[1] == c2.Item2;
        }

        public static bool operator !=(AddrTuple c1, (int, int) c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _item1 + _item2;
        }
    }
}