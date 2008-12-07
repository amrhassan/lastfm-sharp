// RecommendedArtists.cs created with MonoDevelop
// User: amr at 2:56 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Collections.Generic;

namespace Lastfm.Services
{
	
	
	public class RecommendedArtists : Pages<Artist>
	{
		public AuthenticatedUser User {get; private set;}
		
		public RecommendedArtists(AuthenticatedUser user, Session session)
			:base("user.getRecommendedArtists", session)
		{
			this.User = user;
		}
		
		internal override RequestParameters getParams ()
		{
			return User.getParams();
		}

		
		public override Artist[] GetPage (int page)
		{
			if(page < 1)
				throw new Exception("The first page is 1.");
			
			RequestParameters p = getParams();
			p["page"] = page.ToString();
			
			XmlDocument doc = request("user.getRecommendedArtists", p);
			
			List<Artist> list = new List<Artist>();
			foreach(XmlNode node in doc.GetElementsByTagName("artist"))
				list.Add(new Artist(extract(node, "name"), Session));
			
			return list.ToArray();
		}
	}
}
