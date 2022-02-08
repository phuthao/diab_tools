// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     WebCrawlerHelper.cs
// Created Date:  2018/10/22 11:48 AM
// ------------------------------------------------------------------------

using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DiaB.Core.Web.Helpers
{
    public static class WebCrawlerHelper
    {
        public static async Task<HtmlDocument> Crawl(string url)
        {
            return await new HtmlWeb().LoadFromWebAsync(url);
        }
    }
}
