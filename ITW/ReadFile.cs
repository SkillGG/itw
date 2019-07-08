using System.IO;

namespace ITW {
	public static class ReadFile {

		public static void CreateFile(string path) {
			File.Create(path).Dispose( );
		}

		public static bool Readable(string path) {
			if( File.Exists(path) )
				return true;
			return false;
		}

		public static string Read(string path) {
			string p = path;
			string s = "";
			if( File.Exists(p) ) {
				StreamReader sr = new StreamReader(p);
				s = sr.ReadToEnd( );
				sr.Dispose( );
				sr = null;
			} else
				new Debug("ReadFile#Read(string)", $"File {p} does not exists!", Debug.Importance.ERROR);
			return s;
		}

	}
}
