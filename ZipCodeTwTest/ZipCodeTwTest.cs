using ZipCodeTw;

namespace ZipCodeTw_Test;

public class ZipcodeTest
{
    private ZipCodeTwUtil? _zipcode;

    [SetUp]
    public void Setup()
    {
        var chpCsvLines = @"�l���ϸ�,�����W��,�m����,��l���W,�뻼�d��
10058,�O�_��,������,�K�w�����q,��
10079,�O�_��,������,�T����,���
10070,�O�_��,������,�T����,��  48���H�U
10079,�O�_��,������,�T����,��  50���H�W
10068,�O�_��,������,�j�H��,��  15���H�W
10068,�O�_��,������,�j�H��,��  36���H�W
10051,�O�_��,������,���s�_�����q,��   3���H�U
10041,�O�_��,������,���s�_�����q,��  48���H�U
10051,�O�_��,������,���s�n��,��   5���H�U
10041,�O�_��,������,���s�n��,��  18���H�U
10002,�O�_��,������,���s�n��,�@   7��
10051,�O�_��,������,���s�n��,�@   9��
10048,�O�_��,������,���s�n��,��  11���H�W
10001,�O�_��,������,���s�n��,�@  20��
10043,�O�_��,������,���ظ����q,��  25��   3���H�U
10042,�O�_��,������,���ظ����q,��  27����  47��
10010,�O�_��,������,���ظ����q,�@  49��
10042,�O�_��,������,���ظ����q,��  51���H�W
10065,�O�_��,������,���ظ����q,��  79���H�U
10066,�O�_��,������,���ظ����q,��  81���� 101��
10068,�O�_��,������,���ظ����q,�� 103���� 193��
10069,�O�_��,������,���ظ����q,�� 195���� 315��
10067,�O�_��,������,���ظ����q,�� 317���� 417��
10072,�O�_��,������,���ظ����q,�� 419���H�W
10055,�O�_��,������,������,��
10051,�O�_��,������,���R�����q,�@   1��
10052,�O�_��,������,���R�����q,�s   2��   4���H�W
10055,�O�_��,������,���R�����q,��  37���H�U
10060,�O�_��,������,���R�����q,��  48���H�U
10056,�O�_��,������,���R�����q,��  39����  49��
10056,�O�_��,������,���R�����q,��  48��   1����  64��
10062,�O�_��,������,���R�����q,��  51���H�W
10063,�O�_��,������,���R�����q,��  66���H�W
20201,�򶩥�,������,�q�@��,�@   1��
20241,�򶩥�,������,�q�@��,�s   2���H�W
20250,�򶩥�,������,�q�G��,��
20241,�򶩥�,������,�q�T��,���
20248,�򶩥�,������,����@��,��
20249,�򶩥�,������,����G��,��
20249,�򶩥�,������,����T��,��
20249,�򶩥�,������,�թM��,��
20248,�򶩥�,������,�����,��
20243,�򶩥�,������,�׸Z��,��
20249,�򶩥�,������,�[����,��
36046,�]�߿�,�]�ߥ�,�j�H��,��
81245,������,�p���,�ץе�,��
81245,������,�p���,�׵n��,��
81245,������,�p���,�׵���,��
81245,������,�p���,�׵�,��
81245,������,�p���,���I��,��
81257,������,�p���,�_�s��,��
81362,������,�����,�j���@��,�� 331���H�W
81362,������,�����,�j���@��,�� 386���H�W
81362,������,�����,�j���G��,�� 241���H�U
81368,������,�����,�j���G��,�� 200���H�U
81369,������,�����,�j���G��,�� 202���� 698��
81369,������,�����,�j���G��,�� 243���� 479��
81365,������,�����,�j���G��,�� 481���H�W
81354,������,�����,�j���G��,�� 700���H�W
81357,������,�����,�j���@��,��  91����  95��
81357,������,�����,�j���@��,��  96���� 568��
81357,������,�����,�j���@��,�� 201���� 389��";

        this._zipcode = new ZipCodeTwUtil(":memory:", true);
        this._zipcode.LoadChpCsv(chpCsvLines);
    }

    [Test]
    public void TestFind()
    {
        if (_zipcode == null)
            throw new NullReferenceException("ZipCode");

        // # 10043,�O�_��,������,���ظ����q,��  25��   3���H�U
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q25��"), Is.EqualTo("10043"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q25-2��"), Is.EqualTo("10043"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q25-3��"), Is.EqualTo("10043"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q25-4��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q26��"), Is.EqualTo("100"));

        // # 10042,�O�_��,������,���ظ����q,��  27����  47��
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q25��"), Is.EqualTo("10043"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q26��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q27��"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q28��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q29��"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q45��"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q46��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q47��"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q48��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q49��"), Is.EqualTo("10010"));

        // # 10010,�O�_��,������,���ظ����q,�@  49��
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q48��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q49��"), Is.EqualTo("10010"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q50��"), Is.EqualTo("100"));

        // # 10042,�O�_��,������,���ظ����q,��  51���H�W
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q49��"), Is.EqualTo("10010"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q50��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q51��"), Is.EqualTo("10042"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q52��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��ظ����q53��"), Is.EqualTo("10042"));
    }

    [Test]
    public void TestFindGradually()
    {
        if (_zipcode == null)
            throw new NullReferenceException("ZipCode");

        Assert.That(_zipcode.Find("�O�_��"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_��������"), Is.EqualTo("100"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��R�����q"), Is.EqualTo("1005"));
        Assert.That(_zipcode.Find("�O�_�������Ϥ��R�����q1��"), Is.EqualTo("10051"));
    }

    [Test]
    public void TestFindMiddleToken()
    {
        if (_zipcode == null)
            throw new NullReferenceException("ZipCode");

        Assert.That(_zipcode.Find("�����"), Is.EqualTo("813"));
        Assert.That(_zipcode.Find("�j���@��"), Is.EqualTo("81362"));
        Assert.That(_zipcode.Find("�j���G��"), Is.EqualTo("813"));
        Assert.That(_zipcode.Find("����Ϥj���@��"), Is.EqualTo("81362"));
        Assert.That(_zipcode.Find("����Ϥj���G��"), Is.EqualTo("813"));

        Assert.That(_zipcode.Find("�p���"), Is.EqualTo("812"));
        Assert.That(_zipcode.Find("�׵�"), Is.EqualTo("81245"));
        Assert.That(_zipcode.Find("�p����׵�"), Is.EqualTo("81245"));

        Assert.That(_zipcode.Find("������"), Is.EqualTo(""));

        Assert.That(_zipcode.Find("�j�H��"), Is.EqualTo(""));
        Assert.That(_zipcode.Find("�x�_���j�H��"), Is.EqualTo("10068"));
        Assert.That(_zipcode.Find("�]�߿��j�H��"), Is.EqualTo("36046"));
    }
}