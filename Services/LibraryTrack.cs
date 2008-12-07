// LibraryTrack.cs created with MonoDevelop
// User: amr at 4:04 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Lastfm.Services
{
	/// <summary>
	/// A <see cref="Track"/> in a <see cref="Library"/>.
	/// </summary>
	public class LibraryTrack : LibraryItem<Track>
	{
		public LibraryTrack(Track track, int playcount, int tagcount)
			:base(track, playcount, tagcount)
		{}
		
		/// <summary>
		/// The track.
		/// </summary>
		public Track Track { get { return this.item; } } 
	}
}
