# (ISP) Interface Segregation Principle
1.1. Princip segregace rozhran� uv�d�, �e t��da by nem�la b�t nucena implementovat rozhran�, kter� nepou��v�. Tento princip podporuje vytv��en� mal�ch rozhran� specifick�ch pro klienta.

1.2. Princip segregace rozhran� uv�d�, �e "klienti by nem�li b�t nuceni implementovat rozhran�, kter� nepou��vaj�. M�sto jednoho tlust�ho rozhran� je up�ednost�ov�no mnoho mal�ch rozhran� zalo�en�ch na skupin�ch metod, z nich� ka�d� obsluhuje jeden submodul."

M��eme to definovat i jinak. Rozhran� by m�lo v�ce souviset s k�dem, kter� jej pou��v�, ne� s k�dem, kter� jej implementuje. Metody na rozhran� jsou tedy definov�ny t�m, kter� metody klientsk� k�d pot�ebuje, sp�e ne� kter� metody t��da implementuje. Klienti by tedy nem�li b�t nuceni z�viset na rozhran�ch, kter� nepou��vaj�.

Nem�li byste b�t nuceni implementovat rozhran�, kdy� v� objekt tento ��el nesd�l�. ��m v�t�� je rozhran�, t�m pravd�podobn�ji obsahuje metody, kter� nemohou pou��vat v�ichni implement�to�i. To je podstata principu segregace rozhran�.

## P��klad 1

```
public interface IWorker
{
    void Work();
    void Eat();
}

public class Manager : IWorker
{
    // implementation
}

public class Robot : IWorker
{
    // implementation
}
```
T��da Robotje nucena implementovat Eatmetodu, ��m� poru�uje ISP.

```
public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public class Manager : IWorkable, IEatable
{
    // implementation
}

public class Robot : IWorkable
{
    // implementation
}
```
Rozd�len�m IWorkerrozhran� na men�� rozhran� ( IWorkablea IEatable) mohou t��dy implementovat pouze to, co pot�ebuj�, v souladu s ISP.

Podle LSP by ��dn� klient nem�l b�t nucen pou��vat rozhran�, kter� je pro n�j irelevantn�. Jin�mi slovy, klienti by nem�li b�t nuceni z�viset na metod�ch, kter� nepou��vaj�.

## P��klad 2
P�edpokl�dejme, �e pot�ebujeme vybudovat syst�m pro IT firmu, kter� obsahuje role jako TeamLead a Programmer, kde TeamLead rozd�luje velk� �koly na men�� �koly a p�id�luje je sv�m program�tor�m nebo na nich m��e p��mo pracovat.

Na z�klad� specifikac� mus�me vytvo�it rozhran� a t��du TeamLead k jeho implementaci. 
```
public Interface ILead
{
   void CreateSubTask();
   void AssginTask();
   void WorkOnTask();
}

public class TeamLead : ILead
{
   public void AssignTask()
   {
      //Code to assign a task.
   }
   public void CreateSubTask()
   {
      //Code to create a sub task
   }
   public void WorkOnTask()
   {
      //Code to implement perform assigned task.
   }
}
```

Pozd�ji se v�ak do syst�mu zavede dal�� role, nap��klad mana�er, kter� p�id�luje �koly TeamLead a nebude na nich pracovat.

```
public class Manager: ILead
{
   public void AssignTask()
   {
      //Code to assign a task.
   }
   public void CreateSubTask()
   {
      //Code to create a sub task.
   }
   public void WorkOnTask()
   {
      throw new Exception("Manager can't work on Task");
   }
}
```
Proto�e mana�er nem��e pracovat na �kolu a z�rove� nikdo nem��e p�id�lovat �koly mana�erovi, tato WorkOnTask() by nem�la b�t ve t��d� Manager. Ale implementujeme tuto t��du z rozhran� ILead; mus�me poskytnout konkr�tn� metodu. Zde nut�me t��du Manager implementovat metodu WorkOnTask() bez ��elu. To je �patn�. Design poru�uje ISP.

M�me 3 role. Mana�er (rozd�luje a p�id�luje �koly), TeamLead (p�id�lovat, rozd�lovat, pracovat), Program�tor (pracovat)
```
public interface IProgrammer
{
   void WorkOnTask();
}
```

Rozhran�, kter� poskytuje smlouvy pro spr�vu �kol�:
```
public interface ILead
{
   void AssignTask();
   void CreateSubTask();
}
```

Implementace
```
public class Programmer: IProgrammer
{
   public void WorkOnTask()
   {
      //code to implement to work on the Task.
   }
}

public class Manager: ILead
{
   public void AssignTask()
   {
      //Code to assign a Task
   }
   public void CreateSubTask()
   {
   //Code to create a sub taks from a task.
   }
}

public class TeamLead: IProgrammer, ILead
{
   public void AssignTask()
   {
      //Code to assign a Task
   }
   public void CreateSubTask()
   {
      //Code to create a sub task from a task.
   }
   public void WorkOnTask()
   {
      //code to implement to work on the Task.
   }
}
```

## P��klad 3

```
public interface IOrder
{
    void PlaceOrder();
    void CancelOrder();
    void UpdateOrder();
    void CalculateTotal();
    void GenerateInvoice();
    void SendConfirmationEmail();
    void PrintLabel();
}

public class OnlineOrder : IOrder
{
    // Implementation of all methods.
}

public class InStoreOrder : IOrder
{
    // Implementation of all methods.
}
```

V p�edchoz�m p��kladu m�me rozhran� IOrder , kter� obsahuje metody pro zad�n� objedn�vky, zru�en� objedn�vky, aktualizaci objedn�vky, v�po�et sou�tu, vygenerov�n� faktury, odesl�n� potvrzovac�ho e-mailu a tisk �t�tku.

Ne v�echny t��dy klient� implementuj�c� toto rozhran� v�ak vy�aduj� nebo pou��vaj� v�echny tyto metody. To poru�uje ISP, proto�e klienti jsou nuceni z�viset na metod�ch, kter� nepot�ebuj�.

Sledov�n�m ISP m��eme refaktorovat k�d rozd�len�m rozhran� na men��, v�ce zam��en� rozhran�.

```
public interface IOrder
{
    void PlaceOrder();
    void CancelOrder();
    void UpdateOrder();
}

public interface IOrderProcessing
{
    void CalculateTotal();
}

public interface IInvoiceGenerator
{
    void GenerateInvoice();
}

public interface IEmailSender
{
    void SendConfirmationEmail();
}

public interface ILabelPrinter
{
    void PrintLabel();
}

// Implement only the necessary interfaces in client classes.
public class OnlineOrder : IOrder, IOrderProcessing, IInvoiceGenerator, IEmailSender
{
    // Implementation of required methods.
}

public class InStoreOrder : IOrder, IOrderProcessing, ILabelPrinter
{
    // Implementation of required methods.
}
```

Odd�len�m rozhran� nyn� m�me men��, v�ce zam��en� rozhran�, kter� si klienti mohou zvolit implementovat na z�klad� sv�ch specifick�ch pot�eb. Tento p��stup odstra�uje zbyte�n� z�vislosti a umo��uje lep�� roz�i�itelnost a udr�ovatelnost. Klienti mohou implementovat pouze rozhran�, kter� po�aduj�, v�sledkem je �ist�� k�d, kter� je snaz�� pochopit, testovat a upravovat.