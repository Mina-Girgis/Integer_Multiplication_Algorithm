using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class IntegerMultiplication
    {
        #region YOUR CODE IS HERE

        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 large integers of N digits in an efficient way [Karatsuba's Method]
        /// </summary>
        /// <param name="X">First large integer of N digits  [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="Y">Second large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="N">Number of digits (power of 2)</param>
        /// <returns>Resulting large integer of 2xN digits (left padded with 0's if necessarily) [0: least signif., 2xN-1: most signif.]</returns>

        static public byte[] addTwoArrays(byte[] X, byte[] Y)
        {
            int size = Math.Max(X.Length, Y.Length);
            byte[] ans = new byte[size];
            byte carry = 0;
            for (int i = 0; i < size; i++)
            {
                byte num1 = 0;
                byte num2 = 0;
                if (i < X.Length) num1 = X[i];
                if (i < Y.Length) num2 = Y[i];
                int sum = (num1 + num2 + carry);
                carry = 0;
                ans[i] = (byte)(sum % 10);
                carry = (byte)(sum / 10);
            }
            if (carry != 0)
            {
                byte[] ans2 = new byte[size + 1];
                for (int i = 0; i <= size; i++)
                {
                    if (i == size) ans2[i] = carry;
                    else
                    {
                        ans2[i] = ans[i];
                    }
                }
                return ans2;
            }
            return ans;
        }

        static public byte[] subtractTwoArrays(byte[] Z, byte[] M1_plus_M2)
        {

            byte borrow = 0;
            for (int i = 0; i < Z.Length; i++)
            {
                if (i >= M1_plus_M2.Length)
                {
                    Z[i] -= borrow;
                    borrow = 0;
                    continue;
                }
                int z = Z[i] - borrow;
                int M1MinusM2 = M1_plus_M2[i];
                if (z < M1MinusM2)
                {
                    Z[i] = (byte)((byte)(Z[i] + 10 - borrow) - M1_plus_M2[i]);
                    borrow = 1;
                }
                else
                {
                    Z[i] = (byte)((byte)(Z[i] - borrow) - M1_plus_M2[i]);
                    borrow = 0;
                }
            }
            return Z;
        }

        static byte[] addZerosToLeft(byte[] arr, int n)
        {
            int count = 0;
            byte[] result = new byte[arr.Length + n];
            for (int i = n; i < result.Length; i++)
            {
                result[i] = arr[count];
                count++;
            }
            return result;
        }

        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {

            // 123
            //REMOVE THIS LINE BEFORE START CODING
            //throw new NotImplementedException();
            int xSize = X.Length;
            int ySize = Y.Length;
            N = Math.Max(xSize, ySize);
            byte[] ans = new byte[] { 0 };
            if (xSize == 0 || ySize == 0) return ans;
            if (xSize == 1 && ySize == 1)
            {
                int res = X[0] * Y[0];
                if (res < 10)
                {
                    ans = new byte[1];
                    ans[0] = (byte)res;
                }
                else
                {
                    ans = new byte[2];
                    ans[0] = (byte)(res % 10);
                    ans[1] = (byte)(res / 10);
                }
                return ans;
            }

            // Split the input arrays in two halves
            int m = N / 2;

            byte[] A = new byte[(X.Length / 2) + (X.Length % 2)];
            byte[] B = new byte[m];
            byte[] C = new byte[(Y.Length / 2) + (Y.Length % 2)];
            byte[] D = new byte[m];
            // copy X
            for (int i = 0; i < X.Length; i++)
            {
                if (i < m)
                {
                    B[i] = X[i];
                }
                else
                {
                    A[i - m] = X[i];
                }
            }
            // copy Y
            for (int i = 0; i < Y.Length; i++)
            {
                if (i < m)
                {
                    D[i] = Y[i];
                }
                else
                {
                    C[i - m] = Y[i];
                }
            }

            byte[] AC = IntegerMultiply(A, C, N);
            byte[] BD = IntegerMultiply(B, D, N);

            byte[] AC_Plus_BD = addTwoArrays(AC, BD);
            byte[] A_plus_B = addTwoArrays(A, B);
            byte[] C_plus_D = addTwoArrays(C, D);
            AC = addZerosToLeft(AC, m * 2);


            byte[] Z = IntegerMultiply(A_plus_B, C_plus_D, m);
            subtractTwoArrays(Z, AC_Plus_BD);
            byte[] BC_plus_AD = addZerosToLeft(Z, m);

            ans = addTwoArrays(addTwoArrays(BD, BC_plus_AD), AC);
            return ans;
        }

        #endregion
    }
}
