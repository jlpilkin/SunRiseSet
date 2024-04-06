using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRiseSet
{
    public class DecimalValue
    {
        public decimal Value { get; set; }
        public bool IsNaN { get; set; }
        public bool IsInfinity { get; set; }

        public DecimalValue()
        {
            Value = 0M;
            IsNaN = false;
            IsInfinity = false;
        }

        public DecimalValue(double value)
        {
            if (double.IsNaN(value))
            {
                IsNaN = true;
                return;
            }
            if (double.IsPositiveInfinity(value))
            {
                Value = 1M;
                IsInfinity = true;
                return;
            }
            if (double.IsNegativeInfinity(value))
            {
                Value = -1M;
                IsInfinity = true;
                return;
            }

            Value = (decimal)value;
        }

        public DecimalValue(decimal value)
        {
            Value = value;
            IsNaN = false;
            IsInfinity = false;
        }

        public static implicit operator DecimalValue(double value)
        {
            return new DecimalValue(value);
        }

        public static implicit operator DecimalValue(decimal value)
        {
            return new DecimalValue(value);
        }

        public static implicit operator double(DecimalValue value)
        {
            if (value.IsNaN)
            {
                return double.NaN;
            }
            if (value.IsInfinity)
            {
                return value.Value > 0 ? double.PositiveInfinity : double.NegativeInfinity;
            }
            return (double)value.Value;
        }

        public static implicit operator decimal(DecimalValue value)
        {
            if (value.IsNaN)
            {
                return decimal.MinValue;
            }
            if (value.IsInfinity)
            {
                return value.Value > 0 ? decimal.MaxValue : decimal.MinValue;
            }
            return value.Value;
        }

        public static DecimalValue NaN
        {
            get
            {
                return new DecimalValue { IsNaN = true };
            }
        }

        public static DecimalValue Infinity
        {
            get
            {
                return new DecimalValue { Value = 1M, IsInfinity = true };
            }
        }

        public static DecimalValue NegativeInfinity
        {
            get
            {
                return new DecimalValue { Value = -1M, IsInfinity = true };
            }
        }

        public static DecimalValue operator +(DecimalValue a, DecimalValue b)
        {
            if (a.IsNaN || b.IsNaN)
            {
                return new DecimalValue { IsNaN = true };
            }
            if (a.IsInfinity || b.IsInfinity)
            {
                return new DecimalValue { IsInfinity = true };
            }
            return new DecimalValue { Value = a.Value + b.Value };
        }

        public static DecimalValue operator -(DecimalValue a, DecimalValue b)
        {
            if (a.IsNaN || b.IsNaN)
            {
                return new DecimalValue { IsNaN = true };
            }
            if (a.IsInfinity || b.IsInfinity)
            {
                return new DecimalValue { IsInfinity = true };
            }
            return new DecimalValue { Value = a.Value - b.Value };
        }

        public static DecimalValue operator *(DecimalValue a, DecimalValue b)
        {
            if (a.IsNaN || b.IsNaN)
            {
                return new DecimalValue { IsNaN = true };
            }
            if (a.IsInfinity || b.IsInfinity)
            {
                return new DecimalValue { IsInfinity = true };
            }
            return new DecimalValue { Value = a.Value * b.Value };
        }

        public static DecimalValue operator /(DecimalValue a, DecimalValue b)
        {
            if (a.IsNaN || b.IsNaN)
            {
                return new DecimalValue { IsNaN = true };
            }
            if (a.IsInfinity || b.IsInfinity)
            {
                return new DecimalValue { IsInfinity = true };
            }
            return new DecimalValue { Value = a.Value / b.Value };
        }
    }
}
