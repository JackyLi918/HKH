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

                if ("basic" == xn.Name)
                {
                    export.BasicMapping = LoadBasicExport(xn);
                }
                else if ("details" == xn.Name)
                {
                    export.DetailsMapping = LoadDetailsExport(xn);
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

        private BasicExport LoadBasicExport(XmlNode node)
        {
            BasicExport basicExport = new BasicExport();

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;
                BasicExportColumnMapping excolumn = LoadBasicExportColumnMapping(xn);
                basicExport.Add(excolumn.Location, excolumn);
            }

            return basicExport;
        }

        private BasicExportColumnMapping LoadBasicExportColumnMapping(XmlNode node)
        {
            BasicExportColumnMapping column = new BasicExportColumnMapping();

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

        private DetailsExport LoadDetailsExport(XmlNode node)
        {
            DetailsExport detailsExport = new DetailsExport();

            foreach (XmlAttribute xa in node.Attributes)
            {
                switch (xa.Name)
                {
                    case "outputTitle":
                        detailsExport.OutPutTitle = xa.Value.Trim().SafeToBool();
                        break;
                    case "firstRowIndex":
                        detailsExport.FirstRowIndex = xa.Value.Trim().SafeToInt() - 1;
						detailsExport.FirstRowIndex = detailsExport.FirstRowIndex < 0 ? 0 : detailsExport.FirstRowIndex;
                        break;
                    case "pageSize":
                        detailsExport.PageSize = xa.Value.Trim().SafeToInt();
                        break;
					case "fillMode":
                        if (xa.Value == "copy")
                            detailsExport.RowMode = FillRowMode.Copy;
                        else if (xa.Value == "fill")
                            detailsExport.RowMode = FillRowMode.Fill;
                        else
                            detailsExport.RowMode = FillRowMode.New;
                        break;
                    default:
                        break;
                }
            }

            foreach (XmlNode xn in node.ChildNodes)
            {
                if ("#comment" == xn.Name)
                    continue;

                DetailsExportColumnMapping excolumn = LoadDetailsExportColumnMapping(xn);
                detailsExport.Add(excolumn.ColumnName, excolumn);
            }

            detailsExport.MaxColumnIndex = detailsExport.Max(c => c.Value.ColumnIndex);

            return detailsExport;
        }

        private DetailsExportColumnMapping LoadDetailsExportColumnMapping(XmlNode node)
        {
            DetailsExportColumnMapping column = new DetailsExportColumnMapping();

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
