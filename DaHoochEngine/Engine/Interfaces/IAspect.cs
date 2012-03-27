using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectDaHooch
{
	public interface IAspect
	{
		string Name { get; set; }
		int OwnerID { get; set; }
		int AspectID { get; }
		Entity Owner { get; set; }
		AspectContainer Aspects { get; }
		void Initialize();
		bool Initialized { get; }
		void LoadContent();
		bool Loaded { get; }
		IAspect Clone(int NewOwnerID);
	}
}
