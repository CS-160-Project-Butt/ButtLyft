using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace AASC.Partner.API.Helpers
{
    public class DynamicLinqHelper<T>
    {
        private ParameterExpression _paramExpression;
        private Expression CreateEqualityExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            return Expression.Equal(fieldExpression, constantExpression);
        }

        private Expression CreateNotEqualityExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            return Expression.NotEqual(fieldExpression, constantExpression);
        }

        private Expression CreateContainsExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            var callExpression = Expression.Call(fieldExpression, typeof(string).GetMethod("Contains"), constantExpression);
            return callExpression;
        }

        private Expression CreateDoesNotContainExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            var callExpression = Expression.Call(fieldExpression, typeof(string).GetMethod("Contains"), constantExpression);
            var expression = Expression.Not(callExpression);
            return expression;
        }

        private Expression CreateGreaterThanExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            Expression greaterThanExpr = Expression.GreaterThan(fieldExpression, constantExpression);
            return greaterThanExpr;
        }

        private Expression CreateGreaterThanOrEqualToExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            Expression greaterOrEqualToExpr = Expression.GreaterThanOrEqual(fieldExpression, constantExpression);
            return greaterOrEqualToExpr;
        }

        private Expression CreateLessThanExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            Expression lessThanExpr = Expression.LessThan(fieldExpression, constantExpression);
            return lessThanExpr;
        }

        private Expression CreateLessThanOrEqualToExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            Expression lessThanOrEqualToExpr = Expression.LessThanOrEqual(fieldExpression, constantExpression);
            return lessThanOrEqualToExpr;
        }

        private Expression CreateStartsWithExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            var callExpression = Expression.Call(fieldExpression, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constantExpression);
            return callExpression;
        }

        private Expression CreateEndsWithExpression(string propertyName, string propertyFilterValue)
        {
            var propInfo = typeof(T).GetProperty(propertyName);
            //var convertedFilterValue = Convert.ChangeType(propertyFilterValue, propInfo.PropertyType);
            var convertedFilterValue = ConvertToPropType(propInfo, propertyFilterValue);
            var fieldExpression = Expression.Property(_paramExpression, propInfo);
            var constantExpression = Expression.Constant(convertedFilterValue, propInfo.PropertyType);
            var callExpression = Expression.Call(fieldExpression, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constantExpression);
            return callExpression;
        }

        public Expression<Func<T, bool>> CreateFilterPredicate(List<Filter> propertyWithFilterValues, bool useConjunction = true)
        {
            Expression whereCondition = null;
            Expression expr = null;
            _paramExpression = Expression.Parameter(typeof(T), "entity");
            foreach (var propertyFilterPair in propertyWithFilterValues)
            {
                var fieldName = propertyFilterPair.field.Substring(0, 1).ToUpper() +
                    propertyFilterPair.field.Substring(1, propertyFilterPair.field.Length - 1);
                switch (propertyFilterPair.Operator)
                {
                    case "eq": // equals to
                        expr = CreateEqualityExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "neq": // not equals to
                        expr = CreateNotEqualityExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "startswith":
                        expr = CreateStartsWithExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "contains":
                        expr = CreateContainsExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "doesnotcontain":
                        expr = CreateDoesNotContainExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "endswith":
                        expr = CreateEndsWithExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "gt":
                        expr = CreateGreaterThanExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "gte":
                        expr = CreateGreaterThanOrEqualToExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "lt":
                        expr = CreateLessThanExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                    case "lte":
                        expr = CreateLessThanOrEqualToExpression(fieldName, ConvertToPropType(_paramExpression.Type.GetProperty(fieldName), propertyFilterPair.value).ToString());
                        break;
                }
                whereCondition = whereCondition == null
                    ? expr
                    : (useConjunction
                        ? Expression.And(whereCondition, expr)
                        : Expression.Or(whereCondition, expr));
            }
            return Expression.Lambda<Func<T, bool>>(whereCondition, new[] { _paramExpression });
        }

        public static object ConvertToPropType(PropertyInfo property, object value)
        {
            object castValue = null;
            if (property != null)
            {
                Type propType = Nullable.GetUnderlyingType(property.PropertyType);
                bool isNullable = (propType != null);
                if (!isNullable) propType = property.PropertyType;
                bool canCast = (value != null || isNullable);
                if (!canCast) throw new Exception("Can't cast to null on non-nullable type.");

                var converter = TypeDescriptor.GetConverter(propType);
                castValue = (value == null || Convert.IsDBNull(value) ? null : (converter.CanConvertFrom(value.GetType()) ? converter.ConvertFrom(value) : null));

                //castValue = (value == null || Convert.IsDBNull(value) ? null : Convert.ChangeType(value, propType));
            }
            return castValue;
        }
    }
}