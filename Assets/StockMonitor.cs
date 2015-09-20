using UnityEngine;
using System.Collections;
using GAudio;

namespace SoundStock
{
    public class StockMonitor : MonoBehaviour
    {
        public string stockName = "AAPL";
        private PitchShifter pitchShifter;
        private MasterPulseModule masterPulseModule;
        public PulsedPatternModule bassPulse;

        // Use this for initialization
        void Start()
        {
            this.pitchShifter = gameObject.GetComponent<PitchShifter>();
            this.masterPulseModule = gameObject.GetComponent<MasterPulseModule>();
        }

        Trade GetCurrentTrade(Stock stock)
        {
            return stock.trades[(int) (Time.time % StockDemo.dataLength)];
        }

        // Update is called once per frame
        void Update()
        {
            Stock stock;
            if(StockDemo.stocks.TryGetValue(stockName, out stock))
            {
                Trade currentTrade = GetCurrentTrade(stock);
                pitchShifter.metric = ((float)currentTrade.price);
                masterPulseModule.Period = (1f / (float)currentTrade.trades);
                bassPulse.Samples[0].Gain = (float)currentTrade.vwap - 113;
            }
        }
    }
}

