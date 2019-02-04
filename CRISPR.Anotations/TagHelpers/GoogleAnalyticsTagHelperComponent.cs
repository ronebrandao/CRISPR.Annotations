﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRISPR.Anotations.TagHelpers
{
    public class GoogleAnalyticsOptions
    {
        public string TrackingCode { get; set; }
    }

    public class GoogleAnalyticsTagHelperComponent : TagHelperComponent
    {

        private readonly GoogleAnalyticsOptions _googleAnalyticsOptions;

        public GoogleAnalyticsTagHelperComponent(IOptions<GoogleAnalyticsOptions> googleAnalyticOptions)
        {
            _googleAnalyticsOptions = googleAnalyticOptions.Value;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //Inject the code only in the head element
            if (string.Equals(output.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                var trackingCode = _googleAnalyticsOptions.TrackingCode;

                if (!string.IsNullOrEmpty(trackingCode))
                {
                    output.PostContent
                        .AppendHtml("<script async src='https://www.googletagmanager.com/gtag/js?id=")
                        .AppendHtml(trackingCode)
                        .AppendHtml("'></script><script>window.dataLayer=window.dataLayer||[];function gtag(){dataLayer.push(arguments)}gtag('js',new Date);gtag('config','")
                        .AppendHtml(trackingCode)
                        .AppendHtml("',{displayFeaturesTask:'null'});</script>");
                }
            }

            base.Process(context, output);
        }

    }
}
