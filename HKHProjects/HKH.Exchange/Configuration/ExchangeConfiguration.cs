﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Xml;

namespace HKH.Exchange.Configuration
{
    public class ExchangeConfiguration : Dictionary<string, TableMapping>
    {
        private ExchangeConfiguration()
        {
        }

        public static ExchangeConfiguration Load(string path)
        {
            ExchangeConfiguration self = new ExchangeConfiguration();
            if (path.EndsWith(".xml"))
                self.LoadXml(path, null);
            else
                self.LoadJson(path, null);
            return self;
        }
        //public void Write(string path)
        //{
        //    if (path.EndsWith(".xml"))
        //        WriteXml(path);
        //    else
        //        WriteJson(path);
        //}
        //private void WriteXml(string path)
        //{

        //}
        //private void WriteJson(string path)
        //{
        //    var s = JsonSerializer.Serialize(this, new JsonSerializerOptions(JsonSerializerOptions.Default) { ReferenceHandler = ReferenceHandler.Preserve });
        //}
        public static TableMapping GetTableMapping(string path, string tableID)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            if (string.IsNullOrEmpty(tableID) || tableID == "0")
                return null;

            ExchangeConfiguration self = Load(path);
            return self[tableID];
        }

        internal static Export BuildDefaultExport<T>()
        {
            Export export = new Export();
            export.Body = new ExportBody(export);

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                var pType = prop.PropertyType;
                if (prop.CanRead && (pType.IsValueType || pType.IsEnum || pType.Name.EqualsIgnoreCase("system.string")))
                {
                    export.Body.Add(prop.Name, new ExportBodyColumnMapping(export) { PropertyName = prop.Name, ColumnName = "" });
                }
            }

            return export;
        }
        internal static Import BuildDefaultImport<T>(List<string> colNames)
        {
            return null;
        }

        #region load from xml
        private XlsFormat ConvertXlsFormat(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return XlsFormat.Auto;
            return (XlsFormat)Enum.Parse(typeof(XlsFormat), val, true);
        }
        private PropertyType ConvertPropertyType(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return PropertyType.Normal;
            return (PropertyType)Enum.Parse(typeof(PropertyType), val, true);
        }
        private void LoadXml(string path, string tableID)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            if (string.IsNullOrEmpty(tableID))
            {
                foreach (XmlNode xn in document.DocumentElement.ChildNodes)
                {
                    if ("#comment" == xn.Name)
                        continue;
                    TableMapping table = LoadTableMapping(xn);
                    Add(table.Id, table);
                }
            }
            else
            {
                XmlNode tableNode = document.SelectSingleNode(string.Format("//tableMapping[@id='{0}']", tableID));
                if (tableNode != null)
                    Add(tableID, LoadTableMapping(tableNode));
            }
        }

        private TableMapping LoadTableMapping(XmlNode node)
        {
            TableMapping table = new TableMapping();

            foreach (XmlAttribute xa in node.Attributes)
            {
                switch (xa.Name)
                {
                    case "id":
                        table.Id = xa.Value.Trim();
                        break;
                    case "clsType":
                        table.ClassType = xa.Value.Trim();
                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;

                if ("exports" == xn.Name)
                    table.Exports = LoadExports(xn);
                else if ("imports" == xn.Name)
                    table.Imports = LoadImports(xn);
            }
            return table;
        }

        private ExportCollection LoadExports(XmlNode node)
        {
            ExportCollection exports = new ExportCollection();

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;

                Export export = LoadExport(xn);
                exports.Add(export.Id, export);
            }

            return exports;
        }

        private Export LoadExport(XmlNode node)
        {
            Export export = new Export();

            foreach (XmlAttribute xa in node.Attributes)
            {
                switch (xa.Name)
                {
                    case "id":
                        export.Id = xa.Value.Trim();
                        break;
                    case "sheet":
                        export.Sheet = xa.Value.Trim();
                        break;
                    case "dateFormat":
                        export.DateFormat = xa.Value.Trim();
                        break;
                    case "numberFormat":
                        export.NumberFormat = xa.Value.Trim();
                        break;
                    case "xlsFormat":
                        export.XlsFormat = ConvertXlsFormat(xa.Value);
                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;

                if ("header" == xn.Name)
                {
                    export.Header = LoadExportHeader(xn, export);
                }
                else if ("body" == xn.Name)
                {
                    export.Body = LoadExportBody(xn, export);
                }
            }

            if (string.IsNullOrEmpty(export.Sheet))
                export.Sheet = "Sheet1";

            return export;
        }

        private ExportHeader LoadExportHeader(XmlNode node, Export export)
        {
            ExportHeader header = new ExportHeader(export);

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;
                ExportHeaderColumnMapping excolumn = LoadExportHeaderColumnMapping(xn, export);
                header.Add(excolumn.Location, excolumn);
            }

            return header;
        }

        private ExportHeaderColumnMapping LoadExportHeaderColumnMapping(XmlNode node, Export export)
        {
            ExportHeaderColumnMapping column = new ExportHeaderColumnMapping(export);

            foreach (XmlAttribute xa in node.Attributes)
            {
                switch (xa.Name)
                {
                    case "col":
                        column.ColumnName = xa.Value.Trim();
                        break;
                    case "row":
                        column.RowIndex = xa.Value.Trim().SafeToInt() - 1;
                        break;
                    case "prop":
                        column.PropertyName = xa.Value.Trim();
                        break;
                    case "propType":
                        column.PropertyType = ConvertPropertyType(xa.Value.Trim());
                        break;
                    case "offset":
                        column.Offset = xa.Value.Trim().SafeToBool();
                        break;
                    default:
                        break;
                }
            }
            return column;
        }

        private ExportBody LoadExportBody(XmlNode node, Export export)
        {
            ExportBody body = new ExportBody(export);

            foreach (XmlAttribute xa in node.Attributes)
            {
                switch (xa.Name)
                {
                    case "outputTitle":
                        body.OutPutTitle = xa.Value.Trim().SafeToBool();
                        break;
                    case "firstRowIndex":
                        body.FirstRowIndex = xa.Value.Trim().SafeToInt() - 1;
                        body.FirstRowIndex = body.FirstRowIndex < 0 ? 0 : body.FirstRowIndex;
                        break;
                    case "fillMode":
                        if (xa.Value == "copy")
                            body.RowMode = FillRowMode.Copy;
                        else if (xa.Value == "fill")
                            body.RowMode = FillRowMode.Fill;
                        else
                            body.RowMode = FillRowMode.New;
                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;

                ExportBodyColumnMapping excolumn = LoadExportBodyColumnMapping(xn, export);
                body.Add(excolumn.ColumnName, excolumn);
            }

            return body;
        }

        private ExportBodyColumnMapping LoadExportBodyColumnMapping(XmlNode node, Export export)
        {
            ExportBodyColumnMapping column = new ExportBodyColumnMapping(export);

            foreach (XmlAttribute xa in node.Attributes)
            {
                switch (xa.Name)
                {
                    case "col":
                        column.ColumnName = xa.Value.Trim();
                        break;
                    case "prop":
                        column.PropertyName = xa.Value.Trim();
                        break;
                    case "propType":
                        column.PropertyType = ConvertPropertyType(xa.Value.Trim());
                        break;
                    case "title":
                        column.Title = xa.Value.Trim();
                        break;
                    default:
                        break;
                }
            }
            return column;
        }

        private ImportCollection LoadImports(XmlNode node)
        {
            ImportCollection imports = new ImportCollection();

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;

                Import import = LoadImport(xn);
                imports.Add(import.Id, import);
            }

            return imports;
        }

        private Import LoadImport(XmlNode node)
        {
            Import import = new Import();

            foreach (XmlAttribute xa in node.Attributes)
            {
                switch (xa.Name)
                {
                    case "id":
                        import.Id = xa.Value.Trim();
                        break;
                    case "sheet":
                        import.Sheet = xa.Value.Trim();
                        break;
                    case "firstRowIndex":
                        import.FirstRowIndex = xa.Value.Trim().SafeToInt() - 1;
                        import.FirstRowIndex = import.FirstRowIndex < 0 ? 0 : import.FirstRowIndex;
                        break;
                    case "xlsFormat":
                        import.XlsFormat = ConvertXlsFormat(xa.Value);
                        break;
                    case "columnMapType":
                        import.ColumnMapType = ("dataheader".EqualsIgnoreCase(xa.Value) ? ColumnMapType.DataHeader : ColumnMapType.ExcelHeader);
                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;

                ImportColumnMapping imcolumn = LoadImportColumnMapping(xn, import);
                import.Add(imcolumn.ColumnName, imcolumn);
            }

            return import;
        }

        private ImportColumnMapping LoadImportColumnMapping(XmlNode node, Import import)
        {
            ImportColumnMapping column = new ImportColumnMapping(import);

            foreach (XmlAttribute xa in node.Attributes)
            {
                switch (xa.Name)
                {
                    case "col":
                        column.ColumnName = xa.Value.Trim();
                        break;
                    case "prop":
                        column.PropertyName = xa.Value.Trim();
                        break;
                    case "inherit":
                        column.Inherit = xa.Value.Trim().SafeToBool();
                        break;
                    case "from":
                        column.Direction = "left".Equals(xa.Value.Trim(), StringComparison.OrdinalIgnoreCase) ? InheritDirection.Left : InheritDirection.Up;
                        break;
                    default:
                        break;
                }
            }
            return column;
        }
        #endregion

        #region load from json
        private void LoadJson(string path, string tableID)
        {
            using (var stream = File.OpenRead(path))
            {
                var config = JsonNode.Parse(stream).AsObject();
                if (string.IsNullOrEmpty(tableID))
                {
                    foreach (JsonObject jObj in config["exchanges"].AsArray())
                    {
                        TableMapping table = LoadTableMapping(jObj);
                        Add(table.Id, table);
                    }
                }
                else
                {
                    foreach (JsonObject jObj in config["exchanges"].AsArray())
                    {
                        if (jObj["id"].GetValue<string>() == tableID)
                        {
                            TableMapping table = LoadTableMapping(jObj);
                            Add(table.Id, table);
                        }
                    }
                }
            }
        }
        private TableMapping LoadTableMapping(JsonObject jObj)
        {
            TableMapping table = new TableMapping();

            foreach (var kvp in jObj)
            {
                switch (kvp.Key)
                {
                    case "id":
                        table.Id = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "clsType":
                        table.ClassType = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "exports":
                        table.Exports = LoadExports(kvp.Value.AsArray());
                        break;
                    case "imports":
                        table.Imports = LoadImports(kvp.Value.AsArray());
                        break;
                    default:
                        break;
                }
            }
            return table;
        }

        private ExportCollection LoadExports(JsonArray jArray)
        {
            ExportCollection exports = new ExportCollection();

            foreach (JsonObject jObj in jArray)
            {
                Export export = LoadExport(jObj);
                exports.Add(export.Id, export);
            }

            return exports;
        }

        private Export LoadExport(JsonObject jObj)
        {
            Export export = new Export();

            foreach (var kvp in jObj)
            {
                switch (kvp.Key)
                {
                    case "id":
                        export.Id = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "sheet":
                        export.Sheet = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "dateFormat":
                        export.DateFormat = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "numberFormat":
                        export.NumberFormat = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "xlsFormat":
                        export.XlsFormat = ConvertXlsFormat(kvp.Value.GetValue<string>().Trim());
                        break;
                    case "header":
                        export.Header = LoadExportHeader(kvp.Value.AsObject(), export);
                        break;
                    case "body":
                        export.Body = LoadExportBody(kvp.Value.AsObject(), export);
                        break;
                    default:
                        break;
                }
            }

            if (string.IsNullOrEmpty(export.Sheet))
                export.Sheet = "Sheet1";

            return export;
        }

        private ExportHeader LoadExportHeader(JsonObject jObj, Export export)
        {
            ExportHeader header = new ExportHeader(export);

            foreach (JsonObject jItem in jObj["mappings"].AsArray())
            {
                ExportHeaderColumnMapping excolumn = LoadExportHeaderColumnMapping(jItem, export);
                header.Add(excolumn.Location, excolumn);
            }

            return header;
        }

        private ExportHeaderColumnMapping LoadExportHeaderColumnMapping(JsonObject jObj, Export export)
        {
            ExportHeaderColumnMapping column = new ExportHeaderColumnMapping(export);

            foreach (var kvp in jObj)
            {
                switch (kvp.Key)
                {
                    case "col":
                        column.ColumnName = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "row":
                        column.RowIndex = kvp.Value.GetValue<int>() - 1;
                        break;
                    case "prop":
                        column.PropertyName = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "propType":
                        column.PropertyType = ConvertPropertyType(kvp.Value.GetValue<string>().Trim());
                        break;
                    case "offset":
                        column.Offset = kvp.Value.GetValue<bool>();
                        break;
                    default:
                        break;
                }
            }

            return column;
        }

        private ExportBody LoadExportBody(JsonObject jObj, Export export)
        {
            ExportBody body = new ExportBody(export);

            foreach (var kvp in jObj)
            {
                switch (kvp.Key)
                {
                    case "outputTitle":
                        body.OutPutTitle = kvp.Value.GetValue<bool>();
                        break;
                    case "firstRowIndex":
                        body.FirstRowIndex = kvp.Value.GetValue<int>() - 1;
                        body.FirstRowIndex = body.FirstRowIndex < 0 ? 0 : body.FirstRowIndex;
                        break;
                    case "fillMode":
                        var val = kvp.Value.GetValue<string>().Trim();
                        if (val == "copy")
                            body.RowMode = FillRowMode.Copy;
                        else if (val == "fill")
                            body.RowMode = FillRowMode.Fill;
                        else
                            body.RowMode = FillRowMode.New;
                        break;
                    case "mappings":
                        foreach (JsonObject jItem in jObj["mappings"].AsArray())
                        {
                            ExportBodyColumnMapping excolumn = LoadExportBodyColumnMapping(jItem, export);
                            body.Add(excolumn.ColumnName, excolumn); 
                        }
                        break;
                    default:
                        break;
                }
            }

            return body;
        }

        private ExportBodyColumnMapping LoadExportBodyColumnMapping(JsonObject jObj, Export export)
        {
            ExportBodyColumnMapping column = new ExportBodyColumnMapping(export);

            foreach (var kvp in jObj)
            {
                switch (kvp.Key)
                {
                    case "col":
                        column.ColumnName = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "prop":
                        column.PropertyName = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "propType":
                        column.PropertyType = ConvertPropertyType(kvp.Value.GetValue<string>().Trim());
                        break;
                    case "title":
                        column.Title = kvp.Value.GetValue<string>().Trim();
                        break;
                    default:
                        break;
                }
            }

            return column;
        }

        private ImportCollection LoadImports(JsonArray jArray)
        {
            ImportCollection imports = new ImportCollection();

            foreach (JsonObject jObj in jArray)
            {
                Import import = LoadImport(jObj);
                imports.Add(import.Id, import);
            }

            return imports;
        }

        private Import LoadImport(JsonObject jObj)
        {
            Import import = new Import();

            foreach (var kvp in jObj)
            {
                switch (kvp.Key)
                {
                    case "id":
                        import.Id = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "sheet":
                        import.Sheet = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "firstRowIndex":
                        import.FirstRowIndex = kvp.Value.GetValue<int>() - 1;
                        import.FirstRowIndex = import.FirstRowIndex < 0 ? 0 : import.FirstRowIndex;
                        break;
                    case "xlsFormat":
                        import.XlsFormat = ConvertXlsFormat(kvp.Value.GetValue<string>().Trim());
                        break;
                    case "columnMapType":
                        import.ColumnMapType = ("dataheader".EqualsIgnoreCase(kvp.Value.GetValue<string>().Trim()) ? ColumnMapType.DataHeader : ColumnMapType.ExcelHeader);
                        break;
                    case "mappings":
                        foreach (JsonObject jItem in jObj["mappings"].AsArray())
                        {
                            ImportColumnMapping imcolumn = LoadImportColumnMapping(jItem, import);
                            import.Add(imcolumn.ColumnName, imcolumn);
                        }
                        break;
                    default:
                        break;
                }
            }

            return import;
        }

        private ImportColumnMapping LoadImportColumnMapping(JsonObject jObj, Import import)
        {
            ImportColumnMapping column = new ImportColumnMapping(import);

            foreach (var kvp in jObj)
            {
                switch (kvp.Key)
                {
                    case "col":
                        column.ColumnName = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "prop":
                        column.PropertyName = kvp.Value.GetValue<string>().Trim();
                        break;
                    case "inherit":
                        column.Inherit = kvp.Value.GetValue<bool>();
                        break;
                    case "from":
                        column.Direction = "left".Equals(kvp.Value.GetValue<string>().Trim(), StringComparison.OrdinalIgnoreCase) ? InheritDirection.Left : InheritDirection.Up;
                        break;
                    default:
                        break;
                }
            }

            return column;
        }
        #endregion
    }
}
