// Wiki.cs
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

namespace lastfm.Services
{
	public abstract class Wiki : Base
	{
		private string prefix {get; set;}
		
		internal Wiki(string prefix, string[] authData)
			:base(authData)
		{
			this.prefix = prefix;
		}
		
		public DateTime GetPublishedDate()
		{
			XmlDocument doc = request(prefix + ".getInfo");
			
			return DateTime.Parse(extract(doc, "published"));
		}
		
		public string GetSummary()
		{
			// TODO: Clean the string before return
			
			XmlDocument doc = request(prefix + ".getInfo");
      
			return extract(doc, "summary");
		}
		
		public string getContent()
		{
			// TODO: Clean the string first
      
			XmlDocument doc = request(prefix + ".getInfo");
      
			return extract(doc, "content");
		}
	}
}