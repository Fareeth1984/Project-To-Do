﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Web.Mvc;

public static class Extens
{

    //For Datatable to Dynamic List
    public static List<dynamic> DynamicMapToList<T>(this DataTable dt, int pageNo, int? pageSize = null, bool isSpecial = false) where T : new()
    {
        if (dt != null)
        {
            int skip = pageNo * (pageSize.HasValue ? pageSize.Value : 0);
            int take = (pageSize.HasValue ? pageSize.Value : int.MaxValue);

            var dynamicList = new List<dynamic>();
            foreach (DataRow row in dt.Rows.Cast<DataRow>().Skip(skip).Take(take))
            {
                dynamic myObj = new System.Dynamic.ExpandoObject();
                var p = myObj as IDictionary<string, object>;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    p[row.Table.Columns[i].ColumnName] = row[i];
                }
                dynamicList.Add(p);
            }
            return dynamicList;
        }
        return null;
    }

    public static SelectList ToSelectList(this DataTable table, string valueField, string textField, object selectedValue = null)
    {
        List<SelectListItem> list = new List<SelectListItem>();
        foreach (DataRow row in table.Rows)
        {
            list.Add(new SelectListItem()
            {
                Text = row[textField].ToString(),
                Value = row[valueField].ToString()
            });
        }
        if (selectedValue != null)
            return new SelectList(list.OrderBy(w => w.Text, StringComparer.CurrentCulture).ToList(), "Value", "Text", selectedValue);
        else
            return new SelectList(list.OrderBy(w => w.Text, StringComparer.CurrentCulture).ToList(), "Value", "Text");
    }

    public static List<T> MapToList<T>(this IDataReader dr, ref int totalCount) where T : new()
    {
        if (dr != null && dr.FieldCount > 0)
        {
            var entity = typeof(T);
            var entities = new List<T>();
            var propDict = new Dictionary<string, PropertyInfo>();
            var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

            while (dr.Read())
            {
                T newObject = new T();
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                    {
                        var info = propDict[dr.GetName(index).ToUpper()];
                        if ((info != null) && info.CanWrite)
                        {
                            var val = dr.GetValue(index);
                            info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                        }
                    }
                }
                entities.Add(newObject);
                totalCount = dr.GetInt32(dr.GetOrdinal("TOTAL_COUNT"));
            }
            return entities;
        }
        return null;
    }

    public static List<T> ToListof<T>(this DataTable dt)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        var columnNames = dt.Columns.Cast<DataColumn>()
            .Select(c => c.ColumnName)
            .ToList();
        var objectProperties = typeof(T).GetProperties(flags);
        var targetList = dt.AsEnumerable().Select(dataRow =>
        {
            var instanceOfT = Activator.CreateInstance<T>();

            foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
            {
                properties.SetValue(instanceOfT, dataRow[properties.Name], null);
            }
            return instanceOfT;
        }).ToList();

        return targetList;
    }

    public static List<T> DataTableToList<T>(this DataTable table) where T : new()
    {
        List<T> list = new List<T>();
        var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
        {
            PropertyInfo = propertyInfo,
            Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
        }).ToList();

        foreach (var row in table.Rows.Cast<DataRow>())
        {
            T obj = new T();
            foreach (var typeProperty in typeProperties)
            {
                object value = null;
                DataColumnCollection columns = table.Columns;
                if (columns.Contains(typeProperty.PropertyInfo.Name))
                {
                    value = row[typeProperty.PropertyInfo.Name];
                }
                object safeValue = value == null || DBNull.Value.Equals(value)
                    ? null
                    : Convert.ChangeType(value, typeProperty.Type);

                typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
            }
            list.Add(obj);
        }
        return list;
    }
}
