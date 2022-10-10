using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace ZipCodeTw
{
    public class ZipCodeTwUtil
    {
        public static string GetCommonPart(string strA, string strB)
        {
            if (strA == null)
                return strB;
            else if (strB == null)
                return strA;

            int i;
            var minLen = Math.Min(strA.Length, strB.Length);
            for (i = 0; i < minLen; i++)
                if (strA[i] != strB[i])
                    break;

            return strA.Substring(0, i);
        }

        private string _dbPath;
        private string _connStr;

        private bool _keepAlive;

        private SQLiteConnection _conn = null;

        public ZipCodeTwUtil(string dbPath, bool keepAlive = false)
        {
            _dbPath = dbPath;
            _connStr = $"Data Source={_dbPath}";
            _keepAlive = keepAlive;
            if (_keepAlive)
            {
                _connStr = $"Data Source={_dbPath}";
                _conn = new SQLiteConnection(_connStr);
                _conn.Open();
            }
        }

        private SQLiteConnection GetConnection()
        {
            if (_conn != null)
                return _conn;
            else
            {
                var result = new SQLiteConnection(_connStr);
                result.Open();
                _conn = result;
                return result;
            }
        }

        private void CloseConnection()
        {
            if (!_keepAlive)
            {
                if (_conn != null)
                {
                    _conn.Close();
                    _conn = null;
                }
            }
        }

        public void CreateTables()
        {
            var connection = GetConnection();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @"
                    create table if not exists precise (
                        addr_str text,
                        rule_str text,
                        zipcode  text,
                        primary key (addr_str, rule_str)
                    );
                ";
                command.ExecuteNonQuery();

                command = connection.CreateCommand();
                command.CommandText = @"
                    create table if not exists gradual (
                        addr_str text primary key,
                        zipcode  text
                    );
                ";
                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection();
            }
        }

        private int PutPrecise(string addrStr, string ruleStr, string zipCode)
        {
            var connection = GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                insert or ignore into precise values ($addrStr, $ruleStr, $zipCode);";
            command.Parameters.AddWithValue("$addrStr", addrStr);
            command.Parameters.AddWithValue("$ruleStr", ruleStr);
            command.Parameters.AddWithValue("$zipCode", zipCode);
            return command.ExecuteNonQuery();
        }

        private int PutGradual(string addrStr, string zipCode)
        {
            var connection = GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                select zipcode
                from   gradual
                where  addr_str = $addrStr;";
            command.Parameters.AddWithValue("$addrStr", addrStr);

            var reader = command.ExecuteReader();
            string storedZipCode = null;
            if (reader.Read())
            {
                storedZipCode = reader.GetString(0);
            }
            reader.Close();

            var combinedZipCode = GetCommonPart(storedZipCode, zipCode);

            connection.CreateCommand();
            command.CommandText = "replace into gradual values ($addrStr, $zipCode)";
            command.Parameters.AddWithValue("$addrStr", addrStr);
            command.Parameters.AddWithValue("$zipCode", combinedZipCode);
            return command.ExecuteNonQuery();
        }

        private void Put(string headAddrStr, string tailRuleStr, string zipCode)
        {
            var addr = new Address(headAddrStr);

            // # (a, b, c)
            PutPrecise(addr.Flat(), headAddrStr + tailRuleStr, zipCode);

            // # (a, b, c) -> (a,); (a, b); (a, b, c); (b,); (b, c); (c,)
            var lenTokens = addr.Length;
            foreach (var f in Enumerable.Range(0, lenTokens))
                foreach (var l in Enumerable.Range(f, lenTokens - f))
                {
                    PutGradual(addr.Flat(f, l + 1), zipCode);
                }

            if (lenTokens >= 3)
                // # (a, b, c, d) -> (a, c)
                PutGradual(addr.PickToFlat(0, 2), zipCode);
        }

        public void LoadChpCsv(string chpCsvLines)
        {
            try
            {
                CreateTables();
                var connection = GetConnection();

                using (SQLiteTransaction tran = connection.BeginTransaction())
                {
                    using (StringReader sr = new StringReader(chpCsvLines))
                    {
                        bool firstLine = true;
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (firstLine)
                            {
                                firstLine = false;
                                continue;
                            }
                            var row = line.Split(',');
                            var takes = row.Length - 2;
                            Put(
                             String.Join("", row.Skip(1).Take(takes)),
                             row[row.Length - 1],
                             row[0]
                            );
                        }
                    }
                    tran.Commit();
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<Tuple2<string, string>> GetRuleStrZipcodePairs(string addrStr)
        {
            var connection = GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                select rule_str, zipcode
                from   precise
                where  addr_str = $addrStr;
            ";
            command.Parameters.AddWithValue("$addrStr", addrStr);
            var reader = command.ExecuteReader();
            List<Tuple2<string, string>> result = new List<Tuple2<string, string>>();
            while (reader.Read())
            {
                var item = new Tuple2<string, string>(reader.GetString(0), reader.GetString(1));
                result.Add(item);
            }
            reader.Close();
            return result;
        }

        public string GetGradualZipcode(string addrStr)
        {
            var connection = GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                select zipcode
                from   gradual
                where  addr_str = $addrStr;
            ";
            command.Parameters.AddWithValue("$addrStr", addrStr);

            string result = null;
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                result = reader.GetString(0);
            }
            reader.Close();
            return result;
        }

        public string Find(string addrStr)
        {
            try
            {
                var addr = new Address(addrStr);
                var lenAddrTokens = addr.Length;

                // # avoid unnecessary iteration
                var startLen = lenAddrTokens;
                while (startLen >= 0)
                {
                    if (addr.Parse(startLen - 1) == new Tuple2<int, int>(0, 0))
                        break;
                    startLen -= 1;
                }

                for (int i = startLen; i >= 0; i--)
                {
                    addrStr = addr.Flat(i);
                    var rzpairs = GetRuleStrZipcodePairs(addrStr);

                    var villageToken = "";
                    if (lenAddrTokens >= 4)
                        villageToken = addr.Tokens[2].Item4;

                    // # for handling insignificant tokens and redundant unit
                    if ( // # It only runs once, and must be the first iteration.
                        i == startLen &&
                        lenAddrTokens >= 4 &&
                        (villageToken == "村" || villageToken == "里") &&
                        rzpairs.Count > 0)
                    {
                        if (addr.Tokens[3].Item4 == "鄰")
                        {
                            // # delete the insignificant token (whose unit is 鄰)
                            addr.Tokens.RemoveAt(3);
                            lenAddrTokens--;
                        }
                        if (lenAddrTokens >= 4 && addr.Tokens[3].Item4 == "號")
                            // # empty the redundant unit in the token
                            addr.Tokens[2] = new Tuple4<string, string, string, string>("", "", addr.Tokens[2].Item3, "");
                        else
                            // # delete insignificant token (whose unit is 村 or 里)
                            addr.Tokens.RemoveAt(2);

                        rzpairs = GetRuleStrZipcodePairs(addr.Flat(3));
                    }
                    if (rzpairs.Count > 0)
                    {
                        foreach (var rzpair in rzpairs)
                        {
                            var ruleStr = rzpair.Item1;
                            var zipcode = rzpair.Item2;
                            if (new Rule(ruleStr).Match(addr))
                                return zipcode;
                        }
                    }

                    var gzipcode = GetGradualZipcode(addrStr);
                    if (gzipcode != null)
                        return gzipcode;
                }
                return "";
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}