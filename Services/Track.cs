// Track.cs
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
	public class Track : Base, IEquatable<Track>, IShareable, ITaggable
	{
		public string Title {get; private set;}
		public string ArtistName {get; private set;}
		public Artist Artist
		{ get { return new Artist(this.ArtistName, Session); } }
		
		public Wiki Wiki
		{ get { return new TrackWiki(this, Session); } }
    
		public Track(string artistName, string title, Session session)
			:base(session)
		{
			Title = title;
			ArtistName = artistName;
		}
		
		public override string ToString ()
		{
			return this.Artist + " - " + this.Title;
		}
    
		protected override RequestParameters getParams ()
		{
			RequestParameters p = base.getParams ();
			p["artist"] = ArtistName;
			p["track"] = Title;
			
			return p;
		}
		
		public int GetID()
		{
			XmlDocument doc = request("track.getInfo");
			
			return Int32.Parse(extract(doc, "id"));
		}
		
		public TimeSpan GetDuration()
		{
			XmlDocument doc = request("track.getInfo");
			
			// Duration is returned in milliseconds.
			return new TimeSpan(0, 0, 0, 0, Int32.Parse(extract(doc, "duration")));
		}
		
		public bool IsStreamable()
		{
			XmlDocument doc = request("track.getInfo");
			
			int value = Int32.Parse(extract(doc, "streamable"));
			
			if (value == 1)
				return true;
			else
				return false;
		}
		
		public Album GetAlbum()
		{
			XmlDocument doc = request("track.getInfo");
			
			if (doc.GetElementsByTagName("album").Count > 0)
			{
				XmlNode n = doc.GetElementsByTagName("album")[0];
				
				string artist = extract(n, "artist");
				string title = extract(n, "title");
				
				return new Album(artist, title, Session);
			}else{
				return null;
			}
		}
		
		public void Ban()
		{
			//This method requires authentication
			requireAuthentication();
			
			request("track.ban");
		}
		
		public Track[] GetSimilar()
		{
			XmlDocument doc = request("track.getSimilar");
			
			List<Track> list = new List<Track>();
			
			foreach(XmlNode n in doc.GetElementsByTagName("track"))
			{
				list.Add(new Track(extract(n, "name", 1), extract(n, "name"), Session));
			}
			
			return list.ToArray();
		}
		
		public void Love()
		{
			//This method requires authentication
			requireAuthentication();
			
			request("track.love");
		}
		
		public bool Equals(Track track)
		{
			return(track.Title == this.Title && track.ArtistName == this.ArtistName);
		}
		
		public void Share(Recipients recipients, string message)
		{
			if (recipients.Count > 1)
			{
				foreach(string recipient in recipients)
				{
					Recipients r = new Recipients();
					r.Add(recipient);
					Share(r, message);
				}
				
				return;
			}
			
			requireAuthentication();
			
			RequestParameters p = getParams();
			p["recipient"] = recipients[0];
			p["message"] = message;
			
			request("track.Share", p);
		}
		
		public void Share(Recipients recipients)
		{
			if (recipients.Count > 1)
			{
				foreach(string recipient in recipients)
				{
					Recipients r = new Recipients();
					r.Add(recipient);
					Share(r);
				}
				
				return;
			}
			
			requireAuthentication();
			
			RequestParameters p = getParams();
			p["recipient"] = recipients[0];
			
			request("track.Share", p);
		}
		
		public static TrackSearch Search(string artist, string title, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["track"] = title;
			terms["artist"] = artist;
			
			return new TrackSearch(terms, session, itemsPerPage);
		}
		
		public static TrackSearch Search(string artist, string title, Session session)
		{
			return Track.Search(artist, title, session, 30);
		}
		
		public static TrackSearch Search(string title, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["track"] = title;
			
			return new TrackSearch(terms, session, itemsPerPage);
		}
		
		public static TrackSearch Search(string title, Session session)
		{
			return Track.Search(title, session, 30);
		}
		
		public void AddTags(params Tag[] tags)
		{
			//This method requires authentication
			requireAuthentication();
			
			foreach(Tag tag in tags)
			{
				RequestParameters p = getParams();
				p["tags"] = tag.Name;
				
				request("track.addTags", p);
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
			
			XmlDocument doc = request("track.getTags");
			
			TagCollection collection = new TagCollection(Session);
			
			foreach(string name in this.extractAll(doc, "name"))
				collection.Add(name);
			
			return collection.ToArray();
		}
		
		public TopTag[] GetTopTags()
		{
			XmlDocument doc = request("track.getTopTags");
			
			List<TopTag> list = new List<TopTag>();
			foreach(XmlNode n in doc.GetElementsByTagName("tag"))
				list.Add(new TopTag(new Tag(extract(n, "name"), Session), Int32.Parse(extract(n, "count"))));
			
			return list.ToArray();
		}
		
		public TopTag[] GetTopTags(int limit)
		{
			return this.sublist<TopTag>(GetTopTags(), limit);
		}
		
		public void RemoveTags(params Tag[] tags)
		{
			//This method requires authentication
			requireAuthentication();
			
			foreach(Tag tag in tags)
			{
				RequestParameters p = getParams();
				p["tag"] = tag.Name;
				
				request("track.removeTag", p);
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
		
		public TopFan[] GetTopFans()
		{
			XmlDocument doc = request("track.getTopFans");
			
			List<TopFan> list = new List<TopFan>();
			foreach(XmlNode node in doc.GetElementsByTagName("user"))
				list.Add(new TopFan(new User(extract(node, "name"), Session), Int32.Parse(extract(node, "weight"))));
			
			return list.ToArray();
		}
		
		public TopFan[] GetTopFans(int limit)
		{
			return sublist<TopFan>(GetTopFans(), limit);
		}
	}
}