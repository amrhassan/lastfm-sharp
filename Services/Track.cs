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

namespace lastfm.Services
{
	public class Track : TaggableBase
	{
		public string Title {get; private set;}
		public string ArtistName {get; private set;}
		public Artist Artist
		{ get { return new Artist(this.ArtistName, Session); } }
		
		public Wiki Wiki
		{
			get
			{ return new TrackWiki(ArtistName, Title, Session); }
		}
    
		public Track(string artistName, string title, Session session)
			:base("track", session)
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
			request("track.love");
		}
	}
}