using ZipCodeTw;

namespace ZipCodeTw_Test;

public class AddressTest
{
  [SetUp]
  public void Setup()
  {
  }

  [Test]
  public void TestAddressInit()
  {
    var expected_tokens = new List<Tuple4<string, string, string, string>>
          {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
           new Tuple4<string, string, string, string>("", "", "大安", "區"),
           new Tuple4<string, string, string, string>("", "", "市府", "路"),
           new Tuple4<string, string, string, string>("1", "", "", "號")};
    Assert.That(new Address("臺北市大安區市府路1號").Tokens, Is.EqualTo(expected_tokens));
  }

  [Test]
  public void TestAddressInitSubno()
  {
    var expected_tokens = new List<Tuple4<string, string, string, string>>
          {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
           new Tuple4<string, string, string, string>("", "", "大安", "區"),
           new Tuple4<string, string, string, string>("", "", "市府", "路"),
           new Tuple4<string, string, string, string>("1", "之1", "", "號")};

    Assert.That(new Address("臺北市大安區市府路1之1號").Tokens, Is.EqualTo(expected_tokens));
  }

  [Test]
  public void TestTrickyInput()
  {
    var expected_tokens1 = new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "桃園", "縣"),
       new Tuple4<string, string, string, string>("", "", "中壢", "市"),
       new Tuple4<string, string, string, string>("", "", "普義", "")};
    Assert.That(new Address("桃園縣中壢市普義").Tokens, Is.EqualTo(expected_tokens1));

    var expected_tokens2 = new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "桃園", "縣"),
       new Tuple4<string, string, string, string>("", "", "中壢", "市"),
       new Tuple4<string, string, string, string>("", "", "普義", ""),
       new Tuple4<string, string, string, string>("10", "", "", "號")};
    Assert.That(new Address("桃園縣中壢市普義10號").Tokens, Is.EqualTo(expected_tokens2));

    var expected_tokens3 = new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中山", "區"),
       new Tuple4<string, string, string, string>("", "", "敬業1", "路")};
    Assert.That(new Address("臺北市中山區敬業1路").Tokens, Is.EqualTo(expected_tokens3));

    var expected_tokens4 = new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "中山", "區"),
       new Tuple4<string, string, string, string>("", "", "敬業1", "路"),
       new Tuple4<string, string, string, string>("10", "", "", "號")};
    Assert.That(new Address("臺北市中山區敬業1路10號").Tokens, Is.EqualTo(expected_tokens4));
  }

  [Test]
  public void TestAddressInitNormalization()
  {
    var expected_tokens = new List<Tuple4<string, string, string, string>>
      {new Tuple4<string, string, string, string>("", "", "臺北", "市"),
       new Tuple4<string, string, string, string>("", "", "大安", "區"),
       new Tuple4<string, string, string, string>("", "", "市府", "路"),
       new Tuple4<string, string, string, string>("1", "之1", "", "號")};
    Assert.That(new Address("臺北市大安區市府路1之1號").Tokens, Is.EqualTo(expected_tokens));
    Assert.That(new Address("台北市大安區市府路1之1號").Tokens, Is.EqualTo(expected_tokens));
    Assert.That(new Address("臺北市大安區市府路１之１號").Tokens, Is.EqualTo(expected_tokens));
    Assert.That(new Address("臺北市　大安區　市府路 1 之 1 號").Tokens, Is.EqualTo(expected_tokens));
    Assert.That(new Address("臺北市，大安區，市府路 1 之 1 號").Tokens, Is.EqualTo(expected_tokens));
    Assert.That(new Address("臺北市, 大安區, 市府路 1 之 1 號").Tokens, Is.EqualTo(expected_tokens));
    Assert.That(new Address("臺北市, 大安區, 市府路 1 - 1 號").Tokens, Is.EqualTo(expected_tokens));
  }

  [Test]
  public void TestAddressInitNormalizationChineseNumber()
  {
    Assert.That(Address.Normalize("八德路"), Is.EqualTo("八德路"));
    Assert.That(Address.Normalize("三元街"), Is.EqualTo("三元街"));

    Assert.That(Address.Normalize("三號"), Is.EqualTo("3號"));
    Assert.That(Address.Normalize("十八號"), Is.EqualTo("18號"));
    Assert.That(Address.Normalize("三十八號"), Is.EqualTo("38號"));

    Assert.That(Address.Normalize("三段"), Is.EqualTo("3段"));
    Assert.That(Address.Normalize("十八路"), Is.EqualTo("18路"));
    Assert.That(Address.Normalize("三十八街"), Is.EqualTo("38街"));

    Assert.That(Address.Normalize("信義路一段"), Is.EqualTo("信義路1段"));
    Assert.That(Address.Normalize("敬業一路"), Is.EqualTo("敬業1路"));
    Assert.That(Address.Normalize("愛富三街"), Is.EqualTo("愛富3街"));
  }

  [Test]
  public void TestAddressFlat()
  {
    var addr = new Address("臺北市大安區市府路1之1號");

    Assert.That(addr.Flat(1), Is.EqualTo("臺北市"));
    Assert.That(addr.Flat(-3), Is.EqualTo("臺北市"));
    Assert.That(addr.Flat(2), Is.EqualTo("臺北市大安區"));
    Assert.That(addr.Flat(-2), Is.EqualTo("臺北市大安區"));
    Assert.That(addr.Flat(3), Is.EqualTo("臺北市大安區市府路"));
    Assert.That(addr.Flat(-1), Is.EqualTo("臺北市大安區市府路"));

    Assert.That(addr.Flat(), Is.EqualTo("臺北市大安區市府路1之1號"));
  }
}