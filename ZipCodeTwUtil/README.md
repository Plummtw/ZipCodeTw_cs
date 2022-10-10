# This is a port from Moskytw's zipcodetw (Python Library).

## Usage

In-Memory Use (KeepAlive = true)
```
var zipcodetw = new ZipCodeTw.ZipCodeTwUtil(":memory:", true);
```

or Use SQLite File
```
var zipcodetw = new ZipCodeTw.ZipCodeTwUtil(filePath);
```

Then load the entire CSV to generate the SQLite Database
( first time only, but when using the in-memory mode, it needs to be loaded every time ).

```
zipcodetw.LoadChpCsv(csvLinesAsString);
```

After the SQLite Database is generated, next time just pass the filePath,
and use the DB.

Example:
```
zipcodetw.Find("臺北市");
zipcodetw.Find("臺北市信義區");
zipcodetw.Find("臺北市信義區市府路");
zipcodetw.Find("臺北市信義區市府路1號");
```

Example 2:

```
zipcodetw.Find("松山區");
zipcodetw.Find("秀山街");
zipcodetw.Find("台北市秀山街");
```