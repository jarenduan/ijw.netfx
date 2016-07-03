//TODO: need to modify to use string inside
using System;

namespace ijw.Maths
{
    public struct Number : IEquatable<Number>
    {
        public Int64 IntegerPart { get; set; }

        public Int64 Numerator { get; set; }

        public Int64 Denominator { get; set; }

        public Number Add(Number other) {
            Number result = new Number();
            result.IntegerPart = this.IntegerPart + other.IntegerPart;
            if (this.Denominator == other.Denominator) {
                result.Denominator = this.Denominator;
                result.Numerator = this.Numerator + other.Numerator;
            }
            else {
                result.Denominator = this.Denominator * other.Denominator;
                result.Numerator = this.Numerator * other.Denominator + other.Numerator * this.Denominator;
            }
            deduce();
            return result;
        }

        private void deduce() {
            if (Denominator == 1) {
                return;
            }
            if (Numerator < Denominator) {
                return;
            }
            Int64 d = Numerator / Denominator;
            if (d * Denominator == Numerator) {
                this.Numerator = 1;
                this.Denominator = d;
            }
            else {
                this.IntegerPart += Numerator * d - Denominator;
            }
        }

        public override int GetHashCode() {
            return this.IntegerPart.GetHashCode() ^ this.Numerator.GetHashCode() ^ this.Denominator.GetHashCode();
        }

        public static bool operator ==(Number left, Number right) {
            return left.IntegerPart.Equals(right.IntegerPart)
                && left.Denominator.Equals(right.Denominator)
                && left.Numerator.Equals(right.Numerator);
        }

        public static bool operator !=(Number left, Number right) {
            return !(left == right);
        }
        public override bool Equals(object obj) {
            return obj is Number && this == (Number)obj;
        }

        public override string ToString() {
            if (this.Numerator == 0) {
                return this.IntegerPart.ToString();
            }
            else if (this.Denominator == 1) {
                return string.Format("{0}.{1}", this.IntegerPart, this.Numerator);
            }
            else {
                return string.Format("Number: {0} + {1} / {2}", this.IntegerPart, this.Numerator, this.Denominator);
            }
        }

        public bool Equals(Number other) {
            return this.Numerator.Equals(other.Numerator) && this.Denominator.Equals(other.Denominator) && this.IntegerPart.Equals(other.IntegerPart);
        }
    }
}
