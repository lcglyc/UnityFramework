using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kunpo
{
    /// <summary>
    /// 目前版本不支持位数小于0的情况
    /// </summary>
    [System.Serializable]
    public class BigNumber
    {
        const int Precision = 9;

        static BigNumber _Zero = new BigNumber();

        public static BigNumber Zero
        {
            get { return _Zero; }
        }

        public float _f;

        public float f { get { return _f; } }

        public int _digit;

        public int digit { get { return _digit; } }

        public BigNumber(float f = 0f, int digit = 0)
        {
            _f = f;
            _digit = digit;
            Check();
        }

        public static BigNumber operator +(BigNumber bn1, BigNumber bn2)
        {
            int diff = bn1._digit - bn2._digit;
            if (diff > Precision)
            {
                return bn1;
            }
            else if (-diff > Precision)
            {
                return bn2;
            }
            else
            {
                //Debug.Log("Add: " + diff + " " + bn2._f * Mathf.Pow(10f, -diff));
                return new BigNumber(bn1._f + bn2._f * Mathf.Pow(10f, -diff), bn1._digit)
                    .Check();
            }
        }

        public static BigNumber operator +(BigNumber bn, float f)
        {
            return bn + new BigNumber(f, 0);
        }

        public static BigNumber operator -(BigNumber bn1, BigNumber bn2)
        {
            int diff = bn1._digit - bn2._digit;
            if (diff > Precision)
            {
                return bn1;
            }
            else if (-diff > Precision)
            {
                return new BigNumber(-bn2._f, bn2._digit).Check();
            }
            else
            {
                //Debug.Log("Minus: " + diff + " " + bn2._f * Mathf.Pow(10f, -diff));
                return new BigNumber(bn1._f - bn2._f * Mathf.Pow(10f, -diff), bn1._digit)
                    .Check();
            }
        }

        public static BigNumber operator -(BigNumber bn, float f)
        {
            return bn - new BigNumber(f, 0);
        }

        public static BigNumber operator *(BigNumber bn1, BigNumber bn2)
        {
            return new BigNumber(bn1._f * bn2._f, bn1._digit + bn2._digit).Check();
        }

        public static BigNumber operator *(BigNumber bn, float f)
        {
            return new BigNumber(bn._f * f, bn._digit).Check();
        }

        public static BigNumber operator /(BigNumber bn1, BigNumber bn2)
        {
            if (bn2._f == 0f)
            {
                Debug.LogError("bn2 is zero");
                return Zero;
            }
            return new BigNumber(bn1._f / bn2._f, bn1._digit - bn2._digit).Check();
        }

        public static BigNumber operator /(BigNumber bn, float f)
        {
            if (f == 0f)
            {
                Debug.LogError("f is zero");
                return Zero;
            }
            return new BigNumber(bn._f / f, bn._digit).Check();
        }

        public static bool operator ==(BigNumber bn1, BigNumber bn2)
        {
            return bn1._f == bn2._f && bn1._digit == bn2._digit;
        }

        public static bool operator !=(BigNumber bn1, BigNumber bn2)
        {
            return bn1._f != bn2._f || bn1._digit != bn2._digit;
        }

        public static bool operator >(BigNumber bn1, BigNumber bn2)
        {
            bn1.Check();
            bn2.Check();
            if (bn1.f > 0)
            {
                if (bn2.f <= 0)
                {
                    return true;
                }
                else
                {
                    if (bn1.digit > bn2.digit)
                    {
                        return true;
                    }
                    else if (bn1.digit == bn2.digit)
                    {
                        return bn1.f > bn2.f;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (bn1.f == 0)
            {
                if (bn2.f < 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (bn2.f >= 0)
                {
                    return false;
                }
                else
                {
                    if (bn1.digit < bn2.digit)
                    {
                        return true;
                    }
                    else if (bn1.digit == bn2.digit)
                    {
                        return bn1.f > bn2.f;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool operator >(BigNumber bn, float f)
        {
            return bn > new BigNumber(f, 0);
        }

        public static bool operator <(BigNumber bn1, BigNumber bn2)
        {
            return bn2 > bn1;
        }

        public static bool operator <(BigNumber bn, float f)
        {
            return bn < new BigNumber(f, 0);
        }

        public static bool operator >=(BigNumber bn1, BigNumber bn2)
        {
            return !(bn1 < bn2);
        }

        public static bool operator >=(BigNumber bn, float f)
        {
            return bn >= new BigNumber(f, 0);
        }

        public static bool operator <=(BigNumber bn1, BigNumber bn2)
        {
            return !(bn1 > bn2);
        }

        public static bool operator <=(BigNumber bn, float f)
        {
            return bn <= new BigNumber(f, 0);
        }

        public static explicit operator float(BigNumber bn)
        {
            try
            {
                return bn.f * Mathf.Pow(10, bn.digit);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return 0;
            }
        }

        public BigNumber Check()
        {
            if (_f == 0 || float.IsInfinity(_f) || float.IsNaN(_f))
            {
                _f = 0;
                _digit = 0;
            }
            else
            {
                int digit = Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(_f)));
                if (digit != 0)
                {
                    _f *= Mathf.Pow(0.1f, digit);
                    this._digit += digit;
                }
                //Debug.Log("Check: " + _f + " " + _digit + " " + digit + " " + Mathf.Pow(0.1f, digit));
                if (digit < 0)
                {
                    //Debug.LogWarning("The BigNumber is Not Big");
                }
            }
            return this;
        }

        public static BigNumber GetRandom(BigNumber bn1, BigNumber bn2)
        {
            if (bn1 <= bn2)
            {
                return new BigNumber(Random.Range(bn1.f, bn2.f * Mathf.Pow(10, bn2.digit - bn1.digit)),
                    bn1.digit).Check();
            }
            else
            {
                return new BigNumber(Random.Range(bn2.f, bn1.f * Mathf.Pow(10, bn1.digit - bn2.digit)),
                    bn2.digit).Check();
            }
        }

        public BigNumber Negate()
        {
            _f = -_f;
            return this;
        }

        public BigNumber SetToZero()
        {
            _f = 0f;
            _digit = 0;
            return this;
        }

        public override string ToString()
        {
            return this.f + " * 10^" + this.digit;
        }
    }

    public static class BigNumberHelper
    {

        static string[] _PrefixArray = new string[] { string.Empty, "K", "M", "G",
            "T", "P", "E", "Z", "Y", "A", "B", "C", "D", "F", "H", "I", "J", "L",
            "N", "O", "Q", "R", "S", "U", "V", "W", "X", "a", "b", "c", "d", "f",
            "h", "i", "j", "l", "n", "o", "q", "r", "s", "u", "v" ,"w", "x", "AA",
            "AB"};

        public static void Test()
        {
            var bnList = new List<BigNumber>();
            bnList.Add(new BigNumber(8.88888f, 2));
            bnList.Add(new BigNumber(8.8888f, 6));
            bnList.Add(new BigNumber(88.8888f, 9));
            bnList.Add(new BigNumber(1f, 19));
            var fList = new List<float>();
            fList.Add(0.3f);
            fList.Add(333f);
            fList.Add(3333333333333333333f);

            foreach (var bn in bnList)
            {
                Debug.Log("bn: " + bn.ToString());
                Debug.Log(ToStringD2(bn));
                Debug.Log(ToStringD3(bn));
                Debug.Log(string.Empty);
                foreach (var bn2 in bnList)
                {
                    Debug.Log("bn2: " + bn2.ToString());
                    Debug.Log((bn + bn2).ToString());
                    Debug.Log((bn - bn2).ToString());
                    Debug.Log((bn * bn2).ToString());
                    Debug.Log((bn / bn2).ToString());
                    Debug.Log(string.Empty);
                }
                Debug.Log(string.Empty);
                foreach (var f in fList)
                {
                    Debug.Log("f: " + f);
                    Debug.Log((bn + f).ToString());
                    Debug.Log((bn - f).ToString());
                    Debug.Log((bn * f).ToString());
                    Debug.Log((bn / f).ToString());
                    Debug.Log(string.Empty);
                }
            }
        }

        public static string ToStringD2(this BigNumber bn)
        {
            bn.Check();
            if (bn.digit < 2)
            {
                return ((int)(bn.f * Mathf.Pow(10f, bn.digit))).ToString();
            }
            int level = bn.digit / 3;
            int rem = bn.digit % 3;
            if (level >= 0)
            {
                if (level < _PrefixArray.Length - 1)
                {
                    switch (rem)
                    {
                        case 0:
                            return (int)(bn.f * 10) / 10f + _PrefixArray[level];
                        case 1:
                            return (int)(bn.f * 10) + _PrefixArray[level];
                        case 2:
                            return (int)bn.f / 10f + _PrefixArray[level + 1];
                    }
                }
                else
                {
                    Debug.LogWarning("The BigNumber is Too Big");
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public static string ToStringD3(this BigNumber bn)
        {
            bn.Check();
            if (bn.digit < 3)
            {
                return ((int)(bn.f * Mathf.Pow(10f, bn.digit))).ToString();
            }
            int level = bn.digit / 3;
            int rem = bn.digit % 3;
            if (level >= 0)
            {
                if (level < _PrefixArray.Length - 1)
                {
                    switch (rem)
                    {
                        case 0:
                            return (int)(bn.f * 100) / 100f + _PrefixArray[level];
                        case 1:
                            return (int)(bn.f * 100) / 10f + _PrefixArray[level];
                        case 2:
                            return (int)(bn.f * 100) + _PrefixArray[level];
                    }
                }
                else
                {
                    Debug.LogWarning("The BigNumber is Too Big");
                    return string.Empty;
                }
            }
            return string.Empty;
        }
    }
}
