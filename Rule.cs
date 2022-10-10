using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZipCodeTw
{
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

        public static (HashSet<string>, string) Part(string ruleStr)
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

            return (ruleTokens, addrStr);
        }

        protected HashSet<string> _ruleTokens = new HashSet<string>();
        protected string _addrStr = "";

        public Rule(string ruleStr)
        {
            (_ruleTokens, _addrStr) = Part(ruleStr);
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
            if (_ruleTokens.Count > 0 && hisNoPair == (0, 0))
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