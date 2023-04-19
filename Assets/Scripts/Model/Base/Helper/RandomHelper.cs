using System;
using System.Collections.Generic;

namespace ECSModel
{
	public static class RandomHelper
	{
		private static readonly Random random = new Random();

		public static UInt64 RandUInt64()
		{
			var bytes = new byte[8];
			random.NextBytes(bytes);
			return BitConverter.ToUInt64(bytes, 0);
		}

		public static Int64 RandInt64()
		{
			var bytes = new byte[8];
			random.NextBytes(bytes);
			return BitConverter.ToInt64(bytes, 0);
		}

		/// <summary>
		/// 获取lower与Upper之间的随机数
		/// </summary>
		/// <param name="lower"></param>
		/// <param name="upper"></param>
		/// <returns></returns>
		public static int RandomNumber(int lower, int upper)
		{
			int value = random.Next(lower, upper);
			return value;
		}

        // 数组这样拷贝有问题？
        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }
    }
}