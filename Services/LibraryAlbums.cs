// LibraryAlbums.cs created with MonoDevelop
// User: amr at 3:46 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Collections.Generic;

namespace Lastfm.Services
{
	/// <summary>
	/// The albums in a <see cref="Library"/>.
	/// </summary>
	public class LibraryAlbums : Pages<LibraryAlbum> 
	{
		/// <summary>
		/// The library.
		/// </summary>
		public Library Library {get; private set;}
		
		public LibraryAlbums(Library library, Session session)
			:base("library.getAlbums", session)
		{
			Library = library;
		}
		
		internal override RequestParameters getParams ()
		{
			return Library.getParams();
		}

		
		public override LibraryAlbum[] GetPage (int page)
		{			
			if(page < 1)
				throw new Exception("The first page is 1.");
			
			RequestParameters p = getParams();
			p["page"] = page.ToString();
			
			XmlDocument doc = request("library.getAlbums", p);

			List<LibraryAlbum> list = new List<LibraryAlbum>();
			
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
				list.Add(new LibraryAlbum(album, playcount, tagcount));
			}
			
			return list.ToArray();
		}

	}
}
