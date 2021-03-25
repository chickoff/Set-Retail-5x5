using System.Collections.Generic;
using System.Text;
using Set_Retail_5x5.Retail5x5.Common.Converters;

namespace Set_Retail_5x5.Retail5x5.Hardware.SmartCard
{
    public static class asc122H
    {
        //private static byte[] SendBuff = new byte[255];
        //private static byte[] RecvBuff = new byte[255];
        private static string COMPort = "COM3";
        //private static string NumberOfCard;


        private static void Beep(int hReader,int countBeep)
        {
            var led_controls = new acr122.ACR122_LED_CONTROL[2];
            int numControls = 2;
            int t1 = 100;
            int t2 = 100;
            int numTimes = countBeep;

            led_controls[0].finalState = acr122.ACR122_LED_STATE_ON;
            led_controls[0].updateEnabled = true;
            led_controls[0].initialBlinkingState = acr122.ACR122_LED_STATE_OFF;
            led_controls[0].blinkEnabled = false;

            led_controls[1].finalState = acr122.ACR122_LED_STATE_ON;
            led_controls[1].updateEnabled = false;
            led_controls[1].initialBlinkingState = acr122.ACR122_LED_STATE_OFF;
            led_controls[1].blinkEnabled = true;

            acr122.ACR122_SetLedStatesWithBeep(hReader, ref led_controls, numControls, t1, t2, numTimes,
                acr122.ACR122_BUZZER_MODE_T1);
        }

       

        private static void CloseHandle(int hReader)
        {
            try
            {
                acr122.ACR122_Close(hReader);
            }
            catch
            {
                // ignored
            }
        }

        private static int OpenHandle()
        {
            var hReader = 0;

            var retCode = acr122.ACR122_OpenA(COMPort, ref hReader);
            /*
            acr122.ACR122_TIMEOUTS timeouts;

            timeouts.statusTimeout = 100;
            timeouts.numResponseRetries = 100;
            timeouts.numStatusRetries = 100;
            timeouts.responseTimeout = 100;

            acr122.ACR122_SetTimeouts(hReader, ref timeouts);
            */
            return retCode != 0 ? -1 : hReader;
        }

        private static byte[] ScanHandle(int hReader)
        {
            var retCode = 0;
            const int sendLen = 4;
            var recvLen = 255;
           
            if (retCode != 0) return null;

            var sendBuff = new byte[255];
            var recvBuff = new byte[255];

            sendBuff[0] = 0xD4;
            sendBuff[1] = 0x4A;
            sendBuff[2] = 0x01;
            sendBuff[3] = 0x00;

            retCode = acr122.ACR122_DirectTransmit(hReader, ref sendBuff[0], sendLen, ref recvBuff[0], ref recvLen);

            return retCode != 0 ? null : recvBuff;
        }

        public static string[] ScanCard(int countCard)
        {
            var cards = new List<string>();
            var hReader = OpenHandle();
            if (hReader == -1)
            {
                CloseHandle(hReader);
                return null;
            }
            for (var c = 0; c < countCard; c++)
            {
                var recvBuff = ScanHandle(hReader);
                if (recvBuff == null)
                {
                    CloseHandle(hReader);
                    return null;
                }
                var numOfTagFound = (int) recvBuff[2];
                if (numOfTagFound == 0)
                {
                    CloseHandle(hReader);
                    return null;
                }
                var sb = new StringBuilder();
                for (var i = 0; i < 4; i++)
                {
                    var b = recvBuff[i + 8].ToString();
                    sb.Append(b.AddLedZero(3));
                }
                cards.Add(sb.ToString());
            }
            Beep(hReader,1);
            CloseHandle(hReader);
            return cards.ToArray();
        }
    }


}
