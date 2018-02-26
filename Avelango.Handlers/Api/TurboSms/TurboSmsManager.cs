namespace Avelango.Handlers.Api.TurboSms
{
    public static class TurboSmsManager
    {
        public static bool Send(string number, string message) {
            var worker = SMSProject.Services.SMSWorker.GetInstance();
            var res = worker.Auth("AvelangoGate", "AvelangoGatePoint");
            if (res.ToLower().Contains("неверн")) worker.CloseSession();

            try {
                var ress = worker.SendSMS("AVELANGO", number, message, "");
                worker.CloseSession();
                return ress.Length > 1;
            }
            catch {
                return false;
            }
        }
    }
}
