﻿using System;
using System.Text;

namespace MyOrm
{
    static internal class OrmMapExtension
    {
        public static string GetColumnsNamesList(this OrmMap map)
        {
            StringBuilder b = new StringBuilder();
            foreach (string col in map.Columns)
            {
                b.Append(col);
                b.Append(',');
            }
            b.Remove(b.Length - 1, 1);
            return b.ToString();
        }


        public static string BuildSelectAllQuery(this OrmMap map)
        {
            string selectTemplate = "SELECT {0} FROM {1}";
            string argCols = GetColumnsNamesList(map);
            return String.Format(selectTemplate, argCols, map.TableName);
        }

        public static string BuildSelectWhereQuery(this OrmMap map, string whereSection)
        {
            string selectTemplate = "SELECT {0} FROM {1} WHERE " + whereSection;
            string argCols = GetColumnsNamesList(map);
            return String.Format(selectTemplate, argCols, map.TableName);
        }

        public static string BuildInsertQuery(this OrmMap map, string argValues)
        {
            string insertTemplate = "INSERT INTO {0}({1}) VALUES(" + argValues + ")";
            string argCols = GetColumnsNamesList(map);
            return String.Format(insertTemplate, map.TableName, argCols);
        }

        public static string BuildDeleteQuery(this OrmMap map, string whereSection)
        {
            string deleteTemplate = "DELETE FROM {0} WHERE " + whereSection;
            return String.Format(deleteTemplate, map.TableName);
        }

        public static string BuildSubSelectQuery(this OrmMap map, string[] columns, string whereSection)
        { 
            StringBuilder builder = new StringBuilder();
            builder.Append("(SELECT ");
            foreach (string col in columns)
            {
                builder.Append(col);
                builder.Append(',');
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append(" FROM ");
            builder.Append(map.TableName);
            builder.Append(" WHERE (");
            builder.Append(whereSection);
            builder.Append(" ))");
            return builder.ToString();
        }

        public static string BuildSubSelectQuery(this OrmMap map, string column, string whereSection)
        {
            return BuildSubSelectQuery(map, new string[] { column }, whereSection);
        }


        public static object GetId(this OrmMap map, object o)
        {
            return map[map.ID].GetValue(o);
        } 
    }
}
