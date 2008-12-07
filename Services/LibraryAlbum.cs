// LibraryAlbum.cs created with MonoDevelop
// User: amr at 4:03 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Lastfm.Services
{
	/// <summary>
	/// An <see cref="Album"/> in a <see cref="Library"/>.
	/// </summary>
	public class LibraryAlbum : LibraryItem<Album>
	{
		public LibraryAlbum(Album album, int playcount, int tagcount)
			:base(album, playcount, tagcount)
		{}

		/// <summary>
		/// The album.
		/// </summary>
		public Album Album { get { return this.item; } }
	}
}
