// LibraryArtists.cs created with MonoDevelop
// User: amr at 4:19 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Collections.Generic;

namespace Lastfm.Services
{
	/// <summary>
	/// The artists in a <see cref="Library"/>.
	/// </summary>
	public class LibraryArtists : Pages<LibraryArtist>
	{
		
		/// <summary>
		/// The library.
		/// </summary>
		public Library Library {get; private set;}
		
		public LibraryArtists(Library library, Session session)
			:base("library.getArtists", session)
		{
			this.Library = library; 
		}
		
		internal override RequestParameters getParams ()
		{
			return Library.getParams();
		}
		
		public override LibraryArtist[] GetPage (int page)
		{
			if(page < 1)
				throw new Exception("The first page is 1.");
			
			RequestParameters p = getParams();
			p["page"] = page.ToString();
			
			XmlDocument doc = request("library.getArtists", p);

			List<LibraryArtist> list = new List<LibraryArtist>();
			
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
				list.Add(new LibraryArtist(artist, playcount, tagcount));
			}
			
			return list.ToArray();
		}

	}
}
