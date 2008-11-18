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
	[Serializable]
	public class Session
	{
		public string APIKey {get; private set;}
		public string APISecret {get; private set;}
		public string SessionKey {get; private set;}
		
		public bool Authenticated
		{
			get { return !(SessionKey == null); }
		}
		
		private string token {get; set;}
		
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
		
		public void Authenticate(string username, string md5Password)
		{
			RequestParameters p = new Lastfm.RequestParameters();
			
			p["username"] = username;
			p["authToken"] = Utilities.md5(username + md5Password);
			
			XmlDocument doc = (new Request("auth.getMobileSession", this, p)).execute();
			
			SessionKey = doc.GetElementsByTagName("key")[0].InnerText;
		}
		
		private string getAuthenticationToken()
		{
			XmlDocument doc = (new Request("auth.getToken", this, new RequestParameters())).execute();
			
			return doc.GetElementsByTagName("token")[0].InnerText;
		}
		
		public string GetWebAuthenticationUrl()
		{
			token = getAuthenticationToken();
			
			return "http://www.last.fm/api/auth/?api_key=" + APIKey + "&token=" + token;
		}
		
		public void AuthenticateViaWeb()
		{
			RequestParameters p = new Lastfm.RequestParameters();
			p["token"] = token;
			
			Request r = new Request("auth.getSession", this, p);
			r.signIt();
			
			XmlDocument doc = r.execute();
			
			SessionKey = doc.GetElementsByTagName("key")[0].InnerText;
		}
	}
}
