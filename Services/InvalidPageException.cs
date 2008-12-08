// InvalidPageException.cs created with MonoDevelop
// User: amr at 9:46 PM 12/8/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Lastfm.Services
{
	/// <summary>
	/// Gets thrown when an invalid page value is provided.
	/// </summary>
	public class InvalidPageException : Exception
	{
		
		public InvalidPageException(int givenPage, int shouldBe)
			:base("The first page should be " + shouldBe.ToString())
		{
		}
	}
}
