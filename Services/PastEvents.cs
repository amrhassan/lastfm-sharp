// PastEvents.cs created with MonoDevelop
// User: amr at 3:19 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Collections.Generic;

namespace Lastfm.Services
{
	
	
	public class PastEvents : Pages<Event>
	{
		public User User {get; private set;}
		
		public PastEvents(User user, Session session)
			:base("user.getPastEvents", session)
		{
			this.User = user;
		}
		
		internal override RequestParameters getParams ()
		{
			return User.getParams();
		}

		public override Event[] GetPage (int page)
		{
			if(page < 1)
				throw new Exception("The first page is 1.");
			
			RequestParameters p = getParams();
			p["page"] = page.ToString();
			
			XmlDocument doc = request("user.getPastEvents", p);
			
			List<Event> list = new List<Event>();
			foreach(XmlNode node in doc.GetElementsByTagName("event"))
				list.Add(new Event(int.Parse(extract(node, "id")), Session));
			
			return list.ToArray();
		}
	}
}
