using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectDaHooch
{
	public static class SerializerInfo
	{
		private static List<Type> types = new List<Type>();

		public static Type[] GetTypes()
		{
			return types.ToArray();
		}

		public static void AddType(Type t)
		{
			if (!types.Contains(t))
				types.Add(t);
		}
	}
}
