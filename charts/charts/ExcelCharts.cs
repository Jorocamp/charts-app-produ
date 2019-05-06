using DocumentFormat.OpenXml.Packaging;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml;
using A = DocumentFormat.OpenXml.Drawing;
using C = DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace charts
{
    class ExcelCharts
    {


        public static void CreatePieChart(DrawingsPart part, string formulaCat, string formulaVal, int row, bool newe, string title)
        {
            ChartPart chartPart1 = part.AddNewPart<ChartPart>();
            GenerateChartPartContentPie(chartPart1, formulaCat, formulaVal, title);

            GeneratePartContentPie(part, chartPart1, row, newe);

        }

        public static void CreateLineChart(DrawingsPart part, string formulaCat, string formulaVal, int row, bool newe, string title)
        {
            ChartPart chartPart1 = part.AddNewPart<ChartPart>();
            GenerateChartPartContentLine(chartPart1, formulaCat, formulaVal, title);

            GeneratePartContentPie(part, chartPart1, row, newe);

        }

        private static void GenerateChartPartContentLine(ChartPart part, string formulaCat, string formulaVal, string title)
        {
            C.ChartSpace chartSpace1 = new C.ChartSpace();
            chartSpace1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
            chartSpace1.AddNamespaceDeclaration("c", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            chartSpace1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            chartSpace1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            chartSpace1.AddNamespaceDeclaration("mv", "urn:schemas-microsoft-com:mac:vml");
            chartSpace1.AddNamespaceDeclaration("c14", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");

            C.Chart chart1 = new C.Chart();

            C.Title title1 = new C.Title();

            C.ChartText chartText1 = new C.ChartText();

            C.RichText richText1 = new C.RichText();
            A.BodyProperties bodyProperties3 = new A.BodyProperties();
            A.ListStyle listStyle3 = new A.ListStyle();

            A.Paragraph paragraph3 = new A.Paragraph();

            A.ParagraphProperties paragraphProperties3 = new A.ParagraphProperties() { Level = 0 };
            A.DefaultRunProperties defaultRunProperties3 = new A.DefaultRunProperties() { Bold = false };

            paragraphProperties3.Append(defaultRunProperties3);

            A.Run run1 = new A.Run();
            A.Text text1 = new A.Text();
            text1.Text = title;

            run1.Append(text1);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run1);

            richText1.Append(bodyProperties3);
            richText1.Append(listStyle3);
            richText1.Append(paragraph3);

            chartText1.Append(richText1);
            C.Overlay overlay1 = new C.Overlay() { Val = false };

            title1.Append(chartText1);
            title1.Append(overlay1);

            C.PlotArea plotArea1 = new C.PlotArea();
            C.Layout layout1 = new C.Layout();

            C.LineChart lineChart1 = new C.LineChart();
            C.VaryColors varyColors1 = new C.VaryColors() { Val = false };


            C.LineChartSeries lineChartSeries1 = new C.LineChartSeries();
            C.Index index1 = new C.Index() { Val = (UInt32Value)0U };
            C.Order order1 = new C.Order() { Val = (UInt32Value)0U };


            C.ChartShapeProperties chartShapeProperties1 = new C.ChartShapeProperties();

            A.Outline outline1 = new A.Outline() { Width = 19050, CompoundLineType = A.CompoundLineValues.Single };

            A.SolidFill solidFill1 = new A.SolidFill();
            A.RgbColorModelHex rgbColorModelHex1 = new A.RgbColorModelHex() { Val = "3366CC" };

            solidFill1.Append(rgbColorModelHex1);

            outline1.Append(solidFill1);

            chartShapeProperties1.Append(outline1);

            C.Marker marker1 = new C.Marker();
            C.Symbol symbol1 = new C.Symbol() { Val = C.MarkerStyleValues.None };

            marker1.Append(symbol1);

            C.CategoryAxisData categoryAxisData1 = new C.CategoryAxisData();

            C.StringReference stringReference1 = new C.StringReference();
            C.Formula formula1 = new C.Formula();
            formula1.Text = formulaCat;

            stringReference1.Append(formula1);

            categoryAxisData1.Append(stringReference1);

            C.Values values1 = new C.Values();

            C.NumberReference numberReference1 = new C.NumberReference();
            C.Formula formula2 = new C.Formula();
            formula2.Text = formulaVal;

            numberReference1.Append(formula2);

            values1.Append(numberReference1);
            C.Smooth smooth1 = new C.Smooth() { Val = false };

            lineChartSeries1.Append(index1);
            lineChartSeries1.Append(order1);
            lineChartSeries1.Append(chartShapeProperties1);
            lineChartSeries1.Append(marker1);
            lineChartSeries1.Append(categoryAxisData1);
            lineChartSeries1.Append(values1);
            lineChartSeries1.Append(smooth1);
            C.AxisId axisId1 = new C.AxisId() { Val = (UInt32Value)1923141117U };
            C.AxisId axisId2 = new C.AxisId() { Val = (UInt32Value)2022561148U };

            lineChart1.Append(varyColors1);
            lineChart1.Append(lineChartSeries1);
            lineChart1.Append(axisId1);
            lineChart1.Append(axisId2);

            C.CategoryAxis categoryAxis1 = new C.CategoryAxis();
            C.AxisId axisId3 = new C.AxisId() { Val = (UInt32Value)1923141117U };


            C.Scaling scaling1 = new C.Scaling();
            C.Orientation orientation1 = new C.Orientation() { Val = C.OrientationValues.MinMax };

            scaling1.Append(orientation1);
            C.Delete delete1 = new C.Delete() { Val = false };
            C.AxisPosition axisPosition1 = new C.AxisPosition() { Val = C.AxisPositionValues.Bottom };

            C.TextProperties textProperties1 = new C.TextProperties();
            A.BodyProperties bodyProperties1 = new A.BodyProperties();
            A.ListStyle listStyle1 = new A.ListStyle();

            A.Paragraph paragraph1 = new A.Paragraph();

            A.ParagraphProperties paragraphProperties1 = new A.ParagraphProperties() { Level = 0 };
            A.DefaultRunProperties defaultRunProperties1 = new A.DefaultRunProperties() { Bold = false };

            paragraphProperties1.Append(defaultRunProperties1);

            paragraph1.Append(paragraphProperties1);

            textProperties1.Append(bodyProperties1);
            textProperties1.Append(listStyle1);
            textProperties1.Append(paragraph1);
            C.CrossingAxis crossingAxis1 = new C.CrossingAxis() { Val = (UInt32Value)2022561148U };

            categoryAxis1.Append(axisId3);
            categoryAxis1.Append(scaling1);
            categoryAxis1.Append(delete1);
            categoryAxis1.Append(axisPosition1);
            categoryAxis1.Append(textProperties1);
            categoryAxis1.Append(crossingAxis1);


            C.ValueAxis valueAxis1 = new C.ValueAxis();
            C.AxisId axisId4 = new C.AxisId() { Val = (UInt32Value)2022561148U };

            C.Scaling scaling2 = new C.Scaling();
            C.Orientation orientation2 = new C.Orientation() { Val = C.OrientationValues.MinMax };

            scaling2.Append(orientation2);
            C.Delete delete2 = new C.Delete() { Val = false };
            C.AxisPosition axisPosition2 = new C.AxisPosition() { Val = C.AxisPositionValues.Left };

            C.MajorGridlines majorGridlines1 = new C.MajorGridlines();

            C.ChartShapeProperties chartShapeProperties2 = new C.ChartShapeProperties();

            A.Outline outline2 = new A.Outline();

            A.SolidFill solidFill2 = new A.SolidFill();
            A.RgbColorModelHex rgbColorModelHex2 = new A.RgbColorModelHex() { Val = "B7B7B7" };

            solidFill2.Append(rgbColorModelHex2);

            outline2.Append(solidFill2);

            chartShapeProperties2.Append(outline2);

            majorGridlines1.Append(chartShapeProperties2);
            C.NumberingFormat numberingFormat1 = new C.NumberingFormat() { FormatCode = "General", SourceLinked = true };
            C.TickLabelPosition tickLabelPosition1 = new C.TickLabelPosition() { Val = C.TickLabelPositionValues.NextTo };

            C.ChartShapeProperties chartShapeProperties3 = new C.ChartShapeProperties();

            A.Outline outline3 = new A.Outline() { Width = 47625 };
            A.NoFill noFill1 = new A.NoFill();

            outline3.Append(noFill1);

            chartShapeProperties3.Append(outline3);

            C.TextProperties textProperties2 = new C.TextProperties();
            A.BodyProperties bodyProperties2 = new A.BodyProperties();
            A.ListStyle listStyle2 = new A.ListStyle();

            A.Paragraph paragraph2 = new A.Paragraph();

            A.ParagraphProperties paragraphProperties2 = new A.ParagraphProperties() { Level = 0 };
            A.DefaultRunProperties defaultRunProperties2 = new A.DefaultRunProperties() { Bold = false };

            paragraphProperties2.Append(defaultRunProperties2);

            paragraph2.Append(paragraphProperties2);

            textProperties2.Append(bodyProperties2);
            textProperties2.Append(listStyle2);
            textProperties2.Append(paragraph2);
            C.CrossingAxis crossingAxis2 = new C.CrossingAxis() { Val = (UInt32Value)1923141117U };

            valueAxis1.Append(axisId4);
            valueAxis1.Append(scaling2);
            valueAxis1.Append(delete2);
            valueAxis1.Append(axisPosition2);
            valueAxis1.Append(majorGridlines1);
            valueAxis1.Append(numberingFormat1);
            valueAxis1.Append(tickLabelPosition1);
            valueAxis1.Append(chartShapeProperties3);
            valueAxis1.Append(textProperties2);
            valueAxis1.Append(crossingAxis2);

            plotArea1.Append(layout1);
            plotArea1.Append(lineChart1);
            plotArea1.Append(categoryAxis1);
            plotArea1.Append(valueAxis1);

            C.Legend legend1 = new C.Legend();
            C.LegendPosition legendPosition1 = new C.LegendPosition() { Val = C.LegendPositionValues.Right };
            C.Overlay overlay2 = new C.Overlay() { Val = false };

            legend1.Append(legendPosition1);
            legend1.Append(overlay2);
            C.PlotVisibleOnly plotVisibleOnly1 = new C.PlotVisibleOnly() { Val = true };

            chart1.Append(title1);
            chart1.Append(plotArea1);
            chart1.Append(legend1);
            chart1.Append(plotVisibleOnly1);

            chartSpace1.Append(chart1);

            part.ChartSpace = chartSpace1;






            



        }

        private static void GenerateChartPartContentPie(ChartPart chartPart1, string formulaCat, string formulaVal, string title)
        {

            C.ChartSpace chartSpace1 = new C.ChartSpace();
            chartSpace1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
            chartSpace1.AddNamespaceDeclaration("c", "http://schemas.openxmlformats.org/drawingml/2006/chart");
            chartSpace1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            chartSpace1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            chartSpace1.AddNamespaceDeclaration("mv", "urn:schemas-microsoft-com:mac:vml");
            chartSpace1.AddNamespaceDeclaration("c14", "http://schemas.microsoft.com/office/drawing/2007/8/2/chart");



            C.Chart chart1 = new C.Chart();

            C.Title title1 = new C.Title();

            C.ChartText chartText1 = new C.ChartText();

            C.RichText richText1 = new C.RichText();
            A.BodyProperties bodyProperties1 = new A.BodyProperties();
            A.ListStyle listStyle1 = new A.ListStyle();

            A.Paragraph paragraph1 = new A.Paragraph();

            A.ParagraphProperties paragraphProperties1 = new A.ParagraphProperties() { Level = 0 };
            A.DefaultRunProperties defaultRunProperties1 = new A.DefaultRunProperties() { Bold = false };

            paragraphProperties1.Append(defaultRunProperties1);

            A.Run run1 = new A.Run();
            A.Text text1 = new A.Text();
            text1.Text = title;

            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            richText1.Append(bodyProperties1);
            richText1.Append(listStyle1);
            richText1.Append(paragraph1);

            chartText1.Append(richText1);
            C.Overlay overlay1 = new C.Overlay() { Val = false };

            title1.Append(chartText1);
            title1.Append(overlay1);



            C.PlotArea plotArea1 = new C.PlotArea();
            C.Layout layout1 = new C.Layout();


            C.PieChart pieChart1 = new C.PieChart();
            C.VaryColors varyColors1 = new C.VaryColors() { Val = true };

            C.PieChartSeries pieChartSeries1 = new C.PieChartSeries();
            C.Index index1 = new C.Index() { Val = (UInt32Value)0U };
            C.Order order1 = new C.Order() { Val = (UInt32Value)0U };

            C.DataLabels dataLabels1 = new C.DataLabels();
            C.ShowLegendKey showLegendKey1 = new C.ShowLegendKey() { Val = false };
            C.ShowValue showValue1 = new C.ShowValue() { Val = false };
            C.ShowCategoryName showCategoryName1 = new C.ShowCategoryName() { Val = false };
            C.ShowSeriesName showSeriesName1 = new C.ShowSeriesName() { Val = false };
            C.ShowPercent showPercent1 = new C.ShowPercent() { Val = true };
            C.ShowBubbleSize showBubbleSize1 = new C.ShowBubbleSize() { Val = false };
            C.ShowLeaderLines showLeaderLines1 = new C.ShowLeaderLines() { Val = true };

            dataLabels1.Append(showLegendKey1);
            dataLabels1.Append(showValue1);
            dataLabels1.Append(showCategoryName1);
            dataLabels1.Append(showSeriesName1);
            dataLabels1.Append(showPercent1);
            dataLabels1.Append(showBubbleSize1);
            dataLabels1.Append(showLeaderLines1);

            C.CategoryAxisData categoryAxisData1 = new C.CategoryAxisData();

            C.StringReference stringReference1 = new C.StringReference();
            C.Formula formula1 = new C.Formula();
            formula1.Text = formulaCat;

            stringReference1.Append(formula1);

            categoryAxisData1.Append(stringReference1);

            C.Values values1 = new C.Values();

            C.NumberReference numberReference1 = new C.NumberReference();
            C.Formula formula2 = new C.Formula();
            formula2.Text = formulaVal;

            numberReference1.Append(formula2);

            values1.Append(numberReference1);

            pieChartSeries1.Append(index1);
            pieChartSeries1.Append(order1);
            pieChartSeries1.Append(dataLabels1);
            pieChartSeries1.Append(categoryAxisData1);
            pieChartSeries1.Append(values1);

            C.DataLabels dataLabels2 = new C.DataLabels();
            C.ShowLegendKey showLegendKey2 = new C.ShowLegendKey() { Val = false };
            C.ShowValue showValue2 = new C.ShowValue() { Val = false };
            C.ShowCategoryName showCategoryName2 = new C.ShowCategoryName() { Val = false };
            C.ShowSeriesName showSeriesName2 = new C.ShowSeriesName() { Val = false };
            C.ShowPercent showPercent2 = new C.ShowPercent() { Val = true };
            C.ShowBubbleSize showBubbleSize2 = new C.ShowBubbleSize() { Val = false };

            dataLabels2.Append(showLegendKey2);
            dataLabels2.Append(showValue2);
            dataLabels2.Append(showCategoryName2);
            dataLabels2.Append(showSeriesName2);
            dataLabels2.Append(showPercent2);
            dataLabels2.Append(showBubbleSize2);
            C.FirstSliceAngle firstSliceAngle1 = new C.FirstSliceAngle() { Val = (UInt16Value)0U };

            pieChart1.Append(varyColors1);
            pieChart1.Append(pieChartSeries1);
            pieChart1.Append(dataLabels2);
            pieChart1.Append(firstSliceAngle1);

            C.ShapeProperties shapeProperties1 = new C.ShapeProperties();

            A.SolidFill solidFill4 = new A.SolidFill();
            A.RgbColorModelHex rgbColorModelHex4 = new A.RgbColorModelHex() { Val = "FFFFFF" };

            solidFill4.Append(rgbColorModelHex4);

            shapeProperties1.Append(solidFill4);

            plotArea1.Append(layout1);
            plotArea1.Append(pieChart1);
            plotArea1.Append(shapeProperties1);

            C.Legend legend1 = new C.Legend();
            C.LegendPosition legendPosition1 = new C.LegendPosition() { Val = C.LegendPositionValues.Right };
            C.Overlay overlay2 = new C.Overlay() { Val = false };

            legend1.Append(legendPosition1);
            legend1.Append(overlay2);
            C.PlotVisibleOnly plotVisibleOnly1 = new C.PlotVisibleOnly() { Val = true };

            chart1.Append(title1);
            chart1.Append(plotArea1);
            chart1.Append(legend1);
            chart1.Append(plotVisibleOnly1);

            chartSpace1.Append(chart1);

            chartPart1.ChartSpace = chartSpace1;


        }

        private static void GeneratePartContentPie(DrawingsPart part, ChartPart chartPart1, int row, bool newe)
        {


            Xdr.WorksheetDrawing worksheetDrawing1 = new Xdr.WorksheetDrawing();

            if (newe)
            {

                worksheetDrawing1.AddNamespaceDeclaration("xdr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
                worksheetDrawing1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                worksheetDrawing1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                worksheetDrawing1.AddNamespaceDeclaration("c", "http://schemas.openxmlformats.org/drawingml/2006/chart");
                worksheetDrawing1.AddNamespaceDeclaration("cx", "http://schemas.microsoft.com/office/drawing/2014/chartex");
                worksheetDrawing1.AddNamespaceDeclaration("cx1", "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex");
                worksheetDrawing1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
                worksheetDrawing1.AddNamespaceDeclaration("dgm", "http://schemas.openxmlformats.org/drawingml/2006/diagram");
            }

            Xdr.OneCellAnchor oneCellAnchor1 = new Xdr.OneCellAnchor();

            Xdr.FromMarker fromMarker1 = new Xdr.FromMarker();
            Xdr.ColumnId columnId1 = new Xdr.ColumnId();
            columnId1.Text = "0";
            Xdr.ColumnOffset columnOffset1 = new Xdr.ColumnOffset();
            columnOffset1.Text = "0";
            Xdr.RowId rowId1 = new Xdr.RowId();
            rowId1.Text = row.ToString();
            Xdr.RowOffset rowOffset1 = new Xdr.RowOffset();
            rowOffset1.Text = "0";

            fromMarker1.Append(columnId1);
            fromMarker1.Append(columnOffset1);
            fromMarker1.Append(rowId1);
            fromMarker1.Append(rowOffset1);

            Xdr.Extent extent1 = new Xdr.Extent() { Cx = 5743575L, Cy = 2667000L };




            Xdr.GraphicFrame graphicFrame1 = new Xdr.GraphicFrame();

            Xdr.NonVisualGraphicFrameProperties nonVisualGraphicFrameProperties1 = new Xdr.NonVisualGraphicFrameProperties();
            Xdr.NonVisualDrawingProperties nonVisualDrawingProperties1 = new Xdr.NonVisualDrawingProperties() { Id = (UInt32Value)(uint)row, Name = "Chart "+row.ToString() };
            Xdr.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties1 = new Xdr.NonVisualGraphicFrameDrawingProperties();

            nonVisualGraphicFrameProperties1.Append(nonVisualDrawingProperties1);
            nonVisualGraphicFrameProperties1.Append(nonVisualGraphicFrameDrawingProperties1);

            Xdr.Transform transform1 = new Xdr.Transform();
            A.Offset offset1 = new A.Offset() { X = 0L, Y = 0L };
            A.Extents extents1 = new A.Extents() { Cx = 0L, Cy = 0L };

            transform1.Append(offset1);
            transform1.Append(extents1);

            A.Graphic graphic1 = new A.Graphic();

            A.GraphicData graphicData1 = new A.GraphicData() { Uri = "http://schemas.openxmlformats.org/drawingml/2006/chart" };
            C.ChartReference chartReference1 = new C.ChartReference() { Id = part.GetIdOfPart(chartPart1) };

            graphicData1.Append(chartReference1);

            graphic1.Append(graphicData1);

            graphicFrame1.Append(nonVisualGraphicFrameProperties1);
            graphicFrame1.Append(transform1);
            graphicFrame1.Append(graphic1);
            Xdr.ClientData clientData1 = new Xdr.ClientData() { LockWithSheet = false };

            oneCellAnchor1.Append(fromMarker1);
            oneCellAnchor1.Append(extent1);
            oneCellAnchor1.Append(graphicFrame1);
            oneCellAnchor1.Append(clientData1);

            if (newe)
            {

                worksheetDrawing1.Append(oneCellAnchor1);

                part.WorksheetDrawing = worksheetDrawing1;
            }
            else
            {
                part.WorksheetDrawing.Append(oneCellAnchor1);
            }


        }






    }
    }
