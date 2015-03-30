namespace MarcRecordServiceApp.Core.MarcRecord
{
    public enum MarcRecordProvider
    {        
        Lc = 1,    
        Nlm = 2,
        Rbd = 3
    }
    public class MarcRecordProviderValue
    {
        private readonly string _provider;
        public MarcRecordProviderValue(MarcRecordProvider value)
        {
            switch (value)
            {
                  case MarcRecordProvider.Lc:
                    _provider = "z3950.loc.gov";
                    break;
                  case MarcRecordProvider.Nlm:
                    _provider = "ilsz3950.nlm.nih.gov";
                    break;
                  case MarcRecordProvider.Rbd:
                    _provider = "";
                    break;
            }
        }
        public new string ToString()
        {
            return _provider;
        }
    }
}
