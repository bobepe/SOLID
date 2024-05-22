# (SRP) Single Responsibility Principle 
Ka�d� t��da nebo podobn� struktura ve va�em k�du by m�la m�t pouze jednu �lohu. V�e v t�to t��d� by m�lo souviset s jedin�m ��elem.

Resp. ned�lejme p��li� slo�it� t��dy. Sp� ud�lat v�c jednoduch�ch t��d ne� jednu slo�itou

## P��klad 1

``` c#
public class UserCreator
{
    public void CreateUser(string username, string email, string password)
    {
        // Validation logic
        if (!ValidateEmail(email))
        {
            throw new ArgumentException("Invalid email format.");
        }
        // Business rules
        // Database persistence
        SaveUserToDatabase(username, email, password);
    }
    private bool ValidateEmail(string email)
    {
        // Validation logic
    }
    private void SaveUserToDatabase(string username, string email, string password)
    {
        // Database persistence logic
    }
}
```

### Probl�m
V tomto k�du t��da UserCreator poru�uje SRP t�m, �e kombinuje v�ce odpov�dnost�, jako je ov��ov�n� a perzistence datab�ze. To m��e v�st k t�sn� propojen� t��d�, co� zt�uje testov�n� a je n�chyln� ke zbyte�n�m �prav�m.

### �e�en�
Abychom tento probl�m vy�e�ili, m��eme pou��t SRP refaktorov�n�m k�du tak, aby byly tyto odpov�dnosti rozd�leny do jednotliv�ch t��d.

``` c#
public class UserValidator
{
    public bool ValidateEmail(string email)
    {
        // Validation logic
    }
}
public class UserRepository
{
    public void SaveUser(string username, string email, string password)
    {
        // Database persistence logic
    }
}
public class UserCreator
{
    private readonly UserValidator _validator;
    private readonly UserRepository _repository;
    public UserCreator(UserValidator validator, UserRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }
    public void CreateUser(string username, string email, string password)
    {
        if (!_validator.ValidateEmail(email))
        {
            throw new ArgumentException("Invalid email format.");
        }
        // Business rules
        _repository.SaveUser(username, email, password);
    }
}
```

## P��klad 2

```
// Bad example violating SRP
public class Customer
{
    public void AddCustomer()
    {
        // Code to add a customer to the database
    }

    public void SendEmail()
    {
        // Code to send an email to the customer
    }
}

// Good example following SRP
public class CustomerService
{
    public void AddCustomer()
    {
        // Code to add a customer to the database
    }
}

public class EmailService
{
    public void SendEmail()
    {
        // Code to send an email to the customer
    }
}
```

V dan�m p��kladu k�du m�me t��du nazvanou Z�kazn�k , kter� poru�uje princip jednotn� odpov�dnosti (SRP). T��da Customer je zodpov�dn� jak za p�id�n� z�kazn�ka do datab�ze, tak za odesl�n� e-mailu z�kazn�kovi. To poru�uje SRP , proto�e t��da m� n�kolik d�vod� ke zm�n� � pokud dojde ke zm�n�m v datab�zov�ch operac�ch nebo logice odes�l�n� e-mail�, t��da Customer bude muset b�t upravena.

Abychom tento probl�m vy�e�ili a ��dili se SRP, m��eme k�d refaktorovat rozd�len�m odpov�dnost� do dvou odli�n�ch t��d: CustomerService a EmailService .

T��da CustomerService je zodpov�dn� za p�id�n� z�kazn�ka do datab�ze. Obsahuje metodu nazvanou AddCustomer() , kter� zpracov�v� operace souvisej�c� s datab�z�. Odd�len�m funkc� souvisej�c�ch s datab�z� do vlastn� t��dy dodr�ujeme SRP , proto�e t��da CustomerService m� nyn� jedinou odpov�dnost .

T��da EmailService je zodpov�dn� za odes�l�n� e-mail� z�kazn�k�m. Obsahuje metodu nazvanou SendEmail() , kter� zpracov�v� logiku odes�l�n� e-mail�. P�esunut�m funkc� souvisej�c�ch s e-mailem do vlastn� t��dy je odd�l�me od datab�zov�ch operac� a dodr�ujeme SRP .

Rozd�len�m odpov�dnost� do samostatn�ch t��d dos�hneme lep��ho odd�len� z�jm� , co� vede k udr�iteln�j��mu a flexibiln�j��mu k�du. Nyn�, pokud dojde ke zm�n�m v datab�zov�ch operac�ch nebo logice odes�l�n� e-mail�, pot�ebujeme pouze upravit p��slu�nou t��du, ��m� se minimalizuje dopad na ostatn� ��sti k�dov� z�kladny.