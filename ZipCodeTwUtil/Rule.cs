using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZipCodeTw
{
    public struct Tuple4<T1, T2, T3, T4>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
        public T3 Item3 { get; set; }
        public T4 Item4 { get; set; }

        public Tuple4(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public static bool operator ==(Tuple4<T1,T2,T3,T4> c1, Tuple4<T1, T2, T3, T4> c2)
        {
            return c1.Item1.Equals(c2.Item1) && c1.Item2.Equals(c2.Item2) && 
                c1.Item3.Equals(c2.Item3) && c1.Item4.Equals(c2.Item4);
        }

        public static bool operator !=(Tuple4<T1, T2, T3, T4> c1, Tuple4<T1, T2, T3, T4> c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            return obj is Tuple4<T1, T2, T3, T4> tuple &&
                   EqualityComparer<T1>.Default.Equals(Item1, tuple.Item1) &&
                   EqualityComparer<T2>.Default.Equals(Item2, tuple.Item2) &&
                   EqualityComparer<T3>.Default.Equals(Item3, tuple.Item3) &&
                   EqualityComparer<T4>.Default.Equals(Item4, tuple.Item4);
        }

        public override int GetHashCode()
        {
            int hashCode = -1041475770;
            hashCode = hashCode * -1521134295 + EqualityComparer<T1>.Default.GetHashCode(Item1);
            hashCode = hashCode * -1521134295 + EqualityComparer<T2>.Default.GetHashCode(Item2);
            hashCode = hashCode * -1521134295 + EqualityComparer<T3>.Default.GetHashCode(Item3);
            hashCode = hashCode * -1521134295 + EqualityComparer<T4>.Default.GetHashCode(Item4);
            return hashCode;
        }
    }

    public class Rule : Address
    {
        public static Regex RULE_TOKEN_RE = new Regex(@"
            及以上附號|含附號以下|含附號全|含附號
            |
            以下|以上
            |
            附號全
            |
            [連至單雙全](?=[\d全]|$)
        ", RegexOptions.IgnorePatternWhitespace);

        public static Tuple2<HashSet<string>, string> Part(string ruleStr)
        {
            ruleStr = Normalize(ruleStr);
            HashSet<string> ruleTokens = new HashSet<string>();

            string Extract(Match m)
            {
                var groups = m.Groups;
                if (groups == null)
                    return "";

                var group0 = groups[0];
                if (group0 == null)
                    return "";
                var token = group0.Value;
                var retValue = "";

                if (token == "連")
                    token = "";
                else if (token == "附號全")
                    retValue = "號";

                if (!String.IsNullOrEmpty(token))
                    ruleTokens.Add(token);

                return retValue;
            }

            var addrStr = RULE_TOKEN_RE.Replace(ruleStr, Extract);

            return new Tuple2<HashSet<string>, string>(ruleTokens, addrStr);
        }

        protected HashSet<string> _ruleTokens = new HashSet<string>();
        protected string _addrStr = "";

        public Rule(string ruleStr)
        {
            var rule = Part(ruleStr);
            _ruleTokens = rule.Item1;
            _addrStr = rule.Item2;
            base.Initialize(_addrStr);
        }

        public HashSet<string> RuleTokens()
        {
            return _ruleTokens;
        }

        public bool Match(Address addr)
        {
            // # except tokens reserved for rule token  
            var myLastPos = this.Length - 1;
            if (_ruleTokens.Count > 0 && !_ruleTokens.Contains("全"))
                myLastPos--;
            if (_ruleTokens.Contains("至"))
                myLastPos--;

            // # tokens must be matched exactly
            if (myLastPos >= addr.Length)
                return false;

            var i = myLastPos;
            var thisTokens = this.Tokens;
            var addrTokens = addr.Tokens;
            while (i >= 0)
            {
                if (thisTokens[i] != addrTokens[i])
                    return false;
                i--;
            }

            // # check the rule tokens
            var hisNoPair = addr.Parse(myLastPos + 1);
            if (_ruleTokens.Count > 0 && hisNoPair == new Tuple2<int, int>(0, 0))
                return false;

            var myNoPair = this.Parse(-1);
            var myAsstNoPair = this.Parse(-2);

            foreach (var rt in _ruleTokens)
            {
                if (
                  (rt == "單" && !((hisNoPair[0] & 1) == 1)) ||
                  (rt == "雙" && !((hisNoPair[0] & 1) == 0)) ||
                  (rt == "以上" && !(hisNoPair >= myNoPair)) ||
                  (rt == "以下" && !(hisNoPair <= myNoPair)) ||
                  (rt == "至" && !(
                      (myAsstNoPair <= hisNoPair && hisNoPair <= myNoPair) ||
                      (_ruleTokens.Contains("含附號全") && hisNoPair[0] == myNoPair[0])
                  )) ||
                  (rt == "含附號" && !(hisNoPair[0] == myNoPair[0])) ||
                  (rt == "附號全" && !(hisNoPair[0] == myNoPair[0] && hisNoPair[1] > 0)) ||
                  (rt == "及以上附號" && !(hisNoPair >= myNoPair)) ||
                  (rt == "含附號以下" && !(hisNoPair <= myNoPair || hisNoPair[0] == myNoPair[0]))
                  )
                    return false;
            }
            return true;
        }
    }
}