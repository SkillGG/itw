using System;

namespace ITW {
	public class Debug {
		private Debug() { }
		public enum Importance {
			INIT_INFO = 0,
			DRAW_INFO = 1,
			VALUE_INFO = 2,
			NOTIFICATION = 3,
			IMPORTANT_INFO = 4,
			WARN = 5,
			hid = 6,
			ERROR = 7
		}
		private static Importance Minimp = Importance.INIT_INFO;
		public static Importance MinImp { get => Minimp; set => Minimp = value; }

		public Debug(string invoker, object msg, Importance importance = (Importance) 0) {
			uint imp = (uint) importance;
			if( imp >= (uint) MinImp )
				Console.WriteLine($"[{importance.ToString( )}]( " + invoker + " ) " + msg.ToString( ));
		}
		public Debug(string invoker, string msg, Importance importance = (Importance) 0) {
			uint imp = (uint) importance;
			if( imp >= (uint) MinImp )
				Console.WriteLine($"[{importance.ToString( )}]( " + invoker + " ) " + msg);
		}
	}
}
