# (OCP) Open/Closed Principle
Entita by m�la b�t otev�en� pro roz���en�, ale uzav�en� pro �prvy. "Otev��t pro roz���en�" znamen�, �e mus�me navrhnout n� modul/t��du tak, aby nov� funkce mohly b�t p�id�ny pouze tehdy, kdy� jsou vygenerov�ny nov� po�adavky. "Uzav�eno pro �pravy" znamen�, �e jsme ji� vyvinuli t��du a pro�la jednotkov�m testov�n�m. Pak bychom jej nem�li m�nit, dokud nenajdeme chyby. Jak se ��k�, t��da by m�la b�t otev�en� pro roz���en�; m��eme pou��t d�di�nost.

## P��klad 1
```
public class Rectangle{
   public double Height {get;set;}
   public double Wight {get;set; }
}
```

Na�e aplikace pot�ebuje vypo��tat celkovou plochu kolekce obd�ln�k�. Proto�e jsme se ji� nau�ili Princip jednotn� odpov�dnosti (SRP), nemus�me do obd�ln�ku vkl�dat k�d v�po�tu celkov� plochy. Tak�e zde jsem vytvo�il dal�� t��du pro v�po�et plochy.

```
public class AreaCalculator {
   public double TotalArea(Rectangle[] arrRectangles)
   {
      double area;
      foreach(var objRectangle in arrRectangles)
      {
         area += objRectangle.Height * objRectangle.Width;
      }
      return area;
   }
}
```

M��eme ale na�i aplikaci roz���it tak, aby dok�zala vypo��tat plochu nejen obd�ln�k�, ale i kruh�? Nyn� m�me probl�m s v�po�tem plochy, proto�e zp�sob v�po�tu plochy kruhu je jin�. M��eme zm�nit metodu TotalArea tak, aby akceptovala pole objekt� jako argument.

```
public class Rectangle{
   public double Height {get;set;}
   public double Wight {get;set; }
}

public class Circle{
   public double Radius {get;set;}
}

public class AreaCalculator
{
   public double TotalArea(object[] arrObjects)
   {
      double area = 0;
      Rectangle objRectangle;
      Circle objCircle;
      foreach(var obj in arrObjects)
      {
         if(obj is Rectangle)
         {
            area += obj.Height * obj.Width;
         }
         else
         {
            objCircle = (Circle)obj;
            area += objCircle.Radius * objCircle.Radius * Math.PI;
         }
      }
      return area;
   }
}
```

### Probl�m
M��eme p�idat troj�heln�k a vypo��tat jeho plochu p�id�n�m dal��ho bloku "if" v metod� TotalArea AreaCalculator. Ale poka�d�, kdy� zavedeme nov� tvar, mus�me zm�nit metodu TotalArea. T��da AreaCalculator tedy nen� uzav�ena pro �pravy.

### �e�en�
Obecn� to m��eme ud�lat odkazem na abstrakce pro z�vislosti, jako jsou rozhran� nebo abstraktn� t��dy, sp�e ne� pomoc� konkr�tn�ch t��d.

```
public abstract class Shape
{
   public abstract double Area();
}

public class Rectangle: Shape
{
   public double Height {get;set;}
   public double Width {get;set;}
   public override double Area()
   {
      return Height * Width;
   }
}
public class Circle: Shape
{
   public double Radius {get;set;}
   public override double Area()
   {
      return Radius * Radius * Math.PI;
   }
}

public class AreaCalculator
{
   public double TotalArea(Shape[] arrShapes)
   {
      double area=0;
      foreach(var objShape in arrShapes)
      {
         area += objShape.Area();
      }
      return area;
   }
}
```

Nyn� n� k�d n�sleduje jak SRP, tak OCP. Kdykoli zavedete nov� tvar odvozen�m z abstraktn� t��dy �Shape�, nemus�te t��du �AreaCalculator� m�nit.

Nebo to m��eme ud�lat pomoc� rozhran�. Z:

```
public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
}

public class AreaCalculator
{
    public double CalculateArea(Rectangle rectangle)
    {
        return rectangle.Width * rectangle.Height;
    }
}

```

Na:

```
public interface IShape
{
    double CalculateArea();
}

public class Rectangle : IShape
{
    // implementation
}

public class Circle : IShape
{
    // implementation
}
```

## P��klad 2
Uva�ujme sc�n��, kdy slu�ba pro export soubor� zpo��tku podporuje export dat do soubor� CSV.

```
public class FileExporter
{
    public void ExportToCsv(string filePath, DataTable data)
    {
        // Code to export data to a CSV file.
    }
}
```

### Probl�m
V tomto p��kladu t��da FileExporter p��mo implementuje funkce pro export dat do soubor� CSV. Pokud v�ak pozd�ji chceme podporovat export dat do jin�ch form�t� soubor�, jako je Excel nebo JSON, �prava t��dy FileExporter by poru�ila OCP.

### �e�en�
Abychom mohli pou��vat OCP, mus�me navrhnout na�i dom�nu slu�by pro export soubor� tak, aby byla otev�en� pro roz���en�.

Viz n�sleduj�c� p��klad refaktorovan�ho k�du.

```
public abstract class FileExporter
{
    public abstract void Export(string filePath, DataTable data);
}

public class CsvFileExporter : FileExporter
{
    public override void Export(string filePath, DataTable data)
    {
        // Code logic to export data to a CSV file.
    }
}

public class ExcelFileExporter : FileExporter
{
    public override void Export(string filePath, DataTable data)
    {
        // Code logic to export data to an Excel file.
    }
}

public class JsonFileExporter : FileExporter
{
    public override void Export(string filePath, DataTable data)
    {
        // Code logic to export data to a JSON file.
    }
}
```

Ve vylep�en� implementaci zav�d�me abstraktn� t��du FileExporter , kter� definuje spole�n� chov�n� pro v�echny operace exportu soubor�. Ka�d� konkr�tn� export�r soubor� ( CsvFileExporter , ExcelFileExporter a JsonFileExporter ) d�d� z t��dy FileExporter a implementuje metodu Export podle konkr�tn� logiky exportu form�tu souboru.

Pou�it� OCP umo��uje p�id�vat nov� export�ry soubor� bez �prav star�ch, co� usnad�uje p�id�v�n� nov�ch funkc� zaveden�m podt��d z�kladn� t��dy FileExporter.

Tento p��stup zvy�uje flexibilitu k�du, op�tovnou pou�itelnost a udr�ovatelnost. V� k�d bez probl�m� zvl�dne nov� po�adavky a zm�ny, ani� by zav�d�l chyby nebo naru�oval st�vaj�c� funkce.