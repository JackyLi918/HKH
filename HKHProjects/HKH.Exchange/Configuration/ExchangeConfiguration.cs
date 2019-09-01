using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HKH.Exchange.Configuration
{
    public class ExchangeConfiguration : Dictionary<string, TableMapping>
    {
        private ExchangeConfiguration()
        {
        }

        public static ExchangeConfiguration GetInstance(string path)
        {
            ExchangeConfiguration self = new ExchangeConfiguration();
            self.LoadConfiguration(path, null);
            return self;
        }

        public static TableMapping GetTableMapping(string path, string tableID)
        {
            ExchangeConfiguration self = new ExchangeConfiguration();
            self.LoadConfiguration(path, tableID);

            return self[tableID];
        }

        private void LoadConfiguration(string path, string tableID)
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
                    case "sheet":
                        table.Sheet = xa.Value.Trim();
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
                    export.Header = LoadExportHeader(xn);
                }
                else if ("body" == xn.Name)
                {
                    export.Body = LoadExportBody(xn);
                }
            }

            return export;
        }

        private XlsFormat ConvertXlsFormat(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return XlsFormat.Auto;
            return (XlsFormat)Enum.Parse(typeof(XlsFormat), val, true);
        }

        private ExportHeader LoadExportHeader(XmlNode node)
        {
            ExportHeader header = new ExportHeader();

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;
                ExportHeaderColumnMapping excolumn = LoadExportHeaderColumnMapping(xn);
                header.Add(excolumn.Location, excolumn);
            }

            return header;
        }

        private ExportHeaderColumnMapping LoadExportHeaderColumnMapping(XmlNode node)
        {
            ExportHeaderColumnMapping column = new ExportHeaderColumnMapping();

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
                        column.PropertyType = "normal".Equals(xa.Value.Trim(), StringComparison.OrdinalIgnoreCase) ? PropertyType.Normal : PropertyType.Expression;
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

        private ExportBody LoadExportBody(XmlNode node)
        {
            ExportBody body = new ExportBody();

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

                ExportBodyColumnMapping excolumn = LoadExportBodyColumnMapping(xn);
                body.Add(excolumn.ColumnName, excolumn);
            }

            body.MaxColumnIndex = body.Max(c => c.Value.ColumnIndex);

            return body;
        }

        private ExportBodyColumnMapping LoadExportBodyColumnMapping(XmlNode node)
        {
            ExportBodyColumnMapping column = new ExportBodyColumnMapping();

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
                        column.PropertyType = "normal".Equals(xa.Value.Trim(), StringComparison.OrdinalIgnoreCase) ? PropertyType.Normal : PropertyType.Expression;
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
					case "firstRowIndex":
						import.FirstRowIndex = xa.Value.Trim().SafeToInt() - 1;
						import.FirstRowIndex = import.FirstRowIndex < 0 ? 0 : import.FirstRowIndex;
                        break;
                    case "xlsFormat":
                        import.XlsFormat = ConvertXlsFormat(xa.Value);
                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;

                ImportColumnMapping imcolumn = LoadImportColumnMapping(xn);
                import.Add(imcolumn.ColumnName, imcolumn);
            }

            return import;
        }

        private ImportColumnMapping LoadImportColumnMapping(XmlNode node)
        {
            ImportColumnMapping column = new ImportColumnMapping();

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
    }
}
