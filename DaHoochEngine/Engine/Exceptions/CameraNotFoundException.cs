using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectDaHooch
{
	class CameraNotFoundException : Exception
	{
		public string CameraName { get { return this.CameraName; } }
		private string cameraName;

		public CameraNotFoundException(string cameraName)
			: base("Looking for camera named " + cameraName + ". None found.")
		{
			this.cameraName = cameraName;
		}
	}
}
