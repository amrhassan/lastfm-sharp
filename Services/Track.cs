// Track.cs
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

namespace lastfm.Services
{
  public class Track : TaggableBase
  {
    public string Title {get; private set;}
    public string ArtistName {get; private set;}
    public Artist Artist
    { get { return new Artist(this.ArtistName, getAuthData()); } }
    
    public Wiki Wiki
    {
      get
      { return new TrackWiki(ArtistName, Title, getAuthData()); }
    }
    
    public Track(string artistName, string title, string apiKey, string secret, string sessionKey)
      :base("track", new string[] {apiKey, secret, sessionKey})
    {
      Title = title;
      ArtistName = artistName;
    }
    
    public Track(string artistName, string title, string[] authData)
      :base("track", authData)
    {
      Title = title;
      ArtistName = artistName;
    }
    
    public override string ToString ()
    {
      return this.Artist + " - " + this.Title;
    }
    
    protected override RequestParameters getParams ()
    {
      RequestParameters p = base.getParams ();
      p["artist"] = ArtistName;
      p["track"] = Title;
      
      return p;
    }
    
    

  }
}
