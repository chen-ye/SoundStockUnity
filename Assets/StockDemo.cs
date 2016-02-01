using UnityEngine;
using LitJson;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SoundStock
{
    public class Stocks
    {
        public List<Stock> stocks { get; set; }
    }

    public class Stock {
        public string name { get; set; }
        public List<Trade> trades { get; set; }
    }

    public class Trade
    {
        public double timestamp { get; set; }
        public double price { get; set; }
        public double trades { get; set; }
        public double percent { get; set; }
        public double vwap { get; set; }
    }

    public class StockDemo : MonoBehaviour
    {
        public static Dictionary<string, Stock> stocks;
        public static int dataLength = 0;

        public static void LoadTradeData(string json_text)
        {
            Console.WriteLine("Reading data from the following JSON string: {0}",
                              json_text);

            Stocks data = JsonMapper.ToObject<Stocks>(json_text);

            //foreach (Stock stock in data.stocks) {
            //    stocks[stock.name] = stock;
            //    dataLength = stock.trades.Count;
            //    print(stock.name);
            //    foreach(Trade trade in stock.trades)
            //    {
            //        //print(trade.percent);
            //    }
            //}
            print("done loading trade data");
        }


        // Use this for initialization
        IEnumerator Start()
        {
            stocks = new Dictionary<string, Stock>();
            Boolean loadContinuous = true;
            if (loadContinuous)
            {
                String[] symbols = new String[2]{ "NDAQ", "GOOG" };
                return loadContinuously(System.DateTime.Now, symbols);
            }
            else
            {
                return loadHistoricalData();
            }
        }

        IEnumerator loadHistoricalData()
        {
            WWW www = new WWW("file:///C:/Users/Skylion/Documents/GitHub/SoundStock/updatedTrades4.json");
            yield return www;
            if (www.error == null)
            {
                //Sucessfully loaded the JSON string
                Debug.Log("Loaded following JSON string" + www.text);

                //Process books found in JSON file
                LoadTradeData(www.text);
            }
            else
            {
                Debug.Log("ERROR: " + www.error);
            }
        }

        IEnumerator loadContinuously(DateTime now, String[] symbols)
        {
            WWWForm form = new WWWForm();
            form.AddField("_Token", "EE28DC758EE846CB956F7380E7141CE8"); //The token
            form.AddField("Symbols", string.Join(", ", symbols)); //The Sy,bols you want
            DateTime easternTime = System.DateTime.Now; //Current UTC TI
                                                        //DateTime easternTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(thisDate, "UTC", "Eastern Standard Time"); //Converts UTC Time to Eastern
                                                        //TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                                                        //DateTime easternTime = TimeZoneInfo.ConvertTime(System.DateTime.Now, est);

            Console.WriteLine(easternTime.ToString("MM/dd/yy H:mm:ss.fff"));
            form.AddField("DateTime", easternTime.ToString("MM/dd/yy H:mm:ss.fff")); //The proper DateTime in the proper format
            var url = "http://ws.nasdaqdod.com/v1/NASDAQTrades.asmx/GetPreviousTrade HTTP/1.1"; //"http://ws.nasdaqdod.com/v1/NASDAQTrades.asmx/GetNextTrade"; //Post endpoint
            WWW www = new WWW(url, form);
            yield return www;
            if (www.error == null)
            {
                //Sucessfully loaded the JSON string
                Debug.Log("Loaded following JSON string" + www.text);

                //Process books found in JSON file
                print(www.text);
            }
            else
            {
                Debug.Log("ERROR: " + www.error);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnEnable()
        {
            //Subscribe and register, we need the info! Always in OnEnable
            //pulse.RegisterPulseController(this);
            //patternA.onPatternWillPlay += PatternAWillPlay;
        }

        void OnDisable()
        {
            //Always unregister in OnDisable
            //pulse.UnregisterPulseController(this);
            //patternA.onPatternWillPlay -= PatternAWillPlay;
        }
    }
}
