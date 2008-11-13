// Session.cs
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
	public class Session
	{
		public string APIKey {get; private set;}
		public string APISecret {get; private set;}
		public string SessionKey {get; private set;}
		
		public bool Authenticated {get; private set;}
		
		public Session(string apiKey, string apiSecret, string sessionKey)
		{
			APIKey = apiKey;
			APISecret = apiSecret;
			SessionKey = sessionKey;
			
			Authenticated = true;
		}
		
		public Session(string apiKey, string apiSecret)
		{
			APIKey = apiKey;
			APISecret = apiSecret;
			
			Authenticated = false;
		}
		
		public void Authenticate(string sessionKey)
		{
			SessionKey = sessionKey;
			
			Authenticated = true;
		}
	}
}
