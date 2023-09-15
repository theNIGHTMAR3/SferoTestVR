using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public static class SVG
{

    const int USize = 8;
    const int USizePixels = 90;    
    public static void CreateSVG(string svgName,List<TrackRecord> records,List<Vector2> points, int length, int width)
    {
        XNamespace xNamespace = "http://www.w3.org/2000/svg";
        XElement svg = CreateSVGRoot(xNamespace);

        DrawRoom(svg,xNamespace, length,width);
        DrawPathTemplate(svg, xNamespace, points);

        foreach(TrackRecord record in records)
        {
            DrawArrow(svg,xNamespace, record);
        }

        
        string filePath = Path.Combine(TrackMaster.folderPath, svgName);

        using (StreamWriter writer = new StreamWriter(File.Create(filePath)))
        {            
            svg.Save(writer); 
            
            writer.Close();
        }
    }


    private static XElement CreateSVGRoot(XNamespace xNamespace)
    {                
        
        XElement svg = new XElement(xNamespace + "svg");


        svg.Add(new XAttribute("version", "1.0"));
        //svg.Add(new XAttribute("xmlns", "http://www.w3.org/2000/svg"));
        svg.Add(new XAttribute("width", 5 * USizePixels + "pt"));
        svg.Add(new XAttribute("height", 8 * USizePixels + "pt"));
        svg.Add(new XAttribute("viewBox", -USize * 5/2 +" " + -USize * 8 + " " + USize*5 +" " + USize*8));
        svg.Add(new XAttribute("preserveAspectRatio", "xMidYMid meet"));
        return svg;
    }


    private static XElement CreateLine(XNamespace xNamespace, double x1, double y1, double x2, double y2,string color="0,0,0", string width = "0.125")
    {
        XElement line = new XElement(xNamespace + "line");
        line.Add(new XAttribute("x1", x1));
        line.Add(new XAttribute("y1", y1));
        line.Add(new XAttribute("x2", x2));
        line.Add(new XAttribute("y2", y2));        
        line.Add(new XAttribute("style", "stroke:rgb("+color+");stroke-width:"+width));
        return line;
    }

    private static void DrawRoom(XElement svg, XNamespace xNamespace, int length, int width)
    {
        length *= USize;
        width *= USize;

        //left part
        //drawn from the bottom
        svg.Add(CreateLine(xNamespace,-0.5,0, -0.5, -0.5));
        svg.Add(CreateLine(xNamespace,-0.5,-0.5, -width/2, -0.5));
        svg.Add(CreateLine(xNamespace ,- width / 2, -0.5, -width / 2, -(length - 0.5)));
        svg.Add(CreateLine(xNamespace, -width / 2, -(length - 0.5), -0.5, -(length - 0.5)));
        svg.Add(CreateLine(xNamespace, -0.5, -(length - 0.5), -0.5, -length));

        //right part
        svg.Add(CreateLine(xNamespace, 0.5, 0, 0.5, -0.5));
        svg.Add(CreateLine(xNamespace, 0.5, -0.5, width / 2, -0.5));
        svg.Add(CreateLine(xNamespace, width / 2, -0.5, width / 2, -(length - 0.5)));
        svg.Add(CreateLine(xNamespace, width / 2, -(length - 0.5), 0.5, -(length - 0.5)));
        svg.Add(CreateLine(xNamespace, 0.5, -(length - 0.5), 0.5, -length));

    }

    private static void DrawPathTemplate(XElement svg, XNamespace xNamespace, List<Vector2> points)
    {
        for(int i = 0; i < points.Count-1; i++)
        {
            svg.Add(CreateLine(xNamespace, points[i].x, -points[i].y, points[i+1].x, -points[i+1].y, "0,0,255"));
        }
    }




    /// <summary>
    ///       end
    ///        .
    ///       /|\
    /// left / | \ right
    ///        |
    ///        |
    ///      start
    /// </summary>        
    private static void DrawArrow(XElement svg,XNamespace xNamespace,TrackRecord record)
    {

        Vector2 startArrow, endArrow;
        Vector2 leftEnd, rightEnd;

        float angle = 30f/180*Mathf.PI;

        startArrow = record.pos;
        endArrow = record.vel/10;
        leftEnd.x = endArrow.x * Mathf.Cos(angle)  - endArrow.y * Mathf.Sin(angle);
        leftEnd.y = endArrow.x * Mathf.Sin(angle)  + endArrow.y * Mathf.Cos(angle);

        rightEnd.x = endArrow.x * Mathf.Cos(-angle) - endArrow.y * Mathf.Sin(-angle);
        rightEnd.y = endArrow.x * Mathf.Sin(-angle) + endArrow.y * Mathf.Cos(-angle);

        rightEnd *= 0.7f;
        leftEnd *= 0.7f;

        rightEnd += record.pos;
        leftEnd += record.pos;
        endArrow += record.pos;
        
        svg.Add(CreateLine(xNamespace, startArrow.x, -startArrow.y,endArrow.x, -endArrow.y,"255,0,0"));
        svg.Add(CreateLine(xNamespace, rightEnd.x, -rightEnd.y,endArrow.x,- endArrow.y, "255,0,0"));
        svg.Add(CreateLine(xNamespace, leftEnd.x, -leftEnd.y,endArrow.x, -endArrow.y, "255,0,0"));        
    }
}
