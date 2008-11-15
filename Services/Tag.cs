// Tag.cs
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
	public class Tag : Base
	{
		public string Name {get; private set;}    
		
		public Tag(string name, Session session)
			:base(session)
		{
			Name = name;
		}
		
		protected override RequestParameters getParams ()
		{
			RequestParameters p = base.getParams ();
			p["tag"] = this.Name;
			
			return p;
		}
    
		public override string ToString ()
		{
			return this.Name;
		}
		
		public Tag[] GetSimilar()
		{
			XmlDocument doc = request("tag.getSimilar");
			
			List<Tag> list = new List<Tag>();
			foreach(string name in extractAll(doc, "name"))
				list.Add(new Tag(name, Session));
			
			return list.ToArray();
		}
		
		public Dictionary<Album, int> GetTopAlbumsWithCount()
		{
			XmlDocument doc = request("tag.getTopAlbums");
			
			Dictionary<Album, int> dic = new Dictionary<Album,int>();
			foreach(XmlNode n in doc.GetElementsByTagName("album"))
			{
				Album album = new Album(extract(n, "name", 1), extract(n, "name"), Session);
				int count = Int32.Parse(extract(n, "tagcount"));
				
				dic[album] = count;
			}
			
			return dic;
		}
		
		public Album[] GetTopAlbums()
		{
			XmlDocument doc = request("tag.getTopAlbums");
			
			List<Album> list = new List<Album>();
			foreach(XmlNode n in doc.GetElementsByTagName("album"))
				list.Add(new Album(extract(n, "name", 1), extract(n, "name"), Session));
			
			return list.ToArray();
		}
		
		public Dictionary<Artist, int> GetTopArtistsWithCount()
		{
			XmlDocument doc = request("tag.getTopArtists");
			
			Dictionary<Artist, int> dic = new Dictionary<Artist,int>();			
			foreach(XmlNode n in doc.GetElementsByTagName("artist"))
			{
				Artist artist = new Artist(extract(n, "name"), Session);
				int count = Int32.Parse(extract(n, "tagcount"));
				
				dic[artist] = count;
			}
			
			return dic;
		}
		
		public Artist[] GetTopArtists()
		{
			XmlDocument doc = request("tag.getTopArtists");
			
			List<Artist> list = new List<Artist>();
			foreach(string name in extractAll(doc, "name"))
				list.Add(new Artist(name, Session));
			
			return list.ToArray();
		}
		
		public Dictionary<Track, int> GetTopTracksWithCount()
		{
			XmlDocument doc = request("tag.getTopTracks");
			
			Dictionary<Track, int> dic = new Dictionary<Track,int>();			
			foreach(XmlNode n in doc.GetElementsByTagName("track"))
			{
				Track track = new Track(extract(n, "name", 1), extract(n, "name"), Session);
				int count = Int32.Parse(extract(n, "tagcount"));
				
				dic[track] = count;
			}
			
			return dic;
		}
		
		public Track[] GetTopTracks()
		{
			XmlDocument doc = request("tag.getTopTracks");
			
			List<Track> list = new List<Track>();
			foreach(XmlNode n in doc.GetElementsByTagName("track"))
				list.Add(new Track(extract(n, "name", 1), extract(n, "name"), Session));
			
			return list.ToArray();
		}
	}
}
