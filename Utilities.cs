// Utilities.cs
//
//  Copyright (C) 2008 Amr Hassan
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Web;

namespace Lastfm
{
	public static class Utilities
	{
		public static Version AssemblyVersion
		{
			get{ return System.Reflection.Assembly.GetEntryAssembly().GetName().Version; }
		}
		
		internal static string UserAgent
		{
			get { return "lastfm-sharp/" + AssemblyVersion.ToString(); }
		}
		
		public static string md5(string text)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(text);
			
			MD5CryptoServiceProvider c = new MD5CryptoServiceProvider();
			buffer = c.ComputeHash(buffer);
			
			StringBuilder builder = new StringBuilder();
			foreach(byte b in buffer)
				builder.Append(b.ToString("x2").ToLower());
			
			return builder.ToString();
		}
		
		internal static string ParametersToString(RequestParameters collection)
		{
			string values = "";
			foreach(string key in collection.Keys)
				values += HttpUtility.UrlEncode(key) + "=" +
					HttpUtility.UrlEncode(collection[key]) + "&";
			values = values.Substring(0, values.Length - 1);
			
			return values;
		}
		
		internal static byte[] GetPostBytes(RequestParameters collection)
		{
			string values = Utilities.ParametersToString(collection);
			
			return Encoding.ASCII.GetBytes(values);
		}
	}
}
