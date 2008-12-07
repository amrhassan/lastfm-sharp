// LibraryArtist.cs created with MonoDevelop
// User: amr at 4:03 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Lastfm.Services
{
	/// <summary>
	/// An <see cref="Artist"/> in a <see cref="Library"/>.
	/// </summary>
	public class LibraryArtist : LibraryItem<Artist>
	{
		public LibraryArtist(Artist artist, int playcount, int tagcount)
			:base(artist, playcount, tagcount)
		{
		}
		
		/// <summary>
		/// The artist.
		/// </summary>
		public Artist Artist { get { return this.item; } }
	}
}
