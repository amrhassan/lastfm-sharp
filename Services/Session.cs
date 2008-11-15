// Session.cs
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
using System.Xml;


namespace Lastfm.Services
{
	// TODO: make serializable
	
	[Serializable]
	public class Session
	{
		public string APIKey {get; set;}
		public string APISecret {get; set;}
		public string SessionKey {get; set;}
		
		public bool Authenticated
		{
			get { return !(SessionKey == null); }
		}
		
		public Session(string apiKey, string apiSecret, string sessionKey)
		{
			APIKey = apiKey;
			APISecret = apiSecret;
			SessionKey = sessionKey;
		}
		
		public Session(string apiKey, string apiSecret)
		{
			APIKey = apiKey;
			APISecret = apiSecret;
		}
		
		public Session() {}
		
		public void Authenticate(string username, string md5Password)
		{
			RequestParameters p = new Lastfm.RequestParameters();
			
			p["username"] = username;
			p["authToken"] = Utilities.md5(username + md5Password);
			
			XmlDocument doc = (new Request("auth.getMobileSession", this, p)).execute();
			
			SessionKey = doc.GetElementsByTagName("key")[0].InnerText;
		}
	}
}
