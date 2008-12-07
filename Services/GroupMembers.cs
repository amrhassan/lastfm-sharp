// GroupMembers.cs created with MonoDevelop
// User: amr at 2:26 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Collections.Generic;

namespace Lastfm.Services
{
	/// <summary>
	/// The members of a Last.fm group.
	/// </summary>
	public class GroupMembers : Pages<User>
	{
		/// <value>
		/// The concerned group.
		/// </value>
		public Group Group {get; private set;}
		
		public GroupMembers(Group group, Session session)
			:base("group.getMembers", session)
		{
			Group = group;
		}
		
		internal override RequestParameters getParams ()
		{
			return Group.getParams();
		}
		
		public override User[] GetPage(int page)
		{
			if(page < 1)
				throw new Exception("The first page is 1.");
			
			RequestParameters p = getParams();
			p["page"] = page.ToString();
			
			XmlDocument doc = Group.request("group.getMembers", p);
			
			List<User> list = new List<User>();
			foreach(XmlNode node in doc.GetElementsByTagName("user"))
				list.Add(new User(extract(node, "name"), Session));
			
			return list.ToArray();
		}
	}
}
