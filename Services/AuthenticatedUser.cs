// AuthenticatedUser.cs
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
using System.Xml;

namespace Lastfm.Services
{
	public class AuthenticatedUser : User, IHasImage
	{
		private AuthenticatedUser(string username, Session session)
			:base(username, session)
		{
		}
		
		public static AuthenticatedUser GetUser(Session session)
		{
			XmlDocument doc = (new Request("user.getInfo", session, new RequestParameters())).execute();
			
			string name = doc.GetElementsByTagName("name")[0].InnerText;
			
			return new AuthenticatedUser(name, session);
		}
		
		public string GetImageURL()
		{
			XmlDocument doc = request("user.getInfo");
			
			return extract(doc, "image");
		}
		
		public string GetLanguageCode()
		{
			XmlDocument doc = request("user.getInfo");
			
			return extract(doc, "lang");
		}
		
		public string GetCountryName()
		{
			XmlDocument doc = request("user.getInfo");
			
			return extract(doc, "country");
		}
		
		public int GetAge()
		{
			XmlDocument doc = request("user.getInfo");
			
			return Int32.Parse(extract(doc, "age"));
		}
		
		public Gender GetGender()
		{
			XmlDocument doc = request("user.getInfo");
			
			string g = extract(doc, "gender");
			
			if (g=="m")
				return Gender.Male;
			else if (g=="f")
				return Gender.Female;
			else
				return Gender.Unspecified;
		}
		
		public bool IsSubscriber()
		{
			XmlDocument doc = request("user.getInfo");
			
			return (extract(doc, "subscriber") == "1");
		}
		
		public int GetPlaycount()
		{
			XmlDocument doc = request("user.getInfo");
			
			return Int32.Parse(extract(doc, "playcount"));
		}
	}
}
