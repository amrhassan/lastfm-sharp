// Search.cs
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
	public abstract class Search : Base
	{
		private string prefix {get; set;}
		private Dictionary<string, string> searchTerms {get; set;}
		protected XmlDocument lastDoc {get; set;}
		
		public int ItemsPerPage {get; private set;}
		public int ResultCount
		{
			get { return Int32.Parse(extract(lastDoc, "opensearch:totalResults")); }
		}
		
		protected Search(string prefix, Dictionary<string, string> searchTerms,
		                 Session session, int itemsPerPage)
			:base(session)
		{
			this.prefix = prefix;
			this.searchTerms = searchTerms;
			this.ItemsPerPage = itemsPerPage;
			
			lastDoc = request(prefix + ".search");
		}
		
		protected override RequestParameters getParams ()
		{
			RequestParameters p = base.getParams ();
			p["limit"] = ItemsPerPage.ToString();
			foreach(string key in searchTerms.Keys)
				p[key] = searchTerms[key];
			
			return p;
		}
		
		public static AlbumSearch ForAlbums(string albumName, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["album"] = albumName;
			
			return new AlbumSearch(terms, session, itemsPerPage);
		}
		
		public static AlbumSearch ForAlbums(string albumName, Session session)
		{
			// 30 is the default (and maximum) number of results per page.
			return Search.ForAlbums(albumName, session, 30);
		}
		
		public static ArtistSearch ForArtists(string artistName, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["artist"] = artistName;
			
			return new ArtistSearch(terms, session, itemsPerPage);
		}
		
		public static ArtistSearch ForArtists(string artistName, Session session)
		{
			return Search.ForArtists(artistName, session, 30);
		}
		
		public static TagSearch ForTags(string tagName, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["tag"] = tagName;
			
			return new TagSearch(terms, session, itemsPerPage);
		}
		
		public static TagSearch ForTags(string tagName, Session session)
		{
			return Search.ForTags(tagName, session, 30);
		}
		
		public static TrackSearch ForTracks(string artist, string title, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["track"] = title;
			terms["artist"] = artist;
			
			return new TrackSearch(terms, session, itemsPerPage);
		}
		
		public static TrackSearch ForTracks(string artist, string title, Session session)
		{
			return Search.ForTracks(artist, title, session, 30);
		}
		
		public static TrackSearch ForTracks(string title, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["track"] = title;
			
			return new TrackSearch(terms, session, itemsPerPage);
		}
		
		public static TrackSearch ForTracks(string title, Session session)
		{
			return Search.ForTracks(title, session, 30);
		}
	}
}
