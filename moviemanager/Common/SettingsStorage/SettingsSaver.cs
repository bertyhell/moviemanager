﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Common.SettingsStorage
{
    public class SettingsSaver
    {
        private readonly string _filePath;
        private readonly string _rootTag;
        private XmlTextWriter _xmlWriter;

        public SettingsSaver(String filePath, string rootTag)
        {
            _filePath = filePath;
            _rootTag = rootTag;
        }

        public void CreateXmlWriter()
        {
            _tagNameList.Clear();
            _xmlWriter = new XmlTextWriter(_filePath, null) {Formatting = Formatting.Indented};
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement(_rootTag);
        }

        public void CloseXmlWriter()
        {
            _xmlWriter.WriteEndElement();
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Close();
        }

        public void WriteString(string value, string tagName)
        {

            if (TagNameInUse(tagName))
                throw new DuplicateSettingsKeyException(tagName);

            AddUsedTagName(tagName);
            _xmlWriter.WriteStartElement(tagName);
            _xmlWriter.WriteValue(value);
            _xmlWriter.WriteEndElement();
        }

        public string ReadString(string tagName)
        {
            XmlDocument Xdoc = new XmlDocument();
            Xdoc.Load(_filePath);
            XmlNodeList NodeList = Xdoc.GetElementsByTagName(tagName);
            if (NodeList.Count == 0)
                return null;

            return NodeList[0].InnerText;
        }

        public void WriteStringList(List<String> stringList, string listName)
        {
            if (TagNameInUse(listName))
                throw new DuplicateSettingsKeyException(listName);

            AddUsedTagName(listName);
            _xmlWriter.WriteStartElement(listName);
            foreach (string S in stringList)
            {
                _xmlWriter.WriteStartElement("String");
                _xmlWriter.WriteValue(S);
                _xmlWriter.WriteEndElement();
            }
            _xmlWriter.WriteEndElement();
        }

        public List<string> ReadStringList(string listName)
        {
            XmlDocument Xdoc = new XmlDocument();
            Xdoc.Load(_filePath);
            XmlNodeList NodeList = Xdoc.GetElementsByTagName(listName);
            if (NodeList.Count == 0)
                return null;

            XmlNodeList ListElements = ((XmlElement)NodeList[0]).GetElementsByTagName("String");

            return (from XmlElement ListElement in ListElements select ListElement.InnerText).ToList();
        }

        #region helperMethods

        private readonly List<string> _tagNameList = new List<string>();
        private bool TagNameInUse(string tagName)
        {
            return _tagNameList.Contains(tagName);
        }

        private void AddUsedTagName(string tagName)
        {
            if (!TagNameInUse(tagName))
            {
                _tagNameList.Add(tagName);
            }
        }
        #endregion
    }
}
