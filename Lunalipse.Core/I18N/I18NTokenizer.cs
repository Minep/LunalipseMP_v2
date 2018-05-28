using Lunalipse.Common.Data;
using Lunalipse.Common.Data.Errors;
using Lunalipse.Utilities;
using System.Xml;
using static Lunalipse.Utilities.Extended;

namespace Lunalipse.Core.I18N
{
    public class I18NTokenizer
    {
        XmlDocument xd;
        XmlReaderSettings xrs;
        public I18NTokenizer()
        {
            xd = new XmlDocument();
            xrs = new XmlReaderSettings();
            xrs.IgnoreComments = true;
        }

        public bool LoadFromFile(string path)
        {
            if (!path.DExist(FType.FILE))
            {
                ErrorDelegation.OnErrorRaisedI18N?.Invoke(ErrorI18N.I18N_FILE_NOT_FOUND, path);
                return false;
            }
            try
            {
                XmlReader xr = XmlReader.Create(path, xrs);
                xd.Load(xr);
                return true;
            }
            catch(XmlException xex)
            {
                ErrorDelegation.OnErrorRaisedI18N?.Invoke(ErrorI18N.INVALID_INPUT_CONTENT, xex.Message);
                return false;
            }
        }

        public bool LoadFromString(string str)
        {
            try
            {
                xd.LoadXml(str);
                return true;
            }
            catch (XmlException xex)
            {
                ErrorDelegation.OnErrorRaisedI18N?.Invoke(ErrorI18N.INVALID_INPUT_CONTENT, xex.Message);
                return false;
            }
        }

        public void GetPages(SupportLanguages Lang)
        {
            I18NPages i18np = I18NPages.INSTANCE;
            if (!xd.HasChildNodes)
            {
                ErrorDelegation.OnErrorRaisedI18N?.Invoke(ErrorI18N.DAMAGED_FILE_EMPTY);
                return;
            }
            XmlNode xn = xd.SelectSingleNode("/LpsI18N/Lang[@lang='{0}']".FormateEx(Lang.ToString()));
            if(xn != null)
            {
                foreach (XmlNode page in xn.SelectNodes("Page[@key]"))
                {
                    I18NCollection icl = new I18NCollection();
                    foreach (XmlNode item in page.SelectNodes("Key[@name]"))
                    {
                        icl.AddToCollection(item.Attributes["name"].Value, item.InnerText);
                    }
                    i18np.AddPage(page.Attributes["key"].Value, icl);
                }
            }
            else
            {
                ErrorDelegation.OnErrorRaisedI18N?.Invoke(ErrorI18N.DAMAGED_FILE_LANG_MISSING, Lang.ToString());
            }
            return;
        } 
    }
}
