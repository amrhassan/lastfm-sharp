// Library.cs
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
	/// <summary>
	/// A user's library.
	/// </summary>
	public class Library : Base
	{
		/// <summary>
		/// The user who owns the library.
		/// </summary>
		public User User {get; private set;}
		
		public Library(User user, Session session)
			:base(session)
		{
			this.User = user;
		}
		
		public Library(string username, Session session)
			:base(session)
		{
			this.User = new User(username, Session);
		}
		
		protected override RequestParameters getParams()
		{
			RequestParameters p = base.getParams ();
			p["user"] = User.Name;
			
			return p;
		}
		
		public override string ToString()
		{
			return "The library of " + User.Name;
		}
		
		/// <summary>
		/// Add an album to the library.
		/// </summary>
		/// <param name="album">
		/// A <see cref="Album"/>
		/// </param>
		public void AddAlbum(Album album)
		{
			RequestParameters p = getParams();
			
			p["artist"] = album.Artist.Name;
			p["album"] = album.Title;
			
			request("library.addAlbum", p);
		}
		
		/// <summary>
		/// Add an artist to the library.
		/// </summary>
		/// <param name="artist">
		/// A <see cref="Artist"/>
		/// </param>
		public void AddArtist(Artist artist)
		{
			RequestParameters p = getParams();
			
			p["artist"] = artist.Name;
			
			request("library.addArtist", p);
		}
		
		/// <summary>
		/// Add a track to the library.
		/// </summary>
		/// <param name="track">
		/// A <see cref="Track"/>
		/// </param>
		public void AddTrack(Track track)
		{
			RequestParameters p = getParams();
			
			p["artist"] = track.Artist.Name;
			p["track"] = track.Title;
			
			request("library.addTrack", p);
		}
		
		/// <summary>
		/// Returns albums from the library.
		/// </summary>
		/// <param name="page">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="limit">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="LibraryItem`1"/>
		/// </returns>
		public LibraryItem<Album>[] GetAlbums(int page, int limit)
		{
			RequestParameters p = getParams();
			
			p["limit"] = limit.ToString();
			p["page"] = page.ToString();
			
			XmlDocument doc = request("library.getAlbums", p);

			List<LibraryItem<Album>> list = new List<LibraryItem<Album>>();
			
			foreach(XmlNode node in doc.GetElementsByTagName("album"))
			{
				int playcount = 0;
				try
				{ playcount = Int32.Parse(extract(node, "playcount")); }
				catch (FormatException)
				{}
				
				int tagcount = 0;
				try
				{ tagcount = Int32.Parse(extract(node, "tagcount")); }
				catch (FormatException)
				{}
				
				Album album = new Album(extract(node, "name", 1), extract(node, "name"), Session);
				list.Add(new LibraryItem<Album>(album, playcount, tagcount));
			}
			
			return list.ToArray();
		}
		
		/// <summary>
		/// Returns albums from the library.
		/// </summary>
		/// <param name="page">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="LibraryItem`1"/>
		/// </returns>
		public LibraryItem<Album>[] GetAlbums(int page)
		{
			// max limit is 49.
			return GetAlbums(page, 49);
		}
		
		/// <summary>
		/// Returns tracks from the library.
		/// </summary>
		/// <param name="page">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="limit">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="LibraryItem`1"/>
		/// </returns>
		public LibraryItem<Track>[] GetTracks(int page, int limit)
		{
			RequestParameters p = getParams();
			
			p["limit"] = limit.ToString();
			p["page"] = page.ToString();
			
			XmlDocument doc = request("library.getTracks", p);

			List<LibraryItem<Track>> list = new List<LibraryItem<Track>>();
			
			foreach(XmlNode node in doc.GetElementsByTagName("track"))
			{
				int playcount = 0;
				try
				{ playcount = Int32.Parse(extract(node, "playcount")); }
				catch (FormatException)
				{}
				
				int tagcount = 0;
				try
				{ tagcount = Int32.Parse(extract(node, "tagcount")); }
				catch (FormatException)
				{}
				
				Track track = new Track(extract(node, "name", 1), extract(node, "name"), Session);
				list.Add(new LibraryItem<Track>(track, playcount, tagcount));
			}
			
			return list.ToArray();
		}
		
		/// <summary>
		/// Returns tracks from the library.
		/// </summary>
		/// <param name="page">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="LibraryItem`1"/>
		/// </returns>
		public LibraryItem<Track>[] GetTracks(int page)
		{
			// max limit is 49.
			return GetTracks(page, 49);
		}
		
		/// <summary>
		/// Returns artists from the library.
		/// </summary>
		/// <param name="page">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="limit">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="LibraryItem`1"/>
		/// </returns>
		public LibraryItem<Artist>[] GetArtists(int page, int limit)
		{
			RequestParameters p = getParams();
			
			p["limit"] = limit.ToString();
			p["page"] = page.ToString();
			
			XmlDocument doc = request("library.getTracks", p);

			List<LibraryItem<Artist>> list = new List<LibraryItem<Artist>>();
			
			foreach(XmlNode node in doc.GetElementsByTagName("artist"))
			{
				int playcount = 0;
				try
				{ playcount = Int32.Parse(extract(node, "playcount")); }
				catch (FormatException)
				{}
				
				int tagcount = 0;
				try
				{ tagcount = Int32.Parse(extract(node, "tagcount")); }
				catch (FormatException)
				{}
				
				Artist artist = new Artist(extract(node, "name"), Session);
				list.Add(new LibraryItem<Artist>(artist, playcount, tagcount));
			}
			
			return list.ToArray();
		}
		
		/// <summary>
		/// Returns artists from the library.
		/// </summary>
		/// <param name="page">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="LibraryItem`1"/>
		/// </returns>
		public LibraryItem<Artist>[] GetArtists(int page)
		{
			// max limit is 49
			return GetArtists(page, 49);
		}
		
		public int GetTotalArtistPages()
		{
			XmlDocument doc = request("library.getArtists");
			
			return Int32.Parse(doc.GetElementsByTagName("artists")[0].Attributes.GetNamedItem("totalPages").InnerText);
		}
		
		public int GetTotalTrackPages()
		{
			XmlDocument doc = request("library.getTracks");
			
			return Int32.Parse(doc.GetElementsByTagName("tracks")[0].Attributes.GetNamedItem("totalPages").InnerText);
		}
		
		public int GetTotalAlbumPages()
		{
			XmlDocument doc = request("library.getAlbums");
			
			return Int32.Parse(doc.GetElementsByTagName("albums")[0].Attributes.GetNamedItem("totalPages").InnerText);
		}
	}
}
