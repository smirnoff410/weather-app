# Лабораторная работа №4
## SOLID

### Задание
1. Выучить каждый из принципов SOLID
2. Исправить код основываясь на каждый из принципов

### Шаги
1. Single Responsibility Principle - принцип единственной ответственности. Каждый класс должен иметь только одну зону ответственности
```csharp
public class ShopService
{
    public Shop CreateShop(int id)
    {
        return new Shop { Id = id };
    }

    public void CreateOrder(Shop shop)
    {
        //create order
    }

    public void PrintOrder(int id)
    {
        //print order info
    }

    public void GetProductInfo(string category)
    {
        if(category == "pants")
        {
            //get pants info
        }
        if(category == "shoes")
        {
            //get shoes info
        }
    }
}
```

2. Open closed Principle - принцип открытости-закрытости. Классы должны быть открыты для расширения, но закрыты для изменения.
```csharp
public class PaymentService
{
    public void Payment(string type)
    {
        if(type == "card")
        {
            //payment by credit card
        }
        if(type == "cash")
        {
            //payment by cash
        }
    }
}
```

3. Liskov substitution Principle - принцип подстановки Барбары Лисков. Должна быть возможность вместо базового (родительского) типа (класса) подставить любой его подтип (класс-наследник), при этом работа программы не должна измениться.
```csharp
public interface IShopService
{
    public string GetNameShop();
    public string GetLocationShop();
}
public class ZaraShopService : IShopService
{
    public string GetLocationShop()
    {
        return "г. Волгоград, ул. Пушкина, д. Колотушкина";
    }

    public string GetNameShop()
    {
        return "Zara";
    }
}
public class AsosShopService : IShopService
{
    public string GetLocationShop()
    {
        //нет имплементации, т к у онлайн магазина нет координат
        throw new NotImplementedException();
    }

    public string GetNameShop()
    {
        return "ASOS";
    }
}
```

4. Interface Segregation Principle - принцип разделения интерфейсов. Данный принцип обозначает, 
 что не нужно заставлять клиента (класс) реализовывать интерфейс, который не имеет к нему отношения.
```csharp
public interface IMoving
{
    void CarMoving();
    void TramMoving();
    void BusMoving();
}
public class MikeMovingService : IMoving
{
    public void BusMoving()
    {
        //some logic
    }

    public void CarMoving()
    {
        //У Майка нет машины, он не может реализовать этот метод
        throw new NotImplementedException();
    }

    public void TramMoving()
    {
        //some logic
    }
}
public class VasyaMovingService : IMoving
{
    public void BusMoving()
    {
        //logic
    }

    public void CarMoving()
    {
        //logic
    }

    public void TramMoving()
    {
        //Вася живет в городе, где нет трамваев, он не может реализовать этот метод
        throw new NotImplementedException();
    }
}
```

5. Dependency Inversion Principle - принцип инверсии зависимостей. Модули верхнего уровня не должны зависеть от модулей нижнего уровня. И те, и другие должны зависеть от абстракции. Абстракции не должны зависеть от деталей. Детали должны зависеть от абстракций.
```csharp
public class Shop
{
    private Cash _cash;

    public Shop(Cash cash)
    {
        _cash = cash;
    }

    public void DoPayment(int amount)
    {
        _cash.DoTransaction(amount);
    }
}
public class Cash
{
    public void DoTransaction(int amout)
    {
        //logic
    }
}
```
