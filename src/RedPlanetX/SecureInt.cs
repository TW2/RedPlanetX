using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    class SecureInt
    {
        public long Value { get; set; }

        public SecureInt()
        {
            Value = 0;
        }

        public SecureInt(long value)
        {
            Value = value;
        }

        /**
         * Returns number or -1 if error.         
         */
        public short TryGetInt16()
        {
            try
            {
                return Convert.ToInt16(Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        /**
         * Returns number or -1 if error.         
         */
        public int TryGetInt32()
        {
            try
            {
                return Convert.ToInt32(Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        /**
         * Do the addition a + b.
         * Returns number or -1 if error.         
         */
        public static long Add(long a, long b)
        {
            try
            {
                return a + b;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        /**
         * Do the substraction a - b
         * Returns number or -1 if error.         
         */
        public static long Substract(long a, long b)
        {
            try
            {
                return a - b;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        /**
         * Do the addition a / b.
         * Returns number or -1 if error.         
         */
        public static long Divide(long a, long b)
        {
            try
            {
                return a / b;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        /**
         * Do the addition a * b.
         * Returns number or -1 if error.         
         */
        public static long Multiply(long a, long b)
        {
            try
            {
                return a * b;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}
