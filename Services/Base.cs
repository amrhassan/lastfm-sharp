// Base.cs
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
using System.Xml;

namespace Lastfm.Services
{
	public abstract class Base
	{
		protected Session Session {get; set;}
		
		public Base(Session session)
		{
			Session = session;
		}
		
		protected virtual RequestParameters getParams()
		{
			// OVERRIDE ME
			return new RequestParameters();
		}
    
		protected XmlDocument request(string methodName, RequestParameters parameters)
		{
			return (new Request(methodName, Session, parameters)).execute();
		}
    
		protected XmlDocument request(string methodName)
		{
			return (new Request(methodName, Session, getParams())).execute();
		}
		
		protected string extract(XmlNode node, string name, int index)
		{
			return ((XmlElement)node).GetElementsByTagName(name)[index].InnerText;
		}
		
		protected string extract(XmlNode node, string name)
		{
			return extract((XmlElement)node, name, 0);
		}
		
		protected string extract(XmlDocument document, string name)
		{
			return extract(document.DocumentElement, name);
		}
		
		protected string extract(XmlDocument document, string name, int index)
		{
			return extract(document.DocumentElement, name, index);
		}
		
    protected string[] extractAll(XmlNode node, string name, int limitCount)
		{
			string[] s = extractAll(node, name);
			List<string> l = new List<string>();
			
			for(int i = 0; i < limitCount; i++)
				l.Add(s[i]);
			
			return l.ToArray();
		}
    
		protected string[] extractAll(XmlNode node, string name)
		{
			List<string> list = new List<string>();
			
			for(int i = 0; i < ((XmlElement)node).GetElementsByTagName(name).Count; i++)
				list.Add(extract(node, name, i));
			
			return list.ToArray();
		}
		
		protected string[] extractAll(XmlDocument document, string name)
		{
			return extractAll(document.DocumentElement, name);
		}
		
		protected string[] extractAll(XmlDocument document, string name, int limitCount)
		{
			return extractAll(document.DocumentElement, name, limitCount);
		}
		
		protected void requireAuthentication()
		{
			if(!this.Session.Authenticated)
				throw new AuthenticationRequiredException();
		}
		
		protected T[] sublist<T> (T[] original, int limit)
		{
			List<T> list = new List<T>();
			
			for(int i=0; i<limit; i++)
				list.Add(original[i]);
			
			return list.ToArray();
		}
	}
}
