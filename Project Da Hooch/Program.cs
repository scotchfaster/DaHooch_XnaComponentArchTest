using System;
using System.Diagnostics;

//Compiler warnings
//	Implement GetHashCode:	0661
//	Implement Equals:		0660
//	Event never used:		0067
//	Field never used:		0169
//	Variable never used:	0219
/*
0661, 0660, 0067, 0169, 0219
*/

namespace ProjectDaHooch
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			using (Game1Proto6 game = new Game1Proto6())
			{
				game.Run();
			}
		}
	}
}

