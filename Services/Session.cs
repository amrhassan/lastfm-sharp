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
using System.Collections.Generic;

namespace lastfm.Services
{	
	public class Session : Base
	{
		private string token {get; set;}
		
		private Dictionary<string, string> data;
		
		public static Session Create(string apiKey, string secret)
		{
			return new Session(apiKey, secret);
		}
		
		private Session(string apiKey, string secret)
			:base(apiKey, secret, "")
		{
			downloadToken();
		}
		
		private void downloadToken()
		{
			XmlDocument doc = request("auth.getToken");
			
			this.token = extract(doc, "token");
		}
		
		public string GetAuthenticationURL()
		{
			string url = @"http://www.last.fm/api/auth/?api_key=" + APIKey + "&token=" + token;
			
			return url;
		}
		
		private void downloadData()
		{
			RequestParameters p = getParams();
			p["token"] = token;
			XmlDocument doc = request("auth.getSession", p);
			
			this.data = new Dictionary<string, string>();
			
			this.data["key"] = extract(doc, "key");
			if (extract(doc, "subscriber") == "1")
				this.data["subscriber"] = "true";
			else
				this.data["subscriber"] = "false";
			this.data["name"] = extract(doc, "name");
		}
		
		public string GetSessionKey()
		{
			if (this.data == null)
				downloadData();
			
			return this.data["key"];
		}
		
		public string GetAuthenticatedUsername()
		{
			if (this.data == null)
				downloadData();
			
			return this.data["name"];
		}
		
		public bool IsSubscriber()
		{
			if (this.data == null)
				downloadData();
			
			return Boolean.Parse(this.data["subscriber"]);
		}
	}
}
