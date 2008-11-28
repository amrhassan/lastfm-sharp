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
	/// <summary>
	/// A Last.fm album.
	/// </summary>
	public class Album : Base, IEquatable<Album>, IHasImage
	{
		/// <summary>
		/// The name of the artist
		/// </summary>
		public string ArtistName {get; private set;}
		
		/// <summary>
		/// The album title.
		/// </summary>
		public string Title {get; private set;}
		
		/// <summary>
		/// The album title/name.
		/// </summary>
		public string Name {get { return Title; } }
		
		/// <summary>
		/// The album's artist.
		/// </summary>
		public Artist Artist {get { return new Artist(ArtistName, Session); } }
		
		/// <summary>
		/// The album's wiki on Last.fm.
		/// </summary>
		public AlbumWiki Wiki {get { return new AlbumWiki(this, Session); } }
		
		/// <summary>
		/// Createa an album object.
		/// </summary>
		/// <param name="artistName">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="title">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="session">
		/// A <see cref="Session"/>
		/// </param>
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
		
		/// <summary>
		/// Returns the album's MusicBrainz ID if available.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string GetMBID()
		{
			XmlDocument doc = request("album.getInfo");
			
			return extract(doc, "mbid");
		}
		
		/// <summary>
		/// Returns the album ID on Last.fm.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string GetID()
		{
			XmlDocument doc = request("album.getInfo");
			
			return extract(doc, "id");
		}
		
		/// <summary>
		/// Returns the album's release date.
		/// </summary>
		/// <returns>
		/// A <see cref="DateTime"/>
		/// </returns>
		public DateTime GetReleaseDate()
		{
			XmlDocument doc = request("album.getInfo");
			
			return DateTime.Parse(extract(doc, "releasedate"));
		}
		
		/// <summary>
		/// Returns the url to the album cover if available.
		/// </summary>
		/// <param name="size">
		/// A <see cref="AlbumImageSize"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string GetImageURL(AlbumImageSize size)
		{
			XmlDocument doc = request("album.getInfo");
			
			return extractAll(doc, "image", 4)[(int)size];
		}
		
		/// <summary>
		/// Returns the url to the album cover if available.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string GetImageURL()
		{
			return GetImageURL(AlbumImageSize.ExtraLarge);
		}
		
		/// <summary>
		/// Returns the number of listeners on Last.fm.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		public int GetListenerCount()
		{
			XmlDocument doc = request("album.getInfo");
			
			return Int32.Parse(extract(doc, "listeners"));
		}
		
		/// <summary>
		/// Returns the play count on Last.fm.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		public int GetPlaycount()
		{
			XmlDocument doc = request("album.getInfo");
			
			return Int32.Parse(extract(doc, "playcount"));
		}
		
		/// <summary>
		/// Returns an array of the tracks on this album.
		/// </summary>
		/// <returns>
		/// A <see cref="Track"/>
		/// </returns>
		public Track[] GetTracks()
		{
			string url = "lastfm://playlist/album/" + this.GetID();
			
			return (new XSPF(url, Session)).GetTracks();
		}
		
		/// <summary>
		/// Returns the top tags for this album on Last.fm.
		/// </summary>
		/// <returns>
		/// A <see cref="TopTag"/>
		/// </returns>
		public TopTag[] GetTopTags()
		{
			XmlDocument doc = request("album.getInfo");
			XmlNode node = doc.GetElementsByTagName("toptags")[0];
			
			List<TopTag> list = new List<TopTag>();
			foreach(XmlNode n in ((XmlElement)node).GetElementsByTagName("tag"))
			{
				Tag tag = new Tag(extract(n, "name"), Session);
				int count = Int32.Parse(extract(n, "count"));
				
				list.Add(new TopTag(tag, count));
			}
			
			return list.ToArray();
		}
		
		/// <summary>
		/// Returns the top tags for this album on Last.fm.
		/// </summary>
		/// <param name="limit">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="TopTag"/>
		/// </returns>
		public TopTag[] GetTopTags(int limit)
		{
			return sublist<TopTag>(GetTopTags(), limit);
		}
		
		public bool Equals(Album album)
		{
			if(album.Title == this.Title && album.ArtistName == this.ArtistName)
				return true;
			else
				return false;
		}
		
		/// <summary>
		/// Search for an album on Last.fm.
		/// </summary>
		/// <param name="albumName">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="session">
		/// A <see cref="Session"/>
		/// </param>
		/// <param name="itemsPerPage">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="AlbumSearch"/>
		/// </returns>
		public static AlbumSearch Search(string albumName, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["album"] = albumName;
			
			return new AlbumSearch(terms, session, itemsPerPage);
		}
		
		/// <summary>
		/// Search for an album on Last.fm.
		/// </summary>
		/// <param name="albumName">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="session">
		/// A <see cref="Session"/>
		/// </param>
		/// <returns>
		/// A <see cref="AlbumSearch"/>
		/// </returns>
		public static AlbumSearch Search(string albumName, Session session)
		{
			// 30 is the default (and maximum) number of results per page.
			return Album.Search(albumName, session, 30);
		}
		
		/// <summary>
		/// Add tags to this album.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="Tag"/>
		/// </param>
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
		
		/// <summary>
		/// Add tags to this album.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="System.String"/>
		/// </param>
		public void AddTags(params string[] tags)
		{
			foreach(string tag in tags)
				AddTags(new Tag(tag, Session));
		}
		
		/// <summary>
		/// Add tags to this album.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="TagCollection"/>
		/// </param>
		public void AddTags(TagCollection tags)
		{
			foreach(Tag tag in tags)
				AddTags(tag);
		}
		
		/// <summary>
		/// Returns the tags set by the authenticated user to this album.
		/// </summary>
		/// <returns>
		/// A <see cref="Tag"/>
		/// </returns>
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
		
		/// <summary>
		/// Remove from your tags on this album.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="Tag"/>
		/// </param>
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
		
		/// <summary>
		/// Remove from your tags on this album.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="System.String"/>
		/// </param>
		public void RemoveTags(params string[] tags)
		{
			//This method requires authentication
			requireAuthentication();
			
			foreach(string tag in tags)
				RemoveTags(new Tag(tag, Session));
		}
		
		/// <summary>
		/// Remove from the authenticated user's tags on this album.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="TagCollection"/>
		/// </param>
		public void RemoveTags(TagCollection tags)
		{
			foreach(Tag tag in tags)
				RemoveTags(tag);
		}
		
		/// <summary>
		/// Set the authenticated user's tags to only those tags.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="System.String"/>
		/// </param>
		public void SetTags(string[] tags)
		{
			List<Tag> list = new List<Tag>();
			foreach(string name in tags)
				list.Add(new Tag(name, Session));
			
			SetTags(list.ToArray());
		}
		
		/// <summary>
		/// Set the authenticated user's tags to only those tags.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="Tag"/>
		/// </param>
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
		
		/// <summary>
		/// Set the authenticated user's tags to only those tags.
		/// </summary>
		/// <param name="tags">
		/// A <see cref="TagCollection"/>
		/// </param>
		public void SetTags(TagCollection tags)
		{
			SetTags(tags.ToArray());
		}
		
		/// <summary>
		/// Clears all the tags that the authenticated user has set to this album.
		/// </summary>
		public void ClearTags()
		{
			foreach(Tag tag in GetTags())
				RemoveTags(tag);
		}
	}
}
