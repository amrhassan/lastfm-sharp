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
	public class Tag : Base, System.IEquatable<Tag>
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
		
		public TopAlbum[] GetTopAlbums()
		{
			XmlDocument doc = request("tag.getTopAlbums");
			
			List<TopAlbum> list = new List<TopAlbum>();
			foreach(XmlNode n in doc.GetElementsByTagName("album"))
			{
				Album album = new Album(extract(n, "name", 1), extract(n, "name"), Session);
				int count = Int32.Parse(extract(n, "tagcount"));
				
				list.Add(new TopAlbum(album, count));
			}
			
			return list.ToArray();
		}
		
		public TopArtist[] GetTopArtists()
		{
			XmlDocument doc = request("tag.getTopArtists");
			
			List<TopArtist> list = new List<TopArtist>();
			foreach(XmlNode node in doc.GetElementsByTagName("artist"))
			{
				Artist artist = new Artist(extract(node, "name"), Session);
				int count = int.Parse(extract(node, "tagcount"));
				
				list.Add(new TopArtist(artist, count));
			}
			
			return list.ToArray();
		}
		
		public TopTrack[] GetTopTracks()
		{
			XmlDocument doc = request("tag.getTopTracks");
			
			List<TopTrack> list = new List<TopTrack>();
			foreach(XmlNode n in doc.GetElementsByTagName("track"))
			{
				int weight = int.Parse(extract(n, "tagcount"));
				Track track = new Track(extract(n, "name", 1), extract(n, "name"), Session);
				
				list.Add(new TopTrack(track, weight));
			}
			
			return list.ToArray();
		}
		
		public bool Equals(Tag tag)
		{
			if (tag.Name == this.Name)
				return true;
		 else
				return false;
		}
		
		public static TagSearch Search(string tagName, Session session, int itemsPerPage)
		{
			Dictionary<string, string> terms = new Dictionary<string,string>();
			terms["tag"] = tagName;
			
			return new TagSearch(terms, session, itemsPerPage);
		}
		
		public static TagSearch Search(string tagName, Session session)
		{
			return Tag.Search(tagName, session, 30);
		}
		
		public WeeklyChartTimeSpan[] GetWeeklyChartTimeSpans()
		{
			XmlDocument doc = request("tag.getWeeklyChartList");
			
			List<WeeklyChartTimeSpan> list = new List<WeeklyChartTimeSpan>();
			foreach(XmlNode node in doc.GetElementsByTagName("chart"))
			{
				long lfrom = long.Parse(node.Attributes[0].InnerText);
				long lto = long.Parse(node.Attributes[1].InnerText);
				
				DateTime from = Utilities.TimestampToDateTime(lfrom);
				DateTime to = Utilities.TimestampToDateTime(lto);
				
				list.Add(new WeeklyChartTimeSpan(from, to));
			}
			
			return list.ToArray();
		}
		
		public WeeklyArtistChart GetWeeklyArtistChart()
		{
			XmlDocument doc = request("tag.getWeeklyArtistChart");
			
			XmlNode n = doc.GetElementsByTagName("weeklyartistchart")[0];
			
			DateTime nfrom = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[1].InnerText));
			DateTime nto = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[2].InnerText));
			
			WeeklyArtistChart chart = new WeeklyArtistChart(new WeeklyChartTimeSpan(nfrom, nto));
			
			foreach(XmlNode node in doc.GetElementsByTagName("artist"))
			{
				int rank = Int32.Parse(node.Attributes[0].InnerText);
				int playcount = Int32.Parse(extract(node, "playcount"));
				
				WeeklyArtistChartItem item = 
					new WeeklyArtistChartItem(new Artist(extract(node, "name"), Session),
					                         rank, playcount, new WeeklyChartTimeSpan(nfrom, nto));
				
				chart.Add(item);
			}
			
			return chart;
		}
		
		public WeeklyArtistChart GetWeeklyArtistChart(WeeklyChartTimeSpan span)
		{
			RequestParameters p = getParams();
			
			p["from"] = Utilities.DateTimeToTimestamp(span.From).ToString();
			p["to"] = Utilities.DateTimeToTimestamp(span.To).ToString();
			
			XmlDocument doc = request("tag.getWeeklyArtistChart", p);
			
			XmlNode n = doc.GetElementsByTagName("weeklyartistchart")[0];
			
			DateTime nfrom = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[1].InnerText));
			DateTime nto = Utilities.TimestampToDateTime(Int64.Parse(n.Attributes[2].InnerText));
			
			WeeklyArtistChart chart = new WeeklyArtistChart(new WeeklyChartTimeSpan(nfrom, nto));
			
			foreach(XmlNode node in doc.GetElementsByTagName("artist"))
			{
				int rank = Int32.Parse(node.Attributes[0].InnerText);
				int playcount = Int32.Parse(extract(node, "playcount"));
				
				WeeklyArtistChartItem item = 
					new WeeklyArtistChartItem(new Artist(extract(node, "name"), Session),
					                         rank, playcount, new WeeklyChartTimeSpan(nfrom, nto));
				
				chart.Add(item);
			}
			
			return chart;
		}
	}
}
