// User.cs
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
	public class User : Base, IEquatable<User>
	{
		public string Name {get; private set;}
		
		public User(string name, Session session)
			:base(session)
		{
			Name = name;
		}
		
		public override string ToString ()
		{
			return Name;
		}
		
		protected override RequestParameters getParams ()
		{
			RequestParameters p = base.getParams ();
			p["user"] = Name;
			
			return p;
		}
		
		public bool Equals(User user)
		{
			if(user.Name == this.Name)
				return true;
			else
				return false;
		}
		
		public static AuthenticatedUser GetAuthenticatedUser(Session session)
		{
			return AuthenticatedUser.GetUser(session);
		}
		
		public WeeklyTrackChart GetWeeklyTrackChart()
		{
			XmlDocument doc = request("user.getWeeklyTrackChart");
			
			XmlNode n = doc.GetElementsByTagName("weeklytrackchart")[0];
			
			DateTime nfrom = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[1].InnerText));
			DateTime nto = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[2].InnerText));
			
			WeeklyTrackChart chart = new WeeklyTrackChart(nfrom, nto);
			
			foreach(XmlNode node in doc.GetElementsByTagName("track"))
			{
				int rank = Int32.Parse(node.Attributes[0].InnerText);
				int playcount = Int32.Parse(extract(node, "playcount"));
				
				WeeklyTrackChartItem item = 
					new WeeklyTrackChartItem(new Track(extract(node, "artist"), extract(node, "name"), Session),
					                         rank, playcount, nfrom, nto);
				
				chart.Add(item);
			}
			
			return chart;
		}
		
		public WeeklyTrackChart GetWeeklyTrackChart(DateTime from, DateTime to)
		{
			RequestParameters p = getParams();
			
			p["from"] = Utilities.DateTimeToTimestamp(from).ToString();
			p["to"] = Utilities.DateTimeToTimestamp(to).ToString();
			
			XmlDocument doc = request("user.getWeeklyTrackChart", p);
			
			XmlNode n = doc.GetElementsByTagName("weeklytrackchart")[0];
			
			DateTime nfrom = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[1].InnerText));
			DateTime nto = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[2].InnerText));
			
			WeeklyTrackChart chart = new WeeklyTrackChart(nfrom, nto);
			
			foreach(XmlNode node in doc.GetElementsByTagName("track"))
			{
				int rank = Int32.Parse(node.Attributes[0].InnerText);
				int playcount = Int32.Parse(extract(node, "playcount"));
				
				WeeklyTrackChartItem item = 
					new WeeklyTrackChartItem(new Track(extract(node, "artist"), extract(node, "name"), Session),
					                         rank, playcount, nfrom, nto);
				
				chart.Add(item);
			}
			
			return chart;
		}
		
		public WeeklyArtistChart GetWeeklyArtistChart()
		{
			XmlDocument doc = request("user.getWeeklyArtistChart");
			
			XmlNode n = doc.GetElementsByTagName("weeklyartistchart")[0];
			
			DateTime nfrom = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[1].InnerText));
			DateTime nto = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[2].InnerText));
			
			WeeklyArtistChart chart = new WeeklyArtistChart(nfrom, nto);
			
			foreach(XmlNode node in doc.GetElementsByTagName("artist"))
			{
				int rank = Int32.Parse(node.Attributes[0].InnerText);
				int playcount = Int32.Parse(extract(node, "playcount"));
				
				WeeklyArtistChartItem item = 
					new WeeklyArtistChartItem(new Artist(extract(node, "name"), Session),
					                         rank, playcount, nfrom, nto);
				
				chart.Add(item);
			}
			
			return chart;
		}
		
		public WeeklyArtistChart GetWeeklyArtistChart(DateTime from, DateTime to)
		{
			RequestParameters p = getParams();
			
			p["from"] = Utilities.DateTimeToTimestamp(from).ToString();
			p["to"] = Utilities.DateTimeToTimestamp(to).ToString();
			
			XmlDocument doc = request("user.getWeeklyArtistChart", p);
			
			XmlNode n = doc.GetElementsByTagName("weeklyartistchart")[0];
			
			DateTime nfrom = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[1].InnerText));
			DateTime nto = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[2].InnerText));
			
			WeeklyArtistChart chart = new WeeklyArtistChart(nfrom, nto);
			
			foreach(XmlNode node in doc.GetElementsByTagName("artist"))
			{
				int rank = Int32.Parse(node.Attributes[0].InnerText);
				int playcount = Int32.Parse(extract(node, "playcount"));
				
				WeeklyArtistChartItem item = 
					new WeeklyArtistChartItem(new Artist(extract(node, "name"), Session),
					                         rank, playcount, nfrom, nto);
				
				chart.Add(item);
			}
			
			return chart;
		}
		
		public WeeklyAlbumChart GetWeeklyAlbumChart()
		{
			XmlDocument doc = request("user.getWeeklyAlbumChart");
			
			XmlNode n = doc.GetElementsByTagName("weeklyalbumchart")[0];
			
			DateTime nfrom = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[1].InnerText));
			DateTime nto = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[2].InnerText));
			
			WeeklyAlbumChart chart = new WeeklyAlbumChart(nfrom, nto);
			
			foreach(XmlNode node in doc.GetElementsByTagName("album"))
			{
				int rank = Int32.Parse(node.Attributes[0].InnerText);
				int playcount = Int32.Parse(extract(node, "playcount"));
				
				WeeklyAlbumChartItem item = 
					new WeeklyAlbumChartItem(new Album(extract(node, "artist"), extract(node, "name"), Session),
					                         rank, playcount, nfrom, nto);
				
				chart.Add(item);
			}
			
			return chart;
		}
		
		public WeeklyAlbumChart GetWeeklyAlbumChart(DateTime from, DateTime to)
		{
			RequestParameters p = getParams();
			
			p["from"] = Utilities.DateTimeToTimestamp(from).ToString();
			p["to"] = Utilities.DateTimeToTimestamp(to).ToString();
			
			XmlDocument doc = request("user.getWeeklyAlbumChart", p);
			
			XmlNode n = doc.GetElementsByTagName("weeklyalbumchart")[0];
			
			DateTime nfrom = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[1].InnerText));
			DateTime nto = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[2].InnerText));
			
			WeeklyAlbumChart chart = new WeeklyAlbumChart(nfrom, nto);
			
			foreach(XmlNode node in doc.GetElementsByTagName("album"))
			{
				int rank = Int32.Parse(node.Attributes[0].InnerText);
				int playcount = Int32.Parse(extract(node, "playcount"));
				
				WeeklyAlbumChartItem item = 
					new WeeklyAlbumChartItem(new Album(extract(node, "artist"), extract(node, "name"), Session),
					                         rank, playcount, nfrom, nto);
				
				chart.Add(item);
			}
			
			return chart;
		}
	}
}
