// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.IO;
#if MONO_UNIX
using Mono.Unix;
#else
using System.Runtime.InteropServices;
#endif

namespace NDesk.DBus.Transports
{
	abstract class UnixTransport : Transport
	{
		public override void Open (AddressEntry entry)
		{
			string path;
			bool abstr;

			if (entry.Properties.TryGetValue ("path", out path))
				abstr = false;
			else if (entry.Properties.TryGetValue ("abstract", out path))
				abstr = true;
			else
				throw new Exception ("No path specified for UNIX transport");

			Open (path, abstr);
		}

#if !MONO_UNIX
		[DllImport ("libc", SetLastError=true)]
		static extern uint getuid ();
#endif

		public override string AuthString ()
		{
#if MONO_UNIX
			long uid = UnixUserInfo.GetRealUserId ();
#else
			long uid = getuid ();
#endif

			return uid.ToString ();
		}

		public abstract void Open (string path, bool @abstract);
	}
}
