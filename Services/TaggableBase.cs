// TaggableBase.cs
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

namespace lastfm.Services
{
  public abstract class TaggableBase : Base
  {
    private string prefix {get; set;}
    
    public TaggableBase(string prefix, string[] authData)
      :base(authData)
    {
      this.prefix = prefix;
    }
    
    public void AddTags(params Tag[] tags)
    {
      if (tags.Length > 1)
      {
        foreach(Tag tag in tags)
          this.AddTags(tag);
        return;
      }
      
      RequestParameters p = getParams();
      p["tags"] = tags[0].Name;
      
      request(prefix + ".addTags", p);
    }
    
    public void AddTags(params string[] tags)
    {
      foreach(string tag in tags)
        AddTags(new Tag(tag, getAuthData()));
    }
    
    public Tag[] GetTags()
    {
      XmlDocument doc = 
        (new Request(prefix + ".getTags", APIKey, getParams(), Secret, SessionKey)).execute();
      
      List<Tag> list = new List<Tag>();
      
      foreach(string name in this.extractAll(doc.DocumentElement, "name"))
        list.Add(new Tag(name, getAuthData()));
      
      return list.ToArray();
    }
    
    public Tag[] GetTopTags()
    {
      XmlDocument doc = request(prefix + ".getTopTags");
      
      string[] names = extractAll(doc.DocumentElement, "name");
      
      List<Tag> list = new List<Tag>();
      foreach(string name in names)
        list.Add(new Tag(name, getAuthData()));
      
      return list.ToArray();
    }
    
    public Tag[] GetTopTags(int limit)
    {
      Tag[] array = GetTopTags();
      List<Tag> list = new List<Tag>();
      for(int i=0; i<limit; i++)
        list.Add(array[i]);
      
      return list.ToArray();
    }
    
    public void RemoveTags(params Tag[] tags)
    {
      if (tags.Length > 1)
      {
        foreach(Tag t in tags)
          RemoveTags(t);
        return;
      }
      
      RequestParameters p = getParams();
      p["tag"] = tags[0].Name;
      
      request(prefix + ".removeTag", p);
    }
    
    public void RemoveTags(params string[] tags)
    {
      foreach(string tag in tags)
        RemoveTags(new Tag(tag, getAuthData()));
    }
  }
}
