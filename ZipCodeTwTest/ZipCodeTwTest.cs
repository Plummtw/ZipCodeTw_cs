using ZipCodeTw;

namespace ZipCodeTw_Test;

public class ZipcodeTest
{
    private ZipCodeTwUtil? _zipcode;

    [SetUp]
    public void Setup()
    {
        var chpCsvLines = @"郵遞區號,縣市名稱,鄉鎮市區,原始路名,投遞範圍
10058,臺北市,中正區,八德路１段,全
10079,臺北市,中正區,三元街,單全
10070,臺北市,中正區,三元街,雙  48號以下
10079,臺北市,中正區,三元街,雙  50號以上
10068,臺北市,中正區,大埔街,單  15號以上
10068,臺北市,中正區,大埔街,雙  36號以上
10051,臺北市,中正區,中山北路１段,單   3號以下
10041,臺北市,中正區,中山北路１段,雙  48號以下
10051,臺北市,中正區,中山南路,單   5號以下
10041,臺北市,中正區,中山南路,雙  18號以下
10002,臺北市,中正區,中山南路,　   7號
10051,臺北市,中正區,中山南路,　   9號
10048,臺北市,中正區,中山南路,單  11號以上
10001,臺北市,中正區,中山南路,　  20號
10043,臺北市,中正區,中華路１段,單  25之   3號以下
10042,臺北市,中正區,中華路１段,單  27號至  47號
10010,臺北市,中正區,中華路１段,　  49號
10042,臺北市,中正區,中華路１段,單  51號以上
10065,臺北市,中正區,中華路２段,單  79號以下
10066,臺北市,中正區,中華路２段,單  81號至 101號
10068,臺北市,中正區,中華路２段,單 103號至 193號
10069,臺北市,中正區,中華路２段,單 195號至 315號
10067,臺北市,中正區,中華路２段,單 317號至 417號
10072,臺北市,中正區,中華路２段,單 419號以上
10055,臺北市,中正區,丹陽街,全
10051,臺北市,中正區,仁愛路１段,　   1號
10052,臺北市,中正區,仁愛路１段,連   2之   4號以上
10055,臺北市,中正區,仁愛路２段,單  37號以下
10060,臺北市,中正區,仁愛路２段,雙  48號以下
10056,臺北市,中正區,仁愛路２段,單  39號至  49號
10056,臺北市,中正區,仁愛路２段,雙  48之   1號至  64號
10062,臺北市,中正區,仁愛路２段,單  51號以上
10063,臺北市,中正區,仁愛路２段,雙  66號以上
20201,基隆市,中正區,義一路,　   1號
20241,基隆市,中正區,義一路,連   2號以上
20250,基隆市,中正區,義二路,全
20241,基隆市,中正區,義三路,單全
20248,基隆市,中正區,漁港一街,全
20249,基隆市,中正區,漁港二街,全
20249,基隆市,中正區,漁港三街,全
20249,基隆市,中正區,調和街,全
20248,基隆市,中正區,環港街,全
20243,基隆市,中正區,豐稔街,全
20249,基隆市,中正區,觀海街,全
36046,苗栗縣,苗栗市,大埔街,全
81245,高雄市,小港區,豐田街,全
81245,高雄市,小港區,豐登街,全
81245,高雄市,小港區,豐善街,全
81245,高雄市,小港區,豐街,全
81245,高雄市,小港區,豐點街,全
81257,高雄市,小港區,寶山街,全
81362,高雄市,左營區,大中一路,單 331號以上
81362,高雄市,左營區,大中一路,雙 386號以上
81362,高雄市,左營區,大中二路,單 241號以下
81368,高雄市,左營區,大中二路,雙 200號以下
81369,高雄市,左營區,大中二路,雙 202號至 698號
81369,高雄市,左營區,大中二路,單 243號至 479號
81365,高雄市,左營區,大中二路,單 481號以上
81354,高雄市,左營區,大中二路,雙 700號以上
81357,高雄市,左營區,大順一路,單  91號至  95號
81357,高雄市,左營區,大順一路,雙  96號至 568號
81357,高雄市,左營區,大順一路,單 201號至 389巷";

        this._zipcode = new ZipCodeTwUtil(":memory:", true);
        this._zipcode.LoadChpCsv(chpCsvLines);
    }

    [Test]
    public void TestFind()
    {
        if (_zipcode == null)
            throw new NullReferenceException("ZipCode");

        // # 10043,臺北市,中正區,中華路１段,單  25之   3號以下
        Assert.That(_zipcode.Find("臺北市中正區中華路１段25號"), Is.EqualTo("10043"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段25-2號"), Is.EqualTo("10043"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段25-3號"), Is.EqualTo("10043"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段25-4號"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段26號"), Is.EqualTo("100"));

        // # 10042,臺北市,中正區,中華路１段,單  27號至  47號
        Assert.That(_zipcode.Find("臺北市中正區中華路１段25號"), Is.EqualTo("10043"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段26號"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段27號"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段28號"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段29號"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段45號"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段46號"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段47號"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段48號"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段49號"), Is.EqualTo("10010"));

        // # 10010,臺北市,中正區,中華路１段,　  49號
        Assert.That(_zipcode.Find("臺北市中正區中華路１段48號"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段49號"), Is.EqualTo("10010"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段50號"), Is.EqualTo("100"));

        // # 10042,臺北市,中正區,中華路１段,單  51號以上
        Assert.That(_zipcode.Find("臺北市中正區中華路１段49號"), Is.EqualTo("10010"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段50號"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段51號"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段52號"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區中華路１段53號"), Is.EqualTo("10042"));
    }

    [Test]
    public void TestFindGradually()
    {
        if (_zipcode == null)
            throw new NullReferenceException("ZipCode");

        Assert.That(_zipcode.Find("臺北市"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("臺北市中正區仁愛路１段"), Is.EqualTo("1005"));
        Assert.That(_zipcode.Find("臺北市中正區仁愛路１段1號"), Is.EqualTo("10051"));
    }

    [Test]
    public void TestFindMiddleToken()
    {
        if (_zipcode == null)
            throw new NullReferenceException("ZipCode");

        Assert.That(_zipcode.Find("左營區"), Is.EqualTo("813"));
        Assert.That(_zipcode.Find("大中一路"), Is.EqualTo("81362"));
        Assert.That(_zipcode.Find("大中二路"), Is.EqualTo("813"));
        Assert.That(_zipcode.Find("左營區大中一路"), Is.EqualTo("81362"));
        Assert.That(_zipcode.Find("左營區大中二路"), Is.EqualTo("813"));

        Assert.That(_zipcode.Find("小港區"), Is.EqualTo("812"));
        Assert.That(_zipcode.Find("豐街"), Is.EqualTo("81245"));
        Assert.That(_zipcode.Find("小港區豐街"), Is.EqualTo("81245"));

        Assert.That(_zipcode.Find("中正區"), Is.EqualTo(""));

        Assert.That(_zipcode.Find("大埔街"), Is.EqualTo(""));
        Assert.That(_zipcode.Find("台北市大埔街"), Is.EqualTo("10068"));
        Assert.That(_zipcode.Find("苗栗縣大埔街"), Is.EqualTo("36046"));
    }
}