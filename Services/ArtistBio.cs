// ArtistBio.cs
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

namespace lastfm.Services
{
	public class ArtistBio : Wiki
	{
		public string ArtistName {get; private set;}
		public Artist Artist {get{return new Artist(ArtistName, Session);}}
		
		public ArtistBio(string artistName, Session session)
			:base("artist", session)
		{
			this.ArtistName = artistName;
		}
		
		protected override RequestParameters getParams ()
		{
			RequestParameters p = base.getParams ();
			p["artist"] = ArtistName;
			
			return p;
		}
	}
}
