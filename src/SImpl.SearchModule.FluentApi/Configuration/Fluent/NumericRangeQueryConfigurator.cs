
using System;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class NumericRangeQueryConfigurator  
    {
        public NumericRangeQueryConfigurator()
        {
            Query = new NumericRange();
        }

        public NumericRange Query { get; set; }
        public NumericRangeQueryConfigurator WithField(string fieldName)
        {
            Query.Field = fieldName;
            return this;  
        }
        public NumericRangeQueryConfigurator WithMinValue(int value, bool includeEdge = false)
        {
            Query.MinValue = value;
            Query.IncludeMinEdge = includeEdge;
            return this;
        }
        public NumericRangeQueryConfigurator WithMaxValue(int value, bool includeEdge = false)
        {
            Query.MinValue = value;
            Query.IncludeMaxEdge = includeEdge;
            return this;
        }
    }
    public class LongRangeQueryConfigurator  
    {
        public LongRangeQueryConfigurator()
        {
            Query = new LongRange();
        }

        public LongRange Query { get; set; }
        public LongRangeQueryConfigurator WithField(string fieldName)
        {
            Query.Field = fieldName;
            return this;  
        }
        public LongRangeQueryConfigurator WithMinValue(long value, bool includeEdge = false)
        {
            Query.MinValue = value;
            Query.IncludeMinEdge = includeEdge;
            return this;
        }
        public LongRangeQueryConfigurator WithMaxValue(long value, bool includeEdge = false)
        {
            Query.MinValue = value;
            Query.IncludeMaxEdge = includeEdge;
            return this;
        }
    }
    public class DateRangeQueryConfigurator  
    {
        public DateRangeQueryConfigurator()
        {
            Query = new DateRangeQuery();
        }

        public DateRangeQuery Query { get; set; }
        public DateRangeQueryConfigurator WithField(string fieldName)
        {
            Query.Field = fieldName;
            return this;  
        }
        public DateRangeQueryConfigurator WithMinValue(DateTime value, bool includeEdge = false)
        {
            Query.MinValue = value;
            Query.IncludeMinEdge = includeEdge;
            return this;
        }
        public DateRangeQueryConfigurator WithMaxValue(DateTime value, bool includeEdge = false)
        {
            Query.MinValue = value;
            Query.IncludeMaxEdge = includeEdge;
            return this;
        }
    }
    public class StringRangeQueryConfigurator  
    {
        public StringRangeQueryConfigurator()
        {
            Query = new StringRange();
        }

        public StringRange Query { get; set; }
        public StringRangeQueryConfigurator WithField(string fieldName)
        {
            Query.Field = fieldName;
            return this;  
        }
        public StringRangeQueryConfigurator WithMinValue(string value, bool includeEdge = false)
        {
            Query.MinValue = value;
            Query.IncludeMinEdge = includeEdge;
            return this;
        }
        public StringRangeQueryConfigurator WithMaxValue(string value, bool includeEdge = false)
        {
            Query.MinValue = value;
            Query.IncludeMaxEdge = includeEdge;
            return this;
        }
    }
}