using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SoundStock
{
    /// <summary>
    /// StockManager 
    /// </summary>
    public class StockManager : MonoBehaviour
    {

        public const string APIKey = "EE28DC758EE846CB956F7380E7141CE8";
        public Dictionary<string, Queue<Trade>> TradeBuffer { private set; get; }
        private DateTimeOffset lastUpdatedEST { set; get; }

        private string symbols = "";

        private const string timeFormat = "M/d/yyyy H:mm:ss.fff";
        private TimeZoneInfo easternZone;

        // Use this for initialization
        void Start()
        {
            TradeBuffer = new Dictionary<string, Queue<Trade>>();
            easternZone = TimeZoneInfo.CreateCustomTimeZone("Eastern Standard Time", new TimeSpan(-5, 0, 0), "Eastern Time (US & Canada)", "Eastern Standard Time"); //TimeZoneInfo.FromSerializedString("Eastern Standard Time;-300;(UTC-05:00) Eastern Time (US & Canada);Eastern Standard Time;Eastern Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];");
            lastUpdatedEST = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone).AddTicks(-1000000);

            AddStock("NDAQ");
        }

        private void RegenerateSymbols()
        {
            symbols = string.Join(",", TradeBuffer.Select(stock => stock.Key).ToArray());
        }

        public void AddStock(string stockName)
        {
            TradeBuffer[stockName] = new Queue<Trade>();
            RegenerateSymbols();
        } 

        public void RemoveStock(string stockName)
        {
            TradeBuffer.Remove(stockName);
            RegenerateSymbols();
        }

        IEnumerator GetNewTrades()
        {
            WWWForm form = new WWWForm();
            form.AddField("_Token", APIKey); //The token
            form.AddField("Symbols", symbols); //The Symbols you want
            DateTimeOffset currentTimeEST = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            
            form.AddField("StartDateTime", lastUpdatedEST.ToString(timeFormat)); //The proper DateTime in the proper format
            form.AddField("EndDateTime", currentTimeEST.AddTicks(-1000000).ToString(timeFormat)); //The proper DateTime in the proper format
            var url = "http://ws.nasdaqdod.com/v1/NASDAQTrades.asmx/GetTrades HTTP/1.1";

            WWW www = new WWW(url, form);
            yield return www;
            if (www.error == null)
            {
                //Sucessfully loaded the JSON string
                Debug.Log("Loaded following JSON string" + www.text);

            }
            else
            {
                Debug.Log("ERROR: " + www.error);
            }
        }


    }

}
