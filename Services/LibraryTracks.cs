// LibraryTracks.cs created with MonoDevelop
// User: amr at 4:40 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Collections.Generic;

namespace Lastfm.Services
{
	/// <summary>
	/// The tracks in a <see cref="Library"/>.
	/// </summary>
	public class LibraryTracks : Pages<LibraryTrack>
	{
		
		/// <summary>
		/// The library.
		/// </summary>
		public Library Library {get; private set;}
		
		public LibraryTracks(Library library, Session session)
			:base("library.getAlbums", session)
		{
			Library = library;
		}
		
		internal override RequestParameters getParams ()
		{
			return Library.getParams();
		}
		
		public override LibraryTrack[] GetPage (int page)
		{
			if(page < 1)
				throw new Exception("The first page is 1.");
			
			RequestParameters p = getParams();
			p["page"] = page.ToString();
			
			XmlDocument doc = request("library.getTracks", p);

			List<LibraryTrack> list = new List<LibraryTrack>();
			
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
				list.Add(new LibraryTrack(track, playcount, tagcount));
			}
			
			return list.ToArray();
		}


	}
}
