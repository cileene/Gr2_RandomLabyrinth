using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace Utils
{
    public class UGSAnalytics : MonoBehaviour
    {
        private async void Start()
        {
            try
            {
                await UnityServices.InitializeAsync();
                GiveConsent(); //Get user consent according to various legislations
            }
            catch (ConsentCheckException e) // ???
            {
                Debug.Log(e.ToString());
            }
        }

        private void GiveConsent()
        {
            // Call if consent has been given by the user
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log($"Consent has been provided. The SDK is now collecting data!");
        }
    }

    internal class ConsentCheckException : Exception
    {
    }
}