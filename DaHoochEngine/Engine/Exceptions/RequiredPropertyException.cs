using System;

namespace ProjectDaHooch
{
	class RequiredPropertyException : Exception
	{
		public readonly string Property;

		public RequiredPropertyException(string property)
			: base("Required property, " + property + " not set")
		{
			this.Property = property;
		}
	}
}
