//  
//  Copyright (C) 2009 Amr Hassan
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;

namespace Lastfm.Services
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple=true)]
    public sealed class LanguageTextAttribute : Attribute
    {
        public string LanguageText = string.Empty;

        public LanguageTextAttribute(string text)
        {
            LanguageText = text;
        }

        public override string ToString()
        {
            return LanguageText;
        }
    }

    /// <summary>
    /// Languages available for the Last.fm website.
    /// </summary>
    public enum SiteLanguage
    {
        [LanguageTextAttribute("English")]
        English,

        [LanguageTextAttribute("Deutsch")]
        German,

        [LanguageTextAttribute("Español")]
        Spanish,

        [LanguageTextAttribute("Français")]
        French,

        [LanguageTextAttribute("Italiano")]
        Italian,

        [LanguageTextAttribute("Polszczyzna")]
        Polish,

        [LanguageTextAttribute("Português")]
        Portuguese,

        [LanguageTextAttribute("Svenska")]
        Swedish,

        [LanguageTextAttribute("Türkçe")]
        Turkish,

        [LanguageTextAttribute("русский язык")]
        Russian,

        [LanguageTextAttribute("Nihongo")]
        [LanguageTextAttribute("日本語")]
        Japanese,

        [LanguageTextAttribute("Zhōngwén")]
        [LanguageTextAttribute("中文")]
        Chinese
    }
}
