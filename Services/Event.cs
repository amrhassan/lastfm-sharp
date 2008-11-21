// Event.cs
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
	// TODO: A venue class that this object returns. I'm to tired right now.. i'll go get some sleep..
	public class Event : Base, IEquatable<Event>, IShareable, IHasImage
	{
		public int ID {get; private set;}
		
		public Event(int id, Session session)
			:base(session)
		{
			ID = id;
		}
		
		protected override RequestParameters getParams ()
		{
			RequestParameters p = base.getParams ();
			p["event"] = ID.ToString();
			
			return p;
		}
		
		public void SetAttendance(EventAttendance attendance)
		{
			requireAuthentication();
			
			RequestParameters p = getParams();
			int i = (int)attendance;
			p["status"] = i.ToString();
			request("event.attend", p);
		}
		
		public string GetTitle()
		{
			XmlDocument doc = request("event.getInfo");
			
			return extract(doc, "title");
		}
		
		public Artist[] GetArtists()
		{
			XmlDocument doc = request("event.getInfo");
			
			List<Artist> list = new List<Artist>();
			foreach(string name in extractAll(doc, "artist"))
				list.Add(new Artist(name, Session));
			
			return list.ToArray();
		}
		
		public Artist GetHeadliner()
		{
			XmlDocument doc = request("event.getInfo");
			
			return new Artist(extract(doc, "headliner"), Session);
		}
		
		public DateTime GetStartDate()
		{
			XmlDocument doc = request("event.getInfo");
			
			return DateTime.Parse(extract(doc, "startDate"));
		}
		
		public string GetDescription()
		{
			XmlDocument doc = request("event.getInfo");
			
			return extract(doc, "description");
		}
		
		public string GetImageURL(ImageSize size)
		{
			XmlDocument doc = request("event.getInfo");
			
			return extractAll(doc, "image")[(int)size];
		}
		
		public string GetImageURL()
		{
			return GetImageURL(ImageSize.Large);
		}
		
		public int GetAttendantCount()
		{
			XmlDocument doc = request("event.getInfo");
			
			return Int32.Parse(extract(doc, "attendance"));
		}
		
		public int GetReviewCount()
		{
			XmlDocument doc = request("event.getInfo");
			
			return Int32.Parse(extract(doc, "reviews"));
		}
			
		public string GetFlickrTag()
		{
			XmlDocument doc = request("event.getInfo");
			
			return extract(doc, "tag");
		}
		
		public bool Equals(Event eventObject)
		{
			if(eventObject.ToString() == this.ToString())
				return true;
			else
				return false;
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
			
			request("event.Share", p);
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
			
			request("event.Share", p);
		}
	}
}
