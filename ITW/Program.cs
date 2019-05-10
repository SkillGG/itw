using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ITW {
#if WINDOWS || LINUX
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			using( var game = new ITW( ) )
				game.Run( );
		}
	}
#endif
}
