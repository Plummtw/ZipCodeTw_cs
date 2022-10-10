using ZipCodeTw;

namespace ZipCodeTw_Test;

public class RuleTest
{
  [SetUp]
  public void Setup()
  {
  }

  [Test]
  public void TestRuleInit()
  {
    var rule = new Rule("臺北市,中正區,八德路１段,全");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中正", "區"),
       new Tuple4<string, string, string, string>("", "", "八德", "路"),
       new Tuple4<string, string, string, string>("", "", "1", "段")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"全"}));

    rule = new Rule("臺北市,中正區,三元街,單全");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中正", "區"),
       new Tuple4<string, string, string, string>("", "", "三元", "街")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"單", "全"}));

    rule = new Rule("臺北市,中正區,三元街,雙  48號以下");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中正", "區"),
       new Tuple4<string, string, string, string>("", "", "三元", "街"),
       new Tuple4<string, string, string, string>("48", "", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"雙", "以下"}));

    rule = new Rule("臺北市,中正區,大埔街,單  15號以上");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中正", "區"),
       new Tuple4<string, string, string, string>("", "", "大埔", "街"),
       new Tuple4<string, string, string, string>("15", "", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"單", "以上"}));

    rule = new Rule("臺北市,中正區,中華路１段,單  25之   3號以下");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中正", "區"),
       new Tuple4<string, string, string, string>("", "", "中華", "路"),
       new Tuple4<string, string, string, string>("", "", "1", "段"),
       new Tuple4<string, string, string, string>("25", "之3", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"單", "以下"}));

    rule = new Rule("臺北市,中正區,中華路１段,單  27號至  47號");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中正", "區"),
       new Tuple4<string, string, string, string>("", "", "中華", "路"),
       new Tuple4<string, string, string, string>("", "", "1", "段"),
       new Tuple4<string, string, string, string>("27", "", "", "號"),
       new Tuple4<string, string, string, string>("47", "", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"單", "至"}));

    rule = new Rule("臺北市,中正區,仁愛路１段,連   2之   4號以上");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中正", "區"),
       new Tuple4<string, string, string, string>("", "", "仁愛", "路"),
       new Tuple4<string, string, string, string>("", "", "1", "段"),
       new Tuple4<string, string, string, string>("2", "之4", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"以上"}));

    rule = new Rule("臺北市,中正區,杭州南路１段,　  14號含附號");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中正", "區"),
       new Tuple4<string, string, string, string>("", "", "杭州南", "路"),
       new Tuple4<string, string, string, string>("", "", "1", "段"),
       new Tuple4<string, string, string, string>("14", "", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"含附號"}));

    rule = new Rule("臺北市,大同區,哈密街,　  47附號全");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "大同", "區"),
       new Tuple4<string, string, string, string>("", "", "哈密", "街"),
       new Tuple4<string, string, string, string>("47", "", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"附號全"}));

    rule = new Rule("臺北市,大同區,哈密街,雙  68巷至  70號含附號全");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "大同", "區"),
       new Tuple4<string, string, string, string>("", "", "哈密", "街"),
       new Tuple4<string, string, string, string>("68", "", "", "巷"),
       new Tuple4<string, string, string, string>("70", "", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"雙", "至", "含附號全"}));

    rule = new Rule("桃園縣,中壢市,普義,連  49號含附號以下");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "桃園", "縣"),
       new Tuple4<string, string, string, string>("", "", "中壢", "市"),
       new Tuple4<string, string, string, string>("", "", "普義", ""),
       new Tuple4<string, string, string, string>("49", "", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"含附號以下"}));

    rule = new Rule("臺中市,西屯區,西屯路３段西平南巷,　   1之   3號及以上附號");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺中", "市"),
       new Tuple4<string, string, string, string>("", "", "西屯", "區"),
       new Tuple4<string, string, string, string>("", "", "西屯", "路"),
       new Tuple4<string, string, string, string>("", "", "3", "段"),
       new Tuple4<string, string, string, string>("", "", "西平南", "巷"),
       new Tuple4<string, string, string, string>("1", "之3", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"及以上附號"}));
  }

  [Test]
  public void TestRuleTrickyInput()
  {
    var rule = new Rule("新北市,中和區,連城路,雙 268之   1號以下");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "新北", "市"),
       new Tuple4<string, string, string, string>("", "", "中和", "區"),
       new Tuple4<string, string, string, string>("", "", "連城", "路"),
       new Tuple4<string, string, string, string>("268", "之1", "", "號")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"雙", "以下"}));

    rule = new Rule("新北市,泰山區,全興路,全");
    Assert.That(rule.Tokens, Is.EqualTo(new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "新北", "市"),
       new Tuple4<string, string, string, string>("", "", "泰山", "區"),
       new Tuple4<string, string, string, string>("", "", "全興", "路")}));
    Assert.That(rule.RuleTokens, Is.EqualTo(new HashSet<string> {"全"}));
  }

  [Test]
  public void TestRuleMatch()
  {
    var addr = new Address("臺北市大安區市府路5號");
    Assert.IsTrue(new Rule("臺北市大安區市府路全").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路單全").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路雙全").Match(addr));

    // # 以上 & 以下
    Assert.IsFalse(new Rule("臺北市大安區市府路6號以上").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路6號以下").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路5號以上").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路5號").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路5號以下").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路4號以上").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路4號以下").Match(addr));

    // # 至
    Assert.IsFalse(new Rule("臺北市大安區市府路1號至4號").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路1號至5號").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路5號至9號").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路6號至9號").Match(addr));

    // # 附號
    Assert.IsFalse(new Rule("臺北市大安區市府路6號及以上附號").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路6號含附號以下").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路5號及以上附號").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路5號含附號").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路5附號全").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路5號含附號以下").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路4號及以上附號").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路4號含附號以下").Match(addr));

    // # 單雙 x 以上, 至, 以下
    Assert.IsTrue(new Rule("臺北市大安區市府路單5號以上").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路雙5號以上").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路單1號至5號").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路雙1號至5號").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路單5號至9號").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路雙5號至9號").Match(addr));
    Assert.IsTrue(new Rule("臺北市大安區市府路單5號以下").Match(addr));
    Assert.IsFalse(new Rule("臺北市大安區市府路雙5號以下").Match(addr));
  }

  [Test]
  public void TestRuleMatchGradualAddress()
  {
    var rule = new Rule("臺北市中正區丹陽街全");
    Assert.IsFalse(rule.Match(new Address("臺北市")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區仁愛路１段")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區仁愛路１段1號")));
    
    // # standard rule w/ gradual addresses

    rule = new Rule("臺北市,中正區,仁愛路１段,　   1號");
    Assert.IsFalse(rule.Match(new Address("臺北市")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區仁愛路１段")));
    Assert.IsTrue(rule.Match(new Address("臺北市中正區仁愛路１段1號")));
  }

  [Test]
  public void TestRuleMatchRuleAll()
  {
    var rule = new Rule("臺北市,中正區,八德路１段,全");
    Assert.IsTrue(rule.Match(new Address("臺北市中正區八德路１段1號")));
    Assert.IsTrue(rule.Match(new Address("臺北市中正區八德路１段9號")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區八德路２段1號")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區八德路２段9號")));
    
    rule = new Rule("臺北市,中正區,三元街,單全");
    Assert.IsTrue(rule.Match(new Address("臺北市中正區三元街1號")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區三元街2號")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區大埔街1號")));

    rule = new Rule("臺北市,大同區,哈密街,　  45巷全");
    Assert.IsTrue(rule.Match(new Address("臺北市大同區哈密街45巷1號")));
    Assert.IsTrue(rule.Match(new Address("臺北市大同區哈密街45巷9號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街46巷1號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街46巷9號")));
  }

  [Test]
  public void TestRuleMatchTrickyInput()
  {
    // # The address matched by it must have a even number.
    var rule = new Rule("信義路一段雙全");
    Assert.IsFalse(rule.Match(new Address("信義路一段")));
    Assert.IsFalse(rule.Match(new Address("信義路一段1號")));
    Assert.IsTrue(rule.Match(new Address("信義路一段2號")));
  }

  [Test]
  public void TestRuleMatchSubno()
  {
    var rule = new Rule("臺北市,中正區,杭州南路１段,　  14號含附號");
    Assert.IsFalse(rule.Match(new Address("臺北市中正區杭州南路1段13號")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區杭州南路1段13-1號")));
    Assert.IsTrue(rule.Match(new Address("臺北市中正區杭州南路1段14號")));
    Assert.IsTrue(rule.Match(new Address("臺北市中正區杭州南路1段14-1號")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區杭州南路1段15號")));
    Assert.IsFalse(rule.Match(new Address("臺北市中正區杭州南路1段15-1號")));

    rule = new Rule("臺北市,大同區,哈密街,　  47附號全");
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街46號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街46-1號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街47號")));
    Assert.IsTrue(rule.Match(new Address("臺北市大同區哈密街47-1號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街48號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街48-1號")));

    rule = new Rule("臺北市,大同區,哈密街,雙  68巷至  70號含附號全");
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街66號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街66-1巷")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街67號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街67-1巷")));
    Assert.IsTrue(rule.Match(new Address("臺北市大同區哈密街68巷")));
    Assert.IsTrue(rule.Match(new Address("臺北市大同區哈密街68-1號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街69號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街69-1巷")));
    Assert.IsTrue(rule.Match(new Address("臺北市大同區哈密街70號")));
    Assert.IsTrue(rule.Match(new Address("臺北市大同區哈密街70-1號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街71號")));
    Assert.IsFalse(rule.Match(new Address("臺北市大同區哈密街71-1號")));

    rule = new Rule("桃園縣,中壢市,普義,連  49號含附號以下");
    Assert.IsTrue(rule.Match(new Address("桃園縣中壢市普義48號")));
    Assert.IsTrue(rule.Match(new Address("桃園縣中壢市普義48-1號")));
    Assert.IsTrue(rule.Match(new Address("桃園縣中壢市普義49號")));
    Assert.IsTrue(rule.Match(new Address("桃園縣中壢市普義49-1號")));
    Assert.IsFalse(rule.Match(new Address("桃園縣中壢市普義50號")));
    Assert.IsFalse(rule.Match(new Address("桃園縣中壢市普義50-1號")));

    rule = new Rule("臺中市,西屯區,西屯路３段西平南巷,　   2之   3號及以上附號");
    Assert.IsFalse(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷1號")));
    Assert.IsFalse(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷1-1號")));      
    Assert.IsFalse(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷2號")));
    Assert.IsFalse(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷2-2號")));  
    Assert.IsTrue(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷2-3號")));
    Assert.IsTrue(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷3號")));
    Assert.IsTrue(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷3-1號")));
    Assert.IsTrue(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷4號")));
    Assert.IsTrue(rule.Match(new Address("臺中市西屯區西屯路3段西平南巷4-1號")));
    
  }
}