using System;

namespace ProjectDaHooch
{
	public class OrphanedAspectException : Exception
	{
		public readonly Aspect Aspect;

		public OrphanedAspectException(Aspect aspect)
			: base("Aspect#" + aspect.AspectID + "(named '" + aspect.Name + "' has no OwnerID set! Every Aspect need to be owned by an Entity before it can be initialized.")
		{
			this.Aspect = aspect;
		}
	}
}
