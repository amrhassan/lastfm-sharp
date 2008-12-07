// Pages.cs created with MonoDevelop
// User: amr at 2:20 AM 12/7/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Xml;
using System.Collections.Generic;

namespace Lastfm.Services
{
	/// <summary>
	/// An abstract class inherited by data objects that come in pages.
	/// </summary>
	public abstract class Pages<T> : Base
	{
		protected internal string methodName {get; set;}
		
		internal Pages(string methodName, Session session)
			:base(session)
		{
			this.methodName = methodName;
		}
		
		public int GetPageCount()
		{
			XmlDocument doc = request(methodName);
			
			return int.Parse(doc.DocumentElement.ChildNodes[0].Attributes.GetNamedItem("totalPages").InnerText);
		}
		
		public int GetItemsPerPage()
		{
			XmlDocument doc = request(methodName);
			
			return int.Parse(doc.DocumentElement.ChildNodes[0].Attributes.GetNamedItem("perPage").InnerText);
		}
		
		public abstract T[] GetPage(int page);
	}
}
