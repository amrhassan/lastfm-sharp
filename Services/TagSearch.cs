// TagSearch.cs
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

namespace Lastfm.Services
{
	public class TagSearch : Search
	{	
		internal TagSearch(Dictionary<string, string> searchTerms, Session session, int itemsPerPage)
			:base("tag", searchTerms, session, itemsPerPage)
		{}
		
		public Tag[] GetPage(int page)
		{
			RequestParameters p = getParams();
			p["page"] = page.ToString();
			
			lastDoc = request("tag.search", p);
			
			List<Tag> list = new List<Tag>();			
			foreach(XmlNode n in lastDoc.GetElementsByTagName("tag"))
				list.Add(new Tag(extract(n, "name"), Session));
			
			return list.ToArray();
		}
	}
}
