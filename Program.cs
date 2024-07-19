using Newtonsoft.Json;
using NoobsMuc.Coinmarketcap.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace HW
{
  public class Coin
  {
    public string id;
    public string symbol;
    public string name;
    public string image;
    public double currentPrice;
    public double marketCap;
    public int marketCapRank;
    public double fullyDilutedValuation;
    public double totalVolume;
    public double high24h;
    public double low24h;
    public double priceChange24h;
    public double priceChange24hPercentage;
    public double marketCapChange24h;
    public double marketCapChangePercentage24h;
    public double ciculatingSupply;
    public double totalSupply;
    public double maxSupply;
    public double ath;
    public double athChangePercentage;
    public string athDate;
    public double atl;
    public double atlChange;
    public string atlDate;
    public ROI roi;
    public string lastUpdated;
  }

  public class ROI
  {
    public string times;
    public string currency;
    public double percentage;
  }

  public class customers
  {
    public List<person> people { get; set; }
  }

  public class person
  {
    public string name { get; set; }
    public portfolio portfolio { get; set; }
  }

  public class holding
  {
    public string name { get; set; }
    public double quantity{ get; set; }
    public double avgprice { get; set; }
  }

  class Market
  {
    public static Dictionary<string, double> coinCurrentPrice; // current price of portfolio coins 
    static Dictionary<string, Coin> coinData; // simple dictionary of coin Ticker to Coin objects. [not expected to be used] 
    private customers customers; // object holding all portfolio information
    public static HashSet<string> allCoinTickers; // only need to update these coin prices 
    
    public void PNLDisplay()
    {
      Console.WriteLine("-----------PNL at"+ string.Format("{0:HH:mm:ss tt}", DateTime.Now)+"-------");
      foreach (var p in customers.people)
      {
        Console.Write(p.name + ":"); p.portfolio.DisplayPNL();
      }
      Console.WriteLine("----------------------------------------");
    }


    public Market()
    {
      coinData = new Dictionary<string, Coin>();
      coinCurrentPrice = new Dictionary<string, double>();
    }

    public Market(string markets, string portfolio3)
    {
      //adding all coin information to static field
      coinData = new Dictionary<string, Coin>();
      coinCurrentPrice = new Dictionary<string, double>();
      var pathToJson = Path.Combine(markets);
      var r = new StreamReader(pathToJson);
      var myJson = r.ReadToEnd();
      List<Coin> coins = JsonConvert.DeserializeObject<List<Coin>>(myJson);
      foreach (var coin in coins)
      {
        coinData[coin.symbol] = coin;
      }

      // why not working....
      //var pathToJson1 = Path.Combine(portfolio3);
      //var r1 = new StreamReader(pathToJson1);
      //var myJson1 = r1.ReadToEnd();
      //var t = JsonConvert.DeserializeObject<customers>(myJson1);
      //customers cs = JsonSerializer.Deserialize<customers>(myJson1);
      //customers cs = new JavaScriptSerializer().Deserialize<customers>(myJson1);

      // After failing to deserialize the file, I am manually adding the data
      // (already wasted too much time..) after restructuring object strucutre, it is hard to go back..
      manualParse();
      
      //collecting Ticker Set at least one client holds. Only need to update the price of these coins
      allCoinTickers = new HashSet<string>();
      foreach(var p in customers.people)
      {
        foreach(var coin in p.portfolio.coinPortfolio)
        {
          allCoinTickers.Add(coin.Key);
        }
      }
    }

    public void UpdateMarketPrice(Dictionary<string,double> d)
    {
      foreach(var m in d)
      {
        if (!allCoinTickers.Contains(m.Key)) continue; 
        coinCurrentPrice[m.Key]= m.Value;
      }
      foreach(var p in customers.people)
      {
        p.portfolio.UpdatePortfolioValuePNL();
      }
    }

    void manualParse()
    {
      customers = new customers();
      person person1 = new person();
      person1.name = "Person1";

      List<holding> holdings1 = new List<holding>();
      holding holding11 = new holding();
      holding11.name = "BTC"; holding11.quantity = 0.124; holding11.avgprice = 21345.67;
      holdings1.Add(holding11);
      holding holding12 = new holding();
      holding12.name = "ETH"; holding12.quantity = 0.48; holding12.avgprice = 1234.67;
      holdings1.Add(holding12);
      holding holding13 = new holding();
      holding13.name = "BNB"; holding13.quantity = 12; holding13.avgprice = 101.67;
      holdings1.Add(holding13);
      holding holding14 = new holding();
      holding14.name = "TRX"; holding14.quantity = 1241; holding14.avgprice = 0.31;
      holdings1.Add(holding14);
      person1.portfolio = new portfolio(holdings1);

      List<holding> holdings2 = new List<holding>();
      person person2 = new person();
      person2.name = "Person2";
      holding holding21 = new holding();
      holding21.name = "BTC"; holding21.quantity = 0.114; holding21.avgprice = 60011;
      holdings2.Add(holding21);
      holding holding22 = new holding();
      holding22.name = "ETH"; holding22.quantity = 1241; holding22.avgprice = 657.25;
      holdings2.Add(holding22);
      holding holding23 = new holding();
      holding23.name = "BNB"; holding23.quantity = 5; holding23.avgprice = 103;
      holdings2.Add(holding23);
      holding holding24 = new holding();
      holding24.name = "TRX"; holding24.quantity = 111; holding24.avgprice = 0.32;
      holdings2.Add(holding24);
      person2.portfolio = new portfolio(holdings2);

      List<holding> holdings3 = new List<holding>();
      person person3 = new person();
      person3.name = "Person3";
      holding holding31 = new holding();
      holding31.name = "BTC"; holding31.quantity = 0.4; holding31.avgprice = 32345.67;
      holdings3.Add(holding31);
      holding holding32 = new holding();
      holding32.name = "ETH"; holding32.quantity = 12; holding32.avgprice = 1092;
      holdings3.Add(holding32);
      holding holding33 = new holding();
      holding33.name = "BNB"; holding33.quantity = 1; holding33.avgprice = 76.12;
      holdings3.Add(holding33);
      holding holding34 = new holding();
      holding34.name = "TRX"; holding34.quantity = 1221; holding34.avgprice = 11.12;
      holdings3.Add(holding34);
      person3.portfolio = new portfolio(holdings3);

      List<holding> holdings4 = new List<holding>();
      person person4 = new person();
      person4.name = "Person4";
      holding holding41 = new holding();
      holding41.name = "BTC"; holding41.quantity = 0.02; holding41.avgprice = 67521.12;
      holdings4.Add(holding41);
      holding holding42 = new holding();
      holding42.name = "ETH"; holding42.quantity = 986; holding42.avgprice = 1211;
      holdings4.Add(holding42);
      holding holding43 = new holding();
      holding43.name = "BNB"; holding43.quantity = 1; holding43.avgprice = 12;
      holdings4.Add(holding43);
      holding holding44 = new holding();
      holding44.name = "TRX"; holding44.quantity = 98.12; holding44.avgprice = 12.01;
      holdings4.Add(holding44);
      person4.portfolio = new portfolio(holdings4);

      List<holding> holdings5 = new List<holding>();
      person person5 = new person();
      person5.name = "Person4";
      holding holding51 = new holding();
      holding51.name = "BTC"; holding51.quantity = 0; holding51.avgprice = 0;
      holdings5.Add(holding51);
      holding holding52 = new holding();
      holding52.name = "ETH"; holding52.quantity = 6; holding52.avgprice = 751;
      holdings5.Add(holding52);
      holding holding53 = new holding();
      holding53.name = "BNB"; holding53.quantity = 0.0001; holding53.avgprice = 121;
      holdings5.Add(holding53);
      holding holding54 = new holding();
      holding54.name = "TRX"; holding54.quantity = 0; holding54.avgprice = 0;
      holdings5.Add(holding54);
      person5.portfolio = new portfolio(holdings5);

      customers.people = new List<person>();
      customers.people.Add(person1);
      customers.people.Add(person2);
      customers.people.Add(person3);
      customers.people.Add(person4);
      customers.people.Add(person5);
    }
  }

    public class portfolio
    {
      Dictionary<string, double> coinPurchasePrice; // purchased price of portfolio coins
      public Dictionary<string, double> coinPortfolio; //quantity purchased
      Dictionary<string, double> pnl;
      public double TotalPurchasePrice;    
      public double TotalCurrentPortfolioValue;
      
      // PNL means profit or loss percentage? or dollar value?
      public double TotalPNL => TotalPurchasePrice == 0.0 ? 0.0 : (TotalCurrentPortfolioValue - TotalPurchasePrice) / TotalPurchasePrice;

      //empty constructor
      public portfolio()
      {
        coinPortfolio = new Dictionary<string, double>();
        coinPurchasePrice = new Dictionary<string, double>();
        pnl = new Dictionary<string, double>();
        TotalPurchasePrice = 0.0;
        TotalCurrentPortfolioValue = 0.0;
      }
      
     public portfolio(List<holding> holdings)
    {
      coinPortfolio = new Dictionary<string, double>();
      coinPurchasePrice = new Dictionary<string, double>();
      pnl = new Dictionary<string, double>();
      TotalPurchasePrice = 0.0;
      foreach(holding h in holdings)
      {
        if (h.quantity==0.0) continue;  // would this be ok?? Looks a bit dangerous.. 
        coinPortfolio[h.name] = h.quantity;
        coinPurchasePrice[h.name] = h.avgprice;
        pnl[h.name] = 0.0;
        TotalPurchasePrice += h.avgprice * h.quantity;
      }
      TotalCurrentPortfolioValue = TotalPurchasePrice;
    }


      public void UpdatePortfolioValuePNL()
      {
        double portfolioValue = 0.0;
        foreach (var m in coinPortfolio)
        {  
          portfolioValue += coinPortfolio[m.Key] * Market.coinCurrentPrice[m.Key];
          pnl[m.Key] = (Market.coinCurrentPrice[m.Key] - coinPurchasePrice[m.Key]) / coinPurchasePrice[m.Key];
        }
        TotalCurrentPortfolioValue = portfolioValue;
      }

    // Made this, but not plan to use. A bit too much info for 1 min updating screen
      public void Display()
      {
        Console.WriteLine("-----------------------------------");
        Console.WriteLine();
        foreach (var m in coinPortfolio)
        {
          Console.WriteLine("Ticker:" + m.Key);
          Console.WriteLine("Quantity:" + m.Value);
          Console.WriteLine("Purchase Price:" + coinPurchasePrice[m.Key]);
          Console.WriteLine("Current Price:" + Market.coinCurrentPrice[m.Key]);
          Console.WriteLine("PNL for ticker:" + pnl[m.Key]);
          Console.WriteLine();
        }
        Console.WriteLine("Purchase price:" + TotalPurchasePrice);
        Console.WriteLine("Current Portfolio Value:" + TotalCurrentPortfolioValue);
        Console.WriteLine("Total PNL" + TotalPNL);
        Console.WriteLine("-----------------------------------");
      }

      public void DisplayPNL()
      {
        Console.Write(Math.Round(TotalPNL*100,2)+"%"); Console.WriteLine();
      }
    }


    class Program
    {

    static void Main(string[] args)
    {
      // Third party data import... You need license Key as in here..
      ICoinmarketcapClient client = new CoinmarketcapClient("a76e7170-960a-4ea0-9b67-16c26d69a4db");
 
      var m = new Market("markets.json", "Portfolios3.json");
      //m.PNLDisplay();

      var np = new Dictionary<string, double>();
      while(true)
      {
        foreach (string ticker in Market.allCoinTickers)
        {
          np[ticker] = decimal.ToDouble(client.GetCurrencyBySymbol(ticker, "USD").Price);
        }
        m.UpdateMarketPrice(np);
        m.PNLDisplay();
        Thread.Sleep(60000);
      }

      Console.WriteLine("Hello World!");
      }
    }
  
}
