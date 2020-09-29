
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public static class LangDataset
{
    private static string dataset_filename = "language_dataset";
    private static string language;

    //TO BE SPECIFIED
    private static string default_language = "English";
    private static List<string> lang_list = new List<string>{"English","French","Portuguese","German"};
    //

    //private static XmlDocument dataset = new XmlDocument();
    private static TextAsset txtXmlAsset;
    public static XElement allDict;

    public static void initialize()
    {
        //dataset.Load(dataset_filename);
        txtXmlAsset = Resources.Load<TextAsset>(dataset_filename);
        var doc = XDocument.Parse(txtXmlAsset.text);
        allDict = doc.Element("languages");
        language = Application.systemLanguage.ToString();
        if (!lang_list.Contains(language))
        {
            language = default_language;
        }
        Debug.Log(language);
    }

    public static string getText(string elementName)
    {
        return allDict.Element(language).Element(elementName).Value;
    }
}

