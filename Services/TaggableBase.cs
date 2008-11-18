// TaggableBase.cs
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
	public abstract class TaggableBase : Base
	{
		private string prefix {get; set;}
		
		public TaggableBase(string prefix, Session session)
			:base(session)
		{
			this.prefix = prefix;
		}
		
		public void AddTags(params Tag[] tags)
		{
			//This method requires authentication
			requireAuthentication();
			
			if (tags.Length > 1)
			{
				foreach(Tag tag in tags)
					this.AddTags(tag);
				return;
			}
			
			RequestParameters p = getParams();
			p["tags"] = tags[0].Name;
			
			request(prefix + ".addTags", p);
		}
		
		public void AddTags(params string[] tags)
		{
			foreach(string tag in tags)
				AddTags(new Tag(tag, Session));
		}
		
		public void AddTags(TagCollection tags)
		{
			foreach(Tag tag in tags)
				AddTags(tag);
		}
		
		public Tag[] GetTags()
		{
			//This method requires authentication
			requireAuthentication();
			
			XmlDocument doc = request(prefix + ".getTags");
			
			TagCollection collection = new TagCollection(Session);
			
			foreach(string name in this.extractAll(doc, "name"))
				collection.Add(name);
			
			return collection.ToArray();
		}
		
		public Tag[] GetTopTags()
		{
			XmlDocument doc = request(prefix + ".getTopTags");
			
			string[] names = extractAll(doc, "name");
			
			TagCollection collection = new TagCollection(Session);
			foreach(string name in names)
				collection.Add(name);
			
			return collection.ToArray();
		}
		
		public Tag[] GetTopTags(int limit)
		{
			Tag[] array = GetTopTags();
			TagCollection collection = new TagCollection(Session);
			
			for(int i=0; i<limit; i++)
				collection.Add(array[i]);
			
			return collection.ToArray();
		}
		
		public void RemoveTags(params Tag[] tags)
		{
			//This method requires authentication
			requireAuthentication();
			
			if (tags.Length > 1)
			{
				foreach(Tag t in tags)
					RemoveTags(t);
				return;
			}
			
			RequestParameters p = getParams();
			p["tag"] = tags[0].Name;
			
			request(prefix + ".removeTag", p);
		}
		
		public void RemoveTags(params string[] tags)
		{
			//This method requires authentication
			requireAuthentication();
			
			foreach(string tag in tags)
				RemoveTags(new Tag(tag, Session));
		}
		
		public void RemoveTags(TagCollection tags)
		{
			foreach(Tag tag in tags)
				RemoveTags(tag);
		}
		
		public void SetTags(string[] tags)
		{
			List<Tag> list = new List<Tag>();
			foreach(string name in tags)
				list.Add(new Tag(name, Session));
			
			SetTags(list.ToArray());
		}
		
		public void SetTags(Tag[] tags)
		{
			List<Tag> newSet = new List<Tag>(tags);
			List<Tag> current = new List<Tag>(GetTags());
			List<Tag> toAdd = new List<Tag>();
			List<Tag> toRemove = new List<Tag>();
			
			foreach(Tag tag in newSet)
				if(!current.Contains(tag))
					toAdd.Add(tag);
			
			foreach(Tag tag in current)
				if(!newSet.Contains(tag))
					toRemove.Add(tag);
			
			if (toAdd.Count > 0)
				AddTags(toAdd.ToArray());
			if (toRemove.Count > 0)
				RemoveTags(toRemove.ToArray());
		}
		
		public void SetTags(TagCollection tags)
		{
			SetTags(tags.ToArray());
		}
		
		public void ClearTags()
		{
			foreach(Tag tag in GetTags())
				RemoveTags(tag);
		}
	}
}
