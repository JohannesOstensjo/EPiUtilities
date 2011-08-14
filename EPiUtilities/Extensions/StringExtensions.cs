using System.Text;
using EPiServer;
using EPiServer.Web;
using Encoder = Microsoft.Security.Application.Encoder;

namespace EPiUtilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the value of string.IsNullOrEmpty() for the string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool NullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Takes an url of internal format (/example/example.aspx?id=123&amp;epslanguage=no)
        /// and returns the friendly url equivalent. 
        /// </summary>
        /// <param name="internalUrl"></param>
        /// <returns></returns>
        public static string ConvertToExternalUrl(this string internalUrl)
        {
            if (!string.IsNullOrEmpty(internalUrl) && UrlRewriteProvider.IsFurlEnabled)
            {
                var ub = new UrlBuilder(internalUrl);
                Global.UrlRewriteProvider.ConvertToExternal(ub, null, Encoding.UTF8);
                return ub.ToString();
            }
            return internalUrl;
        }

        /// <summary>
        /// Takes an url of friendly external format (/example/example/)
        /// and returns the internal url equivalent. 
        /// </summary>
        /// <param name="externalUrl"></param>
        /// <returns></returns>
        public static string ConvertToInternalUrl(this string externalUrl)
        {
            if (!string.IsNullOrEmpty(externalUrl))
            {
                var ub = new UrlBuilder(externalUrl);
                Global.UrlRewriteProvider.ConvertToInternal(ub);
                return ub.ToString();
            }
            return externalUrl;
        }

        /// <summary>
        /// Returns an html image tag with string as url and specified alt text.
        /// Returns empty string if imageUrl is null or empty.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="altText"></param>
        /// <returns></returns>
        public static string ToImgWithAltText(this string imageUrl, string altText)
        {
            if (!imageUrl.NullOrEmpty())
                return string.Format("<img src=\"{0}\" alt=\"{1}\" />", imageUrl, altText);

            return "";
        }

        /// <summary>
        /// Returns an html image tag with string as url and specified alt text.
        /// Returns empty string if imageUrl is null or empty.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="cssClass"></param>
        /// <param name="altText"></param>
        /// <returns></returns>
        public static string ToImgWithCssClassAndAltText(this string imageUrl, string cssClass, string altText)
        {
            if (!imageUrl.NullOrEmpty())
                return string.Format("<img src=\"{0}\" class=\"{1}\" alt=\"{2}\" />", imageUrl, cssClass, altText);

            return "";
        }

        public static string ToFigureWithImgAndAltAndFigcaption(this string imageUrl, string altText, string figCaption)
        {
            if (!imageUrl.NullOrEmpty())
                return string.Format("<figure><img src=\"{0}\" alt=\"{1}\" /><figcaption>{2}</figcaption></figure>", imageUrl, altText, figCaption);

            return "";
        }

        /// <summary>
        /// If text has content, returns it surrounded by a div. 
        /// Returns empty if text is empty.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToDiv(this string text)
        {
            if (!text.NullOrEmpty())
                return string.Format("<div>{0}</div>", text);

            return "";
        }

        /// <summary>
        /// If text has content, returns it surrounded by a div. 
        /// Returns empty if text is empty.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static string ToDivWithCssClass(this string text, string cssClass)
        {
            if (!text.NullOrEmpty())
                return string.Format("<div class=\"{0}\">{1}</div>", cssClass, text);

            return "";
        }

        /// <summary>
        /// Calls CssEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCssEncoded(this string input)
        {
            return Encoder.CssEncode(input);
        }

        /// <summary>
        /// Calls HtmlAttributeEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToHtmlAttributeEncoded(this string input)
        {
            return Encoder.HtmlAttributeEncode(input);
        }

        /// <summary>
        /// Calls HtmlEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToHtmlEncoded(this string input)
        {
            return Encoder.HtmlEncode(input);
        }

        /// <summary>
        /// Calls HtmlEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="useNamedEntities"></param>
        /// <returns></returns>
        public static string ToHtmlEncoded(this string input, bool useNamedEntities)
        {
            return Encoder.HtmlEncode(input, useNamedEntities);
        }

        /// <summary>
        /// Calls JavaScriptEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToJavaScriptEncoded(this string input)
        {
            return Encoder.JavaScriptEncode(input);
        }

        /// <summary>
        /// Calls JavaScriptEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="emitQuotes"></param>
        /// <returns></returns>
        public static string ToJavaScriptEncoded(this string input, bool emitQuotes)
        {
            return Encoder.JavaScriptEncode(input, emitQuotes);
        }

        /// <summary>
        /// Calls UrlEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToUrlEncoded(this string input)
        {
            return Encoder.UrlEncode(input);
        }

        /// <summary>
        /// Calls UrlEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputEncoding"></param>
        /// <returns></returns>
        public static string ToUrlEncoded(this string input, Encoding inputEncoding)
        {
            return Encoder.UrlEncode(input, inputEncoding);
        }

        /// <summary>
        /// Calls UrlEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="codePage"></param>
        /// <returns></returns>
        public static string ToUrlEncoded(this string input, int codePage)
        {
            return Encoder.UrlEncode(input, codePage);
        }

        /// <summary>
        /// Calls XmlAttributeEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToXmlAttributeEncoded(this string input)
        {
            return Encoder.XmlAttributeEncode(input);
        }

        /// <summary>
        /// Calls XmlEncode from the AntiXss library on the string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToXmlEncoded(this string input)
        {
            return Encoder.XmlEncode(input);
        }
    }
}
