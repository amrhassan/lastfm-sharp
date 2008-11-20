// Album.cs
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
	public class Album : Base, IEquatable<Album>
	{
		public string ArtistName {get; private set;}
		public string Title {get; private set;}
		public string Name {get { return Title; } }
		public Artist Artist {get { return new Artist(ArtistName, Session); } }
		
		public AlbumWiki Wiki {get { return new AlbumWiki(this, Session); } }
		
		public Album(string artistName, string title, Session session)
			:base(session)
		{
			ArtistName = artistName;
			Title = title;
		}
		
		public override string ToString ()
		{
			return ArtistName + " - " + Title;
		}
		
		protected override RequestParameters getParams ()
		{
			RequestParameters p = base.getParams ();
			p["artist"] = ArtistName;
			p["album"] = Title;
			
			return p;
		}
		
		public string GetMBID()
		{
			XmlDocument doc = request("album.getInfo");
			
			return extract(doc, "mbid");
		}
		
		public string GetID()
		{
			XmlDocument doc = request("album.getInfo");
			
			return extract(doc, "id");
		}
		
		public DateTime GetReleaseDate()
		{
			XmlDocument doc = request("album.getInfo");
			
			return DateTime.Parse(extract(doc, "releasedate"));
		}
		
		public string GetImageUrl(AlbumImageSize size)
		{
			XmlDocument doc = request("album.getInfo");
			
			return extractAll(doc, "image", 4)[(int)size];
		}
		
		public string GetImageUrl()
		{
			return GetImageUrl(AlbumImageSize.ExtraLarge);
		}
		
		public int GetListenerCount()
		{
			XmlDocument doc = request("album.getInfo");
			
			return Int32.Parse(extract(doc, "listeners"));
		}
		
		public int GetPlaycount()
		{
			XmlDocument doc = request("album.getInfo");
			
			return Int32.Parse(extract(doc, "playcount"));
		}
		
		public Track[] GetTracks()
		{
			string url = "lastfm://playlist/album/" + this.GetID();
			
			return (new XSPF(url, Session)).GetTracks();
		}
		
		public Tag[] GetTopTags()
		{
			XmlDocument doc = request("album.getInfo");
			
			List<Tag> list = new List<Tag>();
			foreach(string name in extractAll(doc.GetElementsByTagName("toptags")[0], "name"))
				list.Add(new Tag(name, Session));
			
			return list.ToArray();
		}
		
		public Tag[] GetTopTags(int limit)
		{
			List<Tag> list = new List<Tag>();
			
			int i = 0;
			foreach(Tag tag in GetTopTags())
				if (i<limit)
					list.Add(tag);
			
			return list.ToArray();
		}
		
		public bool Equals(Album album)
		{
			if(album.Title == this.Title && album.ArtistName == this.ArtistName)
				return true;
			else
				return false;
		}
		
		public static AlbumSearch Search(string albumName, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["album"] = albumName;
			
			return new AlbumSearch(terms, session, itemsPerPage);
		}
		
		public static AlbumSearch Search(string albumName, Session session)
		{
			// 30 is the default (and maximum) number of results per page.
			return Album.Search(albumName, session, 30);
		}
		
		public void AddTags(params Tag[] tags)
		{
			//This method requires authentication
			requireAuthentication();
			
			foreach(Tag tag in tags)
			{
				RequestParameters p = getParams();
				p["tags"] = tag.Name;
				
				request("album.addTags", p);
			}
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
			
			XmlDocument doc = request("album.getTags");
			
			TagCollection collection = new TagCollection(Session);
			
			foreach(string name in this.extractAll(doc, "name"))
				collection.Add(name);
			
			return collection.ToArray();
		}
		
		public void RemoveTags(params Tag[] tags)
		{
			//This method requires authentication
			requireAuthentication();
			
			foreach(Tag tag in tags)
			{
				RequestParameters p = getParams();
				p["tag"] = tag.Name;
				
				request("album.removeTag", p);
			}
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
