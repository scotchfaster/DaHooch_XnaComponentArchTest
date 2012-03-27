using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectDaHooch
{
	public delegate void RobotFunctionCompleted(float endValue, float duration);
	//public delegate float getter(

	public class RobotFunction
	{
		float value;

		public RobotFunction(ref float valueReference, float endValue, float duration, RobotFunctionCompleted callBack)
		{

		}
	}
}
