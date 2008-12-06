// IHasURL.cs created with MonoDevelop
// User: amr at 12:20 AM 12/6/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Lastfm.Services
{
	/// <summary>
	/// Objects that implement this have url pages at Last.fm
	/// </summary>
	public interface IHasURL
	{
		string GetURL(SiteLanguage language);
		string URL { get; }
	}
}
