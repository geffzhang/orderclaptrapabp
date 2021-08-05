using System;
using System.Collections.Generic;
using System.Text;

namespace OrderClaptrap.EntityFrameworkCore
{
    public static class OrderClaptrapDbProperties
    {
        public static string DbTablePrefix { get; set; } = "OrderClaptrap";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "OrderClaptrap";
    }
}