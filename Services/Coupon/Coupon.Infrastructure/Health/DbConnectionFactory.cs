﻿using Npgsql;
using System.Data;

namespace Coupon.Infrastructure.Health;

public sealed class DbConnectionFactory(string connectionString)
{
    public IDbConnection OpenConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}