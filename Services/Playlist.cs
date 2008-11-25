// Playlist.cs
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
	public class Playlist : Base, IHasImage, System.IEquatable<Playlist>
	{	
		public int ID {get; private set;}
		public User User {get; private set;}
		
		public Playlist(string username, int id, Session session)
			:base(session)
		{
			ID = id;
			User = new User(username, session);
		}
		
		public Playlist(User user, int id, Session session)
			:base(session)
		{
			ID = id;
			User = user;
		}
		
		protected override RequestParameters getParams ()
		{
			RequestParameters p = base.getParams ();
			p["id"] = ID.ToString();
			
			return p;
		}
		
		public Track[] GetTracks()
		{
			string url = "lastfm://playlist/" + ID.ToString();
			
			return (new XSPF(url, Session)).GetTracks();
		}
		
		public void AddTrack(Track track)
		{
			requireAuthentication();
			
			RequestParameters p = getParams();
			p["track"] = track.Title;
			p["artist"] = track.ArtistName;
			
			request("playlist.addTrack", p);
		}
		
		private XmlNode getNode()
		{
			RequestParameters p = new Lastfm.RequestParameters();
			p["user"] = User.Name;
			
			XmlDocument doc = request("user.getPlaylists", p);
			foreach(XmlNode node in doc.GetElementsByTagName("playlist"))
			{
				if (Int32.Parse(extract(node, "id")) == ID)
					return node;
			}
			
			return null;			
		}
		
		public string GetTitle()
		{
			return extract(getNode(), "title");
		}
		
		public string GetDescription()
		{
			return extract(getNode(), "description");
		}
		
		public DateTime GetCreationDate()
		{
			return DateTime.Parse(extract(getNode(), "date"));
		}
		
		public int GetSize()
		{
			return Int32.Parse(extract(getNode(), "size"));
		}
		
		public TimeSpan GetDuration()
		{
			int duration = Int32.Parse(extract(getNode(), "duration"));
			
			// duration is in seconds, i guess..
			return new TimeSpan(0, 0, duration);
		}
		
		public string GetImageURL(ImageSize size)
		{
			return extractAll(getNode(), "image")[(int)size];
		}
		
		public string GetImageURL()
		{
			return GetImageURL(ImageSize.Large);
		}
		
		public bool Equals(Playlist playlist)
		{
			return (this.ID == playlist.ID);
		}
		
		public static Playlist CreateNew(string title, string description, Session session)
		{
			//manually test session for authentication.
			if(!session.Authenticated)
				throw new AuthenticationRequiredException();
			
			
			RequestParameters p = new Lastfm.RequestParameters();
			p["title"] = title;
			p["description"] = description;
			
			XmlDocument doc = (new Request("playlist.create", session, p)).execute();
			int id = Int32.Parse(doc.GetElementsByTagName("id")[0].InnerText);
			
			return new Playlist(AuthenticatedUser.GetUser(session), id, session);
		}
	}
}
