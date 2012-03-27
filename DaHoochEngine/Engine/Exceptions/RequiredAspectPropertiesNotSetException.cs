using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectDaHooch
{
	class RequiredAspectPropertiesNotSetException : Exception
	{
		public readonly Aspect Aspect;
		public readonly string Properties;	//TODO: This should be a list/array. Itemize the properties.

		public RequiredAspectPropertiesNotSetException(Aspect aspect, string properties)
			: base("Required properties not set on Aspect#" + aspect.AspectID + "(named '" + aspect.Name + "')")
		{
			this.Aspect = aspect;
			this.Properties = properties;
		}

	}
}
